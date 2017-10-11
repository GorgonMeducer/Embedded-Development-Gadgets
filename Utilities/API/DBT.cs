using System;
using System.Runtime.InteropServices;

namespace ESnail.Utilities.Win32API
{
    ///<summary>
    /// A C# wrapper class for dbt.h.
    ///</summary>
    public static class DBT
    {
        //
        // BroadcastSpecialMessage constants.
        //
        public const Int32 WM_DEVICECHANGE = 0x219;

        //
        // The following messages are for WM_DEVICECHANGE. The immediate list
        // is for the wParam. ALL THESE MESSAGES PASS A POINTER TO A STRUCT
        // STARTING WITH A DWORD SIZE AND HAVING NO POINTER IN THE STRUCT.
        //
        public const Int32 DBT_DEVICEARRIVAL           = 0x8000;  // system detected a new device
        public const Int32 DBT_DEVICEQUERYREMOVE       = 0x8001;  // wants to remove, may fail
        public const Int32 DBT_DEVICEQUERYREMOVEFAILED = 0x8002;  // removal aborted
        public const Int32 DBT_DEVICEREMOVEPENDING     = 0x8003;  // about to remove, still avail.
        public const Int32 DBT_DEVICEREMOVECOMPLETE    = 0x8004;  // device is gone
        public const Int32 DBT_DEVICETYPESPECIFIC      = 0x8005;  // type specific event
        // if WINVER >= 0x040A
        public const Int32 DBT_CUSTOMEVENT             = 0x8006;  // user-defined event

        public const Int32 DBT_DEVTYP_OEM              = 0x00000000;  // oem-defined device type
        public const Int32 DBT_DEVTYP_DEVNODE          = 0x00000001;  // devnode number
        public const Int32 DBT_DEVTYP_VOLUME           = 0x00000002;  // logical volume
        public const Int32 DBT_DEVTYP_PORT             = 0x00000003;  // serial, parallel
        public const Int32 DBT_DEVTYP_NET              = 0x00000004;  // network resource
        // if WINVER >= 0x040A
        public const Int32 DBT_DEVTYP_DEVICEINTERFACE  = 0x00000005;  // device interface class
        public const Int32 DBT_DEVTYP_HANDLE           = 0x00000006;  // file system handle


        //
        // Two declarations for the DEV_BROADCAST_DEVICEINTERFACE structure.
        //

        // 1)
        // Use this one in the call to RegisterDeviceNotification() and
        // in checking dbch_devicetype in a DEV_BROADCAST_HDR structure:
        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_DEVICEINTERFACE
        {
            public Int32 dbcc_size;
            public Int32 dbcc_devicetype;
            public Int32 dbcc_reserved;
            public Guid  dbcc_classguid;
            public Int16 dbcc_name;
        }

        // 2)
        // Use this to read the dbcc_name String and classguid:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class DEV_BROADCAST_DEVICEINTERFACE_1
        {
            public Int32  dbcc_size;
            public Int32  dbcc_devicetype;
            public Int32  dbcc_reserved;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
            public Byte[] dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
            public Char[] dbcc_name;
        }


        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_HDR
        {
            public Int32 dbch_size;
            public Int32 dbch_devicetype;
            public Int32 dbch_reserved;
        }

    }// class DBT

}// namespace Win32API
