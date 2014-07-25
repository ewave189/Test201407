using System.Drawing;
using LTP.TextEditor;
using LTP.TextEditor.Gui.CompletionWindow;
using LTP.TextEditor.Document;
using LTP.TextEditor.Actions;
namespace Codematic.UserControls
{
    partial class UcCodeView
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
             //ICSharpCode.TextEditor.Document.DefaultFormattingStrategy defaultFormattingStrategy1 = new ICSharpCode.TextEditor.Document.DefaultFormattingStrategy();
             // ICSharpCode.TextEditor.Document.DefaultHighlightingStrategy defaultHighlightingStrategy1 = new ICSharpCode.TextEditor.Document.DefaultHighlightingStrategy();
             // ICSharpCode.TextEditor.Document.GapTextBufferStrategy gapTextBufferStrategy1 = new ICSharpCode.TextEditor.Document.GapTextBufferStrategy();
             // ICSharpCode.TextEditor.Document.DefaultTextEditorProperties defaultTextEditorProperties1 = new ICSharpCode.TextEditor.Document.DefaultTextEditorProperties();
             // System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbQuery));

            this.components = new System.ComponentModel.Container();
             
           // LTP.TextEditor.Document.DefaultDocument defaultDocument1 = new LTP.TextEditor.Document.DefaultDocument();
            LTP.TextEditor.Document.DefaultFormattingStrategy defaultFormattingStrategy1 = new LTP.TextEditor.Document.DefaultFormattingStrategy();
            LTP.TextEditor.Document.DefaultHighlightingStrategy defaultHighlightingStrategy1 = new LTP.TextEditor.Document.DefaultHighlightingStrategy();
            LTP.TextEditor.Document.GapTextBufferStrategy gapTextBufferStrategy1 = new LTP.TextEditor.Document.GapTextBufferStrategy();
            LTP.TextEditor.Document.DefaultTextEditorProperties defaultTextEditorProperties1 = new LTP.TextEditor.Document.DefaultTextEditorProperties();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcCodeView));
           // LTP.TextEditor.Document.DefaultDocument defaultDocument2 = new LTP.TextEditor.Document.DefaultDocument();
            LTP.TextEditor.Document.DefaultFormattingStrategy defaultFormattingStrategy2 = new LTP.TextEditor.Document.DefaultFormattingStrategy();
            LTP.TextEditor.Document.DefaultHighlightingStrategy defaultHighlightingStrategy2 = new LTP.TextEditor.Document.DefaultHighlightingStrategy();
            LTP.TextEditor.Document.GapTextBufferStrategy gapTextBufferStrategy2 = new LTP.TextEditor.Document.GapTextBufferStrategy();
            LTP.TextEditor.Document.DefaultTextEditorProperties defaultTextEditorProperties2 = new LTP.TextEditor.Document.DefaultTextEditorProperties();
          //  LTP.TextEditor.Document.DefaultDocument defaultDocument3 = new LTP.TextEditor.Document.DefaultDocument();
            LTP.TextEditor.Document.DefaultFormattingStrategy defaultFormattingStrategy3 = new LTP.TextEditor.Document.DefaultFormattingStrategy();
            LTP.TextEditor.Document.DefaultHighlightingStrategy defaultHighlightingStrategy3 = new LTP.TextEditor.Document.DefaultHighlightingStrategy();
            LTP.TextEditor.Document.GapTextBufferStrategy gapTextBufferStrategy3 = new LTP.TextEditor.Document.GapTextBufferStrategy();
            LTP.TextEditor.Document.DefaultTextEditorProperties defaultTextEditorProperties3 = new LTP.TextEditor.Document.DefaultTextEditorProperties();
           // LTP.TextEditor.Document.DefaultDocument defaultDocument4 = new LTP.TextEditor.Document.DefaultDocument();
            LTP.TextEditor.Document.DefaultFormattingStrategy defaultFormattingStrategy4 = new LTP.TextEditor.Document.DefaultFormattingStrategy();
            LTP.TextEditor.Document.DefaultHighlightingStrategy defaultHighlightingStrategy4 = new LTP.TextEditor.Document.DefaultHighlightingStrategy();
            LTP.TextEditor.Document.GapTextBufferStrategy gapTextBufferStrategy4 = new LTP.TextEditor.Document.GapTextBufferStrategy();
            LTP.TextEditor.Document.DefaultTextEditorProperties defaultTextEditorProperties4 = new LTP.TextEditor.Document.DefaultTextEditorProperties();
            txtContent_Web = new TextEditorControl();
          //  this.txtContent_Web = new LTP.TextEditor.TextEditorControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.txtContent_CS = new LTP.TextEditor.TextEditorControl();
            this.txtContent_SQL = new LTP.TextEditor.TextEditorControl();
            this.txtContent_XML = new LTP.TextEditor.TextEditorControl();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtContent_Web
            // 
            this.txtContent_Web.ContextMenuStrip = this.contextMenuStrip1;
           // defaultDocument1.FormattingStrategy = defaultFormattingStrategy1;
            defaultHighlightingStrategy1.Extensions = new string[] {
        ".htm",
        ".html"};
            //defaultDocument1.HighlightingStrategy = defaultHighlightingStrategy1;
            //defaultDocument1.ReadOnly = false;
            //defaultDocument1.TextBufferStrategy = gapTextBufferStrategy1;
            //defaultDocument1.TextContent = "";
            defaultTextEditorProperties1.AllowCaretBeyondEOL = false;
            defaultTextEditorProperties1.AutoInsertCurlyBracket = true;
            defaultTextEditorProperties1.BracketMatchingStyle = LTP.TextEditor.Document.BracketMatchingStyle.After;
            defaultTextEditorProperties1.ConvertTabsToSpaces = false;
            defaultTextEditorProperties1.CreateBackupCopy = false;
            defaultTextEditorProperties1.DocumentSelectionMode = LTP.TextEditor.Document.DocumentSelectionMode.Normal;
            defaultTextEditorProperties1.EnableFolding = true;
            defaultTextEditorProperties1.Encoding = ((System.Text.Encoding)(resources.GetObject("defaultTextEditorProperties1.Encoding")));
            defaultTextEditorProperties1.Font = new System.Drawing.Font("新宋体", 9F);
            defaultTextEditorProperties1.HideMouseCursor = false;
            defaultTextEditorProperties1.IndentStyle = LTP.TextEditor.Document.IndentStyle.Smart;
            defaultTextEditorProperties1.IsIconBarVisible = false;
            defaultTextEditorProperties1.LineTerminator = "\r\n";
            defaultTextEditorProperties1.LineViewerStyle = LTP.TextEditor.Document.LineViewerStyle.None;
            defaultTextEditorProperties1.MouseWheelScrollDown = true;
            defaultTextEditorProperties1.MouseWheelTextZoom = true;
            defaultTextEditorProperties1.ShowEOLMarker = false;
            defaultTextEditorProperties1.ShowHorizontalRuler = false;
            defaultTextEditorProperties1.ShowInvalidLines = false;
            defaultTextEditorProperties1.ShowLineNumbers = true;
            defaultTextEditorProperties1.ShowMatchingBracket = true;
            defaultTextEditorProperties1.ShowSpaces = false;
            defaultTextEditorProperties1.ShowTabs = false;
            defaultTextEditorProperties1.ShowVerticalRuler = false;
            defaultTextEditorProperties1.TabIndent = 4;
            defaultTextEditorProperties1.UseAntiAliasedFont = false;
            defaultTextEditorProperties1.UseCustomLine = false;
            defaultTextEditorProperties1.VerticalRulerRow = 80;
           // defaultDocument1.TextEditorProperties = defaultTextEditorProperties1;
           // this.txtContent_Web.Document = defaultDocument1;
            this.txtContent_Web.Encoding = ((System.Text.Encoding)(resources.GetObject("txtContent_Web.Encoding")));
            this.txtContent_Web.IsIconBarVisible = false;
            this.txtContent_Web.Language = LTP.TextEditor.TextEditorControlBase.Languages.CSHARP;
            this.txtContent_Web.Location = new System.Drawing.Point(321, 214);
            this.txtContent_Web.Name = "txtContent_Web";
            this.txtContent_Web.Size = new System.Drawing.Size(200, 200);
            this.txtContent_Web.TabIndex = 0;
            this.txtContent_Web.TextEditorProperties = defaultTextEditorProperties1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_Save});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(116, 26);
            // 
            // menu_Save
            // 
            this.menu_Save.Name = "menu_Save";
            this.menu_Save.Size = new System.Drawing.Size(115, 22);
            this.menu_Save.Text = "保存(&S)";
            this.menu_Save.Click += new System.EventHandler(this.menu_Save_Click);
            // 
            // txtContent_CS
            // 
            this.txtContent_CS.ContextMenuStrip = this.contextMenuStrip1;
            //defaultDocument2.FormattingStrategy = defaultFormattingStrategy2;
            defaultHighlightingStrategy2.Extensions = new string[] {
        ".cs"};
            //defaultDocument2.HighlightingStrategy = defaultHighlightingStrategy2;
            //defaultDocument2.ReadOnly = false;
            //defaultDocument2.TextBufferStrategy = gapTextBufferStrategy2;
            //defaultDocument2.TextContent = "";
            defaultTextEditorProperties2.AllowCaretBeyondEOL = false;
            defaultTextEditorProperties2.AutoInsertCurlyBracket = true;
            defaultTextEditorProperties2.BracketMatchingStyle = LTP.TextEditor.Document.BracketMatchingStyle.After;
            defaultTextEditorProperties2.ConvertTabsToSpaces = false;
            defaultTextEditorProperties2.CreateBackupCopy = false;
            defaultTextEditorProperties2.DocumentSelectionMode = LTP.TextEditor.Document.DocumentSelectionMode.Normal;
            defaultTextEditorProperties2.EnableFolding = true;
            defaultTextEditorProperties2.Encoding = ((System.Text.Encoding)(resources.GetObject("defaultTextEditorProperties2.Encoding")));
            defaultTextEditorProperties2.Font = new System.Drawing.Font("新宋体", 9F);
            defaultTextEditorProperties2.HideMouseCursor = false;
            defaultTextEditorProperties2.IndentStyle = LTP.TextEditor.Document.IndentStyle.Smart;
            defaultTextEditorProperties2.IsIconBarVisible = false;
            defaultTextEditorProperties2.LineTerminator = "\r\n";
            defaultTextEditorProperties2.LineViewerStyle = LTP.TextEditor.Document.LineViewerStyle.None;
            defaultTextEditorProperties2.MouseWheelScrollDown = true;
            defaultTextEditorProperties2.MouseWheelTextZoom = true;
            defaultTextEditorProperties2.ShowEOLMarker = false;
            defaultTextEditorProperties2.ShowHorizontalRuler = false;
            defaultTextEditorProperties2.ShowInvalidLines = false;
            defaultTextEditorProperties2.ShowLineNumbers = true;
            defaultTextEditorProperties2.ShowMatchingBracket = true;
            defaultTextEditorProperties2.ShowSpaces = false;
            defaultTextEditorProperties2.ShowTabs = false;
            defaultTextEditorProperties2.ShowVerticalRuler = false;
            defaultTextEditorProperties2.TabIndent = 4;
            defaultTextEditorProperties2.UseAntiAliasedFont = false;
            defaultTextEditorProperties2.UseCustomLine = false;
            defaultTextEditorProperties2.VerticalRulerRow = 80;
           // defaultDocument2.TextEditorProperties = defaultTextEditorProperties2;
           // this.txtContent_CS.Document = defaultDocument2;
            this.txtContent_CS.Encoding = ((System.Text.Encoding)(resources.GetObject("txtContent_CS.Encoding")));
            this.txtContent_CS.IsIconBarVisible = false;
            this.txtContent_CS.Language = LTP.TextEditor.TextEditorControlBase.Languages.CSHARP;
            this.txtContent_CS.Location = new System.Drawing.Point(219, 8);
            this.txtContent_CS.Name = "txtContent_CS";
            this.txtContent_CS.Size = new System.Drawing.Size(200, 200);
            this.txtContent_CS.TabIndex = 0;
            this.txtContent_CS.TextEditorProperties = defaultTextEditorProperties2;
            // 
            // txtContent_SQL
            // 
            this.txtContent_SQL.ContextMenuStrip = this.contextMenuStrip1;
           // defaultDocument3.FormattingStrategy = defaultFormattingStrategy3;
            defaultHighlightingStrategy3.Extensions = new string[] {
        ".sql"};
            //defaultDocument3.HighlightingStrategy = defaultHighlightingStrategy3;
            //defaultDocument3.ReadOnly = false;
            //defaultDocument3.TextBufferStrategy = gapTextBufferStrategy3;
            //defaultDocument3.TextContent = "";
            defaultTextEditorProperties3.AllowCaretBeyondEOL = false;
            defaultTextEditorProperties3.AutoInsertCurlyBracket = true;
            defaultTextEditorProperties3.BracketMatchingStyle = LTP.TextEditor.Document.BracketMatchingStyle.After;
            defaultTextEditorProperties3.ConvertTabsToSpaces = false;
            defaultTextEditorProperties3.CreateBackupCopy = false;
            defaultTextEditorProperties3.DocumentSelectionMode = LTP.TextEditor.Document.DocumentSelectionMode.Normal;
            defaultTextEditorProperties3.EnableFolding = true;
            defaultTextEditorProperties3.Encoding = ((System.Text.Encoding)(resources.GetObject("defaultTextEditorProperties3.Encoding")));
            defaultTextEditorProperties3.Font = new System.Drawing.Font("新宋体", 9F);
            defaultTextEditorProperties3.HideMouseCursor = false;
            defaultTextEditorProperties3.IndentStyle = LTP.TextEditor.Document.IndentStyle.Smart;
            defaultTextEditorProperties3.IsIconBarVisible = false;
            defaultTextEditorProperties3.LineTerminator = "\r\n";
            defaultTextEditorProperties3.LineViewerStyle = LTP.TextEditor.Document.LineViewerStyle.None;
            defaultTextEditorProperties3.MouseWheelScrollDown = true;
            defaultTextEditorProperties3.MouseWheelTextZoom = true;
            defaultTextEditorProperties3.ShowEOLMarker = false;
            defaultTextEditorProperties3.ShowHorizontalRuler = false;
            defaultTextEditorProperties3.ShowInvalidLines = false;
            defaultTextEditorProperties3.ShowLineNumbers = true;
            defaultTextEditorProperties3.ShowMatchingBracket = true;
            defaultTextEditorProperties3.ShowSpaces = false;
            defaultTextEditorProperties3.ShowTabs = false;
            defaultTextEditorProperties3.ShowVerticalRuler = false;
            defaultTextEditorProperties3.TabIndent = 4;
            defaultTextEditorProperties3.UseAntiAliasedFont = false;
            defaultTextEditorProperties3.UseCustomLine = false;
            defaultTextEditorProperties3.VerticalRulerRow = 80;
            //defaultDocument3.TextEditorProperties = defaultTextEditorProperties3;
            //this.txtContent_SQL.Document = defaultDocument3;
            this.txtContent_SQL.Encoding = ((System.Text.Encoding)(resources.GetObject("txtContent_SQL.Encoding")));
            this.txtContent_SQL.IsIconBarVisible = false;
            this.txtContent_SQL.Language = LTP.TextEditor.TextEditorControlBase.Languages.CSHARP;
            this.txtContent_SQL.Location = new System.Drawing.Point(3, 3);
            this.txtContent_SQL.Name = "txtContent_SQL";
            this.txtContent_SQL.Size = new System.Drawing.Size(200, 200);
            this.txtContent_SQL.TabIndex = 0;
            this.txtContent_SQL.TextEditorProperties = defaultTextEditorProperties3;
            // 
            // txtContent_XML
            // 
            this.txtContent_XML.ContextMenuStrip = this.contextMenuStrip1;
           // defaultDocument4.FormattingStrategy = defaultFormattingStrategy4;
            defaultHighlightingStrategy4.Extensions = new string[] {
        ".xml",
        ".xsl",
        ".xslt",
        ".xsd",
        ".manifest",
        ".config",
        ".addin",
        ".xshd",
        ".wxs",
        ".build",
        ".wsdl"};
            //defaultDocument4.HighlightingStrategy = defaultHighlightingStrategy4;
            //defaultDocument4.ReadOnly = false;
            //defaultDocument4.TextBufferStrategy = gapTextBufferStrategy4;
            //defaultDocument4.TextContent = "";
            defaultTextEditorProperties4.AllowCaretBeyondEOL = false;
            defaultTextEditorProperties4.AutoInsertCurlyBracket = true;
            defaultTextEditorProperties4.BracketMatchingStyle = LTP.TextEditor.Document.BracketMatchingStyle.After;
            defaultTextEditorProperties4.ConvertTabsToSpaces = false;
            defaultTextEditorProperties4.CreateBackupCopy = false;
            defaultTextEditorProperties4.DocumentSelectionMode = LTP.TextEditor.Document.DocumentSelectionMode.Normal;
            defaultTextEditorProperties4.EnableFolding = true;
            defaultTextEditorProperties4.Encoding = ((System.Text.Encoding)(resources.GetObject("defaultTextEditorProperties4.Encoding")));
            defaultTextEditorProperties4.Font = new System.Drawing.Font("新宋体", 9F);
            defaultTextEditorProperties4.HideMouseCursor = false;
            defaultTextEditorProperties4.IndentStyle = LTP.TextEditor.Document.IndentStyle.Smart;
            defaultTextEditorProperties4.IsIconBarVisible = false;
            defaultTextEditorProperties4.LineTerminator = "\r\n";
            defaultTextEditorProperties4.LineViewerStyle = LTP.TextEditor.Document.LineViewerStyle.None;
            defaultTextEditorProperties4.MouseWheelScrollDown = true;
            defaultTextEditorProperties4.MouseWheelTextZoom = true;
            defaultTextEditorProperties4.ShowEOLMarker = false;
            defaultTextEditorProperties4.ShowHorizontalRuler = false;
            defaultTextEditorProperties4.ShowInvalidLines = false;
            defaultTextEditorProperties4.ShowLineNumbers = true;
            defaultTextEditorProperties4.ShowMatchingBracket = true;
            defaultTextEditorProperties4.ShowSpaces = false;
            defaultTextEditorProperties4.ShowTabs = false;
            defaultTextEditorProperties4.ShowVerticalRuler = false;
            defaultTextEditorProperties4.TabIndent = 4;
            defaultTextEditorProperties4.UseAntiAliasedFont = false;
            defaultTextEditorProperties4.UseCustomLine = false;
            defaultTextEditorProperties4.VerticalRulerRow = 80;
           // defaultDocument4.TextEditorProperties = defaultTextEditorProperties4;
           // this.txtContent_XML.Document = defaultDocument4;
            this.txtContent_XML.Encoding = ((System.Text.Encoding)(resources.GetObject("txtContent_XML.Encoding")));
            this.txtContent_XML.IsIconBarVisible = false;
            this.txtContent_XML.Language = LTP.TextEditor.TextEditorControlBase.Languages.CSHARP;
            this.txtContent_XML.Location = new System.Drawing.Point(3, 3);
            this.txtContent_XML.Name = "txtContent_XML";
            this.txtContent_XML.Size = new System.Drawing.Size(200, 200);
            this.txtContent_XML.TabIndex = 0;
            this.txtContent_XML.TextEditorProperties = defaultTextEditorProperties4;
            // 
            // UcCodeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtContent_Web);
            this.Controls.Add(this.txtContent_CS);
            this.Controls.Add(this.txtContent_SQL);
            this.Controls.Add(this.txtContent_XML);
            this.Name = "UcCodeView";
            this.Size = new System.Drawing.Size(1910, 601);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public LTP.TextEditor.TextEditorControl txtContent_SQL;
        public LTP.TextEditor.TextEditorControl txtContent_Web;
        public LTP.TextEditor.TextEditorControl txtContent_CS;
        private LTP.TextEditor.TextEditorControl txtContent_XML;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menu_Save;
    }
}
