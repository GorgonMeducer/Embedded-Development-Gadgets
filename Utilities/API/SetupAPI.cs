using System;
using System.Runtime.InteropServices;

namespace ESnail.Utilities.Win32API
{
    ///<summary>
    /// A C# wrapper class for setupapi.h / setupapi.dll.
    ///</summary>
    public static class SetupAPI
    {
        /* flags for SetupDiGetClassDevs */
        public const Int32 DIGCF_DEFAULT         = 0x00000001;
        public const Int32 DIGCF_PRESENT         = 0x00000002;
        public const Int32 DIGCF_ALLCLASSES      = 0x00000004;
        public const Int32 DIGCF_PROFILE         = 0x00000008;
        public const Int32 DIGCF_DEVICEINTERFACE = 0x00000010;


        /*
        public struct SP_DEVINFO_DATA
        {
            public Int32 cbSize;
            public Guid ClassGuid;
            public Int32 DevInst;
            public Int32 Reserved;
        }
        */
        
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public Int32 cbSize;
            public Guid InterfaceClassGuid;
            public Int32 Flags;
            public IntPtr Reserved;
        }

        /*
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public Int32 cbSize;
            public String DevicePath;
        }
        */


        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern Int32 SetupDiCreateDeviceInfoList(ref System.Guid ClassGuid, Int32 hwndParent);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern Int32 SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, ref System.Guid InterfaceClassGuid, Int32 MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SetupDiGetClassDevs(ref System.Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, Int32 Flags);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, Int32 DeviceInterfaceDetailDataSize, ref Int32 RequiredSize, IntPtr DeviceInfoData);

    }// class SetupAPI

}// namespace Win32API
