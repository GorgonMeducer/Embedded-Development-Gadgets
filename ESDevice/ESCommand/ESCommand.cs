using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Utilities;

namespace ESnail.CommunicationSet.Commands
{
    

    //! /name Command type
    //! @{
    public enum BM_CMD_TYPE : byte
    { 
        BM_CMD_TYPE_NO_PARAMETER,               //!< just command
        BM_CMD_TYPE_WORD_WRITE,                 //!< write a word(2bytes)
        BM_CMD_TYPE_WORD_READ,                  //!< read a word(2bytes)
        BM_CMD_TYPE_BLOCK_WRITE,                //!< write a block
        BM_CMD_TYPE_BLOCK_READ                  //!< read a block
    }
    //! @}

    //! /name Command address
    //! @{
    public enum BM_CMD_ADDR : byte 
    { 
        BM_CMD_ADDR_ADAPTER             = 0x80, //!< Adapter
        
        BM_CMD_ADDR_SMBUS               = 0x91, //!< Communication Interfaces : SMBus 
        BM_CMD_ADDR_SMBUS_EX            = 0x99, //!< Communication Interfaces : SMBus Extend
        BM_CMD_ADDR_SMBUS_PEC           = 0xE1, //!< Communication Interfaces : SMBus               with PEC
        BM_CMD_ADDR_SMBUS_PEC_EX        = 0xE9, //!< Communication Interfaces : SMBus Extend        with PEC
        
        BM_CMD_ADDR_UART                = 0x92, //!< Communication Interfaces : UART
        BM_CMD_ADDR_UART_EX             = 0x9A, //!< Communication Interfaces : UART  Extend
        BM_CMD_ADDR_UART_PEC            = 0xE2, //!< Communication Interfaces : UART                with PEC
        BM_CMD_ADDR_UART_PEC_EX         = 0xEA, //!< Communication Interfaces : UART  Extend        with PEC

        BM_CMD_ADDR_SINGLE_WIRE_UART    = 0x93, //!< Communication Interfaces : Single-wire UART
        BM_CMD_ADDR_SINGLE_WIRE_UART_EX = 0x9B, //!< Communication Interfaces : Single-wire UART Extend
        BM_CMD_ADDR_SINGLE_WIRE_UART_PEC= 0xE3, //!< Communication Interfaces : Single-wire UART    with PEC
        BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX=0xEB,//!< Communication Interfaces : Single-wire UART Extend with PEC

        BM_CMD_ADDR_SPI                 = 0x94, //!< Communication Interfaces : SPI
        BM_CMD_ADDR_SPI_EX              = 0x9C, //!< Communication Interfaces : SPI   Extend
        BM_CMD_ADDR_SPI_PEC             = 0xE4, //!< Communication Interfaces : SPI                 with PEC
        BM_CMD_ADDR_SPI_PEC_EX          = 0xEC, //!< Communication Interfaces : SPI   Extend        with PEC

        BM_CMD_ADDR_I2C                 = 0x95, //!< Communication Interfaces : I2C
        BM_CMD_ADDR_I2C_EX              = 0x9D, //!< Communication Interfaces : I2C   Extend
        BM_CMD_ADDR_I2C_PEC             = 0xE5, //!< Communication Interfaces : I2C                 with PEC
        BM_CMD_ADDR_I2C_PEC_EX          = 0xED, //!< Communication Interfaces : I2C   Extend        with PEC

        BM_CMD_ADDR_LOADER              = 0xA0, //!< Loader
        BM_CMD_ADDR_CHARGER             = 0xB0, //!< Charger
        BM_CMD_ADDR_LCD                 = 0xC0, //!< LCD
        BM_CMD_ADDR_PRN                 = 0xD0, //!< Printer
        BM_CMD_ADDR_PC                  = 0xF0, //!< PC
        BM_CMD_ADDR_ALL                 = 0xFF  //!< All
    }
    //! @}

    //! /name Command response type
    //! @{
    public enum BM_CMD_RT : ushort 
    { 
        BM_CMD_RT_NO_RESPONSE           = 0x0000,   //!< no need response
        BM_CMD_RT_NO_TIME_OUT           = 0xFFFF    //!< wait forever
    }
    //! @}


    //! \name command interface
    //! @{
    public interface IBMCommand
    {
        Boolean SendCommand(ESCommand tCommand);
    }
    //! @}

    //! command 
    public partial class ESCommand : IDisposable
    {
        //! /name private member
        //! @{
        private SafeID           m_ID;
        protected System.Byte   m_Command           = 0x00;
        protected  BM_CMD_TYPE  m_CommandType       = BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER;
        private BM_CMD_ADDR     m_Address           = BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER;
        private Byte            m_SubAddress = 0;
        private BM_CMD_RT       m_ResponseType      = BM_CMD_RT.BM_CMD_RT_NO_RESPONSE;
        private System.String   m_Description       = "";
        private frmCommandEditor m_Form             = null;
        private System.Boolean  m_CancelTransimtter = false;
        private System.Boolean  m_IsDisposed        = false;
        //! @}

        //! a copy constructor with data
        public virtual ESCommand CopyConstruct(System.Byte[] Datas)
        {
            return new ESCommand();
        }

        //! a copy constructor with data and command ID
        public virtual ESCommand CopyConstruct(System.Byte[] Datas, SafeID ID)
        {
            return new ESCommand(ID);
        }

        //! \brief constructor
        public ESCommand()
        {
            m_ID = null;
        }

        //! \brief constructor with command ID
        public ESCommand(SafeID ID)
        {
            m_ID = ID;
        }

        //! \brief a public method for clone a command withoud events related content
        public ESCommand ContentClone()
        {
            //BatteryManageCommand newCommand = (BatteryManageCommand)this.MemberwiseClone();

            ESCommand newCommand = CopyConstruct(this.Data, this.ID);

            newCommand.ContentCopy(this);

            newCommand.CommandRemovedEvent = null;
            newCommand.CommandWizardReportEvent = null;

            return newCommand;
        }

        public ESCommand ContentCopy(ESCommand Command)
        {
            if (null == Command)
            {
                return this;
            }

            this.Command = Command.Command;
            this.AddressValue = Command.AddressValue;
            this.SubAddress = Command.SubAddress;
            this.TimeOut = Command.TimeOut;
            this.IsPureListener = Command.IsPureListener;
            this.Data = Command.Data;
            this.Description = Command.Description;


            return this;
        }


        ~ESCommand()
        {
            this.Dispose();
        }

        public Boolean Disposed
        {
            get { return m_IsDisposed; }
        }

        public void Dispose()
        {
            if (!m_IsDisposed)
            {
                m_IsDisposed = true;
                try
                {
                    //! raising event
                    OnRemove();

                    //! release form
                    if (null != m_Form)
                    {
                        m_Form.Dispose();
                        m_Form = null;
                    }
                }
                catch (Exception) { }
                finally
                {
                    GC.SuppressFinalize(this);
                }
            }
        }
    }    





    //! command read block
    public class ESCommandReadBlock: ESCommand
    {
        protected Byte[] m_Data = null;

        //! \brief default constructor
        public ESCommandReadBlock()
            : base()
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ;
        }

        //! \brief constructor with data
        public ESCommandReadBlock(System.Byte[] Datas) 
            :base()
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ;
            if (null != Datas)
            {
                m_Data = (Byte[])Datas.Clone();
            }
        }

        //! \brief constructor with command ID
        public ESCommandReadBlock(SafeID ID)
            : base(ID)
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ;
        }

        //! \brief constructor with data and command ID
        public ESCommandReadBlock(System.Byte[] Datas, SafeID ID)
            : base(ID)
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ;
            if (null != Datas)
            {
                m_Data = (Byte[])Datas.Clone();
            }
        }

        //! override the base.CopyConstruct
        public override ESCommand CopyConstruct(System.Byte[] Datas)
        {
            return new ESCommandReadBlock(Datas);
        }

        //! override the base.CopyConstruct
        public override ESCommand CopyConstruct(System.Byte[] Datas, SafeID ID)
        {
            return new ESCommandReadBlock(Datas,ID);
        }

        public override Byte[] Data
        {
            get
            {
                return m_Data;
            }
        }

        //! property Command
        public override Byte Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }
    }

    //! command read word
    public class ESCommandReadWord : ESCommandReadBlock
    {
        //! \brief default constructor
        public ESCommandReadWord()
            : base()
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ;
            m_Data = new Byte[2];
        }

        //! \brief constructor with data
        public ESCommandReadWord(Byte[] Datas)
            : base()
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ;
            m_Data = new Byte[2];

            if (null != Datas)
            {
                if (Datas.Length >= 2)
                {
                    m_Data[0] = Datas[0];
                    m_Data[1] = Datas[1];
                }
            }
        }

        //! \brief constructor with command ID
        public ESCommandReadWord(SafeID ID)
            : base(ID)
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ;
            m_Data = new Byte[2];
        }

        //! \brief constructor with data and command ID
        public ESCommandReadWord(Byte[] Datas,SafeID ID)
            : base(ID)
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ;
            m_Data = new Byte[2];

            if (null != Datas)
            {
                if (Datas.Length >= 2)
                {
                    m_Data[0] = Datas[0];
                    m_Data[1] = Datas[1];
                }
            }
        }

        //! override the base.CopyConstruct
        public override ESCommand CopyConstruct(Byte[] Datas)
        {
            return new ESCommandReadWord(Datas);
        }

        //! override the base.CopyConstruct with command ID
        public override ESCommand CopyConstruct(Byte[] Datas, SafeID ID)
        {
            return new ESCommandReadWord(Datas, ID);
        }

        //! \brief constructor with word
        public ESCommandReadWord(UInt16 hwData) 
            : base()
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ;
            m_Data = new Byte[2];

            m_Data[0] = (Byte)(hwData & 0x00FF);
            m_Data[1] = (Byte)(hwData >> 8);
        }

        //! \brief constructor with word and command ID
        public ESCommandReadWord(UInt16 hwData, SafeID ID)
            : base(ID)
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ;
            m_Data = new Byte[2];

            m_Data[0] = (Byte)(hwData & 0x00FF);
            m_Data[1] = (Byte)(hwData >> 8);
        }

        public System.UInt16 DataValue
        {
            get 
            {
                if (null != m_Data)
                {
                    return BitConverter.ToUInt16(m_Data,0);
                }

                return 0;
            }
        }

        //! property Command
        public override Byte Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }
    }

    //! command write block
    public class ESCommandWriteBlock : ESCommand
    {
        protected System.Byte[] m_Data = null;

        //! \brief default constructor
        public ESCommandWriteBlock()
            : base()
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE;
        }

        //! \brief construcotr with data
        public ESCommandWriteBlock(Byte[] Datas)
            : base()
        {
            if (null != Datas)
            {
                m_Data = (Byte[])Datas.Clone();
            }

            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE;
        }

        //! \brief constructor command ID
        public ESCommandWriteBlock(SafeID ID) 
            : base(ID)
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE;
        }

        //! \brief construcotr with data and command ID
        public ESCommandWriteBlock(Byte[] Datas, SafeID ID)
            : base(ID)
        {
            if (null != Datas)
            {
                m_Data = (Byte[])Datas.Clone();
            }

            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE;
        }

        //! override the base.CopyConstruct
        public override ESCommand CopyConstruct(Byte[] Datas)
        {
            return new ESCommandWriteBlock(Datas);
        }

        //! override the base.CopyConstruct with command ID
        public override ESCommand CopyConstruct(Byte[] Datas, SafeID ID)
        {
            return new ESCommandWriteBlock(Datas,ID);
        }

        public override Byte[] Data
        {
            get
            {
                return m_Data;
            }
            set
            {
                if (value != null)
                {
                    m_Data = (Byte[])value.Clone();
                }
            }
        }

        //! property Command
        public override Byte Command
        {
            get { return m_Command; }
            set { m_Command = value ; }
        }
    }

    

    //! command write word
    public class ESCommandWriteWord : ESCommandWriteBlock
    {
        //! \brief default constructor
        public ESCommandWriteWord() 
            : base()
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE;
            m_Data = new Byte[2];
        }

        //! \brief constructor with data
        public ESCommandWriteWord(UInt16 hwData)
            : base()
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE;
            m_Data = new Byte[2];

            m_Data[0] = (Byte)(hwData & 0x00FF);
            m_Data[1] = (Byte)(hwData >> 8);
        }

        //! \brief constructor with command ID
        public ESCommandWriteWord(SafeID ID)
            : base(ID)
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE;
            m_Data = new Byte[2];
        }

        //! \brief constructor with data and command ID
        public ESCommandWriteWord(UInt16 hwData,SafeID ID)
            : base(ID)
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE;
            m_Data = new Byte[2];

            m_Data[0] = (Byte)(hwData & 0x00FF);
            m_Data[1] = (Byte)(hwData >> 8);
        }

        //! \brief constructor with word
        public ESCommandWriteWord(Byte[] Datas)
            : base()
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE;
            m_Data = new Byte[2];
            if (null != Datas)
            {
                if (Datas.Length >= 2)
                {
                    m_Data[0] = Datas[0];
                    m_Data[1] = Datas[1];
                }
            }
        }

        //! \brief constructor with word and command ID
        public ESCommandWriteWord(Byte[] Datas,SafeID ID)
            : base(ID)
        {
            m_CommandType = BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE;
            m_Data = new Byte[2];
            if (null != Datas)
            {
                if (Datas.Length >= 2)
                {
                    m_Data[0] = Datas[0];
                    m_Data[1] = Datas[1];
                }
            }
        }

        //! override the base.CopyConstruct
        public override ESCommand CopyConstruct(Byte[] Datas)
        {
            return new ESCommandWriteWord(Datas);
        }

        //! override the base.CopyConstruct with command ID
        public override ESCommand CopyConstruct(Byte[] Datas, SafeID ID)
        {
            return new ESCommandWriteWord(Datas, ID);
        }

        public override Byte[] Data
        {
            get 
            {
                return m_Data;
            }
            set
            {
                if (null != value)
                {
                    if (value.Length >= 2)
                    {
                        m_Data[0] = value[0];
                        m_Data[1] = value[1];
                    }
                }
            }
        }

        public UInt16 DataValue
        {
            get
            {
                if (null != m_Data)
                {
                    return BitConverter.ToUInt16(m_Data, 0);
                }

                return 0;
            }
            set
            {
                m_Data[0] = (Byte)(value & 0x00FF);
                m_Data[1] = (Byte)(value >> 8);
            }
        }

        //! property Command
        public override Byte Command
        {
            get { return m_Command; }
            set { m_Command = value ; }
        }
    }
}
