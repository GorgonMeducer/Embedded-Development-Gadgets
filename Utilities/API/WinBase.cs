using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;


namespace ESnail.Utilities.Win32API
{
    /// <summary>
    /// A C# wrapper class for winBase.h (windows.h) / kernel32.dll.
    ///</summary>
    public static class WinBase
    {
        //
        // File creation flags must start at the high end since they
        // are combined with the attributes
        //
        //public const UInt32 FILE_FLAG_WRITE_THROUGH      = 0x80000000;
        public const Int32 FILE_FLAG_OVERLAPPED         = 0x40000000;
        public const Int32 FILE_FLAG_NO_BUFFERING       = 0x20000000;
        public const Int32 FILE_FLAG_RANDOM_ACCESS      = 0x10000000;
        public const Int32 FILE_FLAG_SEQUENTIAL_SCAN    = 0x08000000;
        public const Int32 FILE_FLAG_DELETE_ON_CLOSE    = 0x04000000;
        public const Int32 FILE_FLAG_BACKUP_SEMANTICS   = 0x02000000;
        public const Int32 FILE_FLAG_POSIX_SEMANTICS    = 0x01000000;
        public const Int32 FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000;
        public const Int32 FILE_FLAG_OPEN_NO_RECALL     = 0x00100000;

        public const Int32 CREATE_NEW        = 1;
        public const Int32 CREATE_ALWAYS     = 2;
        public const Int32 OPEN_EXISTING     = 3;
        public const Int32 OPEN_ALWAYS       = 4;
        public const Int32 TRUNCATE_EXISTING = 5;

        public const Int32 WAIT_TIMEOUT         = 0x102;
        public const Int32 WAIT_OBJECT_0        = 0;
        public const Int32 INVALID_HANDLE_VALUE = -1;


        public const Int32  FILE_SHARE_READ  = 1;
        public const Int32  FILE_SHARE_WRITE = 2;
        public const UInt32 GENERIC_READ     = 0x80000000;
        public const UInt32 GENERIC_WRITE    = 0x40000000;


        [StructLayout(LayoutKind.Sequential)]
        public class SECURITY_ATTRIBUTES
        {
            public Int32 nLength;
            public Int32 lpSecurityDescriptor;
            public Int32 bInheritHandle;
        }

        //static private WinKernel m_Kernel = new WinKernel();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static unsafe extern Int32 CancelIo(SafeFileHandle hFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static unsafe extern IntPtr CreateEvent(IntPtr SecurityAttributes, Boolean bManualReset, Boolean bInitialState, String lpName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static unsafe extern Boolean CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static unsafe extern SafeFileHandle CreateFile(String lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode, IntPtr lpSecurityAttributes, Int32 dwCreationDisposition, Int32 dwFlagsAndAttributes, Int32 hTemplateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static unsafe extern Boolean GetOverlappedResult(SafeFileHandle hFile, IntPtr lpOverlapped, ref Int32 lpNumberOfBytesTransferred, Boolean bWait);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static unsafe extern Boolean ReadFile(SafeFileHandle hFile, IntPtr lpBuffer, Int32 nNumberOfBytesToRead, ref Int32 lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static unsafe extern Int32 WaitForSingleObject(IntPtr hHandle, Int32 dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static unsafe extern Boolean WriteFile(SafeFileHandle hFile, Byte[] lpBuffer, Int32 nNumberOfBytesToWrite, ref Int32 lpNumberOfBytesWritten, IntPtr lpOverlapped);




        [StructLayout(LayoutKind.Sequential)]
        public class OSVersionInfo
        {
            public int OSVersionInfoSize;
            public int MajorVersion;
            public int MinorVersion;
            public int BuildNumber;
            public int PlatformId;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public String versionString;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OSVersionInfo2
        {
            public int OSVersionInfoSize;
            public int MajorVersion;
            public int MinorVersion;
            public int BuildNumber;
            public int PlatformId;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public String versionString;
        }

        [DllImport("kernel32")]
        public static unsafe extern bool GetVersionEx([In, Out] OSVersionInfo osvi);

        [DllImport("kernel32", EntryPoint = "GetVersionEx")]
        public static unsafe extern bool GetVersionEx2(ref OSVersionInfo2 osvi);


    }// class WinBase



}// namespace Win32API
