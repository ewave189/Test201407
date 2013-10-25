using System;
using System.Collections.Generic;
using System.Text;
using LTP.Utility;
using LTP.CodeHelper;
namespace LTP.BuilderModel
{
    /// <summary>
    /// Model代码生成组件
    /// </summary>
    public class BuilderModelMTable : IBuilder.IBuilderModel
    {
        #region 公有属性
        protected string _modelname = ""; //model类名
        protected string _namespace = "Maticsoft"; //顶级命名空间名
        protected string _modelpath = "";//实体类的命名空间
        protected List<ColumnInfo> _fieldlist;

        /// <summary>
        /// 顶级命名空间名 
        /// </summary>        
        public string NameSpace
        {
            set { _namespace = value; }
            get { return _namespace; }
        }

       
        /// <summary>
        /// 实体类的命名空间
        /// </summary>
        public string Modelpath
        {
            set { _modelpath = value; }
            get { return _modelpath; }
        }

        /// <summary>
        /// 关联的表名
        /// </summary>
        public LTP.IBuilder.SimplePair TableName { get; set; }



        /// <summary>
        /// model类名
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }

        #endregion

        public BuilderModelMTable()
        {
        }

        #region 生成完整Model类
        /// <summary>
        /// 生成完整sModel类
        /// </summary>		
        public string CreatModel()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            //strclass.AppendLine("using DBHelper;");
            strclass.AppendLine("using System.Collections.Generic;");
            strclass.AppendLine("using System.Data ;");
            strclass.AppendLine("namespace " + Modelpath);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 实体类" +  TableName.ToString () );
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "[Serializable]");
            strclass.AppendSpaceLine(1, "public class " + _modelname + ":DataTable");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, "public " + _modelname + "():base ()");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "TableName = \"" + TableName.Name  + "\";");
            strclass.AppendSpaceLine(3, "Init();");
            strclass.AppendSpaceLine(2, "}");
            strclass.AppendLine(CreatModelMethod());
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            strclass.AppendLine("");

            return strclass.ToString();
        }
        #endregion

        
        #region 初始化Model部分

        public  string CreatModelMethod()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "private void Init()");
            strclass.AppendSpaceLine(2, "{");
            
            strclass .AppendSpaceLine(3,"List<DataColumn> colPK = new List<DataColumn>();") ;
            strclass.AppendSpaceLine(3, "DataColumn col;");
            strclass.AppendSpaceLine(3, "#region ModelInit");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                bool ispk = field.IsPK;
                bool cisnull = field.cisNull;
                string deText = field.DeText;
                string  defaultvalue = field.DefaultVal;
                
                if (string.IsNullOrEmpty(deText))
                    deText = field.ColumnName;
                columnType = CodeCommon.DbTypeToCS(columnType);
                strclass .AppendSpaceLine (3,"col = Columns.Add(\""+columnName+"\", typeof("+columnType+") );");
                if (IsIdentity)
                    strclass.AppendSpaceLine(3, "col.AutoIncrement = true;");
                //if (!string.IsNullOrEmpty(defaultvalue))
                //    strclass.AppendSpaceLine(3,string.Format ( "col.DefaultValue = {0};",defaultvalue )); 
                if (ispk)
                    strclass.AppendSpaceLine(3, "colPK.Add(col);");
                string[] de_info = deText.Split(new char[] { ' ','　',':','：',';','；','(','（'}, 2);
                if (de_info.Length == 1)
                    strclass.AppendSpaceLine(3, "col.Caption = \"" + deText + "\";");
                else if (de_info.Length > 1)
                    strclass.AppendSpaceLine(3, string.Format("col.Caption = \"{0}\";//{1}", de_info[0], de_info[1]));
            }
            strclass.AppendSpaceLine(3, "#endregion ModelInit");
            strclass.AppendSpaceLine(3, "PrimaryKey = colPK.ToArray();");
     
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString ();
        } 

        #endregion
    }
}
