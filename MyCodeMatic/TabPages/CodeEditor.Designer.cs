using System.Drawing;
using LTP.TextEditor;
using LTP.TextEditor.Gui.CompletionWindow;
using LTP.TextEditor.Document;
using LTP.TextEditor.Actions;
namespace Codematic
{
    partial class CodeEditor
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtContent = new LTP.TextEditor.TextEditorControl();
            this.SuspendLayout();
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Location = new System.Drawing.Point(0, 0);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(368, 376);
            this.txtContent.TabIndex = 0;
            this.txtContent.Text = "";


            this.txtContent.IsIconBarVisible = false;
            this.txtContent.ShowInvalidLines = false;
            this.txtContent.ShowSpaces = false;
            this.txtContent.ShowTabs = false;
            this.txtContent.ShowEOLMarkers = false;
            this.txtContent.ShowVRuler = false;
            this.txtContent.Language = TextEditorControlBase.Languages.CSHARP;
            this.txtContent.Encoding = System.Text.Encoding.Default;
            this.txtContent.Font = new Font("新宋体", 9);
            // 
            // CodeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 376);
            this.Controls.Add(this.txtContent);
            this.Name = "CodeEditor";
            this.Text = "CodeEditor";
            this.ResumeLayout(false);

        }

        #endregion

        public LTP.TextEditor.TextEditorControl txtContent;
    }
}