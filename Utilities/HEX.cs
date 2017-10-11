using System;
using System.Collections.Generic;
using System.Text;

namespace ESnail.Utilities.HEX
{
    


    static public class HEXBuilder
    {
        //! get word array from a hex string
        public static Boolean HEXStringToU64Array
            (
                System.String strHex,
                ref System.UInt64[] dwResult,
                System.Boolean bStrictCheck
            )
        {
            UInt32 n = 0;

            if (null == strHex)
            {
                return false;
            }

            if (null == dwResult)
            {
                dwResult = new UInt64[1];
            }

            strHex = strHex.Trim();
            strHex = strHex.ToUpper();

            if (strHex.StartsWith("0X"))
            {
                strHex = strHex.Remove(0, 2);
            }

            if (dwResult.Length < 1)
            {
                Array.Resize(ref dwResult, 1);
            }

            dwResult[0] = 0;
            if (strHex == "")
            {
                return false;
            }
            String tOriginalString = strHex;

            do
            {
                if (n < tOriginalString.Length)
                {
                    strHex = tOriginalString.Substring((Int32)n, 1);
                }
                else
                {
                    break;
                }

                if (0 == (n % (sizeof(UInt64) * 2)))
                {
                    if (dwResult.Length <= (n >> 4))
                    {
                        Array.Resize(ref dwResult, dwResult.Length + 1);
                    }
                }

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
                else if (strHex.StartsWith("A"))
                {
                    chTemp = 0x0A;
                }
                else if (strHex.StartsWith("B"))
                {
                    chTemp = 0x0B;
                }
                else if (strHex.StartsWith("C"))
                {
                    chTemp = 0x0C;
                }
                else if (strHex.StartsWith("D"))
                {
                    chTemp = 0x0D;
                }
                else if (strHex.StartsWith("E"))
                {
                    chTemp = 0x0E;
                }
                else if (strHex.StartsWith("F"))
                {
                    chTemp = 0x0F;
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
                dwResult[n >> 4] <<= 4;
                dwResult[n >> 4] |= chTemp;

                n++;

                //strHex = strHex.Remove(0, 1);
            } while (strHex != "");

            return true;
        }

        //! get word array from a hex string
        public static Boolean HEXStringToU64Array(System.String strHex, ref System.UInt64[] hwResult)
        {
            return HEXStringToU64Array(strHex, ref hwResult, true);
        }



        //! get word array from a hex string
        public static Boolean HEXStringToU32Array
            (
                System.String strHex,
                ref System.UInt32[] wResult,
                System.Boolean bStrictCheck
            )
        {
            UInt32 n = 0;

            if (null == strHex)
            {
                return false;
            }

            if (null == wResult)
            {
                wResult = new UInt32[1];
            }

            strHex = strHex.Trim();
            strHex = strHex.ToUpper();

            if (strHex.StartsWith("0X"))
            {
                strHex = strHex.Remove(0, 2);
            }

            if (wResult.Length < 1)
            {
                Array.Resize(ref wResult, 1);
            }

            wResult[0] = 0;
            if (strHex == "")
            {
                return false;
            }
            String tOriginalString = strHex;

            do
            {
                if (n < tOriginalString.Length)
                {
                    strHex = tOriginalString.Substring((Int32)n, 1);
                }
                else
                {
                    break;
                }

                if (0 == (n % (sizeof(UInt32) * 2)))
                {
                    if (wResult.Length <= (n >> 3))
                    {
                        Array.Resize(ref wResult, wResult.Length + 1);
                    }
                }

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
                else if (strHex.StartsWith("A"))
                {
                    chTemp = 0x0A;
                }
                else if (strHex.StartsWith("B"))
                {
                    chTemp = 0x0B;
                }
                else if (strHex.StartsWith("C"))
                {
                    chTemp = 0x0C;
                }
                else if (strHex.StartsWith("D"))
                {
                    chTemp = 0x0D;
                }
                else if (strHex.StartsWith("E"))
                {
                    chTemp = 0x0E;
                }
                else if (strHex.StartsWith("F"))
                {
                    chTemp = 0x0F;
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
                wResult[n >> 3] <<= 4;
                wResult[n >> 3] |= chTemp;

                n++;

                //strHex = strHex.Remove(0, 1);
            } while (strHex != "");

            return true;
        }

        //! get word array from a hex string
        public static Boolean HEXStringToU32Array(System.String strHex, ref System.UInt32[] hwResult)
        {
            return HEXStringToU32Array(strHex, ref hwResult, true);
        }


        //! get word array from a hex string
        public static Boolean HEXStringToU16Array
            (
                String strHex,
                ref System.UInt16[] hwResult,
                Boolean bStrictCheck
            )
        {
            UInt32 n = 0;
            Boolean bIfFindBlankSpace = true;
            if (null == strHex)
            {
                return false;
            }

            if (null == hwResult)
            {
                hwResult = new UInt16[1];
            }

            strHex = strHex.Trim();
            strHex = strHex.ToUpper();

            if (strHex.StartsWith("0X"))
            {
                strHex = strHex.Remove(0,2);
            }

            if (hwResult.Length < 1) 
            {
                Array.Resize(ref hwResult,1);
            }

            hwResult[0] = 0;
            if (strHex == "")
            {
                return false;
            }
            String tOriginalString = strHex;

            do
            {
                if (n < tOriginalString.Length)
                {
                    strHex = tOriginalString.Substring((Int32)n, 1);
                }
                else
                {
                    break;
                }

                if (0 == (n % (sizeof(UInt16) * 2)))
                {
                    if (hwResult.Length <= (n >> 2))
                    {
                        Array.Resize(ref hwResult, hwResult.Length + 1);
                    }
                }

                

                System.Byte chTemp = 0;
                if (strHex.StartsWith("0"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0;
                }
                else if (strHex.StartsWith("1"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 1;
                }
                else if (strHex.StartsWith("2"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 2;
                }
                else if (strHex.StartsWith("3"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 3;
                }
                else if (strHex.StartsWith("4"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 4;
                }
                else if (strHex.StartsWith("5"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 5;
                }
                else if (strHex.StartsWith("6"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 6;
                }
                else if (strHex.StartsWith("7"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 7;
                }
                else if (strHex.StartsWith("8"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 8;
                }
                else if (strHex.StartsWith("9"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 9;
                }
                else if (strHex.StartsWith("A"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0A;
                }
                else if (strHex.StartsWith("B"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0B;
                }
                else if (strHex.StartsWith("C"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0C;
                }
                else if (strHex.StartsWith("D"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0D;
                }
                else if (strHex.StartsWith("E"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0E;
                }
                else if (strHex.StartsWith("F"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0F;
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
                hwResult[n >> 2] <<= 4;
                hwResult[n >> 2] |= chTemp;

                n++;

                //strHex = strHex.Remove(0, 1);
            } while (strHex != "");

            return true;
        }

        //! get word array from a hex string
        public static Boolean HEXStringToU16Array(System.String strHex, ref System.UInt16[] hwResult)
        {
            return HEXStringToU16Array(strHex,ref hwResult,true);
        }

        //! get byte array from a hex string
        public static Boolean HEXStringToByteArray
            (
                System.String strHex,
                ref System.Byte[] cResult,
                System.Boolean bStrictCheck
            )
        {
            UInt32 n = 0;
            UInt32 wIndex = 0;
            Boolean bIfFindBlankSpace = true;
            List<Byte> tResultList = new List<Byte>();
            if (null == strHex)
            {
                return false;
            }

            tResultList.Add(new Byte());

            /*
            if (null == cResult)
            {
                cResult = new Byte[1];
            }
            */
            strHex = strHex.Trim();
            strHex = strHex.ToUpper();

            if (strHex.StartsWith("0X"))
            {
                strHex = strHex.Remove(0, 2);
            }
            /*
            if (cResult.Length < 1)
            {
                cResult = new Byte[1];
            }
            */
            
            if (strHex == "")
            {
                return false;
            }
            //cResult[0] = 0;
            String tOriginalString = strHex;
            UInt32 tCounter = 0;
            Boolean bFirstBlank = false;
            do
            {
                if (tCounter < tOriginalString.Length)
                {
                     strHex = tOriginalString.Substring((Int32)tCounter, 1);
                }
                else
                {
                    break;
                }

                Byte chTemp = 0;

                if (strHex.StartsWith("0"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0;
                }
                else if (strHex.StartsWith("1"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 1;
                }
                else if (strHex.StartsWith("2"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 2;
                }
                else if (strHex.StartsWith("3"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 3;
                }
                else if (strHex.StartsWith("4"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 4;
                }
                else if (strHex.StartsWith("5"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 5;
                }
                else if (strHex.StartsWith("6"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 6;
                }
                else if (strHex.StartsWith("7"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 7;
                }
                else if (strHex.StartsWith("8"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 8;
                }
                else if (strHex.StartsWith("9"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 9;
                }
                else if (strHex.StartsWith("A"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0A;
                }
                else if (strHex.StartsWith("B"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0B;
                }
                else if (strHex.StartsWith("C"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0C;
                }
                else if (strHex.StartsWith("D"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0D;
                }
                else if (strHex.StartsWith("E"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0E;
                }
                else if (strHex.StartsWith("F"))
                {
                    bIfFindBlankSpace = false;
                    chTemp = 0x0F;
                }
                else if (strHex.ToCharArray()[0] == 32)
                {
                    if (false == bIfFindBlankSpace)
                    {
                        bIfFindBlankSpace = true;

                        do
                        {
                            
                            tCounter++;
                            n++;
                            wIndex = n >> 1;

                            n = wIndex * 2;
                            if (tCounter < tOriginalString.Length)
                            {
                                strHex = tOriginalString.Substring((Int32)tCounter, 1);
                            }
                            else
                            {
                                break;
                            }
                        } while (strHex.ToCharArray()[0] == 32);
                    }
                    //strHex = strHex.Remove(0, 1);
                    continue;
                }
                else
                {
                    if (bStrictCheck)
                    {
                        return false;
                    }
                    else if (n > 0)
                    {
                        cResult = tResultList.ToArray();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (0 == (n % (sizeof(Byte) * 2)))
                {
                    if (tResultList.Count <= wIndex)
                    {
                        tResultList.Add(new Byte());
                        //Array.Resize(ref cResult, cResult.Length + 1);
                    }
                }

                tResultList[(Int32)wIndex] <<= 4;
                tResultList[(Int32)wIndex] |= chTemp;

                tCounter++;
                n++;
                wIndex = n >> 1;

            } while (strHex != "");

            cResult = tResultList.ToArray();

            return true;
        }

        //! get byte array from a hex string
        public static System.Boolean HEXStringToByteArray(System.String strHex, ref System.Byte[] cResult)
        {
            return HEXStringToByteArray(strHex, ref cResult, true);
        }

        //! get hex string from byte array
        public static String ByteArrayToHEXString(Byte[] Datas)
        {
            if (null == Datas)
            {
                return "";
            }

            if (0 == Datas.Length)
            {
                return "";
            }

            StringBuilder strbResult = new StringBuilder();

            foreach (Byte tByte in Datas) {
                strbResult.Append(tByte.ToString("X2"));
                strbResult.Append(" ");
            }

            return strbResult.ToString().Trim().ToUpper();
        }

        public static String ByteArrayToHEXString(Byte[] tDatas, DATA_SIZE tSize)
        {
            StringBuilder sbOutput = new StringBuilder();
            Int32 tDataSize = (1 << (Int32)tSize);
            Int32 tCount = tDatas.Length;
            Int32 tIndex = 0;
            do
            {
                if (null == tDatas)
                {
                    break;
                }
                else if (tDatas.Length < tDataSize)
                {
                    break;
                }

                do
                {
                    
                    switch (tSize)
                    {
                        default:
                        case DATA_SIZE.DATA_SIZE_BYTE:
                            sbOutput.Append(tDatas[tIndex].ToString("X2"));
                            break;
                        case DATA_SIZE.DATA_SIZE_HALF_WORD:
                            sbOutput.Append(BitConverter.ToUInt16(tDatas, tIndex).ToString("X4"));
                            break;
                        case DATA_SIZE.DATA_SIZE_WORD:
                            sbOutput.Append(BitConverter.ToUInt32(tDatas, tIndex).ToString("X8"));
                            break;
                        case DATA_SIZE.DATA_SIZE_DOUBLE_WORD:
                            sbOutput.Append(BitConverter.ToUInt64(tDatas, tIndex).ToString("X16"));
                            break;
                    }

                    sbOutput.Append(" ");

                    tIndex += tDataSize;
                    tCount -= tDataSize;
                }
                while (tCount > 0);
            }
            while (false);

            return sbOutput.ToString();
        }
    }
}
