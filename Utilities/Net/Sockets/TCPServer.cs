using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using ESnail.Utilities.Threading;
using ESnail.Utilities.IO;
using System.Threading;
using ESnail.Utilities.Generic;
using ESnail.Utilities;
using System.IO;

namespace ESnail.Net.Sockets
{
    public partial class TCPServer : IDisposable
    {
        private Thread m_TCPThread = null;
        private ManualResetEvent m_StopRequest = new ManualResetEvent(false);
        private ManualResetEvent m_Stoped = new ManualResetEvent(true);
        private IPEndPoint m_LocalEP = new IPEndPoint(IPAddress.Any, 13000);
        private Int32 m_ReadTimeOut = 1000;
        //SafeInvoker m_Invoker = new SafeInvoker();

        public TCPServer()
        {
            m_TCPThread = new Thread(TCPTask);
        }

        public void Start()
        {
            m_StopRequest.Reset();
            m_TCPThread.Start();
        }

        public void RequestStop()
        {
            m_StopRequest.Set();
        }

        public Boolean IsAlive
        {
            get { return m_TCPThread.IsAlive; }
        }

        public IPEndPoint Local
        {
            get { return m_LocalEP; }
        }

        private Int32 m_BufferSize = 64 * 1024;

        public Int32 BufferSize
        {
            get 
            {
                return m_BufferSize;
            }
            set 
            {
                if (0 < value)
                {
                    m_BufferSize = value;
                }
                else
                {
                    m_BufferSize = 64 * 1024;
                }
            }
        }

        public Int32 ReadTimeout
        {
            get 
            {
                return m_ReadTimeOut;
            }
            set
            {
                if (value < 10)
                {
                    m_ReadTimeOut = 10;
                }
                else
                {
                    m_ReadTimeOut = value;
                }
            }
        }

        private void TCPTask()
        {
            m_Stoped.Reset();
            OnTCPServerStarted();

            do
            {
                if (m_StopRequest.WaitOne(0))
                {
                    break;
                }
                Socket tServerSocket = null;
                try
                {
                    tServerSocket = new Socket(m_LocalEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    tServerSocket.Bind(m_LocalEP);
                    tServerSocket.Listen(100);

                    StartAccept(tServerSocket, null);
                    m_StopRequest.WaitOne();
                }
                catch (Exception) { }
                finally
                {
                    if (null != tServerSocket)
                    {
                        tServerSocket.Close();
                        //tServerSocket.Dispose();
                    }
                }
            } while (true);

            OnTCPServerStopped();
            m_Stoped.Set();
        }

        // Begins an operation to accept a connection request from the client 
        //
        // <param name="acceptEventArg">The context object to use when issuing 
        // the accept operation on the server's listening socket</param>
        private void StartAccept(Socket tSocket, SocketAsyncEventArgs acceptEventArg)
        {
            try
            {
                if (acceptEventArg == null)
                {
                    acceptEventArg = new SocketAsyncEventArgs();
                    acceptEventArg.UserToken = tSocket;
                    acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(acceptEventArg_Completed);
                }
                else
                {
                    // socket must be cleared since the context object is being reused
                    acceptEventArg.AcceptSocket = null;
                }

                bool willRaiseEvent = tSocket.AcceptAsync(acceptEventArg);
                if (!willRaiseEvent)
                {
                    ProcessAccept(tSocket, acceptEventArg);
                }
            }
            catch (Exception)
            {
            }
            finally
            { }
        }


        // Process the accept for the socket listener.
        private void ProcessAccept(Socket tSocket, SocketAsyncEventArgs e)
        {
            Socket tClientSocket = e.AcceptSocket;
            if (tClientSocket.Connected)
            {
                TCPRemoteClient tRemoteClient = null;
                //! on client connected
                do
                {
                    IPEndPoint tEndPoint = (IPEndPoint)tClientSocket.RemoteEndPoint;

                    if (null == m_ClientSet.Find(tEndPoint.ToString()))
                    {
                        tRemoteClient = new TCPRemoteClient(tEndPoint);
                        SocketAsyncEventArgs readEventArgs = tRemoteClient.AsyncEventArgsIn;
                        readEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ReadWriteEventArgs_Completed);
                        //readEventArgs.AcceptSocket = tClientSocket;
                        readEventArgs.SetBuffer(new Byte[m_BufferSize], 0, m_BufferSize);

                        SocketAsyncEventArgs writeEventArgs = tRemoteClient.AsyncEventArgsOut;
                        writeEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ReadWriteEventArgs_Completed);
                        //writeEventArgs.AcceptSocket = tClientSocket;
                        tRemoteClient.FirstSendingRequestEvent += new TCPRemoteClient.TCPRemoteClientReport(tRemoteClient_FirstSendingRequestEvent);
                        writeEventArgs.UserToken = tRemoteClient;

                        m_ClientSet.Add(tRemoteClient);
                    }
                    else
                    {
                        tRemoteClient = m_ClientSet[tEndPoint.ToString()];
                    }
                    tRemoteClient.AsyncEventArgsIn.AcceptSocket = tClientSocket;
                    tRemoteClient.AsyncEventArgsOut.AcceptSocket = tClientSocket;

                    this.ProcessSend(tRemoteClient.AsyncEventArgsOut);

                    OnTCPRemoteClientConnectionEvent(tRemoteClient,
                        TCP_CLIENT_STATUS.TCP_CLIENT_CONNECTED);
                } while (false);

                do
                {
                    try
                    {
                        // Get the socket for the accepted client connection and put it into the 
                        // ReadEventArg object user token.

                        if (!tClientSocket.ReceiveAsync(tRemoteClient.AsyncEventArgsIn))
                        {
                            this.ProcessReceive(tRemoteClient.AsyncEventArgsIn);
                        }
                    }
                    catch (SocketException)
                    {
                    }
                    catch (Exception)
                    {
                    }
                } while (false);



            }
            

                // Accept the next connection request.
            this.StartAccept(tSocket, e);
        }

        private void tRemoteClient_FirstSendingRequestEvent(TCPServer.TCPRemoteClient tClient)
        {
            lock (tClient)
            {
                if (!tClient.SendingIdle)
                {
                    return;
                }
                if (null == tClient.AsyncEventArgsOut.AcceptSocket)
                {
                    return;
                }
            }

            this.ProcessSend(tClient.AsyncEventArgsOut);

        }

        void ReadWriteEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
#if false
            if (e.SocketError == SocketError.Success)
            {
                // Done echoing data back to the client.
                Socket tClientSocket = e.AcceptSocket as Socket;
                e.SetBuffer(new Byte[m_BufferSize], 0, m_BufferSize);

                if (!tClientSocket.ReceiveAsync(e))
                {
                    // Read the next block of data send from the client.
                    this.ProcessReceive(e);
                }
            }
            else
            {
                //this.ProcessError(e);
            }
#else
            TCPRemoteClient tRemoteClient = e.UserToken as TCPRemoteClient;
            Socket tClientSocket = e.AcceptSocket;
            if (null == tClientSocket)
            {
                return;
            }
            if (null == tRemoteClient)
            {
                return;
            }

            Monitor.Enter(tRemoteClient);
            Byte[] tOutput = tRemoteClient.FetchOutputStream();
            do
            {
                if (null == tOutput)
                {
                    tRemoteClient.SendingIdle = true;
                    Monitor.Exit(tRemoteClient);
                    break;
                }
                else if (0 == tOutput.Length)
                {
                    tRemoteClient.SendingIdle = true;
                    Monitor.Exit(tRemoteClient);
                    break;
                }
                e.SetBuffer(tOutput, 0, tOutput.Length);
                tRemoteClient.SendingIdle = false;

                Monitor.Exit(tRemoteClient);

                try
                {
                    if (!tClientSocket.SendAsync(e))
                    {
                        // Set the buffer to send back to the client.
                        this.ProcessSend(e);
                    }
                }
                catch (Exception)
                {
                    tRemoteClient.SendingIdle = true;
                }
                finally 
                {
                
                }
            } while (false); 
#endif
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            // Check if the remote host closed the connection.
            if (e.BytesTransferred > 0)
            {
                if (e.SocketError == SocketError.Success)
                {
                    //! get data here
                    Socket tClientSocket = e.AcceptSocket;

                    if (null == tClientSocket)
                    {
                        return;
                    }

                    TCPRemoteClient tRemoteClient = null;
                    //! report data arrived
                    do
                    {
                        IPEndPoint tEndPoint = (IPEndPoint)tClientSocket.RemoteEndPoint;
                        
                        tRemoteClient = m_ClientSet[tEndPoint.ToString()];
                        if (null == tRemoteClient)
                        {
                            return;
                        }
                        tRemoteClient.UpdateTime();

                        try
                        {
                            Byte[] tReportBuffer = new Byte[e.BytesTransferred];
                            Array.Copy(e.Buffer,tReportBuffer,e.BytesTransferred);

                            OnTCPReport(tReportBuffer, tRemoteClient);
                        }
                        catch (Exception )
                        {
                        }
                    } while (false);

#if false
                    if (tClientSocket.Available == 0)
                    {
                        // Set return buffer.
                        //! set output data here
                        Byte[] tOutput = tRemoteClient.FetchOutputStream();
                        Boolean bSending = false;
                        do
                        {
                            if (null == tOutput)
                            {
                                break;
                            }
                            else if (0 == tOutput.Length)
                            {
                                break;
                            }
                            e.SetBuffer(tOutput, 0, tOutput.Length);

                            bSending = true;
                            if (!tClientSocket.SendAsync(e))
                            {
                                // Set the buffer to send back to the client.
                                this.ProcessSend(e);
                            }
                        } while (false);

                        if (!bSending)
                        {
                            if (!tClientSocket.ReceiveAsync(e))
                            {
                                // Read the next block of data sent by client.
                                this.ProcessReceive(e);
                            }
                            
                        }
                    }
                    else if (!tClientSocket.ReceiveAsync(e))
                    {
                        // Read the next block of data sent by client.
                        this.ProcessReceive(e);
                    }
#else
                    if (!tClientSocket.ReceiveAsync(e))
                    {
                        // Read the next block of data sent by client.
                        this.ProcessReceive(e);
                    }
#endif
                }
                else
                {
                    //this.ProcessError(e);
                }
            }
            else
            {
                this.CloseClientSocket(e);
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            Socket tClientSocket = e.AcceptSocket;
            if (null == tClientSocket)
            {
                return;
            }
            //! on client connected
            do
            {
                IPEndPoint tEndPoint = (IPEndPoint)tClientSocket.RemoteEndPoint;
                TCPRemoteClient tRemoteClient = null;
#if false
                if (null == m_ClientSet.Find(tEndPoint.ToString()))
                {
                    tRemoteClient = new TCPRemoteClient(tEndPoint);
                    m_ClientSet.Add(tRemoteClient);
                }
                else
                {
                    tRemoteClient = m_ClientSet[tEndPoint.ToString()];
                }
#else
                tRemoteClient = m_ClientSet[tEndPoint.ToString()];
                if (null == tRemoteClient)
                {
                    return;
                }
#endif
                tRemoteClient.AsyncEventArgsIn.AcceptSocket = null;

                OnTCPRemoteClientConnectionEvent(tRemoteClient,
                    TCP_CLIENT_STATUS.TCP_CLIENT_DISCONNECTED);
            } while (false);

            // close the socket associated with the client
            try
            {
                tClientSocket.Shutdown(SocketShutdown.Send);
            }
            // throws if client process has already closed
            catch (Exception) { }
            tClientSocket.Close();
        }

        private void acceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e.UserToken as Socket, e);
        }


        public void WaitUntilStopped()
        {
            m_Stoped.WaitOne();
        }

        #region events
        public delegate void TCPServerStreamReporter(Byte[] tStream, TCPRemoteClient tTCPClient);
        public event TCPServerStreamReporter TCPStreamReporterEvent;

        private void OnTCPReport(Byte[] tStream, TCPRemoteClient tTCPClient)
        {
            if (null == TCPStreamReporterEvent)
            {
                return;
            }
            TCPStreamReporterEvent.Invoke(tStream, tTCPClient);
            //m_Invoker.BeginInvoke(TCPStreamReporterEvent, tStream, tTCPClient);
        }

        public enum TCP_CLIENT_STATUS
        {
            TCP_CLIENT_CONNECTED,
            TCP_CLIENT_DISCONNECTED,
            TCP_CLIENT_RECEIVE_TIMEOUT,
        }

        public delegate void TCPServerConnectionReport(TCPRemoteClient tTCPClient, TCP_CLIENT_STATUS tStatus);
        public event TCPServerConnectionReport TCPRemoteClientConnectionEvent;

        private void OnTCPRemoteClientConnectionEvent(TCPRemoteClient tTCPClient, TCP_CLIENT_STATUS tStatus)
        {
            //m_Invoker.BeginInvoke(TCPRemoteClientConnectionEvent, tTCPClient, tStatus);
            if (null == TCPRemoteClientConnectionEvent)
            {
                return;
            }
            TCPRemoteClientConnectionEvent.Invoke(tTCPClient, tStatus);
        }

        public delegate void TCPServerReport();
        public event TCPServerReport TCPServerStoppedEvent;
        public event TCPServerReport TCPServerStartedEvent;

        private void OnTCPServerStopped()
        {
            //m_Invoker.BeginInvoke(TCPServerStoppedEvent, null);
            if (null == TCPServerStoppedEvent)
            {
                return;
            }
            TCPServerStoppedEvent.Invoke();
        }

        private void OnTCPServerStarted()
        {
            if (null == TCPServerStartedEvent)
            {
                return;
            }
            TCPServerStartedEvent.Invoke();
            //m_Invoker.BeginInvoke(TCPServerStartedEvent, null);
        }

        #endregion

        #region Dispose
        private bool m_Disposed = false;

        public Boolean Disposed
        {
            get { return m_Disposed; }
        }

        public void Dispose()
        {
            if (m_Disposed)
            {
                return;
            }

            try
            {
                RequestStop();
                m_Stoped.WaitOne();
                m_Stoped.Close();
                m_StopRequest.Close();
                m_ClientSet.Clear();
            }
            catch (Exception) { }
            finally 
            {
                m_Disposed = true;
            }
        }
        #endregion
    }

    public partial class TCPServer
    {
        public class TCPRemoteClient : ISafeID, IDisposable
        {
            private IPEndPoint m_EndPoint = null;
            private SafeID m_ID = null;
            private Boolean m_Available = false;
            private Queue<Byte[]> m_OutputQueue = new Queue<Byte[]>();
            private DateTime m_LastCommunicateTime = new DateTime();

            private SocketAsyncEventArgs m_ReceiveAsynSocket = new SocketAsyncEventArgs();
            private SocketAsyncEventArgs m_SendAsynSocket = new SocketAsyncEventArgs();
            private Boolean m_bSendingIdle = true;

            internal TCPRemoteClient(IPEndPoint tEndPoint)
            {
                m_EndPoint = tEndPoint;
                Initialize();
            }

            internal SocketAsyncEventArgs AsyncEventArgsIn
            {
                get { return m_ReceiveAsynSocket; }
            }

            internal SocketAsyncEventArgs AsyncEventArgsOut
            {
                get { return m_SendAsynSocket; }
            }


            private void Initialize()
            {
                if (null == m_EndPoint)
                {
                    return ;
                }
                m_ID = m_EndPoint.ToString();
                m_Available = true;
            }

            public Object Tag
            {
                get;
                set;
            }

            public Boolean Available
            {
                get { return m_Available; }
            }

            public DateTime LastUpdateTime
            {
                get { return m_LastCommunicateTime; }
            }

            internal void UpdateTime()
            {
                m_LastCommunicateTime = DateTime.Now;
            }

            public IPEndPoint Remote
            {
                get 
                {
                    return m_EndPoint;
                }
            }

            public SafeID ID
            {
                get
                {
                    return m_ID;
                }
                set { }
            }

            public Boolean Send(Byte[] tBuffer)
            {
                if ((null == tBuffer) || (!m_Available))
                {
                    return false;
                }
                else if (0 == tBuffer.Length)
                {
                    return true;
                }

                lock (((ICollection)m_OutputQueue).SyncRoot)
                {
                    m_OutputQueue.Enqueue(tBuffer);

                    if (1 == m_OutputQueue.Count)
                    {
                        OnFirstSendingRequest();
                    }
                }

                return true;
            }

            internal Boolean SendingIdle
            {
                get { return m_bSendingIdle; }
                set { m_bSendingIdle = value; }
            }

            internal delegate void TCPRemoteClientReport(TCPRemoteClient tClient);

            internal event TCPRemoteClientReport FirstSendingRequestEvent;

            private void OnFirstSendingRequest()
            {
                if (null != FirstSendingRequestEvent)
                {
                    FirstSendingRequestEvent.Invoke(this);
                }
            }


            internal Byte[] FetchOutputStream()
            {
                lock (((ICollection)m_OutputQueue).SyncRoot)
                {
                    if (0 == m_OutputQueue.Count) {
                        return null;
                    }
                    return m_OutputQueue.Dequeue();
                }
            }


            #region Dispose

            private Boolean m_Disposed = false;

            public Boolean Disposed
            {
                get { return m_Disposed; }
            }

            public void Dispose()
            {
                if (m_Disposed)
                {
                    return;
                }
                m_Disposed = true;

                try
                {
                    m_OutputQueue.Clear();
                }
                catch (Exception) { }
                finally { }
            }
            #endregion
        }

        private TSet<TCPRemoteClient> m_ClientSet = new TSet<TCPRemoteClient>();

        public TCPRemoteClient Find(IPEndPoint tEndPoint)
        {
            return m_ClientSet.Find(tEndPoint.ToString());
        }

        public Boolean Remove(IPEndPoint tEndPoint)
        {
            return m_ClientSet.Remove(tEndPoint.ToString());
        }

        public Boolean Remove(TCPRemoteClient tClient)
        {
            return m_ClientSet.Remove(tClient.ID);
        }

        public TCPRemoteClient[] Clients
        {
            get { return m_ClientSet.ToArray(); }
        }
    }

}