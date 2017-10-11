using System;
using System.Runtime.InteropServices;

namespace ESnail.Utilities.Win32API
{
    ///<summary>
    /// A C# wrapper class for winuser.h / user32.dll.
    ///</summary>
    public class WinUser
    {
        public const Int32 VM_USER = 0x400;

        public const Int32 DEVICE_NOTIFY_WINDOW_HANDLE          = 0x00000000;
        public const Int32 DEVICE_NOTIFY_SERVICE_HANDLE         = 0x00000001;
        public const Int32 DEVICE_NOTIFY_ALL_INTERFACE_CLASSES  = 0x00000004;

        #region Message: MoseMove
        public const UInt16 WM_MOUSEMOVE                        = 0x0200;
        public const UInt16 MK_CONTROL                          = 0x0008;
        public const UInt16 MK_LBUTTOM                          = 0x0001;
        public const UInt16 MK_MBUTTON                          = 0x0010;
        public const UInt16 MK_RBUTTON                          = 0x0002;
        public const UInt16 MK_SHIFT                            = 0x0004;
        public const UInt16 MK_XBUTTON1                         = 0x0020;
        public const UInt16 MK_XBUTTON2                         = 0x0040;
        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, Int32 Flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean UnregisterDeviceNotification(IntPtr Handle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        //public static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    }// class WinUser

}// namespace Win32API
