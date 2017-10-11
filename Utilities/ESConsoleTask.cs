using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESnail.Utilities.IO;

namespace ESnail.Utilities
{
    public abstract class ESConsoleTask : ESStatusReporter, ISafeID
    {
        protected String m_WorkingPath = Directory.GetCurrentDirectory();             //!< using current directory as default

        protected virtual String Seperators
        {
            get { return " ="; }
        }

        public ESConsoleTask()
        {
        }

        public abstract String HelpInfo
        {
            get;
        }

        public String WorkingPath
        {
            get { return m_WorkingPath; }
        }

        public Boolean UpdateCommandlineOptions(String[] args)
        {
            if (null == args)
            {
                ReportStatus( WorkingStatus.COMPLETE, HelpInfo);
                return false;
            }
            else if (0 == args.Length)
            {
                ReportStatus( WorkingStatus.COMPLETE, HelpInfo);
                return false;
            }

            args = PathEx.Separate(PathEx.CombineEx(' ', args), this.Seperators);

            for (UInt32 tIndex = 0; tIndex < args.Length; tIndex++)
            {
                if (!UpdateOption(args, ref tIndex))
                {
                    return false;
                }
            }

            return true;
        }

        #region Helper functions for get option value

        protected Boolean GetOptionValue(String[] args, ref UInt32 tIndex, ref String tResult, String tErrorInfo)
        {
            if (tIndex + 1 >= args.Length)
            {
                ReportError(tErrorInfo);
                return false;
            }
            tResult = args[tIndex + 1].Trim();
            tIndex++;

            return true;
        }

        protected Boolean GetOptionValue(String[] args, ref UInt32 tIndex, ref UInt32 tResult, String tErrorInfo)
        {
            if (tIndex + 1 >= args.Length)
            {
                ReportError(tErrorInfo);
                return false;
            }

            if (!UInt32.TryParse(args[tIndex + 1].Trim(), out tResult))
            {
                ReportError("Illegal unsigned integer value. \r\n" + tErrorInfo);
                return false;
            }

            
            tIndex++;

            return true;
        }


        protected Boolean GetOptionValue(String[] args, ref UInt32 tIndex, ref Int32 tResult, String tErrorInfo)
        {
            if (tIndex + 1 >= args.Length)
            {
                ReportError(tErrorInfo);
                return false;
            }

            if (!Int32.TryParse(args[tIndex + 1].Trim(), out tResult))
            {
                ReportError("Illegal signed integer value. \r\n" + tErrorInfo);
                return false;
            }


            tIndex++;

            return true;
        }

        protected Boolean GetOptionValue(String[] args, ref UInt32 tIndex, ref Double tResult, String tErrorInfo)
        {
            if (tIndex + 1 >= args.Length)
            {
                ReportError(tErrorInfo);
                return false;
            }

            if (!Double.TryParse(args[tIndex + 1].Trim(), out tResult))
            {
                ReportError("Illegal float point value. \r\n" + tErrorInfo);
                return false;
            }


            tIndex++;

            return true;
        }

        protected Boolean GetOptionHEXValue(String[] args, ref UInt32 tIndex, ref UInt64 tResult, String tErrorInfo)
        {
            if (tIndex + 1 >= args.Length)
            {
                ReportError(tErrorInfo);
                return false;
            }

            UInt64[] tLongHex = null;

            if (!HEX.HEXBuilder.HEXStringToU64Array(args[tIndex + 1].Trim(), ref tLongHex))
            {
                ReportError("Illegal HEX value. \r\n" + tErrorInfo);
                return false;
            }

            tResult = tLongHex[0];
            tIndex++;

            return true;
        }


        protected Boolean GetOptionExistedFile(String[] args, ref UInt32 tIndex, ref String tResult, String tErrorInfo)
        {
            if (tIndex + 1 >= args.Length)
            {
                ReportError(tErrorInfo);
                return false;
            }

            tResult = args[tIndex + 1].Trim();

            if (!File.Exists(tResult))
            {
                ReportError("Could not find target file: "+ tResult + ". \r\n" + tErrorInfo);
                return false;
            }
            
            
            tIndex++;

            return true;
        }

        protected Boolean GetOptionExistedDirctory(String[] args, ref UInt32 tIndex, ref String tResult, String tErrorInfo)
        {
            if (tIndex + 1 >= args.Length)
            {
                ReportError(tErrorInfo);
                return false;
            }

            tResult = args[tIndex + 1].Trim();

            if (!Directory.Exists(tResult))
            {
                ReportError("Could not find target folder: " + tResult + ". \r\n" + tErrorInfo);
                return false;
            }

            tIndex++;

            return true;
        }

        #endregion

        public abstract SafeID ID { get; set; }

        protected abstract Boolean UpdateOption(String[] args, ref UInt32 tIndex);
    }
}
