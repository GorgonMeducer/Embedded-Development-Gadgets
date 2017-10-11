using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Device;
using ESnail.CommunicationSet.Commands;
using System.Timers;
using System.Threading;

namespace ESnail.Device.Telegraphs
{
    //! \name BatteryManageTelegraph
    // @{
    public class XBatteryTelegraph : SinglePhaseTelegraph
    {

        protected const System.Byte AT_BM_SYNC              = (System.Byte)'X';
        protected const System.Byte AT_SB_ENDSYNC           = 0x0D;

        //! telegraph method : Encode
        public override System.Byte[] Encode()
        {
            //! check the command
            if (null == m_Command)
            {
                //! this condition should not happend
                return null;
            }

            if ((null != m_Command.Data) && (m_Command.Data.Length >= 57))
            {
                //! Illegal command
                OnError(BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR_DATA_SIZE_TOO_LARGE);

                return null;
            }

            if (m_Command.IsPureListener)
            {
                //! pure listener
                return null;
            }

            System.Byte[] FrameBuffer = null;
            System.Int32 FrameSize = 7;
            System.Byte CheckSUM = 0;

            if (null != m_Command.Data)
            {
                FrameSize += m_Command.Data.Length;
            }

            try
            {
                //! allocated memory for frame
                Array.Resize(ref FrameBuffer, FrameSize);
            }
            catch (System.OutOfMemoryException e)
            {
                OnError();
                System.Console.WriteLine(e.ToString());
                return null;
            }
                       

            //! begin frame encoding -------------------------------------
            FrameBuffer[0] = AT_BM_SYNC;                            //!< Head
            FrameBuffer[1] = m_Command.AddressValue;                //!< Address
            FrameBuffer[2] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_PC;      //!< PC
            FrameBuffer[3] = m_Command.Command;                     //!< Command

            CheckSUM += FrameBuffer[0];
            CheckSUM += FrameBuffer[1];
            CheckSUM += FrameBuffer[2];

            switch (m_Command.Type)
            {
                case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                    CheckSUM += FrameBuffer[3];
                    FrameBuffer[4] = 0;
                    FrameBuffer[5] = CheckSUM;
                    FrameBuffer[6] = AT_SB_ENDSYNC;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                    FrameBuffer[3] |= 0x80;
                    CheckSUM += FrameBuffer[3];
                    FrameBuffer[4] = 0;
                    FrameBuffer[5] = CheckSUM;
                    FrameBuffer[6] = AT_SB_ENDSYNC;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                    FrameBuffer[3] &= 0x7F;
                    CheckSUM += FrameBuffer[3];
                    //! data length
                    if (null != m_Command.Data)
                    {
                        FrameBuffer[4] = (System.Byte)m_Command.Data.Length;
                        CheckSUM += FrameBuffer[4];
                        System.Int32 n = 0;
                        for (; n < m_Command.Data.Length; n++)
                        {
                            FrameBuffer[5 + n] = m_Command.Data[n];
                            CheckSUM += m_Command.Data[n];
                        }
                        FrameBuffer[5 + n] = CheckSUM;
                        FrameBuffer[5 + n + 1] = AT_SB_ENDSYNC;
                    }
                    else
                    {
                        FrameBuffer[4] = 0;
                        FrameBuffer[5] = CheckSUM;
                        FrameBuffer[6] = AT_SB_ENDSYNC;
                    }
                    break;
            }

            
            //! end frame encoding ---------------------------------------
            lock (m_Timer)
            {
                if (null != m_Timer)
                {
                    //! start timer
                    m_Timer.Enabled = true;
                }
            }

            return FrameBuffer;
        }

        //! telegraph method : Decode
        public override System.Int32 Decode(ref Queue<System.Byte> InputQueue, ref Boolean tRequestDrop)
        {
            //! check input queue
            if ((null == InputQueue) || (null == m_Command) || (0 == InputQueue.Count))
            {
                return 0;
            }
            
            //! frame address
            System.Byte Address = 0;
            System.Byte Command = 0;
            System.Byte CheckSUM = 0;
            System.Byte DataLength = 0;
            System.Byte[] Datas = null;

            tRequestDrop = false;

            //! check the frame
            using (Queue<System.Byte>.Enumerator qEnumerator = InputQueue.GetEnumerator())
            {
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }

                //! check frame head
                if (AT_BM_SYNC != qEnumerator.Current)
                {
                    tRequestDrop = true;
                    qEnumerator.Dispose();
                    return 0;
                }
                else
                {
                    CheckSUM += qEnumerator.Current;
                }


                //! get frame destination address
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                else
                {
                    Address = qEnumerator.Current;
                    CheckSUM += qEnumerator.Current;
                    if ((Byte)BM_CMD_ADDR.BM_CMD_ADDR_PC != Address)
                    {
                        //! this frame may not sent to pc
                        tRequestDrop = true;
                        qEnumerator.Dispose();
                        return 0;
                    }
                }


                //! get frame source address
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                else
                {
                    Address = qEnumerator.Current;
                    CheckSUM += qEnumerator.Current;
                    if (m_Command.AddressValue != Address)
                    {
                        //! this frame not belong to this telegraph
                        tRequestDrop = true;
                        qEnumerator.Dispose();
                        return 0;
                    }
                }

                //! get frame command
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                else
                {
                    Command = qEnumerator.Current;
                    CheckSUM += qEnumerator.Current;

                    Byte temCommand = m_Command.Command;
                    switch (m_Command.Type)
                    {
                        case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                        case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                            temCommand |= 0x80;
                            break;
                        case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                        case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                            temCommand &= 0x7F;
                            break;
                    }

                    if (temCommand != Command)
                    {
                        //! this frame not belong to this telegraph
                        tRequestDrop = true;
                        qEnumerator.Dispose();
                        return 0;
                    }
                }

                //! get frame data length
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                else
                {
                    DataLength = qEnumerator.Current;
                    CheckSUM += qEnumerator.Current;

                    Datas = new Byte[DataLength];
                    //! resize the databuffer: Datas
                    //Array.Resize(ref Datas, DataLength);
                }


                //! get data
                for (System.Int32 n = 0; n < DataLength; n++)
                {
                    //! get frame data length
                    if (!qEnumerator.MoveNext())
                    {
                        qEnumerator.Dispose();
                        return 0;
                    }
                    else
                    {
                        Datas[n] = qEnumerator.Current;
                        CheckSUM += qEnumerator.Current;
                    }
                }

                //! get check sum
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                else if  (qEnumerator.Current != CheckSUM)
                {
                    tRequestDrop = true;
                    qEnumerator.Dispose();
                    return 0;
                }


                //! check frame tail
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                else if (AT_SB_ENDSYNC != qEnumerator.Current)
                {
                    tRequestDrop = true;
                    qEnumerator.Dispose();
                    return 0;
                }

                if (null != m_Timer)
                {
                    lock (m_Timer)
                    {
                        try
                        {
                            if (null != m_Timer)
                            {
                                //! start timer
                                m_Timer.Enabled = false;
                                m_Timer.Stop();
                                m_Timer.Dispose();
                                m_Timer = null;
                            }
                        }
                        catch (Exception Err)
                        {
                            Err.ToString();
                        }
                    }
                }
            }

            //! decoding success! now, we get all data from queue and create a report
            ESCommand tempCommand = m_Command.CopyConstruct(Datas);      //!< create a command

            //! copy command properies from source command m_Command
            tempCommand.AddressValue = Address;
            tempCommand.Command = Command;
            tempCommand.Description = m_Command.Description;
            tempCommand.ID = m_Command.ID;
            tempCommand.ResponseMode = m_Command.ResponseMode;
            tempCommand.TimeOut = m_Command.TimeOut;

            m_Command = tempCommand;

            //! raising event 
            OnDecoderSuccess(tempCommand);

            //! return decode size
            return (7 + DataLength);
        }

        //! constructor
        public XBatteryTelegraph(ESCommand Command)
            : base(Command)
        {

        }

        public override System.String Type
        {
            get { return "ATXBattery-Compliant Telegraph"; }
        }

        public override Telegraph GetTestTelegraph()
        {
            ESCommand tCommand = new ESCommandReadBlock();
            tCommand.Address = BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER;
            tCommand.TimeOut = 500;
            tCommand.Command = (Byte)'h';
            tCommand.Description = "ATXBattery testing telegraph";

            return new XBatteryTelegraph(tCommand);
        }

        public override Telegraph CreateTelegraph(params object[] Args)
        {
            if (null == Args)
            {
                return null;
            }
            if (0 == Args.Length)
            {
                return null;
            }
            if (!(Args[0] is ESCommand))
            {
                return null;
            }

            return new XBatteryTelegraph(Args[0] as ESCommand);
        }
    }
    // @}
}
