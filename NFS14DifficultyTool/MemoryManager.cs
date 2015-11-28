using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;

namespace NFS14DifficultyTool {
    class MemoryManager {
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
        protected static extern UIntPtr OpenProcess(
             ProcessAccessFlags processAccess,
             bool bInheritHandle,
             int processId
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool CloseHandle(UIntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool ReadProcessMemory(
            UIntPtr hProcess,
            UIntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out UIntPtr lpNumberOfBytesRead
        );
        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool ReadProcessMemory(
            UIntPtr hProcess,
            UIntPtr lpBaseAddress,
            [Out, MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
            int dwSize,
            out UIntPtr lpNumberOfBytesRead
        );
        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool ReadProcessMemory(
            UIntPtr hProcess,
            UIntPtr lpBaseAddress,
            UIntPtr lpBuffer,
            int dwSize,
            out UIntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool WriteProcessMemory(
            UIntPtr hProcess,
            UIntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out UIntPtr lpNumberOfBytesWritten
        );
        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern bool WriteProcessMemory(
            UIntPtr hProcess,
            UIntPtr lpBaseAddress,
            UIntPtr lpBuffer,
            int nSize,
            out UIntPtr lpNumberOfBytesWritten
        );

        protected UIntPtr ProcessHandle;

        public MemoryManager(string processName) {
            OpenProcess(processName);
        }

        public bool OpenProcess(string processName) {
            Process[] procList = Process.GetProcessesByName(processName);
            if (procList.Length < 1)
                return false;

            UIntPtr p;
            p = OpenProcess(ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.VirtualMemoryWrite, true, procList[0].Id);
            if (p == default(UIntPtr))
                return false;
            ProcessHandle = p;
            return true;
        }

        public bool CloseHandle() {
            return CloseHandle(ProcessHandle);
        }

        public bool ReadProcessMemory(UIntPtr lpBaseAddress, byte[] lpBuffer, out UIntPtr lpNumberOfBytesWritten) {
            return ReadProcessMemory(ProcessHandle, lpBaseAddress, lpBuffer, lpBuffer.Length, out lpNumberOfBytesWritten);
        }

        public bool WriteProcessMemory(UIntPtr lpBaseAddress, byte[] lpBuffer, out UIntPtr lpNumberOfBytesWritten) {
            return WriteProcessMemory(ProcessHandle, lpBaseAddress, lpBuffer, lpBuffer.Length, out lpNumberOfBytesWritten);
        }

        //From JaredPar at http://stackoverflow.com/a/321404 (http://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array)
        public static byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public long FindObject(byte[] searchBytes) {
            byte[] buff = new byte[1024 * 64];
            UIntPtr bytesRead;
            int i = 0,
                j = 0; //j may be kept in-between i increments if we begin finding results at the end of our bytesRead
            for (long PTR = 0x0000000000000000; PTR < 0x7FFFFFFFFFFFFFFF; PTR += buff.Length) {
                if (ReadProcessMemory((UIntPtr)PTR, buff, out bytesRead)) {
                    for (i = 0; i < (int)bytesRead; i++) {
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
            return -1;
        }

        public byte[] Read(long addr, int size) {
            byte[] buff = new byte[size];
            UIntPtr lpNumberOfBytesRead;
            if (!ReadProcessMemory((UIntPtr)addr, buff, out lpNumberOfBytesRead))
                return null;
            byte[] ret = new byte[(int)lpNumberOfBytesRead];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = buff[i];
            return ret;
        }
        public bool ReadBool(long addr) {
            return BitConverter.ToBoolean(Read(addr, 1), 0);
        }
        public int ReadInt(long addr) {
            return BitConverter.ToInt32(Read(addr, 4), 0);
        }
        public float ReadFloat(long addr) {
            return BitConverter.ToSingle(Read(addr, 4), 0);
        }
        public double ReadDouble(long addr) {
            return BitConverter.ToDouble(Read(addr, 8), 0);
        }

        public bool Write(long addr, byte[] value) {
            UIntPtr lpNumberOfBytesWritten;
            return WriteProcessMemory((UIntPtr)addr, value, out lpNumberOfBytesWritten);
        }
        public bool WriteBool(long addr, bool value) {
            return Write(addr, BitConverter.GetBytes(value));
        }
        public bool WriteInt(long addr, int value) {
            return Write(addr, BitConverter.GetBytes(value));
        }
        public bool WriteFloat(long addr, float value) {
            return Write(addr, BitConverter.GetBytes(value));
        }
        public bool WriteDouble(long addr, double value) {
            return Write(addr, BitConverter.GetBytes(value));
        }

        //TODO TEST
        public MemoryManager() {
            if (!OpenProcess("nfs14"))
                OpenProcess("nfs14_x86"); //TODO is this needed?

            long addr = 0;
            //addr = FindObject(new byte[] { 0x07, 0x3C, 0x76, 0xE4, 0x86, 0x4A, 0xEC, 0x06, 0x54, 0x09, 0xEF, 0xF7, 0x7D, 0x57, 0x8B, 0x2C });
            addr = FindObject(StringToByteArray("073C76E4864AEC065409EFF77D578B2C"));

            bool testYay;
            testYay = ReadBool(addr + 100);

            CloseHandle();
        }
    }
}
