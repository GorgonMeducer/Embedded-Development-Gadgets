using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace ESnail.Utilities
{
    //! \name Battery Manager ID
    //! @{
    public struct SafeID  : IComparable
    {
        private String m_strID;
        private static StringComparer m_tComparator = StringComparer.Create(new System.Globalization.CultureInfo("en-us"), true);

        //! constructor
        public SafeID(System.String strID)
        {
            m_strID = formatID(strID);
        }

        public override Boolean Equals(object obj)
        {
            try
            {
                if (((SafeID)obj).m_strID == this.m_strID)
                {
                    return true;
                }
            }
            catch (Exception) { }

            return false;
        }

        public static implicit operator SafeID(String from)
        {
            return new SafeID(from);
        }

        public static implicit operator String(SafeID from)
        {
            return from.m_strID;
        }

        public static Boolean operator ==(SafeID ID_A, SafeID ID_B)
        {
            if (ID_A.m_strID == ID_B.m_strID)
            {
                return true;
            }

            return false;
        }

        public static Boolean operator !=(SafeID ID_A, SafeID ID_B)
        {
            if (ID_A.m_strID == ID_B.m_strID)
            {
                return false;
            }

            return true;
        }

        public override Int32 GetHashCode()
        {
            return m_strID.GetHashCode();
        }

        private static Int32 s_RandomSeed = 0;

        //! string formattor
        static public String formatID(String strID)
        {
            s_RandomSeed++;

            Random RandomID = new Random(s_RandomSeed);
            if (strID == null)
            {
                strID = System.DateTime.Now.ToBinary().ToString("X") + BitConverter.ToUInt64(BitConverter.GetBytes(RandomID.NextDouble()),0).ToString("X16");
                
                return strID;
            }
            //! remove leading and ending blank space
            strID = strID.Trim();

            if (strID == "")
            {
                strID = System.DateTime.Now.ToBinary().ToString("X") + BitConverter.ToUInt64(BitConverter.GetBytes(RandomID.NextDouble()), 0).ToString("X16");
                return strID;
            }

            

            //! upper
            strID = strID.ToUpper();

            //! format string : replace all nonLetter and nonDigit letters into underline
            Char[] cTemp = strID.ToCharArray();
            for (System.Int32 nIndex = 0; nIndex < strID.Length; nIndex++)
            {
                if (!Char.IsLetterOrDigit(cTemp[nIndex]))
                {
                    if (cTemp[nIndex] != '_')
                    {
                        strID = strID.Replace(cTemp[nIndex], '_');
                        cTemp = strID.ToCharArray();
                    }
                }
            }

            return strID;
        }

        public int CompareTo(object obj)
        {
            int tResult = m_tComparator.Compare(this.ToString(), obj.ToString());
            return tResult;
        }

        static public SafeID RandomID
        {
            get { return formatID(null); }
        }

        //! property for get string ID
        public String ID
        {
            get
            {
                return m_strID;
            }
            set
            {
                m_strID = formatID(value);
            }
        }
        //! @}

        public override String ToString()
        {
            return m_strID;
        }

    }

    //! \name interface ISafeID
    //! @{
    public interface ISafeID
    {
        //! property for get ID
        SafeID ID
        {
            get;
            set;
        }
    }
    //! @}

    public interface IAvailable
    {
        Boolean Available
        {
            get;
        }
    }

   

}
