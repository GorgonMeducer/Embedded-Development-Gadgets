using System;


namespace HID
{
    public static class HidLibConstants
    {
        //
        // Error codes used by the HidLib
        //
        public const UInt16 RES_OK                                   = 0x0;
        public const UInt16 ERROR_NO_DEVICE                          = 0x1;
        public const UInt16 ERROR_INVALID_HANDLE                     = 0x2;
        public const UInt16 ERROR_INVALID_INPUTREPORT_LENGTH         = 0x3;
        public const UInt16 ERROR_FAILED_TO_FIND_DEVICE              = 0x4;
        public const UInt16 ERROR_FAILED_TO_GET_DEVICE_ATTRIBUTES    = 0x5;
        public const UInt16 ERROR_FAILED_TO_CREATE_DEVICE_FILEHANDLE = 0x6;
        public const UInt16 ERROR_FAILED_TO_READ_FROM_DEVICE         = 0x7;
        public const UInt16 ERROR_FAILED_TO_WRITE_TO_DEVICE          = 0x8;
        public const UInt16 ERROR_INVALID_BUFFERSIZE                 = 0x9;
        public const UInt16 ERROR_FAILED_TO_CLOSE_DEVICE             = 0xA;
        public const UInt16 RES_NO_DATA                              = 0xB;

    }// class HidLibConstants

}// namespace HidLib
