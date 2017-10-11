using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;

namespace ESnail.Utilities.Windows.Forms.Interfaces
{
    //! \name interface for getting/creating editor
    //! @{
    public interface IEditor
    {
        //! get editor form
        Form Editor
        {
            get;
        }

        //! method for create a editor
        Form CreateEditor();
    }
    //! @}
    

    //! \name interface for getting Editor, page and panel
    //! @{
    public interface IEditorEx : IEditor
    {
        //! get editor table page
        TabPage EditorPage
        {
            get;
        }

        //! get editor panel
        Panel EditorPanel
        {
            get;
        }
    }
    //! @}

    public interface IMdiChild
    {
        Form MdiParent
        {
            get;
            set;
        }
    
    }

    public interface IMenu
    {
        MenuStrip Menu
        {
            get;
        }

        ToolStripMenuItem[] MenuItems
        {
            get;
        }
    }

    //! \name interface for getting/creating default control
    //! @{
    public interface IControl
    {
        //! get default control
        Control DefaultControl
        {
            get;
        }

        //! create a default control
        Control CreateControl();
    }
    //! @}

    public interface IComponent 
    {
        Component DefaultComponent
        {
            get;
        }

        Component CreateComponent();
    }   

    /*! \name interface for creating specified control and 
     *!       getting available control brief information
     */
    //! @{
    public interface IControls : IControl
    {
        //! create a specified control
        Control CreateControl(System.String Target);

        //! get available control brief information
        String[] GetAvailableControlInfo();
    }
    //! @}

    
}
