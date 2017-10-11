using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ESnail.Utilities.IO
{
    public interface IESFileStream
    {
        void Close();
        FileMode FileMode
        {
            get;
        }

        FileAccess FileAccess
        {
            get;
        }

        FileShare FileShare
        {
            get;
        }

        Boolean Available
        {
            get;
        }
    }

    public abstract class ESFileStream : Stream, IDisposable, IESFileStream
    {
        protected FileMode m_FileMode = FileMode.Open;
        protected String m_Path = null;
        protected FileAccess m_FileAccess = FileAccess.ReadWrite;
        protected FileShare m_FileShare = FileShare.None;
        protected Boolean m_Available = false;
        protected FileStream m_File = null;

        public ESFileStream(String tPath, FileMode tMode, FileAccess tAccess)
        {
            Create(tPath, tMode, tAccess, FileShare.None);
        }

        public ESFileStream(String tPath, FileMode tMode, FileAccess tAccess, FileShare tFileShare)
        {
            Create(tPath, tMode, tAccess, tFileShare);
        }

        private void Create(String tPath, FileMode tMode, FileAccess tAccess, FileShare tFileShare)
        {
            m_FileMode = tMode;
            if (null == tPath)
            {
                throw new ArgumentNullException("tPath", "File path string should not be a null.");
            }
            else if ("" == tPath.Trim())
            {
                throw new ArgumentException("tFilePath", "path is an empty string ,contains only white space, or contains one or more invalid characters.");
            }

            m_Path = tPath;
            m_FileAccess = tAccess;
            m_FileShare = tFileShare;

            Initialize();
        }

        private void Initialize()
        {
            try
            {
                m_File = new FileStream(m_Path, m_FileMode, m_FileAccess, m_FileShare);
            }
            catch (Exception Err)
            {
                throw Err;
            }

            m_Available = true;
        }

        #region IDisposable Members

        public Boolean m_Disposed = false;

        public Boolean Disposed
        {
            get { return m_Disposed; }
        }

        ~ESFileStream()
        {
            Dispose();
        }

        public new void Dispose()
        {
            if (!m_Disposed)
            {
                m_Disposed = true;

                try
                {
                    _Dispose();
                }
                catch (Exception) { }

                try
                {
                    Close();
                }
                catch (Exception) { }
                finally
                {
                    GC.SuppressFinalize(this);
                }
            }
        }

        protected virtual void _Dispose()
        {

        }

        #endregion


        public virtual Boolean AutoCopyToClipBoard
        {
            get;
            set;
        }

        public override void Close()
        {
            try
            {
                if (null != m_File)
                {
                    m_File.Close();
                    m_File.Dispose();
                }
            }
            catch (Exception) { }
            finally
            {
                m_File = null;
                m_Available = false;
            }
        }

        public virtual FileMode FileMode
        {
            get { return m_FileMode; }
        }

        public virtual FileAccess FileAccess
        {
            get { return m_FileAccess; }
        }

        public virtual FileShare FileShare
        {
            get { return m_FileShare; }
        }

        public virtual Boolean Available
        {
            get { return m_Available; }
        }

        public virtual Int32 Offset
        {
            get;
            set;
        }

        public override bool CanRead
        {
            get 
            {
                if (null != m_File)
                {
                    return m_File.CanRead;
                }

                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                if (null != m_File)
                {
                    return m_File.CanWrite;
                }

                return false;
            }
        }



        public override bool CanSeek
        {
            get
            {
                if (null != m_File)
                {
                    return m_File.CanSeek;
                }

                return false;
            }
        }

        public override void Flush()
        {
            if (null != m_File)
            {
                m_File.Flush();
            }
        }

        public override long Length
        {
            get 
            {
                if (null != m_File)
                {
                    return m_File.Length;
                }

                return -1;
            }
        }

        public override long Position
        {
            get
            {
                if (null != m_File)
                {
                    return m_File.Position;
                }
                return -1;
            }
            set
            {
                if (null != m_File)
                {
                    m_File.Position = value;
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (null != m_File)
            {
                return m_File.Read(buffer, offset, count);
            }

            return -1;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (null != m_File)
            {
                return m_File.Seek(offset, origin);
            }

            return -1;
        }

        public override void SetLength(long value)
        {
            if (null != m_File)
            {
                m_File.SetLength(value);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (null != m_File)
            {
                m_File.Write(buffer, offset, count);
            }
        }

        public Boolean EndOfStream
        {
            get {
                if (null == m_File)
                {
                    return true;
                }

                return m_File.Position >= (m_File.Length - 1);
            }
        }
    }

    public abstract class ESMemoryFileStream : ESFileStream
    {
        protected VirtualMemorySpace m_MemorySpace = new VirtualMemorySpace();
        protected UInt32 m_AccessPointer = 0;

        public ESMemoryFileStream(String tPath, FileMode tMode, FileAccess tAccess, FileShare tFileShare)
            : base(tPath, tMode, tAccess, tFileShare)
        {
            Initialization();
        }

        public ESMemoryFileStream(String tPath, FileMode tMode, FileAccess tAccess)
            : base(tPath, tMode, tAccess)
        {
            Initialization();
        }

        private void Initialization()
        { 
            if (!this.Available)
            {
                m_Available = false;
                return;
            }

            if (m_FileAccess != FileAccess.Read)
            {
                m_MemorySpace.BeginUpdateMemorySpaceEvent += new VirtualMemorySpaceImage.BeginUpdateMemorySpace(OnBeginUpdateMemorySpaceEvent);
                m_MemorySpace.EndUpdateMemorySpaceEvent += new VirtualMemorySpaceImage.EndUpdateMemorySpace(OnEndUpdateMemorySpaceEvent);
                m_MemorySpace.UpdateMemorySpaceEvent += new VirtualMemorySpaceImage.UpdateMemorySpace(OnUpdateMemorySpaceEvent);

            }

            if (m_FileAccess != FileAccess.Write)
            {
                m_MemorySpace.LoadMemoryBlockEvent += new VirtualMemorySpaceImage.LoadMemoryBlock(LoadMemoryBlockFromTargetFile);

                FillMemorySpace();
            }
            
        }

        protected abstract void OnWriteMemoryToFile();

        public override void Close()
        {
            if (null != m_MemorySpace)
            {
                //! memory space
                m_MemorySpace.BeginUpdateMemorySpaceEvent -= new VirtualMemorySpaceImage.BeginUpdateMemorySpace(OnBeginUpdateMemorySpaceEvent);
                m_MemorySpace.EndUpdateMemorySpaceEvent -= new VirtualMemorySpaceImage.EndUpdateMemorySpace(OnEndUpdateMemorySpaceEvent);
                m_MemorySpace.UpdateMemorySpaceEvent -= new VirtualMemorySpaceImage.UpdateMemorySpace(OnUpdateMemorySpaceEvent);
                m_MemorySpace.LoadMemoryBlockEvent -= new VirtualMemorySpaceImage.LoadMemoryBlock(LoadMemoryBlockFromTargetFile);

                do {
                    if (m_FileAccess == System.IO.FileAccess.Read)
                    {
                        break;
                    }
                    //! write memory block to the file
                    OnWriteMemoryToFile();

                } while (false);

                m_MemorySpace = null;
            }

            base.Close();
        }

        protected abstract Boolean LoadMemoryBlockFromTargetFile(UInt32 tTargetAddress, ref Byte[] tData, Int32 tSize);

        protected abstract void FillMemorySpace();

        protected abstract void OnUpdateMemorySpaceEvent(UInt32 tAddress, Byte[] tData);

        protected abstract void OnEndUpdateMemorySpaceEvent();

        protected abstract void OnBeginUpdateMemorySpaceEvent();

        //! \brief hex stream length (not the file length)
        public override long Length
        {
            get
            {
                return m_MemorySpace.Size;
            }
        }

        public override void Flush()
        {
            if ((null == m_File) || (null == m_MemorySpace))
            {
                throw new ObjectDisposedException("HexFileStream");
            }

            m_MemorySpace.Update();
            m_File.Flush();
        }

        public MemoryBlock[] MemoryBlocks
        {
            get { return m_MemorySpace.MemoryBlocks; }
        }

        public VirtualMemorySpaceImage MemorySpace
        {
            get { return m_MemorySpace; }
        }

        public virtual Boolean Write(MemoryBlock tBlock)
        {
            return m_MemorySpace.Write(tBlock);
        }

        public virtual Boolean Write(MemoryBlock[] tBlocks)
        {
            return m_MemorySpace.Write(tBlocks);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (null == buffer || 0 == count)
            {
                return;
            }

            if (m_MemorySpace.Write((UInt32)offset, buffer))
            {
                m_AccessPointer += (UInt32)offset;
            }
        }

        public override void WriteByte(byte value)
        {
            Byte[] tBuffer = new Byte[1];
            tBuffer[0] = value;
            if (m_MemorySpace.Write(m_AccessPointer, tBuffer))
            {
                m_AccessPointer++;
            }
        }

        //! \brief read bytes
        public override Int32 Read(Byte[] array, Int32 offset, Int32 count)
        {
            if (m_FileAccess == FileAccess.Write)
            {
                throw new NotSupportedException(); ;
            }
            else if ((null == m_File) || (null == m_MemorySpace))
            {
                throw new ObjectDisposedException("HexFileStream");
            }

            else if (!m_File.CanRead)
            {
                throw new NotSupportedException();
            }

            else if (null == array)
            {
                throw new ArgumentNullException();
            }
            else if ((0 == array.Length) || (0 == count))
            {
                return 0;
            }
            else if ((offset < 0) || (count < 0) || (array.Length < (offset + count)))
            {
                throw new ArgumentOutOfRangeException();
            }

            Byte[] tBuffer = new Byte[count];
            if (!m_MemorySpace.Read(m_AccessPointer, tBuffer))
            {
                throw new IOException();
            }
            m_AccessPointer += (UInt32)count;

            //! copy buffer
            tBuffer.CopyTo(array, offset);

            return count;
        }

        //! \brief read one byte from current position
        public override Int32 ReadByte()
        {
            Byte[] tBuffer = new Byte[1];

            if (1 == Read(tBuffer, 0, 1))
            {
                return tBuffer[0];
            }

            throw new IOException();
        }


        //! \brief get/set current address
        public override Int64 Position
        {
            get
            {
                return m_AccessPointer;
            }
            set
            {
                Seek(value, SeekOrigin.Begin);
            }
        }

        //! \brief set hex file max length
        public override void SetLength(Int64 value)
        {
            if ((null == m_File) || (null == m_MemorySpace))
            {
                throw new ObjectDisposedException("HexFileStream");
            }

            else if ((!m_File.CanWrite) || (!m_File.CanSeek))
            {
                throw new NotSupportedException();
            }

            else if ((value < 0) || (value > UInt32.MaxValue))
            {
                throw new ArgumentException("Out of memory space range");
            }

            m_MemorySpace.SpaceLength = (UInt32)value;
        }

        //! \brief seek access pointer
        public override Int64 Seek(Int64 offset, SeekOrigin origin)
        {
            if (null == m_File)
            {
                throw new ObjectDisposedException("HexFileStream");
            }
            else if (!m_File.CanSeek)
            {
                throw new NotSupportedException();
            }

            switch (origin)
            {
                case SeekOrigin.Begin:
                    m_AccessPointer = 0;
                    break;
                case SeekOrigin.Current:
                    break;
                case SeekOrigin.End:
                    m_AccessPointer = (UInt32)m_MemorySpace.Size;
                    break;
            }

            if (
                    ((offset + (Int64)m_AccessPointer) > UInt32.MaxValue)
                || ((offset + (Int64)m_AccessPointer) < 0)
               )
            {
                throw new ArgumentException("Out of memory space range.");
            }

            m_AccessPointer = (UInt32)((Int64)m_AccessPointer + offset);

            return m_AccessPointer;
        }
    }

    public abstract class ESRecordFileStream : ESFileStream
    {
        protected StreamReader m_StreamReader = null;
        protected StreamWriter m_StreamWriter = null;

        public ESRecordFileStream(String tPath, FileMode tMode, FileAccess tAccess, FileShare tFileShare)
            : base(tPath, tMode, tAccess, tFileShare)
        {
            Initialization();
        }

         

        private void Initialization()
        { 
            if (!this.Available)
            {
                m_Available = false;
                return;
            }
            m_Available = false;

            try
            {
                if (m_File.CanRead)
                {
                    m_StreamReader = new StreamReader(m_File);
                }
                if (m_File.CanWrite)
                {
                    m_StreamWriter = new StreamWriter(m_File);
                }
            }
            catch (Exception Err)
            {
                throw Err;
            }

            m_Available = true;
        }

        public override void Close()
        {
            try
            {
                if (null != m_StreamWriter)
                {
                    m_StreamWriter.Close();
                }
            }
            catch (Exception) { }

            base.Close();
        }

        public String ReadLine()
        {
            do
            {
                if (!this.Available)
                {
                    break ;
                }
                if (null == m_File)
                {
                    break;
                }
                if (!m_File.CanRead)
                {
                    break;
                }

                StringBuilder sbLine = new StringBuilder();
                do
                {
#if false
                    Byte[] tBuffer = new Byte[sizeof(Char)];

                    for (UInt32 n = 0; n < tBuffer.Length;n++)
                    {
                        int tTemp = m_File.ReadByte();
                        if (-1 == tTemp)
                        {
                            goto ReadLineExit;
                        }
                        tBuffer[n] = (Byte)tTemp;
                    }

                    Char tLetter = BitConverter.ToChar(tBuffer, 0);
#else
                    int tByte = this.ReadByte();
                    if (-1 == tByte)
                    {
                        break;
                    }

#endif


                    sbLine.Append((Char)tByte); //(Char)tTemp);
                    do
                    {
                        String tTemp = sbLine.ToString();
                        Int32 tIndex = tTemp.IndexOfAny(Environment.NewLine.ToCharArray());
                        if (tIndex != -1)
                        {
                            return tTemp.Substring(0, tIndex);
                        }
                    } while (false);

                } while (true);


                return sbLine.ToString();

            } while(false);

            return null;
        }
        
    }

}
