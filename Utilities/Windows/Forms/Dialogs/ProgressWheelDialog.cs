using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ESnail.Utilities.Windows.Forms.Dialogs
{
    public partial class ProgressWheelDialog : Form
    {
        public ProgressWheelDialog()
        {
            InitializeComponent();
        }


        public void SetIndication(String tInfo)
        {
            do
            {
                if (null == tInfo)
                {
                    break;
                }

                labInfo.Text = tInfo;

                return;
            } while (false);

            labInfo.Text = "Loading...";
        }
    }

    
}
