using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NFS14DifficultyTool {
    class OtherProgram {
        [Flags]
        public enum ProcessAccessFlags : uint {
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
        public static extern UIntPtr OpenProcess(
             ProcessAccessFlags processAccess,
             bool bInheritHandle,
             int processId
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(UIntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(
            UIntPtr hProcess,
            UIntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out UIntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(
            UIntPtr hProcess,
            UIntPtr lpBaseAddress,
            [Out, MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
            int dwSize,
            out UIntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(
            UIntPtr hProcess,
            UIntPtr lpBaseAddress,
            UIntPtr lpBuffer,
            int dwSize,
            out UIntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(
            UIntPtr hProcess,
            UIntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out UIntPtr lpNumberOfBytesWritten
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(
            UIntPtr hProcess,
            UIntPtr lpBaseAddress,
            UIntPtr lpBuffer,
            int nSize,
            out UIntPtr lpNumberOfBytesWritten
        );

        static void Main(string[] args) {
            Process[] procList = Process.GetProcessesByName("nfs14");
            if (procList.Length < 1)
                procList = Process.GetProcessesByName("nfs14_x86"); //TODO is this needed?
            if (procList.Length < 1)
                return;
            UIntPtr procPtr = OpenProcess(ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.VirtualMemoryWrite, true, procList[0].Id);

            long addr = 0;
            //addr = FindObject(procPtr, new byte[] { 0x07, 0x3C, 0x76, 0xE4, 0x86, 0x4A, 0xEC, 0x06, 0x54, 0x09, 0xEF, 0xF7, 0x7D, 0x57, 0x8B, 0x2C });
            addr = FindObject(procPtr, StringToByteArray("073C76E4864AEC065409EFF77D578B2C"));
        }

        //From JaredPar at http://stackoverflow.com/a/321404 (http://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array)
        public static byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        static long FindObject(UIntPtr p, byte[] searchBytes) {
            byte[] buff = new byte[1024 * 64];
            UIntPtr bytesRead;
            int i = 0,
                j = 0; //j may be kept in-between i increments if we begin finding results at the end of our bytesRead
            for (long PTR = 0x0000000000000000; PTR < 0x7FFFFFFFFFFFFFFF; PTR += buff.Length) {
                if (ReadProcessMemory(p, (UIntPtr)PTR, buff, buff.Length, out bytesRead)) {
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
    }
}
