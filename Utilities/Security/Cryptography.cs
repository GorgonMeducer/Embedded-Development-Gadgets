using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace ESnail.Utilities.Security.Cryptography
{
    static public class AESHelper
    {       
        static public void GenerateRandomSTable(out Byte[] tSTable, out Byte[] tInverseSTable)
        {
            Random tRandom = new Random(DateTime.Now.GetHashCode());
            Boolean[] tFlagArray = new Boolean[256];

            tSTable = new Byte[256];
            tInverseSTable = new Byte[256];
            Byte tIndex = 0;
            for (Int32 n = 0; n < 256; n++)
            {
                Byte tValue = (Byte)tRandom.Next(255);

                do
                {
                    while (tFlagArray[++tIndex]);
                }
                while (tValue-- > 0);

                tSTable[tIndex] = (Byte)n;
                tInverseSTable[n] = tIndex;

                tFlagArray[tIndex] = true;
            }
        }

        static public void GeneralSTable(Byte[] tSerialNumber, out Byte[] tSTable, out Byte[] tInverseSTable)
        {
            do
            {
                if (null != tSerialNumber)
                {
                    if (0 != tSerialNumber.Length)
                    {
                        break;
                    }
                }
                GenerateRandomSTable(out tSTable, out tInverseSTable);
                return;
            } while (false);

            Boolean[] tFlagArray = new Boolean[256];
            tSTable = new Byte[256];
            tInverseSTable = new Byte[256];
            Byte tIndex = 0;
            UInt32 tSNIndex = 0;
            for (Int32 n = 0; n < 256; n++)
            {
                Byte tValue = tSerialNumber[tSNIndex++];
                tSNIndex %= (UInt32)tSerialNumber.Length;

                do
                {
                    while (tFlagArray[++tIndex]);
                }
                while (tValue-- > 0);

                tSTable[tIndex] = (Byte)n;
                tInverseSTable[n] = tIndex;

                tFlagArray[tIndex] = true;
            }
        }

    }
}
