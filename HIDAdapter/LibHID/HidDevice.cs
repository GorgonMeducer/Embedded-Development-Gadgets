using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.IO;
using ESnail.Utilities;

namespace HID
{

    /// <summary>
    /// 
    /// </summary>
    public class HidDevice
    {
        private SafeFileHandle   mHidHandle      = null;
        private SafeFileHandle   mReadHandle     = null;
        private SafeFileHandle   mWriteHandle    = null;
        private Hid              mHid            = new Hid();
        private String           mDevicePathName = null;
        private Boolean          mFoundDevice    = false;
        private Guid             mHidGuid        = Guid.Empty;

        public short vendorID()
        {
            return mHid.DeviceAttributes.VendorID;
        }

        public short productID()
        {
            return mHid.DeviceAttributes.ProductID;
        }

        public short majorVersionNumber()
        {
            short majorNumber = (short)(mHid.DeviceAttributes.VersionNumber & 0xF0);
            majorNumber >>= 4;

            return majorNumber;
        }

        public short minorVersionNumber()
        {
            short minorNumber = (short)(mHid.DeviceAttributes.VersionNumber & 0x0F);

            return minorNumber;
        }

        protected Boolean flushQueue()
        {
            return mHid.FlushQueue(mReadHandle);

        }// flashQueue()

        private Byte[] m_WriteBuffer = null;

        virtual protected UInt16 openDevice(String devicePath)
        {
            mDevicePathName = devicePath;

            if (null != mHidHandle)
            {
                if (!mHidHandle.IsInvalid)
                {
                    return HidLibConstants.RES_OK;
                }
            }
            //! try to check whether this device is connected by other driver
            try
            {
                mHidHandle = ESnail.Utilities.Win32API.WinBase.CreateFile(mDevicePathName,
                                           0,
                                           0,
                                           IntPtr.Zero,
                                           ESnail.Utilities.Win32API.WinBase.OPEN_EXISTING,
                                           0,
                                           0);

                if (mHidHandle.IsInvalid)
                {
                    return HidLibConstants.ERROR_FAILED_TO_CREATE_DEVICE_FILEHANDLE;
                }

                mHidHandle.Close();
            }
            catch (Exception) 
            {
                return HidLibConstants.ERROR_FAILED_TO_CREATE_DEVICE_FILEHANDLE;
            }

            try
            {
                mHidHandle = ESnail.Utilities.Win32API.WinBase.CreateFile(mDevicePathName,
                                               0,
                                               ESnail.Utilities.Win32API.WinBase.FILE_SHARE_READ | ESnail.Utilities.Win32API.WinBase.FILE_SHARE_WRITE,
                                               IntPtr.Zero,
                                               ESnail.Utilities.Win32API.WinBase.OPEN_EXISTING,
                                               0,
                                               0);
            }
            catch (Exception)
            {
                return HidLibConstants.ERROR_INVALID_HANDLE;
            }

            if (!mHidHandle.IsInvalid)
            {
                mHid.DeviceAttributes.Size = Marshal.SizeOf(mHid.DeviceAttributes);
                try
                {
                    Boolean success = ESnail.Utilities.Win32API.HID.HidD_GetAttributes(mHidHandle, ref mHid.DeviceAttributes);

                    if (success)
                    {
                        mFoundDevice = true;
                    }
                    else
                    {
                        mHidHandle.Close();
                    }
                }
                catch (Exception)
                {
                    mHidHandle.Close();
                }
            }
            else
            {
                mHidHandle.Close();
            }


            if (mFoundDevice)
            {
                mHid.Capabilities = mHid.GetDeviceCapabilities(mHidHandle);

                try
                {
                    mReadHandle = ESnail.Utilities.Win32API.WinBase.CreateFile(mDevicePathName,
                                                          ESnail.Utilities.Win32API.WinBase.GENERIC_READ,
                                                          ESnail.Utilities.Win32API.WinBase.FILE_SHARE_READ | ESnail.Utilities.Win32API.WinBase.FILE_SHARE_WRITE,
                                                          IntPtr.Zero,
                                                          ESnail.Utilities.Win32API.WinBase.OPEN_EXISTING,
                                                          ESnail.Utilities.Win32API.WinBase.FILE_FLAG_OVERLAPPED,
                                                          0);
                }
                catch (Exception)
                {
                    return HidLibConstants.ERROR_INVALID_HANDLE;
                }

                if (!mReadHandle.IsInvalid)
                {
                    try
                    {
                        mWriteHandle = ESnail.Utilities.Win32API.WinBase.CreateFile(mDevicePathName,
                                                     ESnail.Utilities.Win32API.WinBase.GENERIC_WRITE,
                                                     ESnail.Utilities.Win32API.WinBase.FILE_SHARE_READ | ESnail.Utilities.Win32API.WinBase.FILE_SHARE_WRITE,
                                                     IntPtr.Zero,
                                                     ESnail.Utilities.Win32API.WinBase.OPEN_EXISTING,
                                                     0,//Win32API.WinBase.FILE_FLAG_OVERLAPPED,
                                                     0);
                    
                        mHid.FlushQueue(mReadHandle);
                    }
                    catch (Exception )
                    {
                        return HidLibConstants.ERROR_INVALID_HANDLE;
                    }
                }
                else
                {
                    return HidLibConstants.ERROR_INVALID_HANDLE;
                }

                try
                {
                    m_WriteBuffer = new Byte[mHid.Capabilities.OutputReportByteLength];
                }
                catch (Exception )
                {
                    m_WriteBuffer = null;
                }

                return HidLibConstants.RES_OK;
            }

            return HidLibConstants.ERROR_NO_DEVICE;

        }// openDevice()


        
        virtual public UInt16 closeDevice()
        {
            UInt16 result = HidLibConstants.RES_OK;

            if (mFoundDevice == true)
            {
                try
                {
                    // Close open handles to the device.
                    closeFileHandle(mHidHandle);
                    closeFileHandle(mReadHandle);
                    closeFileHandle(mWriteHandle);
                    mFoundDevice = false;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                    result = HidLibConstants.ERROR_FAILED_TO_CLOSE_DEVICE;
                }
                finally
                {
                    mHidHandle = null;
                    mReadHandle = null;
                    mWriteHandle = null;
                }

            }

            m_WriteBuffer = null;

            return result;

        }// closeDevic()
  
        private void closeFileHandle(SafeFileHandle fileHandle)
        {
            if( !( fileHandle == null ) ) 
            { 
                if( !( fileHandle.IsInvalid ) ) 
                { 
                    fileHandle.Close(); 
                }
                fileHandle.Dispose();
            } 

        }// closeFileHandle()




        /// <param name="messageData">
        /// The System.Windows.Forms.Message.LParam value.
        /// </param>
        public Boolean isDevice(IntPtr messageData)
        {
            Boolean result = false;

            /*
            if (mFoundDevice)
            {
                result = DeviceManagement.DeviceNameMatch(messageData, mDevicePathName);
            }
            */
            
            if ((null != mDevicePathName) && (mDevicePathName.Trim() != ""))
            {
                try
                {
                    result = DeviceManagement.DeviceNameMatch(messageData, mDevicePathName);
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e.ToString());

                    return false;
                }
            }
            

            return result;

        }// isDevice()




        /// <summary>
        /// Reads a buffer from the HID device using interrupt transfer.
        /// Increase the size of the buffer if tha app cant keep up with reading each report
        /// individually. Should probably run in a separate thread.
        /// </summary>
        /// <param name="data">Data returned from the device.</param>
        /// <returns></returns>
        virtual public UInt16 readFromDevice(ref Byte[] data)
        {
            try
            {
                if (mReadHandle.IsInvalid)
                {
                    return HidLibConstants.ERROR_INVALID_HANDLE;
                }
                
                if (mHid.Capabilities.InputReportByteLength <= 0)
                {
                    return HidLibConstants.ERROR_INVALID_INPUTREPORT_LENGTH;
                }

                //! resize the buffer
                if ((null == data) || (data.Length != mHid.Capabilities.InputReportByteLength))
                {
                    data = new Byte[mHid.Capabilities.InputReportByteLength];
                }
                
                Hid.InputReportViaInterruptTransfer myInputReport = new Hid.InputReportViaInterruptTransfer();
                Boolean myDeviceDetected = false;
                Int32 result = ESnail.Utilities.Win32API.WinBase.WAIT_OBJECT_0;

                myInputReport.Read(mHidHandle,
                                   mReadHandle,
                                   mWriteHandle, 
                                   ref myDeviceDetected,
                                   ref data,
                                   ref result);

                if (result == ESnail.Utilities.Win32API.WinBase.WAIT_OBJECT_0)
                {
                    return HidLibConstants.RES_OK;
                }
                else if (result == ESnail.Utilities.Win32API.WinBase.WAIT_TIMEOUT)
                {
                    return HidLibConstants.RES_NO_DATA;
                }
                else
                {
                    return HidLibConstants.ERROR_FAILED_TO_READ_FROM_DEVICE;
                }

                //return HidLibConstants.RES_OK;
            }
            catch(ExecutionEngineException )
            {
                //System.Console.WriteLine(e);

                return HidLibConstants.ERROR_FAILED_TO_READ_FROM_DEVICE;
            }

        }// readFromDevice()




        public UInt16 writeToDevice(byte[] data)
        {
            if (mWriteHandle.IsInvalid || mWriteHandle.IsClosed)
            {
                return HidLibConstants.ERROR_INVALID_HANDLE;
            }

            if (null == data)
            {
                return HidLibConstants.ERROR_INVALID_BUFFERSIZE; 
            }

            if (0 == data.Length)
            {
                //! no data need to be write
                return HidLibConstants.RES_OK;
            }

            /*
            //For a write operation to succeed, the buffer must match the size of 
            //the outputreportbuffer length and begin with a 0.
            if (data.Length > mHid.Capabilities.OutputReportByteLength - 1)
            {
                return HidLibConstants.ERROR_INVALID_BUFFERSIZE;
            }
            */

            if (null == m_WriteBuffer)
            {
                m_WriteBuffer = new Byte[mHid.Capabilities.OutputReportByteLength];
            }
            System.Int32 nIndex = 0;

            while (nIndex < data.Length)
            {
                //! set buffer to zero
                //m_WriteBuffer.s
                m_WriteBuffer[0] = 0;
                //! try to copy data to buffer
                if ((data.Length - nIndex) < (m_WriteBuffer.Length - 1))
                {
                    Array.Clear(m_WriteBuffer,1,m_WriteBuffer.Length-1);
                }
                for (System.Int32 n = 0; n < (m_WriteBuffer.Length - 1); n++)
                {
                    m_WriteBuffer[n + 1] = data[nIndex++];
                    if (nIndex >= data.Length)
                    {
                        //! all bytes were copy to the buffer
                        break;
                    }
                }
                
                //! try to write data to buffer
                try
                {
                    //outputReportBuffer[0] = 0; // this is the report ID
                    //data.CopyTo(outputReportBuffer, 1);
                    
                    Hid.OutputReportViaInterruptTransfer myOutputReport = new Hid.OutputReportViaInterruptTransfer();
                    
                    bool success = myOutputReport.Write(m_WriteBuffer, mWriteHandle);

                    if (!success)
                    {
                        return HidLibConstants.ERROR_FAILED_TO_WRITE_TO_DEVICE;
                    }
                }
                catch
                {
                    return HidLibConstants.ERROR_FAILED_TO_WRITE_TO_DEVICE;
                }
            }
            return HidLibConstants.RES_OK; 

        }// writeToDevice()
        

        
        
        public short getInputReportByteSize()
        {
            short result = 0;

            if (mFoundDevice)
            {
                result = mHid.Capabilities.InputReportByteLength;
            }

            return result;

        }// getInputReportByteSize()




        private Int32 GetInputReportBufferSize()
        {
            Int32 numberOfInputBuffers = 0;

            Boolean success = mHid.GetNumberOfInputBuffers(mHidHandle, ref numberOfInputBuffers);           

            return numberOfInputBuffers;

        }// GetInputReportBufferSize()



        protected Boolean isConnected()    
        {
            return mFoundDevice;
        }

    }// class HidDevice

}// namespace HidLib
