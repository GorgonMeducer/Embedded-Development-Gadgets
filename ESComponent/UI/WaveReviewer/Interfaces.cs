using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ESnail.Component.UI.WaveViewer
{
    //! \brief interface IWave
    //! @{
    public interface IWave
    {
        
        //! \brief set/get wave Visible state
        Boolean Visible
        {
            set;
            get;
        }
        
        //! \brief set/get wave color
        System.Drawing.Color WaveColor
        {
            set;
            get;
        }

        //! \brief set/get wave width
        Int32 LineWidth
        {
            set;
            get;
        }
        
        //! \brief set/get wave selected state
        Boolean Selected
        {
            set;
            get;
        }
        
        //! \brief set/get wave Unit
        String Unit
        {
            set;
            get;
        }

        //! \brief set/get wave title
        String Title
        {
            set;
            get;
        }

        //! \brief set/get wave description
        String Description
        {
            set;
            get;
        }

        //! \brief set/get wave zoom factor
        Double Zoom
        {
            set;
            get;
        }

        //! \brief zero point position
        Double ZeroPoint
        {
            set;
            get;
        }

        //! \brief set/get gride state
        Boolean ShowGride
        {
            set;
            get;
        }
    }
    //! @}

}
