using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ESnail.Utilities.Shell
{
    public class AsynchronouseShell : IDisposable
    {
        public enum RdEventType
        {
            ErrorEvent,
            DataEvent,
            StopEvent
        }

        public struct RdEventArgs
        {
            public RdEventType eventtype;
            public System.String info;
        };

        public delegate void RdEvent(RdEventArgs args);

        private Thread m_ReadThread = null;
        private Form m_RootParent = null;

        public AsynchronouseShell(String szCommand, String szCurrentDirectory,Form RootParent)
        {
            if (null == szCommand)
            {
                szCommand = "";
            }

            m_RootParent = RootParent;

            m_szCommand = szCommand;
            m_szCurrentDirectory = szCurrentDirectory;
            m_running = false;

            m_processInfo = new PROCESS_INFORMATION();
            m_PipeData = new byte[BUF_SIZE];

            m_ReadThread = new Thread(this.DoReading);
            m_ReadThread.IsBackground = true;
        }

        private Boolean m_RequestStop = false;

        private void DoReading()
        {
            while (true)
            {
                if (m_RequestStop)
                {
                    m_RequestStop = false;
                    return;
                }


                uint NumBytesRead = 0;
                uint TotalBytesAvailable = 0;
                uint BytesLeftThisMessage = 0;


                Boolean Success = PeekNamedPipe(m_PipeReadHandle, m_PipeData, 1, ref NumBytesRead,
                    ref TotalBytesAvailable, ref BytesLeftThisMessage);
                if (!Success)
                {
                    RaiseRdEvent(RdEventType.ErrorEvent, "PeekNamedPipe failed.");
                    continue;
                }

                //System.Console.SetOut(new TextWriter());

                //! if get data 
                if (NumBytesRead > 0)
                {
                    Success = ReadFile(m_PipeReadHandle, m_PipeData, BUF_SIZE, out NumBytesRead, IntPtr.Zero);
                    if (!Success)
                    {
                        RaiseRdEvent(RdEventType.ErrorEvent, "ReadFileError failed.");
                        continue;
                    }

                    dealReadData(m_PipeData, NumBytesRead);
                    continue;
                }

                //! check thread state
                if (WaitForSingleObject(m_processInfo.hProcess, 0) == WAIT_OBJECT_0)
                {
                    m_running = false;
                    RaiseRdEvent(RdEventType.StopEvent, "terminated normal.");
                    
                    FreeHandle();

                    return;
                }


                Thread.Sleep(10);
            }
        }

        public AsynchronouseShell(String szCommand)
            :this(szCommand,null,null)
        {
        }

        public AsynchronouseShell()
            :this(null,null,null)
        {
        }

        public AsynchronouseShell(System.String szCommand,Form RootParent)
            : this(szCommand, null, RootParent)
        {
        }

        public AsynchronouseShell(Form RootParent)
            : this(null, null, RootParent)
        {
        }

        #region Windows API

        [StructLayout(LayoutKind.Sequential)]
        class PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public System.Int32 dwProcessId;
            public System.Int32 dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        class SECURITY_ATTRIBUTES
        {
            public System.Int32 nLength;
            public IntPtr lpSecurityDescriptor;
            public System.Int32 bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        class STARTUPINFO
        {
            public System.Int32 cb;
            public System.String lpReserved;
            public System.String lpDesktop;
            public System.String lpTitle;
            public System.Int32 dwX;
            public System.Int32 dwY;
            public System.Int32 dwXSize;
            public System.Int32 dwYSize;
            public System.Int32 dwXCountChars;
            public System.Int32 dwYCountChars;
            public System.Int32 dwFillAttribute;
            public System.Int32 dwFlags;
            public System.Int16 wShowWindow;
            public System.Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern System.Boolean CreateProcess(System.String lpApplicationName, System.String lpCommandLine, SECURITY_ATTRIBUTES lpProcessAttributes,
            SECURITY_ATTRIBUTES lpThreadAttributes, System.Boolean bInheritHandles, System.Int32 dwCreationFlags, IntPtr lpEnvironment,
            System.String lpCurrentDirectory, STARTUPINFO lpStartupInfo, PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll")]
        static extern System.Boolean CreatePipe(out IntPtr hReadPipe, out IntPtr hWritePipe,
           SECURITY_ATTRIBUTES lpPipeAttributes, uint nSize);

        [DllImport("kernel32.dll", EntryPoint = "PeekNamedPipe", SetLastError = true)]
        static extern System.Boolean PeekNamedPipe(IntPtr handle, byte[] buffer, uint nBufferSize, ref uint bytesRead,
                    ref uint bytesAvail, ref uint BytesLeftThisMessage);

        [DllImport("kernel32.dll")]
        static extern System.Boolean ReadFile(IntPtr hFile, byte[] lpBuffer,
           uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        static extern Int32 WaitForSingleObject(IntPtr handle, Int32 milliseconds);

        [DllImport("kernel32.dll", SetLastError = true)][return: MarshalAs(UnmanagedType.Bool)]
        static extern System.Boolean CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)][return: MarshalAs(UnmanagedType.Bool)]
        static extern System.Boolean TerminateProcess(IntPtr hProcess, uint uExitCode);



        const System.Int32 STARTF_USESHOWWINDOW             = 0x00000001;
        const System.Int32 STARTF_USESTDHANDLES             = 0x00000100;
        const System.Int16 SW_HIDE                          = 0;
        const System.Int32 WAIT_OBJECT_0                    = 0;

        #endregion


        public void Run()
        {
            
            if (m_running) 
            {
                return;
            }

            SECURITY_ATTRIBUTES SecurityAttributes = new SECURITY_ATTRIBUTES();
            STARTUPINFO StartupInfo = new STARTUPINFO();

            System.Boolean Success = false;

            //! create pipe
            SecurityAttributes.nLength = Marshal.SizeOf(SecurityAttributes);
            SecurityAttributes.bInheritHandle = 1;
            SecurityAttributes.lpSecurityDescriptor =IntPtr.Zero;
            Success = CreatePipe(out m_PipeReadHandle, out m_PipeWriteHandle, SecurityAttributes, 0);

            if (!Success)
            {
                RaiseRdEvent(RdEventType.ErrorEvent, "CreatePipe failed.");
                return;
            }


            //! create thread
            StartupInfo.cb = Marshal.SizeOf(StartupInfo);
            StartupInfo.dwFlags = STARTF_USESHOWWINDOW | STARTF_USESTDHANDLES;
            StartupInfo.wShowWindow = SW_HIDE;
            StartupInfo.hStdOutput = m_PipeWriteHandle;
            StartupInfo.hStdError = m_PipeWriteHandle;
            Success = CreateProcess
                        (
                            null, 
                            m_szCommand, 
                            null, 
                            null, 
                            true, 
                            0, 
                            IntPtr.Zero,
                            m_szCurrentDirectory,
                            StartupInfo, 
                            m_processInfo
                        );
            if (!Success)
            {
                RaiseRdEvent(RdEventType.ErrorEvent, "CreateProcess failed.");
                return;
            }


            //! success
            m_running = true;
            //m_readtimer.Start();
            m_ReadThread.Start();
        }


        //! raising event
        private void RaiseRdEvent(RdEventType eventtype,System.String info)
        {
            RdEventArgs args = new RdEventArgs();
            args.eventtype = eventtype;
            args.info = info;


            if (null != m_RootParent)
            {
                //! raising asynchronouse event
                try
                {
                    m_RootParent.BeginInvoke(RdEventHandler, args);
                }
                catch (Exception )
                {
                }
            }
            else 
            {
                if (RdEventHandler != null)
                {
                    RdEventHandler(args);
                }
            }
        }

        private void dealReadData(byte[] data,uint num)
        {
            Array.Resize(ref data, (System.Int32)num);
            
            Encoding ec = Encoding.Default;
            System.String str = ec.GetString(data);

            //! replace '\b' backspace with whitespace 
            //str.Replace('\b', ' ');

            //! raising event
            RaiseRdEvent(RdEventType.DataEvent, str);
        }


        public void Stop()
        {
            if (!m_running) 
            { 
                return; 
            }
            
            m_running = false;

            m_RequestStop = true;
            m_ReadThread.Join();

            TerminateProcess(m_processInfo.hProcess, 0);
            RaiseRdEvent(RdEventType.StopEvent, "terminated by user.");
            
            FreeHandle();
        }
        

        private void FreeHandle()
        {
            CloseHandle(m_processInfo.hThread);
            m_processInfo.hThread = IntPtr.Zero;
            CloseHandle(m_processInfo.hProcess);
            m_processInfo.hProcess = IntPtr.Zero;
            CloseHandle(m_PipeReadHandle);
            m_PipeReadHandle = IntPtr.Zero;
            CloseHandle(m_PipeWriteHandle);
            m_PipeWriteHandle = IntPtr.Zero;
        }

        

        public Boolean Running
        {
            get { return m_running; }
        }

        public String Command
        {
            set 
            {
                if (m_running) 
                { 
                    return; 
                }
                m_szCommand = value; 
            }
            get { return m_szCommand; }
        }

        public String CurrentDirectory
        {
            set
            {
                if (m_running) 
                {
                    return;
                }

                m_szCurrentDirectory = value;
            }
            get { return m_szCurrentDirectory; }
        }
        #region Dispose 

        
        public System.Boolean IsDisposed
        {
            get { return disposed; }
        }

        protected virtual void Dispose(System.Boolean disposing)
        {
            //do something
            if (!IsDisposed)
            {
                if (disposing)
                {
                    /*free managed resource*/
                
                }
                //free unmanaged resource
                Stop();
                
                disposed = true;
            }
            
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AsynchronouseShell()      
        {
            Dispose(false);
        }

        #endregion

        private System.String m_szCommand;
        private System.String m_szCurrentDirectory;
        private System.Boolean m_running;
        private System.Boolean disposed = false;
        public event RdEvent RdEventHandler;
        private IntPtr m_PipeReadHandle;
        private IntPtr m_PipeWriteHandle;
        private PROCESS_INFORMATION m_processInfo;
        System.Byte[] m_PipeData;
        private const System.Int32 BUF_SIZE = 8192;
    }

}

