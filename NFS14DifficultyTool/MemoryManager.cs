using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace NFS14DifficultyTool {
    public class MemoryManager {
        //Most (if not all) of these defs are from http://www.pinvoke.net/ - good stuff!
        protected enum ProcessorArchitecture {
            X86 = 0,
            X64 = 9,
            @Arm = -1,
            Itanium = 6,
            Unknown = 0xFFFF,
        }
        [StructLayout(LayoutKind.Sequential)]
        protected struct SystemInfo {
            public ProcessorArchitecture ProcessorArchitecture; // WORD
            public uint PageSize; // DWORD
            public IntPtr MinimumApplicationAddress; // (long)void*
            public IntPtr MaximumApplicationAddress; // (long)void*
            public IntPtr ActiveProcessorMask; // DWORD*
            public uint NumberOfProcessors; // DWORD (WTF)
            public uint ProcessorType; // DWORD
            public uint AllocationGranularity; // DWORD
            public ushort ProcessorLevel; // WORD
            public ushort ProcessorRevision; // WORD
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern void GetSystemInfo(out SystemInfo Info);

        [Flags]
        protected enum ProcessAccessFlags : uint {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern IntPtr OpenProcess(
             ProcessAccessFlags processAccess,
             bool bInheritHandle,
             int processId
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool CloseHandle(IntPtr hProcess);

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead
        );
        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out, MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead
        );
        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            IntPtr lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out IntPtr lpNumberOfBytesWritten
        );
        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            IntPtr lpBuffer,
            int nSize,
            out IntPtr lpNumberOfBytesWritten
        );

        public bool CloseHandleOnFailedReadWrite { get; set; }

        protected IntPtr processHandle;
        protected SystemInfo sysInfo;

        protected struct FindObjectInfo {
            public bool aborting;
            public int numRunning;
        }
        protected FindObjectInfo findObjectInfo;
        protected Object findObjectLock;

        public MemoryManager() {
            CloseHandleOnFailedReadWrite = false;
            GetSystemInfo(out sysInfo);
            findObjectInfo.aborting = false;
            findObjectInfo.numRunning = 0;
            findObjectLock = new Object();
        }

        public bool IsProcessOpen() {
            return processHandle != default(IntPtr);
        }

        public bool OpenProcess(string processName) {
            Process[] procList = Process.GetProcessesByName(processName);
            if (procList.Length < 1)
                return false;

            processHandle = OpenProcess(
                ProcessAccessFlags.VirtualMemoryOperation
                    | ProcessAccessFlags.VirtualMemoryRead
                    | ProcessAccessFlags.VirtualMemoryWrite,
                true, procList[procList.Length - 1].Id);
            return IsProcessOpen();
        }

        public bool CloseHandle() {
            if (IsProcessOpen() && CloseHandle(processHandle)) {
                processHandle = default(IntPtr);
                return true;
            }
            return false;
        }

        public bool ReadProcessMemory(IntPtr lpBaseAddress, byte[] lpBuffer, out IntPtr lpNumberOfBytesRead) {
            lpNumberOfBytesRead = IntPtr.Zero;
            return IsProcessOpen() && ReadProcessMemory(processHandle, lpBaseAddress, lpBuffer, lpBuffer.Length, out lpNumberOfBytesRead);
        }

        public bool WriteProcessMemory(IntPtr lpBaseAddress, byte[] lpBuffer, out IntPtr lpNumberOfBytesWritten) {
            lpNumberOfBytesWritten = IntPtr.Zero;
            return IsProcessOpen() && WriteProcessMemory(processHandle, lpBaseAddress, lpBuffer, lpBuffer.Length, out lpNumberOfBytesWritten);
        }

        //From JaredPar at http://stackoverflow.com/a/321404 (http://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array)
        public static byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        //From Tomalak at http://stackoverflow.com/a/311179 (http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa)
        public static string ByteArrayToString(byte[] hex) {
            return BitConverter.ToString(hex).Replace("-", "");
        }

        public void AbortFindObject() {
            lock (findObjectLock)
                if (findObjectInfo.numRunning > 0)
                    findObjectInfo.aborting = true;
        }

        public IntPtr FindObject(byte[] searchBytes, int searchAlignment = 4) {
            lock (findObjectLock)
                findObjectInfo.numRunning++;

            IntPtr ret = Find(searchBytes, searchAlignment);

            lock (findObjectLock) {
                findObjectInfo.numRunning--;
                if (findObjectInfo.numRunning < 1)
                    findObjectInfo.aborting = false;
            }

            return ret;
        }
        protected IntPtr Find(byte[] searchBytes, int searchAlignment) {
            //TODO why am I using AllocationGranularity here?
            //TODO should probably be LongLength, this whole thing is a long mess
            byte[] buff = new byte[Math.Max(sysInfo.AllocationGranularity, searchBytes.Length)];
            IntPtr bytesRead;
            //TODO maybe i should be long or uint with UIntPtrs
            int i = 0;
            int j = 0; //j may be kept in-between i increments if we begin finding results at the end of our bytesRead - though in practice this should never happen
            for (IntPtr PTR = IntPtr.Zero; (long)PTR < (long)sysInfo.MaximumApplicationAddress; PTR += buff.Length) {
                if (findObjectInfo.aborting || !IsProcessOpen())
                    break;
                if (ReadProcessMemory(PTR, buff, out bytesRead)) {
                    for (i = 0; i + j < (int)bytesRead; i += searchAlignment) {
                        while (j < searchBytes.Length && i + j < (int)bytesRead) {
                            if (buff[i + j] != searchBytes[j]) {
                                j = 0;
                                break;
                            }
                            j++;
                        }
                        if (j == searchBytes.Length)
                            return PTR + i;
                    }
                }
            }

            return IntPtr.Zero;
        }

        public byte[] Read(IntPtr addr, int size) {
            byte[] buff = new byte[size];
            IntPtr lpNumberOfBytesRead;
            if (!ReadProcessMemory((IntPtr)addr, buff, out lpNumberOfBytesRead) && CloseHandleOnFailedReadWrite)
                CloseHandle();
            return buff;
        }
        public bool ReadBool(IntPtr addr) {
            return BitConverter.ToBoolean(Read(addr, sizeof(bool)), 0);
        }
        public int ReadInt(IntPtr addr) {
            return BitConverter.ToInt32(Read(addr, sizeof(int)), 0);
        }
        public float ReadFloat(IntPtr addr) {
            return BitConverter.ToSingle(Read(addr, sizeof(float)), 0);
        }
        public double ReadDouble(IntPtr addr) {
            return BitConverter.ToDouble(Read(addr, sizeof(double)), 0);
        }

        public bool Write(IntPtr addr, byte[] value) {
            IntPtr lpNumberOfBytesWritten;
            if (!WriteProcessMemory((IntPtr)addr, value, out lpNumberOfBytesWritten)) {
                if (CloseHandleOnFailedReadWrite)
                    CloseHandle();
                return false;
            }
            return true;
        }
        public bool WriteBool(IntPtr addr, bool value) {
            return Write(addr, BitConverter.GetBytes(value));
        }
        public bool WriteInt(IntPtr addr, int value) {
            return Write(addr, BitConverter.GetBytes(value));
        }
        public bool WriteFloat(IntPtr addr, float value) {
            return Write(addr, BitConverter.GetBytes(value));
        }
        public bool WriteDouble(IntPtr addr, double value) {
            return Write(addr, BitConverter.GetBytes(value));
        }
    }
}
