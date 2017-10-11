using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace ESnail.Utilities.Win32API
{
    ///<summary>
    /// A C# wrapper class for hidpi.h/hidspi.h / hid.dll.
    ///</summary>
    public class HID
    {
        //
        //  From hidpi.h
        //  Typedef enum defines a set of integer constants for HidP_Report_Type
        //
        public const Int16 HidP_Input   = 0;
        public const Int16 HidP_Output  = 1;
        public const Int16 HidP_Feature = 2;


        [StructLayout(LayoutKind.Sequential)]
        public struct HIDD_ATTRIBUTES
        {
            public Int32 Size;
            public Int16 VendorID;
            public Int16 ProductID;
            public Int16 VersionNumber;
        }


        public struct HIDP_CAPS
        {
            public Int16 Usage;
            public Int16 UsagePage;
            public Int16 InputReportByteLength;
            public Int16 OutputReportByteLength;
            public Int16 FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public Int16[] Reserved;
            public Int16 NumberLinkCollectionNodes;
            public Int16 NumberInputButtonCaps;
            public Int16 NumberInputValueCaps;
            public Int16 NumberInputDataIndices;
            public Int16 NumberOutputButtonCaps;
            public Int16 NumberOutputValueCaps;
            public Int16 NumberOutputDataIndices;
            public Int16 NumberFeatureButtonCaps;
            public Int16 NumberFeatureValueCaps;
            public Int16 NumberFeatureDataIndices;
        }


        //  If IsRange is false, UsageMin is the Usage and UsageMax is unused.
        //  If IsStringRange is false, StringMin is the String index and StringMax is unused.
        //  If IsDesignatorRange is false, DesignatorMin is the designator index and DesignatorMax is unused.
        /*
        internal struct HidP_Value_Caps
        {
            internal Int16 UsagePage;
            internal Byte  ReportID;
            internal Int32 IsAlias;
            internal Int16 BitField;
            internal Int16 LinkCollection;
            internal Int16 LinkUsage;
            internal Int16 LinkUsagePage;
            internal Int32 IsRange;
            internal Int32 IsStringRange;
            internal Int32 IsDesignatorRange;
            internal Int32 IsAbsolute;
            internal Int32 HasNull;
            internal Byte  Reserved;
            internal Int16 BitSize;
            internal Int16 ReportCount;
            internal Int16 Reserved2;
            internal Int16 Reserved3;
            internal Int16 Reserved4;
            internal Int16 Reserved5;
            internal Int16 Reserved6;
            internal Int32 LogicalMin;
            internal Int32 LogicalMax;
            internal Int32 PhysicalMin;
            internal Int32 PhysicalMax;
            internal Int16 UsageMin;
            internal Int16 UsageMax;
            internal Int16 StringMin;
            internal Int16 StringMax;
            internal Int16 DesignatorMin;
            internal Int16 DesignatorMax;
            internal Int16 DataIndexMin;
            internal Int16 DataIndexMax;
        }
        */


        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Boolean HidD_FlushQueue(SafeFileHandle HidDeviceObject);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Boolean HidD_FreePreparsedData(IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Boolean HidD_GetAttributes(SafeFileHandle HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Boolean HidD_GetFeature(SafeFileHandle HidDeviceObject, Byte[] lpReportBuffer, Int32 ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Boolean HidD_GetInputReport(SafeFileHandle HidDeviceObject, Byte[] lpReportBuffer, Int32 ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern void HidD_GetHidGuid(ref System.Guid HidGuid);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Boolean HidD_GetNumInputBuffers(SafeFileHandle HidDeviceObject, ref Int32 NumberBuffers);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Boolean HidD_GetPreparsedData(SafeFileHandle HidDeviceObject, ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Boolean HidD_SetFeature(SafeFileHandle HidDeviceObject, Byte[] lpReportBuffer, Int32 ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Boolean HidD_SetNumInputBuffers(SafeFileHandle HidDeviceObject, Int32 NumberBuffers);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Boolean HidD_SetOutputReport(SafeFileHandle HidDeviceObject, Byte[] lpReportBuffer, Int32 ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Int32 HidP_GetCaps(IntPtr PreparsedData, ref HIDP_CAPS Capabilities);

        [DllImport("hid.dll", SetLastError = true)]
        public static unsafe extern Int32 HidP_GetValueCaps(Int32 ReportType, Byte[] ValueCaps, ref Int32 ValueCapsLength, IntPtr PreparsedData);

    }// class HID

}// namespace Win32API
