using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;

namespace ESnail.Utilities
{
    public abstract class AsynMonitor<TType> : ESDisposableClass
    {
        private System.Timers.Timer m_Timer = null;
        private TType m_Item = default(TType);
        private Thread m_Thread = null;
        private Int32 m_Timerout = 0;
        private ManualResetEvent m_TimeoutRequest = new ManualResetEvent(false);
        private ManualResetEvent m_StopRequest = new ManualResetEvent(false);
        #region constructor
        public AsynMonitor()
        {
            Initialize();
        }

        public AsynMonitor(TType tObject)
        {
            m_Item = tObject;

            Initialize();
        }

        public AsynMonitor(Int32 tTimeout)
        {
            if (tTimeout > 0)
            {
                m_Timerout = tTimeout;
            }

            Initialize();
        }

        public AsynMonitor(TType tObject, Int32 tTimeout)
        {
            if (tTimeout > 0)
            {
                m_Item = tObject;
                m_Timerout = tTimeout;
            }

            Initialize();
        }
        #endregion


        private void Initialize()
        {
            
            if (m_Timerout > 0)
            {
                m_Timer = new System.Timers.Timer(m_Timerout);
                m_Timer.Elapsed += new ElapsedEventHandler(m_Timer_Elapsed);
            }
        }

        public TType Tag
        {
            get { return m_Item; }
        }
        
        private void m_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (m_Timer)
            {
                m_Timer.Stop();
            }
            m_TimeoutRequest.Set();
        }
        
        public Boolean Start()
        {
            if (m_bDisposed)
            {
                return false;
            }

            if (null == AsynMonitorRoutine)
            {
                return false;
            }

            
            if (OnAsynMonitor())
            {
                OnAsynMonitorReport();
                Dispose();
                return true;
            }
            
            m_Thread = new Thread(MonitorTask);
            m_Thread.Priority = ThreadPriority.BelowNormal;
            m_Thread.IsBackground = false;
            m_Thread.Name = "AsynMonitorTask";
            m_Thread.Start();
            

            return true;
        }

        public void Close()
        {
            Dispose();
        }

        private void MonitorTask()
        {
            
            if (null != m_Timer)
            {
                lock (m_Timer)
                {
                    m_Timer.Start();
                }
            }
            Thread tThread = m_Thread;

            //Console.WriteLine("[" + tThread.ManagedThreadId.ToString("X") + "]EnterMonitor...");
            while (!OnAsynMonitor())
            {
                //Console.Write("[" + tThread.ManagedThreadId.ToString("X") + "]Runtask at" + DateTime.Now.ToLongTimeString() + "...");
                if (m_StopRequest.WaitOne(0))
                {
                    //Console.WriteLine("Request Stop.");
                    OnAsynMonitorCancelled();
                    return;
                }
                else if (m_TimeoutRequest.WaitOne(0))
                {
                    //Console.WriteLine("Timeout!");
                    OnAsynMonitorTimeout();
                    return;
                }
                //Console.WriteLine("OK");
                //Thread.Sleep(50);
            }
            //Console.WriteLine("[" + tThread.ManagedThreadId.ToString("X") + "]Request ExitMoitor...");
            OnAsynMonitorReport();
            //Console.WriteLine("[" + tThread.ManagedThreadId.ToString("X") + "]ExitMnitor.");

            /*
            if (null != m_Timer)
            {
                lock (m_Timer)
                {
                    try
                    {
                        m_Timer.Stop();
                        m_Timer.Close();
                        m_Timer.Elapsed -= new System.Timers.ElapsedEventHandler(m_Timer_Elapsed);
                    }
                    catch (Exception) { }
                    finally
                    {
                        m_Timer = null;
                    }
                }
            }
             */
        }

        #region events
        public delegate Boolean AsynMonitorCallback(ref TType tItem);

        public event AsynMonitorCallback AsynMonitorRoutine;

        private Boolean OnAsynMonitor()
        {
            if (null != AsynMonitorRoutine)
            {
                try
                {
                    return AsynMonitorRoutine.Invoke(ref m_Item);
                }
                catch (Exception) { }
            }

            return true;
        }

        public delegate void AsynMonitorReport(TType tItem);

        public event AsynMonitorReport AsynMonitorReportEvent;

        private void OnAsynMonitorReport()
        {
            try
            {
                Dispose();
            }
            catch (Exception) { }
            finally
            {
                if (null != AsynMonitorReportEvent)
                {
                    try
                    {
                        AsynMonitorReportEvent.Invoke(m_Item);
                    }
                    catch (Exception )
                    {
                        
                    }
                }
            }
            
        }

        public event AsynMonitorReport AsynMonitorTimeout;

        private void OnAsynMonitorTimeout()
        {
            try
            {
                Dispose();
            }
            catch (Exception) { }
            finally
            {
                if (null != AsynMonitorTimeout)
                {
                    try
                    {
                        AsynMonitorTimeout.Invoke(m_Item);
                    }
                    catch (Exception) { }
                }
            }
        }

        public event AsynMonitorReport AsynMonitorCancelled;

        private void OnAsynMonitorCancelled()
        {
            try
            {
                Dispose();
            }
            catch (Exception) { }
            finally
            {

                if (null != AsynMonitorCancelled)
                {
                    try
                    {
                        AsynMonitorCancelled.Invoke(m_Item);
                        //AsynMonitorCancelled.BeginInvoke(m_Item, new AsyncCallback(ReportCallBack), AsynMonitorCancelled);
                    }
                    catch (Exception) { }
                }
            }
        }
        /*
        private void ReportCallBack(IAsyncResult ar)
        {
            AsynMonitorReport tAsynMonitorCancelled = ar.AsyncState as AsynMonitorReport;
            if (null != tAsynMonitorCancelled)
            {
                tAsynMonitorCancelled.EndInvoke(ar);
            }
        }
        */
        #endregion


        #region IDisposable Members

#if false
        ~AsynMonitor()
        {
            Dispose();
        }

        private Boolean m_Disposed = false;

        public Boolean Disposed
        {
            get { return m_Disposed; }
        }

        protected abstract void _Dispose();
#endif

        public override void Dispose()
        {
            if (!m_bDisposed)
            {
                m_bDisposed = true;

                try
                {
                    //! disposed managed objects
                    try
                    {
                        _Dispose();
                    }
                    catch (Exception) { }
                    
                    //! stop timer
                    if (null != m_Timer)
                    {
                        lock (m_Timer)
                        {
                            try
                            {
                                m_Timer.Stop();
                                m_Timer.Close();
                                m_Timer.Elapsed -= new System.Timers.ElapsedEventHandler(m_Timer_Elapsed);
                            }
                            catch (Exception) { }
                            finally
                            {
                                m_Timer = null;
                            }
                        }
                    }
                    
                    //! disable active thread
                    if (null != m_Thread)
                    {
                        try
                        {
                            m_StopRequest.Set();
                            //m_Thread.Join();
                            //m_Thread.Abort();
                        }
                        catch (Exception) { }
                        finally
                        {
                            m_Thread = null;
                        }
                    }
                }
                catch (Exception) { }
                finally
                {
                    GC.SuppressFinalize(this);
                }
            }
        }

        

        

#endregion
    }
}