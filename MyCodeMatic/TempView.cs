using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Codematic.UserControls;
namespace Codematic
{
    public partial class TempView : Form
    {
        TempNode TreeClickNode; //右键菜单单击的节点
        DataSet ds ; //菜单数据
        LTP.CmConfig.AppSettings settings;
        string tempfilepath = "temptree.xml"; //菜单树文件
        
        public TempView()
        {
            InitializeComponent();
            settings = LTP.CmConfig.AppConfig.GetSettings();
            LoadTreeview();
        }

        #region 初始化Treeview项

        private void LoadTreeview()
        {
            ds = new DataSet(); //菜单数据
            treeView1.Nodes.Clear();
            TempNode rootNode = new TempNode("代码模版");
            rootNode.NodeType = "root";
            rootNode.ImageIndex = 0;
            rootNode.SelectedImageIndex = 0;
            rootNode.Expand();
            treeView1.Nodes.Add(rootNode);
                        
            ds.ReadXml(tempfilepath);
            DataTable dt=ds.Tables[0];
            
            DataRow[] drs = dt.Select("ParentID= " + 0); //选出所有一级节点	
            foreach (DataRow r in drs)
            {
                string nodeid = r["NodeID"].ToString();
                string text = r["Text"].ToString();
                string filepath = r["FilePath"].ToString();
                string nodetype = r["NodeType"].ToString();
                
                TempNode node = new TempNode(text);
                node.NodeID = nodeid;
                node.NodeType = nodetype;
                node.FilePath = filepath;
                if (nodetype == "folder")
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 1;
                }
                else
                {
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                }                
                rootNode.Nodes.Add(node);
                
                int sonparentid = int.Parse(nodeid); // or =location
                CreateNode(sonparentid,node,dt);
            }
            

        }

        //邦定任意节点
        public void CreateNode(int parentid, TreeNode parentnode, DataTable dt)
        {
            DataRow[] drs = dt.Select("ParentID= " + parentid); //选出所有子节点			
            foreach (DataRow r in drs)
            {
                string nodeid = r["NodeID"].ToString();
                string text = r["Text"].ToString();
                string filepath = r["FilePath"].ToString();
                string nodetype = r["NodeType"].ToString();
                
                TempNode node = new TempNode(text);
                node.NodeID = nodeid;
                node.NodeType = nodetype;
                node.FilePath = filepath;
                if (nodetype == "folder")
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 1;
                }
                else
                {
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                }                        

                parentnode.Nodes.Add(node);

                int sonparentid = int.Parse(nodeid);// or =location
                CreateNode(sonparentid, node, dt);

            }//endforeach		

        }


        #endregion

        #region treeView1操作

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {            
        }
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Point mpt = new Point(e.X, e.Y);
                TreeClickNode = (TempNode)this.treeView1.GetNodeAt(mpt);
                this.treeView1.SelectedNode = TreeClickNode;
                if (TreeClickNode != null)
                {                   
                    if (e.Button == MouseButtons.Right)
                    {
                        CreatMenu(TreeClickNode.NodeType);
                        contextMenuStrip1.Show(treeView1, mpt);
                    }
                }
            }
            catch
            {
            }
        }

        #region 创建treeview 右键菜单

        private void CreatMenu(string NodeType)
        {
            //this.contextMenuStrip1.Items.Clear();
            switch (NodeType)
            {
                case "folder":
                    打开ToolStripMenuItem.Enabled = false;
                    新建ToolStripMenuItem.Visible = true;
                    break;
                case "file":
                    打开ToolStripMenuItem.Enabled = true;
                    新建ToolStripMenuItem.Visible = false;
                    break;
            }
        }

        #endregion

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //TreeNode node = (TreeNode)e.Item;          
            //if ((node.Tag.ToString() == "table") || (node.Tag.ToString() == "view") || (node.Tag.ToString() == "column"))
            //{
            //    DoDragDrop(node, DragDropEffects.Copy);
            //}
        }

        #endregion

        private string GetMaxNodeID(DataTable dt)
        {
            int maxval = 1;
            foreach (DataRow row in dt.Rows)
            {
                string nodeid = row["NodeID"].ToString();
                if (maxval < int.Parse(nodeid))
                {
                    maxval = int.Parse(nodeid);//5*1*a*s*p*x
                }
            }            
            return (maxval+1).ToString();

            //object obj = dt.Compute("max(CONVERT(int,NodeID))+1", "");
            //object obj = dt.Compute("max(NodeID)+1", "");
            //return obj.ToString();
        }     

        #region 右键菜单事件

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TempNode SelNode = (TempNode)this.treeView1.SelectedNode;
                string selstr = SelNode.Text;
                string filepath = SelNode.FilePath;
                if (filepath.Trim() != "")
                {
                    CodeTemplate codetempfrm = (CodeTemplate)Application.OpenForms["CodeTemplate"];
                    if (codetempfrm != null)
                    {
                        codetempfrm.SettxtTemplate(filepath);
                    }
                    else
                    {
                        MessageBox.Show("尚未打开模版代码生成编辑器！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("所选文件已经不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //新建文件夹
        private void 文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TempNode SelNode = (TempNode)this.treeView1.SelectedNode;
                string nodeid = SelNode.NodeID;

                int orderid = 1;
                if (SelNode != null)
                {
                    orderid = SelNode.Nodes.Count + 1;
                }

                string text = SelNode.Text;
                string filepath = SelNode.FilePath;
                string nodetype = SelNode.NodeType;                
                string newNodeid=GetMaxNodeID(ds.Tables[0]);

                //向ds增加一条数据              
                DataRow newRow = ds.Tables[0].NewRow();
                newRow["NodeID"] = newNodeid; 
                newRow["Text"] = "新建文件夹";
                newRow["FilePath"] = "";
                newRow["NodeType"] = "folder";
                newRow["ParentID"] = nodeid;
                newRow["OrderID"] = orderid;
                ds.Tables[0].Rows.Add(newRow);
                ds.Tables[0].AcceptChanges();
                ds.WriteXml(tempfilepath);

                //在treeview上增加一个节点
                TempNode node = new TempNode("新建文件夹");
                node.NodeID = newNodeid;
                node.ParentID = nodeid;
                node.NodeType = "folder";               
                node.ImageIndex = 0;
                node.SelectedImageIndex = 1;                
                SelNode.Nodes.Add(node);
                SelNode.Expand();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //新建模版文件
        private void 模版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TempNode SelNode = (TempNode)this.treeView1.SelectedNode;
                string nodeid = SelNode.NodeID;

                int orderid = 1;
                if (SelNode != null)
                {
                    orderid = SelNode.Nodes.Count + 1;
                }
                string newNodeid = GetMaxNodeID(ds.Tables[0]);
                //向ds增加一条数据
                DataRow newRow = ds.Tables[0].NewRow();
                newRow["NodeID"] = newNodeid;
                newRow["Text"] = "新建模版";
                string filepath = "新建模版.cmt";
                if ((settings.TemplateFolder != null) && (settings.TemplateFolder != ""))
                {
                    filepath = settings.TemplateFolder + "\\" + filepath;
                }
                else
                {
                    filepath = "Template\\新建模版.cmt";
                }

                // 是否已经存在
                //while (File.Exists(filepath))
                //{
                //}

                newRow["FilePath"] = filepath;
                newRow["NodeType"] = "file";
                newRow["ParentID"] = nodeid;
                newRow["OrderID"] = orderid;
                ds.Tables[0].Rows.Add(newRow);
                ds.Tables[0].AcceptChanges();
                ds.WriteXml(tempfilepath);
                                
                File.Create(filepath);                               

                //在treeview上增加一个节点
                TempNode node = new TempNode("新建模版");
                node.NodeID = newNodeid;
                node.ParentID = nodeid;
                node.FilePath = filepath;
                node.NodeType = "file";
                node.ImageIndex = 2;
                node.SelectedImageIndex = 2;
                SelNode.Nodes.Add(node);
                SelNode.Expand();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTreeview();
        }
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TempNode SelNode = (TempNode)this.treeView1.SelectedNode;
                string nodeid = SelNode.NodeID;
                string text = SelNode.Text;
                string filepath = SelNode.FilePath;
                string nodetype = SelNode.NodeType;
                
                //从ds删除一条数据
                foreach (DataRow row in ds.Tables[0].Select("NodeID='" + nodeid + "'"))
                {
                    ds.Tables[0].Rows.Remove(row);
                }
                ds.Tables[0].AcceptChanges();
                ds.WriteXml(tempfilepath);
                if (nodetype == "file")
                {
                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);
                    }
                }

                //删除节点
                treeView1.Nodes.Remove(SelNode);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TempNode SelNode = (TempNode)this.treeView1.SelectedNode;
                if (SelNode != null && SelNode.Parent != null)
                {
                    treeView1.SelectedNode = SelNode;
                    treeView1.LabelEdit = true;
                    if (!SelNode.IsEditing)
                    {
                        SelNode.BeginEdit();
                    }
                }
                else
                {
                    MessageBox.Show("没有选择节点或该节点是根节点.\n" , "无效选择");
                }
                                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (e.Label.IndexOfAny(new char[] { '@', '.', ',', '!' }) == -1)
                    {
                        e.Node.EndEdit(false);
                        TempNode SelNode = (TempNode)e.Node;
                        string nodeid = SelNode.NodeID;
                        string nodetype=SelNode.NodeType;
                        string newNodetext = e.Label;
                        string filepath = SelNode.FilePath;
                        string newfilepath = filepath;
                        if (nodetype == "file")
                        {
                            int end = filepath.LastIndexOf("\\");
                            newfilepath = filepath.Substring(0, filepath.Length - end-1) + newNodetext + ".cmt";

                            File.Move(filepath, newfilepath);
                        }                     
                        
                        foreach (DataRow row in ds.Tables[0].Select("NodeID='" + nodeid + "'"))
                        {
                            row["Text"] = newNodetext;
                            row["FilePath"] = newfilepath;
                        }
                        ds.Tables[0].AcceptChanges();
                        ds.WriteXml(tempfilepath);

                    }
                    else
                    {
                        e.CancelEdit = true;
                        MessageBox.Show("无效节点或无效字符: '@','.', ',', '!'","节点编辑");
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    e.CancelEdit = true;
                    MessageBox.Show("无效节点或节点名称不能为空！", "节点编辑");
                    e.Node.BeginEdit();
                }
                this.treeView1.LabelEdit = false;
            }

        }
        #endregion

        

    }
}