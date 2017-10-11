using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ESnail.Utilities.IO;
using ESnail.Utilities.Log;

namespace ESnail.Utilities.Test
{

    

    public abstract partial class TestReportReaderTask : ESConsoleTask
    {
        private LOG_LEVEL m_LogLevel = LOG_LEVEL.PROGRESS;

        private String m_ReportFile = "TestReport.csv";

        private const String c_HelpInfo =
                "Options \r\n" +
                "    -f <folder path>\r\n" +
                "        Specify the working folder. The Current folder will be used as default.\r\n" +
                "    -o <File Name>\r\n" +
                "        Specify the outpuf file name. If it is ignored, \"TestReport.csv\" will be used as default.\r\n" +
                "\r\n";
        private String m_TargetFileExtensions = "*.log|*.txt";

        public TestReportReaderTask()
        {
            LogWriter.RegisterLogReceiver("ReportService", new LogMessage(LogMessageReceiver), false);
        }

        private void LogMessageReceiver(String tMessage)
        {
            Console.Write(tMessage);
        }

        public override String HelpInfo
        {
            get { return c_HelpInfo; }
        }

        

        protected override Boolean UpdateOption(string[] args, ref uint tIndex)
        {
            if (null == args)
            {
                ReportStatus(WorkingStatus.ERROR, c_HelpInfo);
                return false;
            }
            else if (tIndex >= args.Length)
            {
                throw new Exception("Illegal Parameter: The index is bigger than the lenght of array args[]");
                //return false;
            }

            String tOption = args[tIndex].Trim();

            switch (tOption)
            {
                case "-f":              //!< set input folder
                    do
                    {
                        String tPath = null;

                        if (!this.GetOptionExistedDirctory(args, ref tIndex, ref tPath,
                            "Please specify working folder using -f with following syntax:\r\n" +
                            "    -f <folder path>"))
                        {
                            return false;
                        }

                        m_WorkingPath = Path.GetFullPath(tPath);

                    } while (false);
                    break;

                case "-o":              //!< set output file
                    do
                    {
                        String tPath = null;

                        if (!this.GetOptionValue(args, ref tIndex, ref tPath,
                            "Please specify output file name using -o with following syntax:\r\n" +
                            "    -o <File Name>"))
                        {
                            return false;
                        }

                        m_ReportFile = tPath;
                        if (!Path.HasExtension(m_ReportFile))
                        {
                            m_ReportFile += ".csv";
                        }
                    } while (false);
                    break;

                case "-log":
                    do
                    {
                        String tLogLevel = null;
                        if (!this.GetOptionValue(args, ref tIndex, ref tLogLevel,
                                    "Please specify the depth of logging using -log with following syntax:\r\n" +
                                    "    -log <LogLevel>  or  -log=<LogLevel>\r\n" +
                                    "          LogLevel could be 0 ~ 3 or none, procedure, progress and details\r\n"))
                        {
                            break ;
                        }

                        switch (tLogLevel)
                        {
                            case "0":
                            case "none":
                                m_LogLevel = LOG_LEVEL.NONE;
                                break;
                            case "1":
                            case "procedure":
                                m_LogLevel = LOG_LEVEL.PROCEDURE;
                                break;
                            case "2":
                            case "progress":
                                m_LogLevel = LOG_LEVEL.PROGRESS;
                                break;
                            case "3":
                            case "details":
                                m_LogLevel = LOG_LEVEL.DETAILS;
                                break;
                            default:
                                ReportStatus(WorkingStatus.WARNING,
                                    "Please specify the depth of logging using -log with following syntax:\r\n" +
                                    "    -log <LogLevel>  or  -log=<LogLevel>\r\n" +
                                    "          LogLevel could be 0 ~ 3 or none, procedure, progress and details\r\n");
                                break;
                        }

                    } while (false);
                    break;

                case "-log0":
                    m_LogLevel = LOG_LEVEL.NONE;
                    break;

                case "-log1":
                    m_LogLevel = LOG_LEVEL.PROCEDURE;
                    break;

                case "-log2":
                    m_LogLevel = LOG_LEVEL.PROGRESS;
                    break;

                case "-log3":
                    m_LogLevel = LOG_LEVEL.DETAILS;
                    break;
            }

            return true;
        }


        protected abstract Type ServiceType
        {
            get;
        }

        public void DoTask()
        {
            do
            {
                //! generate services

                String[] tFiles = Directory.GetFiles(m_WorkingPath);
                if (null == tFiles)
                {
                    ReportError("No Target Files found.");
                    break;
                }
                else if (0 == tFiles.Length)
                {
                    ReportError("No Target Files found.");
                    break;
                }
                List<String> m_FileList = new List<string>();
                foreach (String tFile in tFiles)
                {
                    if (!File.Exists(tFile))
                    {
                        continue;
                    }

                    if (!PathEx.CheckExtensionList(tFile, m_TargetFileExtensions))
                    {
                        continue;
                    }
                    m_FileList.Add(tFile);
                }

                if (0 == m_FileList.Count)
                {
                    ReportError("No valid files (" + m_TargetFileExtensions + ") found in target folder! \r\n" + m_WorkingPath);
                    return;
                }

                using (ReportReaderEngine tReporter = new ReportReaderEngine())
                {
                    tReporter.LogLevel = m_LogLevel;
                    if (!tReporter.RequestGenerateReport(m_FileList.ToArray(), this.ServiceType))
                    {
                        ReportError("Failed in reading test files");
                        break;
                    }

                    //! wait for complete
                    tReporter.CompleteSignal.WaitOne();
                    
                    GenerateReport(Path.Combine(m_WorkingPath, m_ReportFile), tReporter.Reports);
                }

            } while (false);

            return;
        }

        protected abstract void GenerateReport(String tReportPath, TestReport[] tReports);
        


    }


}
