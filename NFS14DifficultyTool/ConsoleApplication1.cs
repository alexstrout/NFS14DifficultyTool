using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ConsoleApplication1 {
    class Program {
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
        public static extern IntPtr OpenProcess(
             ProcessAccessFlags processAccess,
             bool bInheritHandle,
             int processId
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out, MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            IntPtr lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out IntPtr lpNumberOfBytesWritten
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            IntPtr lpBuffer,
            int nSize,
            out IntPtr lpNumberOfBytesWritten
        );

        static void Main(string[] args) {
            Process[] procs = Process.GetProcessesByName("nfs14");
            if (procs.Length <= 0)  //proces not found
                return; //can replace with exit nag(message)+exit;
            IntPtr p = OpenProcess(ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.VirtualMemoryWrite, true, procs[0].Id);

            byte[] searchBytes = { 0x07, 0x3C, 0x76, 0xE4, 0x86, 0x4A, 0xEC, 0x06, 0x54, 0x09, 0xEF, 0xF7, 0x7D, 0x57, 0x8B, 0x2C };
            byte[] buff = new byte[1024 * 64];
            IntPtr bytesRead;

            //0x949DBC10
            //0x0000000000000000
            long addr = 0;
            for (long PTR = 0x0000000000000000; PTR < 0x7FFFFFFFFFFFFFFF; PTR += buff.Length) {
                if (ReadProcessMemory(p, (IntPtr)PTR, buff, buff.Length, out bytesRead)) {
                    for (int i = 0; i < (int)bytesRead - searchBytes.Length; i++) {
                        bool found = true;
                        for (int j = 0; j < searchBytes.Length; j++) {
                            if (buff[i + j] != searchBytes[j]) {
                                found = false;
                                break;
                            }
                        }
                        if (found) {
                            addr = PTR + i;
                            break;
                        }
                    }
                    if (addr != 0) {
                        //addrList.Add(addr);
                        //addr = 0;
                        break;
                    }
                }
            }
        }
    }
}
