using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Utilities.Threading;
using ESnail.Utilities.Generic;
using ESnail.Utilities.IO;
using ESnail.Utilities;
using System.IO;
using System.Threading;
using ESnail.Utilities.Log;
using System.Reflection;

namespace ESnail.Utilities.Test
{


    public partial class ReportReaderEngine : ReportEngine
    {
       
        public Boolean RequestGenerateReport(String[] tFiles, Type tServiceType)
        {
            m_tComplete.Reset();
            m_ReportList.Clear();
            do
            {
                if (null == tFiles)
                {
                    m_tComplete.Set();
                    return false;
                }
                else if (0 == tFiles.Length)
                {
                    m_tComplete.Set();
                    break;
                }
                System.Reflection.ConstructorInfo tConstructor = tServiceType.GetConstructor(new Type[] { typeof(TestReport) });
                
                if (null == tConstructor)
                {
                    m_tComplete.Set();
                    break ;
                }

                

                do
                {
                    //! generate services


                    List<ReportReaderService> tServiceList = new List<ReportReaderService>();
                    foreach (String tFile in tFiles)
                    {
                        ReportReaderService tService = tConstructor.Invoke(new Object[] { new TestReport(tFile) }) as ReportReaderService;
                        if (null == tService)
                        {
                            continue;
                        }
                        if (!tService.Available)
                        {
                            continue;
                        }

                        tService.LogLevel = this.LogLevel;
                        tService.TimeStampEnabled = false;
                        if (LogLevel > LOG_LEVEL.NONE)
                        {
                            tService.EnableLog = true;
                            
                        }
                        if (LogLevel >= LOG_LEVEL.PROGRESS)
                        {
                            tService.TimeStampEnabled = true;
                        }

                        tServiceList.Add(tService);
                        m_ReportList.Add(tService.Target);
                    }

                    m_Engine.AddServices(tServiceList.ToArray());

                } while (false);


            }
            while (false);

            
            return true;
        }

        

        #region Internal Engine and Servies



        public abstract partial class ReportReaderService : ReportService
        {

            public ReportReaderService(TestReport tItem)
                : base(tItem)
            {
                
            }

            public ReportReaderService(TestReport tItem, Int32 SafeTimeOutSetting)
                : base(tItem, SafeTimeOutSetting)
            {
                
            }

            protected abstract void DataSeeker(StreamReader tReadStream, TestReport tReport); 

            public override Boolean DoService()
            {
                __BeginLog();
                do
                {
                    __WriteLogLine("Start reading test output...");
                    if (!Target.Available)
                    {
                        __WriteLogLine("Target File is not available");
                        break;
                    }


                    try
                    {
                        if (LogLevel >= LOG_LEVEL.PROGRESS)
                        {
                            __WriteLog("Try to read file ");
                            __WriteLog(Target.Source);
                            __WriteLog(" ...\r\n");
                        }
                        using (StreamReader tReadStream = new StreamReader(Target.Source))
                        {
                            DataSeeker(tReadStream, Target);
                        }

                        if (LogLevel >= LOG_LEVEL.PROGRESS)
                        {
                            __WriteLogLine(Target.Nodes.Length.ToString() + " nodes are found");
                        }

                        if (LogLevel >= LOG_LEVEL.DETAILS)
                        {
                            foreach (TestNode tNode in Target.Nodes)
                            {
                                __WriteLogLine(tNode.ToString());
                            }
                        }
                            
                    }
                    catch (Exception Err)
                    {
                        Target.ReportError(Err.ToString());
                    }
                    finally
                    {

                    }
                } while (false);

                __WriteLogLine("Reading service is stopped.");
                __EndLog();

                return false;
            }

            protected override void _Dispose()
            {
                
            }
        }
        #endregion
    }



}
