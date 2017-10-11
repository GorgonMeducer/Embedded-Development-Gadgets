using System;
using System.Collections.Generic;
using System.Text;
using ESnail.CommunicationSet.Commands;
using ESnail.Utilities;

namespace ESnail.Device.Telegraphs
{
    //! \name Old version of telegraph format used by SB200
    public class SmartBatteryTelegraph : SinglePhaseTelegraph
    {
        protected const Byte AT_BM_SYNC = (System.Byte)'U';
        protected const Byte AT_SB_ENDSYNC = 0x0D;

        //! constructor
        public SmartBatteryTelegraph(ESCommand Command)
            : base(Command)
        { 
            
        }

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

            Byte[] FrameBuffer = null;
            Int32 FrameSize = 7;
            Byte CheckSUM = 0;

            if (null != m_Command.Data)
            {
                FrameSize += m_Command.Data.Length;
            }

            try
            {
                //! allocated memory for frame
                FrameBuffer = new Byte[FrameSize];
            }
            catch (System.OutOfMemoryException e)
            {
                OnError();
                System.Console.WriteLine(e.ToString());
                return null;
            }


            //! begin frame encoding -------------------------------------
            FrameBuffer[0] = AT_BM_SYNC;                            //!< Head

            switch (m_Command.Address)
            { 
                case BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER:               //!< Adapter
                    FrameBuffer[1] = (System.Byte)'M';
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_CHARGER:               //!< Charger
                    FrameBuffer[1] = (System.Byte)'C';
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_LOADER:                //!< Loader
                    FrameBuffer[1] = (System.Byte)'E';    
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_ALL:                   //!< All
                    FrameBuffer[1] = (System.Byte)'A';
                    break;
                
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC:
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS:                 //!< SMBus
                    FrameBuffer[1] = (System.Byte)'S';
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_LCD:                   //!< LCD
                    FrameBuffer[1] = (System.Byte)'D';      
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC:
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI:                   //!< SPI
                    FrameBuffer[1] = (System.Byte)'I';
                    break;

                case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC:
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_UART:                  //!< UART
                    FrameBuffer[1] = (System.Byte)'R';              
                    break;

                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC:
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART:      //!< Single-wire UART
                    FrameBuffer[1] = (System.Byte)'W';
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_PRN:                   //!< printer
                    FrameBuffer[1] = (System.Byte)'T';
                    break;

                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC:
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C:
                    FrameBuffer[1] = (System.Byte)'B';
                    break;
                default:
                    FrameBuffer[1] = (Byte)((Byte)m_Command.AddressValue | (Byte)0x80); //!< User Address
                    break;
            }

            
            FrameBuffer[2] = (Byte)'P';                             //!< PC

            
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
                    switch (m_Command.Address)
                    {
                        case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_UART_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX:

                        case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC:
                        case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SPI:
                        case BM_CMD_ADDR.BM_CMD_ADDR_UART:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART:
                            FrameBuffer[3] |= 0x80;
                            break;
                    }
                    CheckSUM += FrameBuffer[3];
                    FrameBuffer[4] = 0;
                    FrameBuffer[5] = CheckSUM;
                    FrameBuffer[6] = AT_SB_ENDSYNC;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                    switch (m_Command.Address)
                    {
                        case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_UART_EX:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX:

                        case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC:
                        case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SPI:
                        case BM_CMD_ADDR.BM_CMD_ADDR_UART:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART:
                            FrameBuffer[3] &= 0x7F;
                            break;
                    }
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

            if (null != m_Timer)
            {
                //! end frame encoding ---------------------------------------
                lock (m_Timer)
                {
                    //! start timer
                    m_Timer.Enabled = true;
                }
            }

            return FrameBuffer;
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
                    if ((Byte)'P' != Address)
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

                    switch (Address)
                    { 
                        case (Byte)'M':         //!< Adapter
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER;
                            break;
                        case (Byte)'S':         //!< SMBus
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS;
                            break;
                        case (Byte)'I':         //!< SPI
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI;
                            break;
                        case (Byte)'R':         //!< UART
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART;
                            break;
                        case (Byte)'W':         //!< Single-wire UART
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART;
                            break;
                        case (Byte)'T':         //!< Printer
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_PRN;
                            break;
                        case (Byte)'E':         //!< Loader
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_LOADER;
                            break;
                        case (Byte)'C':         //!< Charger
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_CHARGER;
                            break;
                        case (Byte)'D':         //!< LCD
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_LCD;
                            break;
                        case (Byte)'A':         //!< All
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_ALL;
                            break;
                        case (Byte)'B':
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C;
                            break;
                        default:
                            if (Address > 0x7F)
                            {
                                Address &= 0x7F;
                            }
                            else
                            {
                                Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_ALL;
                            }
                            break;

                    }

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

                    switch (m_Command.Address)
                    { 
                        case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS:
                        case BM_CMD_ADDR.BM_CMD_ADDR_SPI:
                        case BM_CMD_ADDR.BM_CMD_ADDR_UART:
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
                            break;
                    }

                    if ((temCommand != Command) && (Command != 0x41))
                    {
                        //! this frame not belong to this telegraph
                        tRequestDrop = true;
                        qEnumerator.Dispose();
                        return 0;
                    }
                    Command = temCommand;
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

                //! verify check sum
                if (qEnumerator.Current != CheckSUM)
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

                if (AT_SB_ENDSYNC != qEnumerator.Current)
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
                                m_Timer.Stop();
                                m_Timer.Enabled = false;
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

        public override System.String Type
        {
            get { return "ATSB200-Compliant Telegraph"; }
        }

        public override Telegraph GetTestTelegraph()
        {
            ESCommand tCommand = new ESCommandReadBlock();
            tCommand.Address = BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER;
            tCommand.TimeOut= 500;
            tCommand.Command = (Byte)'t';
            tCommand.Description = "ATSB200 testing telegraph";

            return new SmartBatteryTelegraph(tCommand);
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

            return new SmartBatteryTelegraph(Args[0] as ESCommand);
        }
    }
}
