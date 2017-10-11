using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Device;

namespace ESnail.Device.Adapters.USB.HID
{
    public abstract class ToolsBuilder : ToolBuilder
    {
        public override string ToolName
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

        public override bool RefreshTools()
        {
            throw new NotImplementedException();
        }
    }
}
