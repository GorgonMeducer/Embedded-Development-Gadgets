using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ESnail.Utilities.Threading
{
    public class CultureEnsure : ESDisposableClass
    {
        private String m_strOriginalCulture = null;
        private String m_strTargetCulture = null;

        public CultureEnsure(String tTargetCulture)
        {
            m_strOriginalCulture = Thread.CurrentThread.CurrentCulture.Name;

            if (null == tTargetCulture)
            {
                return;
            }
            if ("" == tTargetCulture.Trim())
            {
                return;
            }
            m_strTargetCulture = tTargetCulture;

            if (m_strOriginalCulture != m_strTargetCulture)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(m_strTargetCulture);
                }
                catch (Exception Err)
                {
                    try
                    {
                        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(m_strOriginalCulture);
                    }
                    catch (Exception) { }
                    throw Err;
                }
            }
        }

        protected override void _Dispose()
        {
            if (m_strOriginalCulture != m_strTargetCulture)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(m_strOriginalCulture);
                }
                catch (Exception Err)
                {
                    throw Err;
                }
                
            }
        }
    }
}
