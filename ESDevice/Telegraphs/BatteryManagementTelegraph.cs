using System;
using System.Collections.Generic;
using System.Text;
using ESnail.CommunicationSet.Commands;
using ESnail.Utilities;

namespace ESnail.Device.Telegraphs
{
    public class BatteryManagementTelegraph : SmartBatteryTelegraph
    {
        //! constructor
        public BatteryManagementTelegraph(ESCommand Command)
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
                if (null != m_Timer)
                {
                    //! end frame encoding ---------------------------------------
                    lock (m_Timer)
                    {
                        //! start timer
                        m_Timer.Enabled = true;
                    }
                }
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
            catch (OutOfMemoryException e)
            {
                OnError();
                Console.WriteLine(e.ToString());
                return null;
            }


            //! begin frame encoding -------------------------------------
            FrameBuffer[0] = AT_BM_SYNC;                            //!< Head
            FrameBuffer[1] = m_Command.SubAddress;                  //!< subaddress
            switch (m_Command.Address)
            {
                case BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER:               //!< Adapter
                    FrameBuffer[2] = 0x00;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_CHARGER:               //!< Charger
                    FrameBuffer[2] = 0x18;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_LOADER:                //!< Loader
                    FrameBuffer[2] = 0x19;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_PRN:                   //!< printer
                    FrameBuffer[2] = 0x1B;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_LCD:                   //!< LCD
                    FrameBuffer[2] = 0x1A;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_ALL:                   //!< All
                    FrameBuffer[2] = 0x7F;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS:                 //!< SMBus
                    FrameBuffer[2] = 0x04;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC:
                    FrameBuffer[2] = 0x06;
                    break;                
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI:                   //!< SPI
                    FrameBuffer[2] = 0x14;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC:
                    FrameBuffer[2] = 0x16;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_UART:                  //!< UART
                    FrameBuffer[2] = 0x0C;
                    break;                
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC:
                    FrameBuffer[2] = 0x0E;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART:      //!< Single-wire UART
                    FrameBuffer[2] = 0x10;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC:
                    FrameBuffer[2] = 0x12;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C:                   //!< I2C
                    FrameBuffer[2] = 0x08;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC:
                    FrameBuffer[2] = 0x0A;
                    break;

                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX:                 //!< SMBus
                    FrameBuffer[2] = 0x24;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX:
                    FrameBuffer[2] = 0x26;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX:                   //!< SPI
                    FrameBuffer[2] = 0x34;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX:
                    FrameBuffer[2] = 0x36;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_EX:                  //!< UART
                    FrameBuffer[2] = 0x2C;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX:
                    FrameBuffer[2] = 0x2E;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX:      //!< Single-wire UART
                    FrameBuffer[2] = 0x30;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX:
                    FrameBuffer[2] = 0x32;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_EX:                   //!< I2C
                    FrameBuffer[2] = 0x28;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC_EX:
                    FrameBuffer[2] = 0x2A;
                    break;                


                default:
                    FrameBuffer[2] = (Byte)((Byte)m_Command.AddressValue); //!< User Address
                    break;
            }

            FrameBuffer[2] |= (Byte)0x80;
            FrameBuffer[3] = m_Command.Command;                     //!< Command

            switch (m_Command.Address)
            {
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C:
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC:
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART:
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC:
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS:
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC:
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI:
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC:
                case BM_CMD_ADDR.BM_CMD_ADDR_UART:
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC:
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_EX:
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX:
                    switch (m_Command.Type)
                    { 
                        case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                        case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                            FrameBuffer[2] |= 0x01;
                            break;
                        case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                        case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                        case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                            break;
                    }
                    break;
            }

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
                        FrameBuffer[4] = (Byte)m_Command.Data.Length;
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
            Byte Address = 0;
            Byte Command = 0;
            Byte CheckSUM = 0;
            Byte DataLength = 0;
            Byte[] Datas = null;

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
                    if (Address != m_Command.SubAddress)
                    {
                        //! error
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
                    if (0x80 != (Address & 0x80))
                    {
                        //! error
                        tRequestDrop = true;
                        qEnumerator.Dispose();
                        return 0;
                    }

                    Address &= 0x7E;

                    switch (Address)
                    {
                        case 0x00:                      //!< adapter / gateway
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER;
                            break;
                        case 0x04:                      //!< smbus
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS;
                            break;
                        case 0x06:                      
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC;
                            break;
                        case 0x08:                      //!< I2C
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C;
                            break;
                        case 0x0A:
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC;
                            break;
                        case 0x0C:                      //!< uart
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART;
                            break;
                        case 0x0E:
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC;
                            break;
                        case 0x10:                      //!< single-wire uart
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART;
                            break;
                        case 0x12:
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC;
                            break;
                        case 0x14:                      //!< spi
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI;
                            break;
                        case 0x16:
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC;
                            break;
                        case 0x18:                      //!< charger
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_CHARGER;
                            break;
                        case 0x19:                      //!< loader
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_LOADER;
                            break;
                        case 0x1A:                      //!< lcd
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_LCD;
                            break;
                        case 0x1B:                      //!< printer
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_PRN;
                            break;

                        case 0x24:                      //!< smbus extend
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX;
                            break;
                        case 0x26:
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX;
                            break;
                        case 0x28:                      //!< I2C extend
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_EX;
                            break;
                        case 0x2A:
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC_EX;
                            break;
                        case 0x2C:                      //!< uart extend
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_EX;
                            break;
                        case 0x2E:
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX;
                            break;
                        case 0x30:                      //!< single-wire uart
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX;
                            break;
                        case 0x32:
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX;
                            break;
                        case 0x34:                      //!< spi
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX;
                            break;
                        case 0x36:
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX;
                            break;

                        case 0x7F:                      //!< all
                            Address = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_ALL;
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
            ESCommand tempCommand = null;   //!< create a command
            if ((Datas.Length > 2) && (BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ) == m_Command.Type)
            {
                tempCommand = new ESCommandReadBlock(Datas);
            }
            else
            { 
                tempCommand = m_Command.CopyConstruct(Datas);
            }

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
            get { return "BM300-Compliant Telegraph"; }
        }

        public override Telegraph GetTestTelegraph()
        {
            ESCommand tCommand = new ESCommandReadBlock();
            tCommand.Address = BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER;
            tCommand.TimeOut = 500;
            tCommand.Command = 0x01;
            tCommand.Description = "BM300 testing telegraph";

            return new BatteryManagementTelegraph(tCommand);
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

            return new BatteryManagementTelegraph(Args[0] as ESCommand);
        }
    }
}
