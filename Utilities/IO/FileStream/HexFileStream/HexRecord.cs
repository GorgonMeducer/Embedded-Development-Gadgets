using System;
using System.Collections.Generic;
using System.Text;

namespace ESnail.Utilities.IO
{

    //! \name record structure in Intel-Hexadecimal file
    //! @{
    internal abstract class HEXRecord
    {
        protected Boolean m_bAvailable = false;

        //! \brief property for check whether this record is available or not.
        public Boolean Available
        {
            get { return m_bAvailable; }
        }


        //! \name record type
        //! @{
        internal enum Type : byte
        { 
            DATA_RECORD                     = 0,            //!< data record
            END_OF_FILE_RECORD              = 1,            //!< end of file record
            EXTEND_SEGMENT_ADDRESS_RECORD   = 2,            //!< extend segment address record
            START_SEGMENT_ADDRESS_RECORD    = 3,            //!< start segment address record
            EXTEND_LINEAR_ADDRESS_RECORD    = 4,            //!< extend linear address record
            START_LINEAR_ADDRESS_RECORD     = 5             //!< start linear address record
        }
        //! @}

        //! \brief property for getting record type
        /*! \note Each record has a RECTYP field which specifies the record type of this record. 
         *        The RECTYP field is used to interpret the remaining information within the 
         *        record. The encoding for all the current record types are:
         *          ’00’ Data Record
         *          ’01’ End of File Record
         *          ’02’ Extended Segment Address Record
         *          ’03’ Start Segment Address Record
         *          ’04’ Extended Linear Address Record
         *          ’05’ Start Linear Address Record
         */
        public abstract Type RecordType
        {
            get;
        }

        /*! \note Each record has a RECLEN field which specifies the number of bytes of 
 *        information or data which follows the RECTYP field of the record. Note that 
 *        one data byte is represented by two ASCII characters. The maximum value of 
 *        the RECLEN field is hexadecimal ’FF’ or 255.
 */
        public abstract Int32 RecordLength
        {
            get;
        }

        /*! \note Each record has a LOAD OFFSET field which specifies the 16-bit starting load 
         *        offset of the data bytes, therefore this field is only used for Data Records. 
         *        In other records where this field is not used, it should be coded as four 
         *        ASCII zero characters (’0000’ or 03030303OH).
         */
        public virtual UInt16 LoadOffset
        {
            get { return 0; }
        }

        /*! \note Each record has a variable length INFO/DATA field, it consists of zero or 
         *        more bytes encoded as pairs of hexadecimal digits. The interpretation of 
         *        this field depends on the RECTYP field.
         */
        public abstract Byte[] Data
        {
            get;
        }


        //! \brief method to parse record string 
        //! \param tHexRecord a record string
        //! \return a reference to a new record object
        public static HEXRecord Parse(String tHexRecord)
        {
            //! check input 
            if (null == tHexRecord)
            {
                return null;
            }
            
            tHexRecord = tHexRecord.Trim().ToUpper();
            if ("" == tHexRecord)
            {
                return null;
            }

            //! \brief check record head
            if (':' != tHexRecord[0])
            {
                return null;
            }
            if (!IsHexNumber(tHexRecord.Substring(1)))
            {
                return null;
            }

            Int32 tRecordLength = 0;
            Int32 tCheckSUM = 0;
            Int32 tLoadOffSet = 0;
            Byte tRecordType = 0;
            Byte[] tData = null;

            //! try to get record length
            try
            {
                tRecordLength = Int32.Parse(tHexRecord.Substring(1, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            catch (Exception )
            {
                return null;
            }
            tCheckSUM += tRecordLength;

            //! check record string length
            if (tHexRecord.Length < (11 + tRecordLength * 2))
            { 
                //! incomplete record
                return null;
            }

            //! try to get LoadOffset
            try
            {
                tLoadOffSet = UInt16.Parse(tHexRecord.Substring(3,4), System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            catch (Exception )
            {
                return null;
            }
            tCheckSUM += tLoadOffSet >> 8;
            tCheckSUM += tLoadOffSet & 0xFF;

            //! try to get record type
            try
            {
                tRecordType = Byte.Parse(tHexRecord.Substring(7, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            catch (Exception Err)
            {
                Err.ToString();
                return null;
            }

            //! check record type value
            if (tRecordType > 5)
            {
                return null;
            }
            else if (
                        ((Type)tRecordType != Type.DATA_RECORD)
                    &&  (tLoadOffSet != 0)
                    )
            {
                //! illegal record loadoffset
                return null;
            }

            tCheckSUM += tRecordType;
            
            //! get data byte
            tData = new Byte[tRecordLength];
            if (null == tData)
            {
                //! failed to allocated memory
                return null;
            }

            //! read all data bytes in a record
            for (Int32 n = 0; n < tData.Length; n++)
            {
                try
                {
                    tData[n] = Byte.Parse(tHexRecord.Substring(9 + n * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                }
                catch (Exception )
                {
                    return null;
                }
                tCheckSUM += tData[n];
            }

            //! get check sum
            try
            {
                tCheckSUM += Byte.Parse(tHexRecord.Substring(9 + tData.Length * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            catch (Exception Err)
            {
                Err.ToString();
                return null;
            }
            if (0 != (tCheckSUM & 0xFF))
            {
                //! check sum error
                return null;
            }

            //! \brief check record type
            switch ((Type)tRecordType)
            { 
                case Type.DATA_RECORD:                              //!< data record
                    return new DataRecord((UInt16)tLoadOffSet, tData);

                case Type.END_OF_FILE_RECORD:                       //!< EOF record
                    return new EndOfFileRecord();

                case Type.EXTEND_LINEAR_ADDRESS_RECORD:             //!< extend linear address record
                    return new ExtendLinearAddressRecord(tData);    

                case Type.EXTEND_SEGMENT_ADDRESS_RECORD:            //!< extend segment address
                    return new ExtendSegmentAddressRecord(tData);   

                case Type.START_LINEAR_ADDRESS_RECORD:              //!< start linear address record
                    return new StartLinearAddressRecord(tData);     

                case Type.START_SEGMENT_ADDRESS_RECORD:
                    return new StartSegmentAddressRecord(tData);        //!< start segment address

                default:
                    //! brief no such type
                    return null;
            }
            
            return null;
        }

        //! \brief get HEX record String
        public override String ToString()
        {
            StringBuilder tsbHexRecord = new StringBuilder();
            if (m_bAvailable)
            {
                return null;
            }

            UInt32 tCheckSUM = 0;

            //! record head
            tsbHexRecord.Append(':');

            //! record length
            tsbHexRecord.Append(this.RecordLength.ToString("X2"));
            tCheckSUM += (UInt32)RecordLength;

            //! record load offset
            tsbHexRecord.Append(this.LoadOffset.ToString("X4"));
            tCheckSUM += (UInt32)(LoadOffset & 0x00FF);
            tCheckSUM += (UInt32)(LoadOffset >> 8);

            //! record type
            tsbHexRecord.Append(((Byte)this.RecordType).ToString("X2"));
            tCheckSUM += (Byte)RecordType;

            //! record data
            foreach (Byte tItem in Data)
            {
                tsbHexRecord.Append(tItem.ToString("X2"));
                tCheckSUM += tItem;
            }

            //! record check sum
            tsbHexRecord.Append(((Byte)((UInt16)0x100 - (UInt16)(tCheckSUM & 0xFF))).ToString("X2"));

            return tsbHexRecord.ToString();
        }


        //! \brief method for check whecher string only consist of hex value
        static public Boolean IsHexNumber(String tHexString)
        {
            if (null == tHexString)
            {
                return false;
            }

            tHexString = tHexString.Trim().ToUpper();
            if ("" == tHexString)
            {
                return false;
            }

            foreach (Char tSymble in tHexString.ToCharArray())
            { 
                if (!Char.IsNumber(tSymble))
                {
                    if ((tSymble < 'A') || (tSymble > 'F'))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }
    //! @}

    /*! \note The End of File Record specifies the end of the hexadecimal object 
     *        file.
     */
    //! \name the End of File record
    //! @{
    internal class EndOfFileRecord : HEXRecord
    {
        //! \brief length always is zero.
        public override int RecordLength
        {
            get { return 0; }
        }


        //! \brief get record type
        public override HEXRecord.Type RecordType
        {
            get { return Type.END_OF_FILE_RECORD; }
        }

        //! \brief no data
        public override byte[] Data
        {
            get
            {
                return new Byte[0];
            }
        }
    }
    //! @}

    /*! \note The 32-bit Extended Linear Address Record is used to specify bits 
     *        16-31 of the Linear Base Address (LBA), where bits 0-15 of the LBA 
     *        are zero. Bits 16-31 of the LBA are referred to as the Upper Linear 
     *        Base Address (ULBA). The absolute memory address of a content byte 
     *        in a subsequent Data Record is obtained by adding the LBA to an 
     *        offset calculated by adding the LOAD OFFSET field of the containing 
     *        Data Record to the index of the byte in the Data Record (0, 1, 2, 
     *        ... n). This offset addition is done modulo 4G (i.e., 32-bits), 
     *        ignoring any carry, so that offset wrap-around loading (from 
     *        OFFFFFFFFH to OOOOOOOOOH) results in wrapping around from the end 
     *        to the beginning of the 4G linear address defined by the LBA. The 
     *        linear address at which a particular byte is loaded is calculated 
     *        as:
     *        
     *        (LBA + DRLO + DRI) MOD 4G
     *        
     *        where:
     *        DRLO is the LOAD OFFSET field of a Data Record.
     *        DRI is the data byte index within the Data Record.
     *        
     *        When an Extended Linear Address Record defines the value of LBA, 
     *        it may appear anywhere within a 32-bit hexadecimal object file. 
     *        This value remains in effect until another Extended Linear Address 
     *        Record is encountered. The LBA defaults to zero until an Extended 
     *        Linear Address Record is encountered.
     */
    //! \name the Extend Linear Address Record
    //! @{
    internal class ExtendLinearAddressRecord : HEXRecord
    {
        private UInt16 m_UpperLinearBaseAddress = 0;
        private Byte[] m_Data = null;

        public ExtendLinearAddressRecord(Byte[] tData)
        {
            if (null == tData)
            {
                return;
            }
            else if (2 != tData.Length)
            {
                return ;
            }

            m_Data = tData;

            try
            {
                Byte[] tTemp = new Byte[2];
                tTemp[0] = tData[1];
                tTemp[1] = tData[0];

                m_UpperLinearBaseAddress = BitConverter.ToUInt16(tTemp, 0);
            }
            catch (Exception )
            {
                return;
            }

            m_bAvailable = true;
        }

        //! \brief record data length should always be 2
        public override Int32 RecordLength
        {
            get 
            {
                if (!m_bAvailable)
                {
                    return 0;
                }
                return 2; 
            }
        }


        //! \brief get record type
        public override HEXRecord.Type RecordType
        {
            get { return Type.EXTEND_LINEAR_ADDRESS_RECORD; }
        }

        //! \brief get data
        public override Byte[] Data
        {
            get
            {
                if (!m_bAvailable)
                {
                    return null;
                }

                return m_Data;
            }
        }

        //! \brief get the Upper Linear Base Address
        public UInt32 UpperLinearBaseAddress
        {
            get
            {
                if (!m_bAvailable)
                {
                    return 0;
                }

                return ((UInt32)m_UpperLinearBaseAddress) << 16;
            }
        }

    }
    //! @}

    //! \name the Start Segment Address Record
    //! @{
    internal class StartSegmentAddressRecord : HEXRecord
    {
        private UInt32 m_StartSegmentAddress = 0;
        private Byte[] m_Data = null;

        public StartSegmentAddressRecord(Byte[] tData)
        {
            if (null == tData)
            {
                return;
            }
            else if (4 != tData.Length)
            {
                return;
            }

            m_Data = tData;

            try
            {
                Byte[] tTemp = new Byte[4];
                tTemp[0] = tData[3];
                tTemp[1] = tData[2];
                tTemp[2] = tData[1];
                tTemp[3] = tData[0];

                m_StartSegmentAddress = BitConverter.ToUInt32(tTemp, 0);
            }
            catch (Exception )
            {
            }

            m_bAvailable = true;
        }

        //! \brief always return 4
        public override int RecordLength
        {
            get 
            {
                if (!m_bAvailable)
                {
                    return 0;
                }

                return 4;
            }
        }


        //! \brief get record type
        public override HEXRecord.Type RecordType
        {
            get { return Type.START_SEGMENT_ADDRESS_RECORD; }
        }

        //! \brief get data
        public override Byte[] Data
        {
            get
            {
                if (!m_bAvailable)
                {
                    return null;
                }

                return m_Data;
            }
        }

        //! \brief get start segment address
        public UInt32 StartSegmentAddress
        {
            get
            {
                if (!m_bAvailable)
                {
                    return 0;
                }

                return m_StartSegmentAddress & 0x000FFFFF;
            }
        }

        public UInt16 CS
        {
            get
            {
                if (!m_bAvailable)
                {
                    return 0;
                }

                return (UInt16)(m_StartSegmentAddress & 0x0000FFFF);
            }
        }

        public UInt16 IP
        {
            get
            {
                if (!m_bAvailable)
                {
                    return 0;
                }

                return (UInt16)((m_StartSegmentAddress >> 16) & 0x000F);
            }
        }

    }
    //! @}

    /*! \note The Data Record provides a set of hexadecimal digits that represent 
     *        the ASCII code for data bytes that make up a portion of a memory image. 
     *        The method for calculating the absolute address (linear in the 8-bit 
     *        and 32-bit case and segmented in the 16-bit case) for each byte of data 
     *        is described in the discussions of the Extended Linear Address Record 
     *        and the Extended Segment Address Record.
     */
    //! \name the Data Record
    //! @{
    internal class DataRecord : HEXRecord
    {
        private Byte[] m_Data = null;
        private UInt16 m_LoadOffset = 0;

        //! \brief constructor for the Data Record
        public DataRecord(UInt16 tLoadOffset, Byte[] tData)
        {
            if (null == tData)
            {
                return;
            }
            else if (tData.Length > 255)
            {
                return;
            }

            m_Data = tData;
            m_LoadOffset = tLoadOffset;

            m_bAvailable = true;
        }

        //! \brief get record data length
        public override int RecordLength
        {
            get 
            {
                if (null == m_Data)
                {
                    return 0;
                }

                return m_Data.Length;
            }
        }

        //! \brief get load offset
        public override UInt16 LoadOffset
        {
            get 
            {
                if (!m_bAvailable)
                {
                    return 0;
                }

                return m_LoadOffset;
            }
        }

        //! \brief get record type
        public override HEXRecord.Type RecordType
        {
            get { return Type.DATA_RECORD; }
        }

        //! \brief get record data
        public override Byte[] Data
        {
            get
            {
                if (!m_bAvailable)
                {
                    return null;
                }

                return m_Data;
            }
        }

    }
    //! @}

    /*! \note The 16-bit Extended Segment Address Record is used to specify bits 
     *        4-19 of the Segment Base Address (SBA), where bits 0-3 of the SBA 
     *        are zero. Bits 4-19 of the SBA are referred to as the Upper Segment 
     *        Base Address (USBA). The absolute memory address of a content byte 
     *        in a subsequent Data Record is obtained by adding the SBA to an 
     *        offset calculated by adding the LOAD OFFSET field of the containing 
     *        Data Record to the index of the byte in the Data Record (0, 1, 2, 
     *        ... n). This offset addition is done modulo 64K (i.e., 16-bits), 
     *        ignoring any carry, so that offset wrap-around loading (from OFFFFH 
     *        to OOOOOH) results in wrapping around from the end to the beginning 
     *        of the 64K segment defined by the SBA. The address at which a 
     *        particular byte is loaded is calculated as:
     *        
     *        SBA + ([DRLO + DRI] MOD 64K)
     *        
     *        where:
     *        DRLO is the LOAD OFFSET field of a Data Record.
     *        DRI is the data byte index within the Data Record.
     *        
     *        When an Extended Segment Address Record defines the value of SBA, 
     *        it may appear anywhere within a 16-bit hexadecimal object file. This 
     *        value remains in effect until another Extended Segment Address Record 
     *        is encountered. The SBA defaults to zero until an Extended Segment 
     *        Address Record is encountered.
     */
    //! \name the Extend Segment Address Record
    //! @{
    internal class ExtendSegmentAddressRecord : HEXRecord
    {
        private UInt16 m_ExtendSegmentUpperBaseAddress = 0;
        private Byte[] m_Data = null;

        public ExtendSegmentAddressRecord(Byte[] tData)
        {
            if (null == tData)
            {
                return;
            }
            else if (2 != tData.Length)
            {
                return;
            }

            m_Data = tData;

            try
            {
                Byte[] tTemp = new Byte[2];
                tTemp[0] = tData[1];
                tTemp[1] = tData[0];

                m_ExtendSegmentUpperBaseAddress = BitConverter.ToUInt16(tTemp, 0);
            }
            catch (Exception )
            {
                return;
            }

            m_bAvailable = true;
        }

        //! \brief always return 2
        public override int RecordLength
        {
            get 
            {
                if (!m_bAvailable)
                {
                    return 0;
                }

                return 2;
            }
        }


        //! \brief get record type
        public override HEXRecord.Type RecordType
        {
            get { return Type.EXTEND_SEGMENT_ADDRESS_RECORD; }
        }

        //! \brief get data
        public override Byte[] Data
        {
            get
            {
                if (!m_bAvailable)
                {
                    return null;
                }

                return m_Data;
            }
        }

        //! \brief get extend segment base address
        public UInt32 ExtendSegmentBaseAddress
        {
            get
            {
                if (!m_bAvailable)
                {
                    return 0;
                }

                return ((UInt32)m_ExtendSegmentUpperBaseAddress) << 4;
            }
        }

    }
    //! @}
    
    /*! \note The Start Linear Address Record is used to specify the execution start 
     *        address for the object file. The value given is the 32-bit linear address 
     *        for the EIP register. Note that this record only specifies the code 
     *        address within the 32-bit linear address space of the 80386. If the code 
     *        is to start execution in the real mode of the 80386, then the Start 
     *        Segment Address Record should be used instead, since that record specifies 
     *        both the CS and IP register contents necessary for real mode.
     */
    //! \name the Start Linear Address Record
    //! @{
    internal class StartLinearAddressRecord : HEXRecord
    {
        private UInt32 m_StartLinearAddress = 0;
        private Byte[] m_Data = null;

        public StartLinearAddressRecord(Byte[] tData)
        {
            if (null == tData)
            {
                return;
            }
            else if (4 != tData.Length)
            {
                return;
            }

            m_Data = tData;

            try
            {
                Byte[] tTemp = new Byte[4];
                tTemp[0] = tData[3];
                tTemp[1] = tData[2];
                tTemp[2] = tData[1];
                tTemp[3] = tData[0];

                m_StartLinearAddress = BitConverter.ToUInt32(tTemp, 0);
            }
            catch (Exception )
            {
                return ;
            }

            m_bAvailable = true;
        }

        //! \brief always return 4
        public override Int32 RecordLength
        {
            get 
            {
                if (!m_bAvailable)
                {
                    return 0;
                }

                return 4;
            }
        }

        //! \brief get record type
        public override HEXRecord.Type RecordType
        {
            get { return Type.START_LINEAR_ADDRESS_RECORD; }
        }

        //! \brief get data
        public override Byte[] Data
        {
            get
            {
                if (!m_bAvailable)
                {
                    return null;
                }

                return m_Data;
            }
        }

        //! \brief get start linear address (EIP)
        public UInt32 StartLinearAddress
        {
            get
            {
                if (!m_bAvailable)
                {
                    return 0;
                }

                return m_StartLinearAddress;
            }
        }

    }
    //! @}
}
