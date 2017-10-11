using System;
using System.Collections.Generic;
using System.Text;

namespace ESnail.Utilities
{
    static class MathEx
    {
        public static Double Sin(Double tDegree)
        {
            Double tAngle = System.Math.PI * tDegree / 180.0;

            return Math.Sin(tAngle);
        }

        public static Double Cos(Double tDegree)
        {
            Double tAngle = System.Math.PI * tDegree / 180.0;

            return Math.Cos(tAngle);
        }

        public static Double Tan(Double tDegree)
        {
            Double tAngle = System.Math.PI * tDegree / 180.0;

            return Math.Tan(tAngle);
        }


    }
}
