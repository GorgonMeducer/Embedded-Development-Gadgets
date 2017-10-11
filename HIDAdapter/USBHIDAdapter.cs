using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Device;
using ESnail.Device.Adapters.USB;
using ESnail.Utilities;

namespace ESnail.Device.Adapters.USB.HID
{
    //! abstract class for all hid adapter
    public abstract class USBHIDAdapter : SingleEndPointUSBDeviceAdapter
    {
        //! constructor
        public USBHIDAdapter(SafeID tID)
            : base(tID, new ESnailHIDDriver())
        {             
            
        }

        //! find HID devices
        public static String[] FindHIDDevice(UInt16 VenderID,UInt16 ProductID)
        {
            ESnailHIDDriver Driver = new ESnailHIDDriver();

            return Driver.FindDevice(Driver.DeviceType(),VenderID,ProductID);
        }

        //! adapter type
        public override String Type
        {
            get
            {
                return "USB HID-Compliant Adapter";
            }
        }
    }
}
