using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ESnail.Utilities.Windows
{
    public delegate void WindowsMessageProcessor(Message m);

    //! \name windows message handler
    //! @{
    public class WindowsMessageHandler
    {
        public event WindowsMessageProcessor WindowsMessageArrived;

        public void OnWindowsMessageArrive(Message m)
        {
            if (null != WindowsMessageArrived)
            {
                try
                {
                    WindowsMessageArrived.Invoke(m);
                }
                catch (Exception) { }
            }
        }
    }
    //! @}


}
