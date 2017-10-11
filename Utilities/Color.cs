using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ESnail.Utilities.Color
{
    static public class ColorBuilder
    {
        //! \brief method for get a complementary color of a specified color
        static public System.Drawing.Color GetComplementaryColor(System.Drawing.Color TargetColor)
        {
            return System.Drawing.Color.FromArgb(0xFF - TargetColor.R, 0xFF - TargetColor.G, 0xFF - TargetColor.B); 
        }
    }
}
