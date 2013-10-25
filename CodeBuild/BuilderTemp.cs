using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.Data;
using System.IO;
using System.Xml.Xsl;
using LTP.IDBO;
using LTP.Utility;
using LTP.CodeHelper;
namespace LTP.CodeBuild
{
    /// <summary>
    /// 模板代码生成
    /// </summary>
    public class BuilderTemp
    {
        protected IDbObject dbobj;
        private string _dbname;
        private string _tablename;
        //private Hashtable _keys; //主键或条件字段列表
        //private ArrayList _fieldlist = new ArrayList();
        private string strXslt="";
        private List<ColumnInfo> _keys; //主键或条件字段列表
        private List<ColumnInfo> _fieldlist;

        public string DbName
        {
            set { _dbname = value; }
            get { return _dbname; }
        }
        public string TableName
        {
            set { _tablename = value; }
            get { return _tablename; }
        }
        //public Hashtable Keys
        //{
        //    set { _keys = value; }
        //    get { return _keys; }
        //}
        //public ArrayList Fieldlist
        //{
        //    set { _fieldlist = value; }
        //    get { return _fieldlist; }
        //}
        public List<ColumnInfo> Keys
        {
            set { _keys = value; }
            get { return _keys; }
        }
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        public string Fields
        {
            get
            {
                StringPlus _fields = new StringPlus();
                foreach (object obj in Fieldlist)
                {
                    _fields.Append("'" + obj.ToString() + "',");
                }
                _fields.DelLastComma();
                return _fields.Value;
            }
        }

        public BuilderTemp(IDbObject idbobj, string dbName, string tableName, List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string strxslt)
        {
            dbobj = idbobj;
            DbName = dbName;
            TableName = tableName;           
            Fieldlist = fieldlist;
            Keys = keys;
            Fieldlist = fieldlist;
            Keys = keys;
            //foreach (ColumnInfo key in keys)
            //{
            //    _key = key.ColumnName;
            //    _keyType = key.TypeName;
            //    if (key.IsIdentity)
            //    {
            //        _key = key.ColumnName;
            //        _keyType = CodeCommon.DbTypeToCS(key.TypeName);
            //        break;
            //    }
            //}
            strXslt = strxslt;
        }

        #region  根据模版得到生成的代码GetCode
        /// <summary>
        /// 根据模版得到生成的代码
        /// </summary>
        /// <returns></returns>
        public string GetCode()
        {            
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            if (Fieldlist.Count>0)
            {               
                                 
                //XslCompiledTransform xslt = new XslCompiledTransform();
                //XslTransform xslt = new XslTransform();
                ////StreamReader srFile = new StreamReader(strXslt, Encoding.Default);
                ////XmlTextReader x = new XmlTextReader(srFile);
                ////xslt.Load(x);

                //xslt.Load(strXslt);

                ////XmlTextReader xtr = new XmlTextReader(GetXml(dtrows));
                ////XmlTextWriter xtw = new XmlTextWriter();
                //xslt.Transform(GetXml2(), null, stringWriter, null);

                XslCompiledTransform xctf = new XslCompiledTransform();
                xctf.Load(strXslt);
                xctf.Transform(GetXml2(), null, stringWriter );
             
            }            
            return stringWriter.ToString();
        }
        ///// <summary>
        ///// 根据模版得到生成的代码
        ///// </summary>
        ///// <returns></returns>
        //public string GetCode()
        //{
        //    DataTable dt = dbobj.GetColumnInfoList(DbName, TableName);
        //    System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        //    if (dt != null)
        //    {
        //        DataRow[] dtrows;
        //        if (Fieldlist.Count > 0)
        //        {
        //            dtrows = dt.Select("ColumnName in (" + Fields + ")", "colorder asc");
        //        }
        //        else
        //        {
        //            dtrows = dt.Select();
        //        }

        //        //XslCompiledTransform xslt = new XslCompiledTransform();
        //        XslTransform xslt = new XslTransform();

        //        //StreamReader srFile = new StreamReader(strXslt, Encoding.Default);
        //        //XmlTextReader x = new XmlTextReader(srFile);
        //        //xslt.Load(x);

        //        xslt.Load(strXslt);

        //        //XmlTextReader xtr = new XmlTextReader(GetXml(dtrows));
        //        //XmlTextWriter xtw = new XmlTextWriter();
        //        xslt.Transform(GetXml2(dtrows), null, stringWriter, null);
        //    }
        //    return stringWriter.ToString();
        //}


        #endregion

        /// <summary>
        /// 得到数据的xml
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXml(DataRow[] dtrows)
        {
            //string xmlDoc = "temp.xml";
            Stream w = new System.IO.MemoryStream(); 

            XmlTextWriter writer = new XmlTextWriter(w, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument(true);
            writer.WriteStartElement("Schema");

            writer.WriteStartElement("TableName");
            writer.WriteAttributeString("value", "Authors");
            writer.WriteEndElement();

            writer.WriteStartElement("FIELDS");

            foreach (DataRow row in dtrows)
            {
                string columnName = row["ColumnName"].ToString();
                string columnType = row["TypeName"].ToString();
                //string IsIdentity = row["IsIdentity"].ToString();

                writer.WriteStartElement("FIELD");
                writer.WriteAttributeString("Name", columnName);
                writer.WriteAttributeString("Type", columnType);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();

            //return w;           
            TextReader stringReader = new StringReader(writer.ToString());
            XmlDocument xml = new XmlDocument();
            xml.Load(stringReader);
            return xml;


        }
        /// <summary>
        /// 得到数据的xml
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXml2()
        {
            string xmlDoc = @"Template\temp.xml"; //临时数据文件temp.xml

            XmlTextWriter writer = new XmlTextWriter(xmlDoc, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument(true);
            writer.WriteStartElement("Schema");

            writer.WriteStartElement("TableName");
            writer.WriteAttributeString("value", TableName);
            writer.WriteEndElement();

            //字段属性
            writer.WriteStartElement("FIELDS");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string Length = field.Length;
                bool IsIdentity = field.IsIdentity;
                bool isPK = field.IsPK;
                string deText = field.DeText;
                string defaultVal = field.DefaultVal;

                writer.WriteStartElement("FIELD");                
                writer.WriteAttributeString("Name", columnName);
                writer.WriteAttributeString("Type", CodeCommon.DbTypeToCS(columnType));
                writer.WriteAttributeString("Desc", deText);
                writer.WriteAttributeString("defaultVal", defaultVal);                
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            //主键字段
            writer.WriteStartElement("PrimaryKeys");
            foreach (ColumnInfo field in Keys)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string Length = field.Length;
                bool IsIdentity = field.IsIdentity;
                bool isPK = field.IsPK;
                string deText = field.DeText;
                string defaultVal = field.DefaultVal;

                writer.WriteStartElement("FIELD");                
                writer.WriteAttributeString("Name", columnName);
                writer.WriteAttributeString("Type", CodeCommon.DbTypeToCS(columnType));
                writer.WriteAttributeString("Desc", deText);
                writer.WriteAttributeString("defaultVal", defaultVal);                
                writer.WriteEndElement();
            }           
            writer.WriteEndElement();
            

            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
                      
           
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlDoc);
            return xml;


        }
    }
}
