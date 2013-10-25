using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using LTP.TextEditor;
namespace Codematic
{
    public partial class CodeEditor : Form
    {
        public CodeEditor()
        {
            InitializeComponent();
        }

        public CodeEditor(string tempFile, string FileType)
        {
            InitializeComponent();
            switch (FileType)
            {
                case "cs":
                    txtContent.Language = TextEditorControlBase.Languages.CSHARP;
                    break;
                case "vb":
                    txtContent.Language = TextEditorControlBase.Languages.VBNET;
                    break;
                case "html":
                    txtContent.Language = TextEditorControlBase.Languages.HTML;
                    break;
                case "sql":
                    txtContent.Language = TextEditorControlBase.Languages.SQL;
                    break;
                case "cpp":
                    txtContent.Language = TextEditorControlBase.Languages.CPP;
                    break;
                case "js":
                    txtContent.Language = TextEditorControlBase.Languages.JavaScript;
                    break;
                case "java":
                    txtContent.Language = TextEditorControlBase.Languages.Java;
                    break;
                case "xml":
                    txtContent.Language = TextEditorControlBase.Languages.XML;
                    break;
                case "txt":                                  
                    txtContent.Language = TextEditorControlBase.Languages.XML;
                    break;
            }
            StreamReader srFile = new StreamReader(tempFile, Encoding.Default);
            string Contents = srFile.ReadToEnd();
            srFile.Close();
            this.txtContent.Text = Contents;
        }

        public CodeEditor(string strCode, string FileType,string temp)
        {
            InitializeComponent();
            switch (FileType)
            {
                case "cs":
                    txtContent.Language = TextEditorControlBase.Languages.CSHARP;
                    break;
                case "vb":
                    txtContent.Language = TextEditorControlBase.Languages.VBNET;
                    break;
                case "html":
                    txtContent.Language = TextEditorControlBase.Languages.HTML;
                    break;
                case "sql":
                    txtContent.Language = TextEditorControlBase.Languages.SQL;
                    break;
                case "cpp":
                    txtContent.Language = TextEditorControlBase.Languages.CPP;
                    break;
                case "js":
                    txtContent.Language = TextEditorControlBase.Languages.JavaScript;
                    break;
                case "java":
                    txtContent.Language = TextEditorControlBase.Languages.Java;
                    break;
                case "xml":
                    txtContent.Language = TextEditorControlBase.Languages.XML;
                    break;
                case "txt":
                    txtContent.Language = TextEditorControlBase.Languages.XML;
                    break;
            }
            this.txtContent.Text = strCode;
        }
    }
}