using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Codematic.UserControls
{
    public partial class UcDatatypeMap : UserControl
    {
        string datatypefile = Application.StartupPath + "\\datatype.ini";
        LTP.Utility.INIFile datatype;
        public UcDatatypeMap()
        {
            InitializeComponent();

            datatype = new LTP.Utility.INIFile(datatypefile);
        }
    }
}
