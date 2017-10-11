using System;
using System.Collections.Generic;
using System.Text;

//! A common device interface for battery studio.
namespace ESnail.Device
{
    //! /name ESnail.Device Common Interface
    //! @{
    public interface IDevice
    {
        //! Get a brief information string about this device
        String DeviceInfo();

        //! Get a version string about this device
        String Version();

        //! Get a device type information
        String DeviceType();

        //! Registe a device notification
        Boolean RegisterDeviceNotification(IntPtr hwndHandle);

        //! Unregiste a device notification
        Boolean UnregisterDeviceNotification();

        //! Get Error Information
        String GetErrorInfo();

        //! is my device
        Boolean IsMyDevice(IntPtr pMessage);

        //! Find all avaiable device
        String[] FindDevice(String DeviceType,params Object[] Parameter);

        //! Open device with device path name
        Boolean OpenDevice(String strDevicePathName);

        //! Close device with device path name
        Boolean CloseDevice();

        //! property 
        Boolean isOpen
        {
            get;
        }

        //! write data to device
        Boolean WriteDevice(Byte[] Data);

        //! read data from device
        Boolean ReadDevice(ref Byte[] Data);
    }
    //! @{
}
