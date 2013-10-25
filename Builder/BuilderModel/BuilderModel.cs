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
    public class BuilderModel : IBuilder.IBuilderModel
    {        
        #region 公有属性
        protected string _modelname=""; //model类名
        protected string _namespace = "Maticsoft"; //顶级命名空间名
        protected string _modelpath="";//实体类的命名空间
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

        public LTP.IBuilder.SimplePair TableName { get; set; }

        
        #endregion

        public BuilderModel()
        {
        }        

        #region 生成完整Model类

        /// <summary>
        /// 生成完整sModel类
        /// </summary>		
        public string CreatSimpleModel()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("namespace " + Modelpath);
            strclass.AppendLine("{");

            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 类" + TableName.ToString());
            strclass.AppendSpaceLine(1, "/// </summary>"); 
            strclass.AppendSpaceLine(1, "public class " + _modelname);
            strclass.AppendSpaceLine(1, "{"); 
            strclass.AppendLine(CreatModelMethod());
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            strclass.AppendLine("");

            return strclass.ToString();
        }

        public string CreatJavaModel()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(1, "/**");
            strclass.AppendSpaceLine(1, " *" + TableName.ToString());
            strclass.AppendSpaceLine(1, " */"); 
            strclass.AppendSpaceLine(1, "public class " + _modelname);
            strclass.AppendSpaceLine(1, "{"); 
            strclass.AppendLine(CreatJavaModelMethod());
            strclass.AppendSpaceLine(1, "}");
          

            return strclass.ToString();
        }

        /// <summary>
        /// 生成完整sModel类
        /// </summary>		
        public string CreatModel()
        {            
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("namespace " + Modelpath);
            strclass.AppendLine("{");
            
            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 实体类" +TableName.ToString ());
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "[Serializable]");
            strclass.AppendSpaceLine(1, "public class " + _modelname);
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, "public " + _modelname + "()");
            strclass.AppendSpaceLine(2, "{}");
            strclass.AppendLine(CreatModelMethod());
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            strclass.AppendLine("");

            return strclass.ToString();
        }
        #endregion

        #region 生成Model属性部分
        public string CreatJavaModelMethod()
        { 
            StringPlus strclass2 = new StringPlus(); 
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                bool ispk = field.IsPK;
                bool cisnull = field.cisNull;
                string deText = field.DeText;
                columnType = CodeCommon.DbTypeToJava(columnType); 
                
                strclass2.AppendSpaceLine(2, "/**");
                strclass2.AppendSpaceLine(2, " * " + deText);
                strclass2.AppendSpaceLine(2, " */ ");
                strclass2.AppendSpaceLine(2, "public " + columnType +  " " +   columnName + ";" );//属性
                 
            } 

            return strclass2.ToString();
        }
        /// <summary>
        /// 生成实体类的属性
        /// </summary>
        /// <returns></returns>
        public string CreatModelMethod()
        {

            StringPlus strclass = new StringPlus();
           // StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            //strclass.AppendSpaceLine(2, "#region Model");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                bool ispk = field.IsPK;
                bool cisnull = field.cisNull;
                string deText = field.DeText;
                columnType = CodeCommon.DbTypeToCS(columnType);
                if (columnType != "decimal")
                    columnType = field.ColumnType.Name;
                string isnull = "";
                if (CodeCommon.isValueType(columnType) && columnType != "decimal")
                {
                    if ((!IsIdentity) && (!ispk) && (cisnull))
                    {
                        isnull = "?";//代表可空类型
                    }
                }
                //strclass1.AppendSpaceLine(2, "private " + columnType + isnull + " _" + columnName.ToLower() + ";");//私有变量
                strclass2.AppendSpaceLine(2, "/// <summary>");
                strclass2.AppendSpaceLine(2, "/// " + deText);
                strclass2.AppendSpaceLine(2, "/// </summary>");
                strclass2.AppendSpaceLine(2, "public " + columnType + isnull + " " + columnName +  "{ get ; set; }");//属性
                
                //strclass2.AppendSpaceLine(3, "set{" + " _" + columnName.ToLower() + "=value;}");
                //strclass2.AppendSpaceLine(3, "get{return " + "_" + columnName.ToLower() + ";}");
                //strclass2.AppendSpaceLine(2, "}");
            }
            //strclass.Append(strclass1.Value);
            strclass.Append(strclass2.Value);
            //strclass.AppendSpaceLine(2, "#endregion Model");

            return strclass.ToString();
        }

        #endregion
    }
}
