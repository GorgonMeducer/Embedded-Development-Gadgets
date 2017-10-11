using System;
using System.Collections.Generic;
using System.Text;
using ESnail.CommunicationSet.Commands;
using ESnail.Utilities;

namespace ESnail.Device.Telegraphs
{
    public class GSFrameTelegraph : SinglePhaseTelegraph
    {
        //! \brief constructor
        public GSFrameTelegraph(ESCommand tCommand)
            : base(tCommand)
        {

        }


        //! telegraph method : Encode
        public override Byte[] Encode()
        {
            //! check the command
            if (null == m_Command)
            {
                //! this condition should not happend
                return null;
            }

            /*
            if (null == m_Command.Data)
            {
                if ((m_Command.Type != BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER))
                {
                    //! Illegal command
                    OnError(BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR_DATA_SIZE_TOO_LARGE);

                    return null;
                }
            }
            else */
            if (null != m_Command.Data)
            {
                if (m_Command.Data.Length > (UInt16.MaxValue - 7))
                {
                    //! Illegal command
                    OnError(BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR_DATA_SIZE_TOO_LARGE);
                    return null;
                }
            }

            if (m_Command.IsPureListener)
            {
                //! pure listener
                return null;
            }

            List<Byte> tFrameByteList = new List<Byte>();
            Int32 FrameSize = 7;
            UInt16 tDataLength = 0;
            UInt16 CheckSUM = 0xFFFF;

            if (null != m_Command.Data)
            {
                tDataLength = (UInt16)m_Command.Data.Length;
                FrameSize += tDataLength;
            }

            //! begin frame encoding -------------------------------------
            tFrameByteList.Add(0xA5);                                       //!< head

            tFrameByteList.AddRange(BitConverter.GetBytes((UInt16)(tDataLength + 1)));  //!< data length
            Get_CRC(ref CheckSUM, tFrameByteList[1]);
            Get_CRC(ref CheckSUM, tFrameByteList[2]);
            tFrameByteList.Add(m_Command.AddressValue);                     //! bus
            Get_CRC(ref CheckSUM, tFrameByteList[3]);

            tFrameByteList.Add(m_Command.Command);                          //!< command
            Get_CRC(ref CheckSUM, tFrameByteList[4]);

            if (null != m_Command.Data)
            {
                tFrameByteList.AddRange(m_Command.Data);                    //!< add data
                foreach (Byte tItem in m_Command.Data)
                {
                    Get_CRC(ref CheckSUM, tItem);
                }
            }

            tFrameByteList.AddRange(BitConverter.GetBytes(CheckSUM));       //!< CRC16 check sum
            //tFrameByteList.Add(0x55);                                       //!< tail


            //! end frame encoding ---------------------------------------
            lock (m_Timer)
            {
                if (null != m_Timer)
                {
                    //! start timer
                    m_Timer.Enabled = true;
                }
            }

            return tFrameByteList.ToArray();
        }

        //! telegraph method : Decode
        public override Int32 Decode(ref Queue<System.Byte> InputQueue, ref Boolean tRequestDrop)
        {
            //! check input queue
            if ((null == InputQueue) || (null == m_Command) || (0 == InputQueue.Count))
            {
                return 0;
            }

            //! frame address
            System.Byte Command = 0;
            System.Byte Address = 0;
            UInt16 CheckSUM = 0xFFFF;
            UInt16 DataLength = 0;
            System.Byte[] Datas = null;

            tRequestDrop = false;

            //! check the frame
            using (Queue<Byte>.Enumerator qEnumerator = InputQueue.GetEnumerator())
            {
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }

                //! check frame head
                if (0xA5 != qEnumerator.Current)
                {
                    tRequestDrop = true;
                    qEnumerator.Dispose();
                    return 0;
                }


                Byte[] tLength = new Byte[2];
                //! get frame destination address
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                else
                {
                    tLength[0] = qEnumerator.Current;
                    Get_CRC(ref CheckSUM, qEnumerator.Current);
                }


                //! get data length
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                else
                {
                    tLength[1] = qEnumerator.Current;
                    Get_CRC(ref CheckSUM, qEnumerator.Current);

                    DataLength = BitConverter.ToUInt16(tLength, 0);
                    if (0 == DataLength)
                    {
                        qEnumerator.Dispose();
                        return 0;
                    }
                    DataLength--;
                    Datas = new Byte[DataLength];
                }

                //! get frame command
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                else
                {
                    Address = qEnumerator.Current;
                    Get_CRC(ref CheckSUM, qEnumerator.Current);

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
                    Get_CRC(ref CheckSUM, qEnumerator.Current);

                    if ((m_Command.Command != Command) && (0xAC != Command))
                    {
                        //! this frame not belong to this telegraph
                        tRequestDrop = true;
                        qEnumerator.Dispose();
                        return 0;
                    }
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
                        Get_CRC(ref CheckSUM, qEnumerator.Current);
                    }
                }

                Byte[] tCheckSumBuffer = new Byte[2];
                //! get check sum Low Byte
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                else
                {
                    tCheckSumBuffer[0] = qEnumerator.Current;
                }

                //! get check sum High Byte
                if (!qEnumerator.MoveNext())
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                else
                {
                    tCheckSumBuffer[1] = qEnumerator.Current;
                    if (CheckSUM != BitConverter.ToUInt16(tCheckSumBuffer, 0))
                    {
                        tRequestDrop = true;
                        qEnumerator.Dispose();
                        return 0;
                    }
                }

                /*
                //! check frame tail
                if ((!qEnumerator.MoveNext()) || (0x55 != qEnumerator.Current))
                {
                    qEnumerator.Dispose();
                    return 0;
                }
                */
                if (null != m_Timer)
                {
                    lock (m_Timer)
                    {
                        try
                        {
                            if (null != m_Timer)
                            {
                                //! start timer
                                m_Timer.Stop();
                                m_Timer.Enabled = false;
                                m_Timer.Dispose();
                                m_Timer = null;
                            }
                        }
                        catch (Exception) { }
                    }
                }

            }

            //! decoding success! now, we get all data from queue and create a report
            ESCommand tempCommand = new ESCommandReadBlock(Datas);   //!< create a command

            //! copy command properies from source command m_Command
            tempCommand.AddressValue = m_Command.AddressValue;
            tempCommand.Command = Command;
            tempCommand.AddressValue = Address;
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


        public override String Type
        {
            get { return "GSFrame-Telegraph"; }
        }

        public override Telegraph GetTestTelegraph()
        {
            ESCommand tCommand = new ESCommandReadBlock();
            tCommand.AddressValue = 0;
            tCommand.TimeOut = 500;
            tCommand.Command = 0x01;
            tCommand.Description = "GSFrame-Telegraph testing telegraph";

            return new GSFrameTelegraph(tCommand);
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

            return new GSFrameTelegraph(Args[0] as ESCommand);
        }

        private static UInt16 Get_CRC(ref UInt16 tCRC16, Byte tData)
        {
            tData ^= (Byte)((UInt16)tCRC16 & 0x00FF);
            tData ^= (Byte)(tData << 4);

            tCRC16 = (UInt16)((((UInt16)tData << 8) | (tCRC16 >> 8)) ^ (Byte)(tData >> 4) ^ ((UInt16)((UInt16)tData << 3)));

            return tCRC16;
        }
    }
}
