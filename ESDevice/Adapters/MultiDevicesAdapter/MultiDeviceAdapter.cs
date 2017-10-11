using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Utilities;

namespace ESnail.Device.Adapters
{
    public abstract class MultiDeviceAdapter : Adapter
    {

        protected IDevice[] m_Devices = null;

        //! constructor 
        public MultiDeviceAdapter(SafeID tID, IDevice[] DeviceInterfaces)
            : base(tID)            
        {
            m_Devices = DeviceInterfaces;
        }

        //! get adapter type
        public override string Type
        {
            get { return "Multiple Devices Adapter"; }
        }

        //! open
        public override bool Open
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
