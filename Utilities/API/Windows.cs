using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;


namespace ESnail.Utilities.Win32API
{
    public static class Windows
    {
        public static WinBase.OSVersionInfo Version
        {
            get
            {
                do
                {
                    try
                    {
                        WinBase.OSVersionInfo tOSVersion = new WinBase.OSVersionInfo();

                        tOSVersion.OSVersionInfoSize = Marshal.SizeOf(tOSVersion);

                        WinBase.GetVersionEx(tOSVersion);

                        return tOSVersion;
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
                while(false);

                return null;
            }
        }
    }
}