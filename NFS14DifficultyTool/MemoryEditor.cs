using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Memedit
{
    public class MemoryEditor
    {
        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("Kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UInt32 nSize, ref UInt32 lpNumberOfBytesRead);

        string pname = "";
        IntPtr hand;
        public MemoryEditor(string ProcName) 
        {
            pname = ProcName.Replace(".exe", "");
            Process[] proclist = Process.GetProcesses();
            foreach (Process pr in proclist)
            {
                if (pr.ToString() == "System.Diagnostics.Process (" + pname + ")")
                {
                    hand = pr.Handle;
                }
            }
        }

        public bool Write(int Address, byte[] data)
        {
            bool success = false;
            Process[] proclist = Process.GetProcesses();
            IntPtr bytesout;
            success = WriteProcessMemory(hand, (IntPtr)Address, data, (UIntPtr)data.Length, out bytesout);
            return success;
        }

        public byte[] Read(int Address, int length)
        {
            byte[] ret = new byte[length];
            uint o = 0;
            ReadProcessMemory(hand, (IntPtr)Address, ret, (UInt32)ret.Length, ref o);
            return ret;
        }
        public int ReadInt32(int Address)
        {
            return BitConverter.ToInt32(Read(Address, 4), 0);
        }
        public float ReadSingle(int Address)
        {
            return BitConverter.ToSingle(Read(Address, 4), 0);
        }
        public string ReadString(int Address, int length, bool isUnicode)
        {
            if (isUnicode)
            {
                UnicodeEncoding enc = new UnicodeEncoding();
                return enc.GetString(Read(Address, length));
            }
            else
            {
                ASCIIEncoding enc = new ASCIIEncoding();
                return enc.GetString(Read(Address, length));
            }
        }
    }
}