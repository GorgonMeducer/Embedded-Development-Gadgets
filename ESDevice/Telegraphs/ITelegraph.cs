using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Device;

namespace ESnail.Device.Telegraphs
{  

    //! \name Interface: IBMTelegraph
    public interface ISPTelegraph : ITelegraph
    {
        //! a method to send telegraph
        Boolean TryToSendTelegraph(SinglePhaseTelegraph telTarget);
    }
}
