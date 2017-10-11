using System;
using System.Collections.Generic;
using System.Text;

namespace ESnail.Utilities.DEC
{
    public static class DECBuilder
    {
        //! get word array from a dec string
        public static System.Boolean DECStringToWord
            (
                System.String strHex,
                ref System.UInt16 hwResult,
                System.Boolean bStrictCheck
            )
        {
            UInt32 n = 0;
            UInt32 wResult = 0;

            Int32 Sign = 1;

            if (null == strHex)
            {
                return false;
            }

            strHex = strHex.Trim();
            strHex = strHex.ToUpper();


            wResult = 0;
            if (strHex == "")
            {
                return false;
            }

            if (strHex.StartsWith("-"))
            {
                Sign = -1;
                strHex = strHex.Remove(0,1);
            }
            else if (strHex.StartsWith("+"))
            {
                //Sign = 1;
                strHex = strHex.Remove(0,1);
            }

            while (strHex != "")
            {
                System.Byte chTemp = 0;
                if (strHex.StartsWith("0"))
                {
                    chTemp = 0;
                }
                else if (strHex.StartsWith("1"))
                {
                    chTemp = 1;
                }
                else if (strHex.StartsWith("2"))
                {
                    chTemp = 2;
                }
                else if (strHex.StartsWith("3"))
                {
                    chTemp = 3;
                }
                else if (strHex.StartsWith("4"))
                {
                    chTemp = 4;
                }
                else if (strHex.StartsWith("5"))
                {
                    chTemp = 5;
                }
                else if (strHex.StartsWith("6"))
                {
                    chTemp = 6;
                }
                else if (strHex.StartsWith("7"))
                {
                    chTemp = 7;
                }
                else if (strHex.StartsWith("8"))
                {
                    chTemp = 8;
                }
                else if (strHex.StartsWith("9"))
                {
                    chTemp = 9;
                }
                else
                {
                    if (bStrictCheck)
                    {
                        return false;
                    }
                    else if (n > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                wResult *= 10;
                wResult += chTemp;

                n++;
                
                strHex = strHex.Remove(0, 1);
            }

            wResult = (UInt32)((Int32)wResult * Sign);

            if (Math.Abs((Int32)wResult) > 65535)
            {
                return false;
            }
            
            hwResult = (UInt16)(wResult & 0xFFFF);

            return true;
        }

        //! get word array from a dec string
        public static System.Boolean DECStringToWord(System.String strHex, ref System.UInt16 hwResult)
        {
            return DECStringToWord(strHex, ref hwResult, true);
        }
    }
}
