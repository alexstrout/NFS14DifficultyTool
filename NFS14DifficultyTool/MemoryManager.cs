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
        [DllImport("kernel32.dll", SetLastError = false)]
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
        [DllImport("kernel32.dll")]
        protected static extern IntPtr OpenProcess(
             ProcessAccessFlags processAccess,
             bool bInheritHandle,
             int processId
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool CloseHandle(IntPtr hObject);

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

        public bool ProcessOpen { get; protected set; }

        protected IntPtr processHandle;
        protected SystemInfo sysInfo;

        public MemoryManager() {
            ProcessOpen = false;
            GetSystemInfo(out sysInfo);
        }

        public bool OpenProcess(string processName) {
            Process[] procList = Process.GetProcessesByName(processName);
            if (procList.Length < 1)
                return false;

            IntPtr p;
            p = OpenProcess(ProcessAccessFlags.VirtualMemoryOperation | ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.VirtualMemoryWrite, true, procList[0].Id);
            if (p == default(IntPtr))
                return false;
            processHandle = p;
            ProcessOpen = true;
            return true;
        }

        public bool CloseHandle() {
            if (ProcessOpen && CloseHandle(processHandle)) {
                ProcessOpen = false;
                return true;
            }
            return false;
        }

        public bool ReadProcessMemory(IntPtr lpBaseAddress, byte[] lpBuffer, out IntPtr lpNumberOfBytesWritten) {
            return ReadProcessMemory(processHandle, lpBaseAddress, lpBuffer, lpBuffer.Length, out lpNumberOfBytesWritten);
        }

        public bool WriteProcessMemory(IntPtr lpBaseAddress, byte[] lpBuffer, out IntPtr lpNumberOfBytesWritten) {
            return WriteProcessMemory(processHandle, lpBaseAddress, lpBuffer, lpBuffer.Length, out lpNumberOfBytesWritten);
        }

        //From JaredPar at http://stackoverflow.com/a/321404 (http://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array)
        public static byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public IntPtr FindObject(byte[] searchBytes, int byteAlignment = 4) {
            byte[] buff = new byte[sysInfo.AllocationGranularity];
            IntPtr bytesRead;
            //TODO maybe i should be long or uint with UIntPtrs
            int i = 0,
                j = 0; //j may be kept in-between i increments if we begin finding results at the end of our bytesRead - though in practice this should never happen
            for (IntPtr PTR = IntPtr.Zero; (long)PTR < (long)sysInfo.MaximumApplicationAddress; PTR += buff.Length) {
                if (ReadProcessMemory(PTR, buff, out bytesRead)) {
                    for (i = 0; i < (int)bytesRead; i += byteAlignment) {
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
            if (!ReadProcessMemory((IntPtr)addr, buff, out lpNumberOfBytesRead))
                return null;
            return buff;
        }
        public bool ReadBool(IntPtr addr) {
            return BitConverter.ToBoolean(Read(addr, 1), 0);
        }
        public int ReadInt(IntPtr addr) {
            return BitConverter.ToInt32(Read(addr, 4), 0);
        }
        public float ReadFloat(IntPtr addr) {
            return BitConverter.ToSingle(Read(addr, 4), 0);
        }
        public double ReadDouble(IntPtr addr) {
            return BitConverter.ToDouble(Read(addr, 8), 0);
        }

        public bool Write(IntPtr addr, byte[] value) {
            IntPtr lpNumberOfBytesWritten;
            return WriteProcessMemory((IntPtr)addr, value, out lpNumberOfBytesWritten);
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
