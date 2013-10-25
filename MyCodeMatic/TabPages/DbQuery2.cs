using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace Codematic 
{
    public partial class DbQuery2 : Form
    {
        public DbQuery2( )
        {
            InitializeComponent();
            txtName.Text = "SQL";
        }
        public string SqlName { get; set; }
        public string Sql { get; set; }
        public string NameInfo { get; set; }
        public string ColumnInfos { get; set; }
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Regex.IsMatch(txtName.Text, @"^[A-Z][\w]*$"))
                    throw new Exception("請輸入符合規則的名稱");
                if (txtName.Text.Length < 4)
                    throw new Exception("名稱長充必須>3");
                if (string.IsNullOrEmpty(txtName.Text) )
                    throw new Exception("請輸入名稱");
                if (string.IsNullOrEmpty(txtSql.Text))
                    throw new Exception("請輸入SQL");
             //   if(string.IsNullOrEmpty ( NameInfo 
                SqlName = txtName.Text.Trim ();
                Sql = txtSql.Text;
                if(!string.IsNullOrEmpty (txtNameInfo.Text ))
                NameInfo = txtNameInfo.Text;
                if (!string.IsNullOrEmpty(txtColInfo.Text))
                    ColumnInfos = txtColInfo.Text;
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
