using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities.Windows.Forms.Interfaces;
using System.Xml;
using ESnail.Component.UI.WaveViewer;
using ESnail.Device;
using ESnail.Utilities;
using ESnail.Utilities.XML;

namespace ESnail.Component
{
    //! \name parameter access mode
    //! @{
    public enum BM_PARAMETER_TYPE
    { 
        BM_TYPE_CONST,                  //!< constant parameter
        BM_TYPE_COMMAND,                //!< command, only send data out
        BM_TYPE_DATA,                   //!< data, only read data in
        BM_TYPE_SETTING                 //!< setting, write/read data out/in
    }
    //! @}

    //! \name parameter data type
    //! @{
    public enum BM_PARAMETER_DATA_TYPE
    { 
        BM_DATA_TYPE_NONE,
        BM_DATA_TYPE_HEX_U8,
        BM_DATA_TYPE_HEX_U16,
        BM_DATA_TYPE_HEX_U32,
        BM_DATA_TYPE_HEX_U64,
        BM_DATA_TYPE_DEC_S8,
        BM_DATA_TYPE_DEC_S16,
        BM_DATA_TYPE_DEC_S32,
        BM_DATA_TYPE_DEC_S64,
        BM_DATA_TYPE_DEC_U8,
        BM_DATA_TYPE_DEC_U16,
        BM_DATA_TYPE_DEC_U32,
        BM_DATA_TYPE_DEC_U64,

        BM_DATA_TYPE_STRING,
        BM_DATA_TYPE_BYTES,

        BM_DATA_TYPE_SINGLE,
        BM_DATA_TYPE_DOUBLE,

        BM_DATA_TYPE_FLAGS_U8,
        BM_DATA_TYPE_FLAGS_U16,

        BM_DATA_TYPE_BOOLEAN
    }
    //! @}

    public delegate System.Boolean ParameterRequest(IBMParameter BMParameter);

   

    //! \name interface for setting/exporting/importing parameter control advanced setting
    //! @{
    public interface IBMPControlAdvance : IEditorEx, IOBJXMLSettingIO
    {
        //! event for issuing requests
        event ParameterRequest ParameterCommandRequestEvent;
        event ParameterRequest ParameterInformationRequestEvent;
        event ParameterRequest ParameterSettingRequestEvent;

        Boolean CopySetting(IBMPControlAdvance tControl);
        String Value
        {
            get;
            set;
        }

        String Type
        {
            get;
        }
    }
    //! @}

    

    //! \name BatteryManageItem Updated/Edit result
    //! @{
    public enum BM_PARAMETER_UPDATED_RESULT
    {
        BM_PARAMETER_CANCELLED,                  //!< updating / editing is cancelled
        BM_PARAMETER_UPDATED,                    //!< updated
        BM_PARAMETER_APPLY                       //!< applied, all modifies are submitted
    }
    //! @}

    public delegate void ParameterSettingUpdated(BM_PARAMETER_UPDATED_RESULT Result, IBMParameter BMParameter);
    public delegate void ParameterRemoved(IBMParameter BMParameter);
    //public delegate void ParameterValueUpdated(IBMParameter BMParameter);

    public class IBMParameterComparer : IComparer<IBMParameter>
    {

        public Int32 Compare(IBMParameter x, IBMParameter y)
        {
            if (x.Index == y.Index)
            {
                return 0;
            }
            else if (x.Index > y.Index)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }

    //! \name interface for numeric variable
    public interface IVariable
    {
        Boolean IsNumeric
        {
            get;
        }

        String Min
        {
            get;
        }

        String Max
        {
            get;
        }

        String Default
        {
            get;
        }
    }

    //! \name interface for parameters 
    public interface IBMParameter : IEditorEx, IBMDataLogControl, IWave, IControls, IVariable
    {
        event ParameterSettingUpdated DesignParameterSettingUpdatedEvent;
        event ParameterRemoved DesignParameterRemovedEvent;


        event ParameterRequest ParameterReadingRequestIssued;
        event ParameterRequest ParameteWritingRequestIssued;
        event ParameterRequest ParameterCommandRequestIssued;
        
        //event ParameterRequest ParameterCommandRequestEvent;

        void BeginUpdate();
        void EndUpdate();

        Boolean Modified
        {
            get;
        }

        SafeID ID
        {
            get;
            set;
        }
        
        //! \brief property for setting/getting parameter Unit
        new String Unit
        {
            get;
            set;
        }
        
        //! \brief property for setting/getting parameter Label
        String Label
        {
            get;
            set;
        }

        //! \brief property for setting/getting internal value
        Byte[] InternalValue
        {
            get;
            set;
        }

        Boolean CanRead
        {
            get;
        }

        Boolean CanWrite
        {
            get;
        }

        //! \brief property for setting/getting value
        String Value
        {
            get;
            set;
        }

        String PreWriteValue
        {
            get;
            set;
        }

        //! \brief property for setting/getting speicial function
        SafeID SpecialFunctionID
        {
            get;
            set;
        }

        //! \brief property for setting/getting parameter type
        BM_PARAMETER_TYPE Type
        {
            get;
        }

        //! \brief property for setting/getting parameter data type
        BM_PARAMETER_DATA_TYPE DataType
        {
            get;
            set;
        }

        
        //! \brief property for getting/setting parameter description
        new String Description
        {
            get;
            set;
        }
        
        //! \brief property for getting/setting control index
        String SelectedControl
        {
            get;
            set;
        }

        //! \brief property for getting control
        IBMPControlAdvance Control
        {
            get;
        }

        //! \brief property for getting/setting category
        String Category
        {
            get;
            set;
        }

        Boolean ReadOnly
        {
            get;
        }

        Boolean Available
        {
            get;
            set;
        }

        Boolean InVisible
        {
            get;
            set;
        }

        Int32 Index
        {
            get;
            set;
        }

        String Command
        {
            get;
            set;
        }

        String BlockOffset
        {
            get;
            set;
        }

    }

    public interface IBMComponent : ISafeID, IOBJXMLSettingIO, IDisposable, IControl, IMenu, IMdiChild
    {
        //! \brief property for get component name
        String ComponentName
        {
            get;
        }

        String Name
        {
            get;
            set;
        }

        //! \brief property for getting component description
        String ComponentDescription
        {
            get;
        }

        //! \brief property for getting component version information
        String Version
        {
            get;
        }

        //! \brief property for getting component company
        String Company
        {
            get;
        }

        //! \brief property for getting an about window
        Form About
        {
            get;
        }

        //! \brief property for getting an About panel
        Panel AboutPanel
        {
            get;
        }

        //! \brief setting page
        TabPage SettingPage
        {
            get;
        }

        //! \brief setting panel
        Panel SettingPanel
        {
            get;
        }

        //! \brief get/set debug mode
        Boolean DebugModeEnabled
        {
            get;
            set;
        }

        
        Adapter Adapter
        {
            get;
            set;
        }
        


        Boolean Open();

        Boolean Close();

        //! \brief import adapters' settings
        Boolean ImportAdapterSetting(XmlDocument xmlDoc, XmlNode xmlRoot);

        //! \brief export adapters' settings
        Boolean ExportAdapterSetting(XmlDocument xmlDoc, XmlNode xmlRoot);

        //! \brief deploy component 
        Boolean Deploy(XmlDocument xmlDoc, XmlNode xmlRoot, String tPath, String tName);

        //! \briel seal component
        Boolean Seal(XmlDocument xmlDoc, XmlNode xmlRoot);
    }

    

    //! \name component interface for solution design
    public interface IBMComponentDesign : IBMComponent 
    {
        Boolean DesignEnabled
        {
            get;
            set;
        }

        //! \brief property for getting a special parameter ID list
        String[] SpecialParameterIDList
        {
            get;
        }

        //! \brief method for getting a solution preview window
        Form CreatePreviewWindow(Object m_Solution);

        //! \brief default setting page
        TabPage DefaultSettingPage
        {
            get;
        }

        //! \brief default setting panel
        Panel DefaultSettingPanel
        {
            get;
        }

        IBMComponentDesign CreateComponent(Object m_Solution);

        //! import setting from specified file
        Boolean ImportDefaultSetting(XmlDocument xmlDoc, XmlNode xmlRoot);

        //! export setting to specified file
        Boolean ExportDefaultSetting(XmlDocument xmlDoc, XmlNode xmlRoot);

    }


}
