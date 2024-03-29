﻿using System;
using System.Data;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using LTP.CodeHelper;
using MySql.Data.MySqlClient;
using LTP.IDBO;
namespace LTP.DbObjects.MySQL
{
    /// <summary>
    /// 数据库信息类。
    /// </summary>
    public class DbObject : IDbObject
    {
        string cmcfgfile = Application.StartupPath + @"\cmcfg.ini";
        LTP.Utility.INIFile cfgfile;
        bool isdbosp = false;

        #region  属性
        public string DbType
        {
            get { return "MySQL"; }
        }
        private string _dbconnectStr;
        public string DbConnectStr
        {
            set { _dbconnectStr = value; }
            get { return _dbconnectStr; }
        }
        MySqlConnection connect = new MySqlConnection();

        #endregion

        #region 构造函数，构造基本信息
        public DbObject()
        {
            IsDboSp();
        }

        /// <summary>
        /// 构造一个数据库连接
        /// </summary>
        /// <param name="connect"></param>
        public DbObject(string DbConnectStr)
        {
            _dbconnectStr = DbConnectStr;
            connect.ConnectionString = DbConnectStr;
        }
        /// <summary>
        /// 构造一个连接字符串
        /// </summary>
        /// <param name="SSPI">是否windows集成认证</param>
        /// <param name="Ip">服务器IP</param>
        /// <param name="User">用户名</param>
        /// <param name="Pass">密码</param>
        public DbObject(bool SSPI, string Ip, string User, string Pass)
        {
            connect = new MySqlConnection();
            if (SSPI)
            {
                //_dbconnectStr="Integrated Security=SSPI;Data Source="+Ip+";Initial Catalog=mysql";
                _dbconnectStr = String.Format("server={0};user id={1}; password={2}; database=mysql; pooling=false", Ip, User, Pass);
            }
            else
            {
                _dbconnectStr = String.Format("server={0};user id={1}; password={2}; database=mysql; pooling=false", Ip, User, Pass);

            }
            connect.ConnectionString = _dbconnectStr;

        }


        #endregion

        #region  是否采用sp(存储过程)的方式获取数据结构信息
        /// <summary>
        /// 是否采用sp的方式获取数据结构信息
        /// </summary>
        /// <returns></returns>
        private bool IsDboSp()
        {
            if (File.Exists(cmcfgfile))
            {
                cfgfile = new LTP.Utility.INIFile(cmcfgfile);
                string val = cfgfile.IniReadValue("dbo", "dbosp");
                if (val.Trim() == "1")
                {
                    isdbosp = true;
                }
            }
            return isdbosp;
        }

        #endregion

        #region 打开数据库 OpenDB(string DbName)

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="DbName">要打开的数据库</param>
        /// <returns></returns>
        private MySqlCommand OpenDB(string DbName)
        {
            try
            {
                if (connect.ConnectionString == "")
                {
                    connect.ConnectionString = _dbconnectStr;
                }
                if (connect.ConnectionString != _dbconnectStr)
                {
                    connect.Close();
                    connect.ConnectionString = _dbconnectStr;
                }
                MySqlCommand dbCommand = new MySqlCommand();
                dbCommand.Connection = connect;
                if (connect.State == System.Data.ConnectionState.Closed)
                {
                    connect.Open();
                }
                dbCommand.CommandText = "use " + DbName + "";
                dbCommand.ExecuteNonQuery();
                return dbCommand;

            }
            catch (System.Exception ex)
            {
                string str = ex.Message;
                return null;
            }

        }
        #endregion

        #region ADO.NET 操作

        public int ExecuteSql(string DbName, string SQLString)
        {
            MySqlCommand dbCommand = OpenDB(DbName);
            dbCommand.CommandText = SQLString;
            int rows = dbCommand.ExecuteNonQuery();
            return rows;
        }
        public DataSet Query(string DbName, string SQLString)
        {
            DataSet ds = new DataSet();
            try
            {
                OpenDB(DbName);
                MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connect);
                command.Fill(ds, "ds");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
        public MySqlDataReader ExecuteReader(string DbName, string strSQL)
        {
            try
            {
                OpenDB(DbName);
                MySqlCommand cmd = new MySqlCommand(strSQL, connect);
                MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw ex;
            }
        }
        public object GetSingle(string DbName, string SQLString)
        {
            try
            {
                MySqlCommand dbCommand = OpenDB(DbName);
                dbCommand.CommandText = SQLString;
                object obj = dbCommand.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
                else
                {
                    return obj;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public DataSet RunProcedure(string DbName, string storedProcName, IDataParameter[] parameters, string tableName)
        {

            OpenDB(DbName);
            DataSet dataSet = new DataSet();
            MySqlDataAdapter sqlDA = new MySqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(connect, storedProcName, parameters);
            sqlDA.Fill(dataSet, tableName);

            return dataSet;

        }
        private MySqlCommand BuildQueryCommand(MySqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            MySqlCommand command = new MySqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (MySqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// List根据字符串排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0代表相等，-1代表y大于x，1代表x大于y</returns>
        private int CompareStrByOrder(string x, string y)
        {
            if (x == "")
            {
                if (y == "")
                {
                    return 0;// If x is null and y is null, they're equal. 
                }
                else
                {
                    return -1;// If x is null and y is not null, y is greater. 
                }
            }
            else
            {
                if (y == "")  // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    int retval = x.CompareTo(y);
                    return retval;
                }
            }
        }

        #endregion


        #region 得到数据库的名字列表 GetDBList()

        /// <summary>
        /// 得到数据库的名字列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetDBList()
        {
            List<string> dblist = new List<string>();
            string strSql = "SHOW DATABASES";
            MySqlDataReader reader = ExecuteReader("mysql", strSql);
            while (reader.Read())
            {
                dblist.Add(reader.GetString(0));
            }
            reader.Close();

            dblist.Sort(CompareStrByOrder);

            return dblist;

        }
        #endregion

        #region  得到数据库的所有表和视图 的名字



        /// <summary>
        /// 得到数据库的所有表名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<string> GetTables(string DbName)
        {
            string strSql = "SHOW TABLES";
            List<string> tabNames = new List<string>();
            MySqlDataReader reader = ExecuteReader(DbName, strSql);
            while (reader.Read())
            {
                tabNames.Add(reader.GetString(0));
            }
            reader.Close();
            tabNames.Sort(CompareStrByOrder);
            return tabNames;
        }
        public Dictionary<string, string> GetTablesEx(string DbName)
        {

            string strSql = string.Format ("select table_name,table_comment from Information_schema.TABLES where   table_schema = '{0}' ",DbName );//order by id
            //return Query(DbName,strSql).Tables[0];

            Dictionary<string, string> tabNames = new Dictionary<string, string>();
            DataTable dt = Query(DbName, strSql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string caption = null;
                if (dr[1] != DBNull.Value)
                    caption = dr[1].ToString();
                tabNames.Add(dr[0].ToString(), caption);
            }

            return tabNames;
        }
        public Dictionary<string, string> GetViewsEx(string DbName)
        {
            return null;
        }
        public DataTable GetTablesSP(string DbName)
        {
            MySqlParameter[] parameters = {
					new MySqlParameter("@table_name", MySqlDbType.VarChar,384),
					new MySqlParameter("@table_owner", MySqlDbType.VarChar,384),
                    new MySqlParameter("@table_qualifier", MySqlDbType.VarChar,384),
                    new MySqlParameter("@table_type", MySqlDbType.VarChar,100)
            };
            parameters[0].Value = null;
            parameters[1].Value = null;
            parameters[2].Value = null;
            parameters[3].Value = "'TABLE'";

            DataSet ds = RunProcedure(DbName, "sp_tables", parameters, "ds");
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dt.Columns["TABLE_QUALIFIER"].ColumnName = "db";
                dt.Columns["TABLE_OWNER"].ColumnName = "cuser";
                dt.Columns["TABLE_NAME"].ColumnName = "name";
                dt.Columns["TABLE_TYPE"].ColumnName = "type";
                dt.Columns["REMARKS"].ColumnName = "remarks";
                return dt;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到数据库的所有视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetVIEWs(string DbName)
        {
            //if (isdbosp)
            //{
            //    return GetVIEWsSP(DbName);
            //}
            //string strSql="select [name] from sysobjects where xtype='V' and [name]<>'syssegments' and [name]<>'sysconstraints' order by [name]";//order by id
            //return Query(DbName,strSql).Tables[0];
            return null;
        }
        /// <summary>
        /// 得到数据库的所有视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetVIEWsSP(string DbName)
        {
            MySqlParameter[] parameters = {
					new MySqlParameter("@table_name", MySqlDbType.VarChar,384),
					new MySqlParameter("@table_owner", MySqlDbType.VarChar,384),
                    new MySqlParameter("@table_qualifier", MySqlDbType.VarChar,384),
                    new MySqlParameter("@table_type", MySqlDbType.VarChar,100)
            };
            parameters[0].Value = null;
            parameters[1].Value = null;
            parameters[2].Value = null;
            parameters[3].Value = "'VIEW'";

            DataSet ds = RunProcedure(DbName, "sp_tables", parameters, "ds");
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dt.Columns["TABLE_QUALIFIER"].ColumnName = "db";
                dt.Columns["TABLE_OWNER"].ColumnName = "cuser";
                dt.Columns["TABLE_NAME"].ColumnName = "name";
                dt.Columns["TABLE_TYPE"].ColumnName = "type";
                dt.Columns["REMARKS"].ColumnName = "remarks";
                return dt;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到数据库的所有表和视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetTabViews(string DbName)
        {
            string strSql = "SHOW TABLES";//order by id

            DataTable dt = Query(DbName, strSql).Tables[0];
            dt.Columns[0].ColumnName = "name";
            return dt;
        }
        /// <summary>
        /// 得到数据库的所有表和视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetTabViewsSP(string DbName)
        {
            return null;
        }

        /// <summary>
        /// 得到数据库的所有存储过程名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetProcs(string DbName)
        {
            return null;
        }
        #endregion

        #region  得到数据库的所有表和视图 的列表信息
        /// <summary>
        /// 得到数据库的所有表的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<TableInfo> GetTablesInfo(string DbName)
        {
            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            string strSql = "SHOW TABLE STATUS";
            MySqlDataReader reader = ExecuteReader(DbName, strSql);
            while (reader.Read())
            {

                tab = new TableInfo();
                tab.TabName = reader.GetString("Name");
                try
                {
                    if (reader["Create_time"] != null)
                    {
                        tab.TabDate = reader.GetString("Create_time");
                    }
                }
                catch
                { }
                tab.TabType = "U";
                tab.TabUser = "dbo";
                tablist.Add(tab);

            }
            reader.Close();
            return tablist;

        }
        /// <summary>
        /// 得到数据库的所有视图的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<TableInfo> GetVIEWsInfo(string DbName)
        {
            return null;
        }
        /// <summary>
        /// 得到数据库的所有表和视图的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<TableInfo> GetTabViewsInfo(string DbName)
        {
            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            string strSql = "SHOW TABLE STATUS";
            MySqlDataReader reader = ExecuteReader(DbName, strSql);
            while (reader.Read())
            {
                tab = new TableInfo();
                tab.TabName = reader.GetString("Name");
                try
                {
                    if (reader["Create_time"] != null)
                    {
                        tab.TabDate = reader.GetString("Create_time");
                    }
                }
                catch
                { }               
                tab.TabType = "U";
                tab.TabUser = "dbo";
                tablist.Add(tab);
            }
            reader.Close();
            return tablist;
        }
        /// <summary>
        /// 得到数据库的所有存储过程的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<TableInfo> GetProcInfo(string DbName)
        {
            return null;
        }
        #endregion

        #region 得到对象定义语句
        /// <summary>
        /// 得到视图或存储过程的定义语句
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public string GetObjectInfo(string DbName, string objName)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select a.name,'',a.xtype,a.crdate,b.text ");
            strSql.Append("select b.text ");
            strSql.Append("from sysobjects a, syscomments b  ");
            strSql.Append("where a.xtype='p' and a.id = b.id  ");
            strSql.Append(" and a.name= '" + objName + "'");
            return GetSingle(DbName, strSql.ToString()).ToString();
        }
        #endregion

        #region 得到(快速)数据库里表的列名和类型 GetColumnList(string DbName,string TableName)

        /// <summary>
        /// 得到数据库里表或视图的列名和类型
        /// </summary>
        /// <param name="DbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public List<ColumnInfo> GetColumnList(string DbName, string TableName)
        {
            return GetColumnInfoList(DbName, TableName);

        }
        public List<ColumnInfo> GetColumnListSP(string DbName, string TableName)
        {
            return GetColumnInfoList(DbName, TableName);
        }
        #endregion


        #region 得到表的列的详细信息 GetColumnInfoList(string DbName,string TableName)
        /// <summary>
        /// 得到数据库里表或视图的列的详细信息
        /// </summary>
        /// <param name="DbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public List<ColumnInfo> GetColumnInfoList(string DbName, string TableName)
        {
            try
            {
                string sql = "select * from " + TableName + " where 1 = 0 ";
                DataTable tbl = Query(DbName, sql).Tables[0];
                //if (isdbosp)
                //{               
                //   return GetColumnInfoListSP(DbName, TableName);                
                //}           
                string strSql = "SHOW COLUMNS FROM " + TableName;
                strSql = string.Format("SELECT column_name field,data_type type,is_nullable ,column_key  ,extra ,column_comment comment,column_default FROM Information_schema.columns where table_name = '{0}' and table_schema = '{1}' ", TableName, DbName);
                List<ColumnInfo> collist = new List<ColumnInfo>();
                ColumnInfo col;
                MySqlDataReader reader = ExecuteReader(DbName, strSql);
                int n = 1;
                while (reader.Read())
                {
                    col = new ColumnInfo();
                    col.Colorder = n.ToString();
                    if ((!Object.Equals(reader["Field"], null)) && (!Object.Equals(reader["Field"], System.DBNull.Value)))
                    {
                        string tname = reader["Field"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                col.ColumnName = Encoding.Default.GetString((Byte[])reader["Field"]);
                                break;
                            case "":
                                break;
                            default:
                                col.ColumnName = reader["Field"].ToString();
                                break;
                        }
                    }
                    if ((!Object.Equals(reader["Type"], null)) && (!Object.Equals(reader["Type"], System.DBNull.Value)))
                    {
                        string tname = reader["Type"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                col.TypeName = Encoding.Default.GetString((Byte[])reader["Type"]);
                                break;
                            case "":
                                break;
                            default:
                                col.TypeName = reader["Type"].ToString();
                                break;
                        }
                    }
                    string typename = col.TypeName, len = "", pre = "", scal = "";
                    TypeNameProcess(col.TypeName, out typename, out len, out pre, out scal);
                    col.TypeName = typename;
                    col.ColumnType = tbl.Columns[col.ColumnName].DataType;
                    col.Length = len;
                    col.Preci = pre; 
                    col.Scale = scal;
                    col.DeText = reader["comment"].ToString();
                    if ((!Object.Equals(reader["column_key"], null)) && (!Object.Equals(reader["column_key"], System.DBNull.Value)))
                    {
                        string skey = "";
                        string tname = reader["column_key"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                skey = Encoding.Default.GetString((Byte[])reader["column_key"]);
                                break;
                            case "":
                                break;
                            default:
                                skey = reader["column_key"].ToString();
                                break;
                        }
                        col.IsPK = (skey.Trim() == "PRI") ? true : false;
                    }
                    if ((!Object.Equals(reader["is_nullable"], null)) && (!Object.Equals(reader["is_nullable"], System.DBNull.Value)))
                    {
                        string snull = "";
                        string tname = reader["is_nullable"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                snull = Encoding.Default.GetString((Byte[])reader["is_nullable"]);
                                break;
                            case "":
                                break;
                            default:
                                snull = reader["is_nullable"].ToString();
                                break;
                        }
                        col.cisNull = (snull.Trim() == "YES") ? true : false;
                    }
                    if ((!Object.Equals(reader["column_default"], null)) && (!Object.Equals(reader["column_default"], System.DBNull.Value)))
                    {
                        string tname = reader["column_default"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                col.DefaultVal = Encoding.Default.GetString((Byte[])reader["column_default"]);
                                break;
                            case "":
                                break;
                            default:
                                col.DefaultVal = reader["column_default"].ToString();
                                break;
                        }
                    }
                    col.IsIdentity = false;
                    if ((!Object.Equals(reader["Extra"], null)) && (!Object.Equals(reader["Extra"], System.DBNull.Value)))
                    {

                        string tname = reader["Extra"].GetType().Name;
                        string identity = "";
                        switch (tname)
                        {
                            case "Byte[]":
                                identity = Encoding.Default.GetString((Byte[])reader["Extra"]);
                                break;
                            case "":
                                break;
                            default:
                                identity = reader["Extra"].ToString();
                                break;
                        }
                        if (identity.Trim() == "auto_increment")
                        {
                            col.IsIdentity = true;
                        }
                    }

                    collist.Add(col);
                    n++;
                }
                reader.Close();
                return collist;
            }
            catch (System.Exception ex)
            {
                throw new Exception("获取列数据失败" + ex.Message);
            }

        }

        //对类型名称 解析
        private void TypeNameProcess(string strName, out string TypeName, out string Length, out string Preci, out string Scale)
        {
            TypeName = strName;
            int n = strName.IndexOf("(");
            Length = "";
            Preci = "";
            Scale = "";
            if (n > 0)
            {
                TypeName = strName.Substring(0, n);
                switch (TypeName.Trim().ToUpper())
                {
                    //只有大小(M)
                    case "TINYINT":
                    case "SMALLINT":
                    case "MEDIUMINT":
                    case "INT":
                    case "INTEGER":
                    case "BIGINT":
                    case "TIMESTAMP":
                    case "CHAR":
                    case "VARCHAR":
                        {
                            int m = strName.IndexOf(")");
                            Length = strName.Substring(n + 1, m - n - 1);
                        }
                        break;
                    case "FLOAT"://(M,D)
                    case "DOUBLE":
                    case "REAL":
                    case "DECIMAL":
                    case "DEC":
                    case "NUMERIC":
                        {
                            int m = strName.IndexOf(")");
                            string strlen = strName.Substring(n + 1, m - n - 1);
                            int i = strlen.IndexOf(",");
                            Length = strlen.Substring(0, i);
                            Scale = strlen.Substring(i + 1);
                        }
                        break;
                    case "ENUM"://(M1,M2,M3)
                    case "SET":
                        {
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        public DataTable GetColumnInfoListSP(string DbName, string TableName)
        {
            return null;
        }

        #endregion


        #region 得到数据库里表的主键 GetKeyName(string DbName,string TableName)

        //创建列信息表
        public DataTable CreateColumnTable()
        {
            DataTable table = new DataTable();
            DataColumn col;

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "colorder";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "ColumnName";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "TypeName";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "Length";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "Preci";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "Scale";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "IsIdentity";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "isPK";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "cisNull";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "defaultVal";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "deText";
            table.Columns.Add(col);

            return table;
        }

        /// <summary>
        /// 得到数据库里表的主键
        /// </summary>
        /// <param name="DbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public DataTable GetKeyName(string DbName, string TableName)
        {
            DataTable dtkey = CreateColumnTable();
            List<ColumnInfo> collist = GetColumnInfoList(DbName, TableName);
            DataTable dt = LTP.CodeHelper.CodeCommon.GetColumnInfoDt(collist);
            DataRow[] rows = dt.Select(" isPK='√' or IsIdentity='√' ");
            foreach (DataRow row in rows)
            {
                DataRow nrow = dtkey.NewRow();
                nrow["colorder"] = row["colorder"];
                nrow["ColumnName"] = row["ColumnName"];
                nrow["TypeName"] = row["TypeName"];
                nrow["Length"] = row["Length"];
                nrow["Preci"] = row["Preci"];
                nrow["Scale"] = row["Scale"];
                nrow["IsIdentity"] = row["IsIdentity"];
                nrow["isPK"] = row["isPK"];
                nrow["cisNull"] = row["cisNull"];
                nrow["defaultVal"] = row["defaultVal"];
                nrow["deText"] = row["deText"];
                dtkey.Rows.Add(nrow);
            }
            return dtkey;


        }
        #endregion

        #region 得到数据表里的数据 GetTabData(string DbName,string TableName)

        /// <summary>
        /// 得到数据表里的数据
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public DataTable GetTabData(string DbName, string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from " + TableName + "");
            return Query(DbName, strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 根据SQL查询得到数据表里的数据
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public DataTable GetTabDataBySQL(string DbName, string strSQL)
        {
            return Query(DbName, strSQL).Tables[0];
        }

        #endregion


        #region 数据库属性操作

        /// <summary>
        /// 修改表名称
        /// </summary>
        /// <param name="OldName"></param>
        /// <param name="NewName"></param>
        /// <returns></returns>
        public bool RenameTable(string DbName, string OldName, string NewName)
        {
            try
            {
                MySqlCommand dbCommand = OpenDB(DbName);
                dbCommand.CommandText = "RENAME TABLE " + OldName + " TO " + NewName + "";
                dbCommand.ExecuteNonQuery();
                return true;
            }
            catch//(System.Exception ex)
            {
                //string str=ex.Message;	
                return false;
            }
        }

        /// <summary>
        /// 删除表
        /// </summary>	
        public bool DeleteTable(string DbName, string TableName)
        {
            try
            {
                MySqlCommand dbCommand = OpenDB(DbName);
                dbCommand.CommandText = "DROP TABLE " + TableName + "";
                dbCommand.ExecuteNonQuery();
                return true;
            }
            catch//(System.Exception ex)
            {
                //string str=ex.Message;	
                return false;
            }
        }

        /// <summary>
        /// 得到版本号
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            try
            {
                string strSql = "execute master..sp_msgetversion ";//select @@version
                return Query("master", strSql).Tables[0].Rows[0][0].ToString();
            }
            catch//(System.Exception ex)
            {
                //string str=ex.Message;	
                return "";
            }
        }


        /// <summary>
        /// 得到创建表 的脚本
        /// </summary>
        /// <returns></returns>
        public string GetTableScript(string DbName, string TableName)
        {
            string strScript = "";
            string strSql = "SHOW CREATE TABLE " + TableName;
            MySqlDataReader reader = ExecuteReader(DbName, strSql);
            while (reader.Read())
            {
                strScript = reader.GetString(1);
            }
            reader.Close();
            return strScript;

        }
        #endregion



    }
}
