﻿using System;
using System.Data;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SqlClient;
using LTP.CodeHelper;
using LTP.IDBO;
namespace LTP.DbObjects.SQL2005
{
	/// <summary>
	/// 数据库信息类。
	/// </summary>
	public class DbObject:IDbObject
	{
        string cmcfgfile = Application.StartupPath + @"\cmcfg.ini";
        LTP.Utility.INIFile cfgfile;
        bool isdbosp = false;

		#region  属性
        public string DbType
        {
            get { return "SQL2005"; }            
        }
		private string _dbconnectStr;	
		public string DbConnectStr
		{
			set{_dbconnectStr=value;}
			get{return _dbconnectStr;}
		}
		SqlConnection connect = new SqlConnection();
		
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
			_dbconnectStr=DbConnectStr;
			connect.ConnectionString=DbConnectStr;
		}		
		/// <summary>
		/// 构造一个连接字符串
		/// </summary>
		/// <param name="SSPI">是否windows集成认证</param>
		/// <param name="Ip">服务器IP</param>
		/// <param name="User">用户名</param>
		/// <param name="Pass">密码</param>
		public DbObject(bool SSPI,string Ip,string User,string Pass)
		{		
			connect = new SqlConnection();
			if(SSPI)
			{
				_dbconnectStr="Integrated Security=SSPI;Data Source="+Ip+";Initial Catalog=master";
				//constr="Provider=SQLOLEDB;Data Source="+ip+";Initial Catalog=master;Integrated Security=SSPI";//这是OleDbConnection连接的字符串
			}
			else
			{				
				if(Pass=="")
				{
					_dbconnectStr="user id="+User+";initial catalog=master;data source="+Ip+";Connect Timeout=30";
				}
				else
				{
					_dbconnectStr="user id="+User+";password="+Pass+";initial catalog=master;data source="+Ip+";Connect Timeout=30";
				}			
			}
			connect.ConnectionString=_dbconnectStr;

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
		private SqlCommand OpenDB(string DbName)
		{
			try
			{
				if(connect.ConnectionString=="")
				{
					connect.ConnectionString=_dbconnectStr;
				}
				if(connect.ConnectionString!=_dbconnectStr)
				{
					connect.Close();
					connect.ConnectionString=_dbconnectStr;
				}
				SqlCommand dbCommand = new SqlCommand();
				dbCommand.Connection=connect;	
				if(connect.State==System.Data.ConnectionState.Closed)
				{
					connect.Open();
				}	
				dbCommand.CommandText="use ["+DbName+"]";
				dbCommand.ExecuteNonQuery();
				return dbCommand;

			}
			catch(System.Exception ex)
			{
				string str=ex.Message;	
				return null;
			}
			
		}
		#endregion

		#region ADO.NET 操作

		public int ExecuteSql(string DbName,string SQLString)
		{
			SqlCommand dbCommand=OpenDB(DbName);
			dbCommand.CommandText=SQLString;
			int rows=dbCommand.ExecuteNonQuery();
			return rows;
		}
		public DataSet Query(string DbName,string SQLString)
		{			
			DataSet ds = new DataSet();
			try
			{		
				OpenDB(DbName);
				SqlDataAdapter command = new SqlDataAdapter(SQLString,connect);				
				command.Fill(ds,"ds");
			}
			catch(System.Data.SqlClient.SqlException ex)
			{				
				throw new Exception(ex.Message);
			}			
			return ds;				
		}
        public SqlDataReader ExecuteReader(string DbName, string strSQL)
        {
            try
            {
                OpenDB(DbName);
                SqlCommand cmd = new SqlCommand(strSQL, connect);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
                SqlCommand dbCommand = OpenDB(DbName);
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
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(connect, storedProcName, parameters);
            sqlDA.Fill(dataSet, tableName);
           
            return dataSet;
            
        }
        private SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
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


		#region 得到数据库的名字列表 GetDBList()

		/// <summary>
		/// 得到数据库的名字列表
		/// </summary>
		/// <returns></returns>
		public List<string> GetDBList()
		{					
            List<string> dblist=new List<string>();
            string strSql = "select name from sysdatabases order by name";
            //return Query("master",strSql).Tables[0];
            SqlDataReader reader = ExecuteReader("master", strSql);
            while (reader.Read())
            {
                dblist.Add(reader.GetString(0));
            }
            reader.Close();
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
            if (isdbosp)
            {
                return GetTablesSP(DbName);
            }
			string strSql="select [name] from sysobjects where xtype='U'and [name]<>'dtproperties' order by [name]";//order by id
            //return Query(DbName,strSql).Tables[0];
            List<string> tabNames = new List<string>();
            SqlDataReader reader = ExecuteReader(DbName, strSql);
            while (reader.Read())
            {
                tabNames.Add(reader.GetString(0));
            }
            reader.Close();
            return tabNames;
		}

  

        /// <summary>
        /// 得到数据库的所有表名(表名,表說明)
        /// </summary>
        /// <param name="DbName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetTablesEx(string DbName)
        {

            string strSql = "select  obj .name ,value  from sysobjects obj left join sys.extended_properties  on id =major_id and minor_id = 0    where xtype='U'and obj.[name]<>'dtproperties'   order by obj.name ";//order by id
            //return Query(DbName,strSql).Tables[0];
           
            Dictionary<string, string> tabNames = new Dictionary<string, string>();
            DataTable dt = Query(DbName, strSql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string caption = null;
                if (dr[1] !=DBNull.Value )
                    caption = dr[1].ToString () ;
                tabNames.Add(dr[0].ToString(), caption);
            }
           
            return tabNames;
        }
        public Dictionary<string, string> GetViewsEx(string DbName)
        {  
            string strSql = "select  obj .name ,value  from sysobjects obj left join sys.extended_properties  on id =major_id and minor_id = 0    where xtype='V'and obj.[name]<>'dtproperties'   order by obj.name ";//order by id
            //return Query(DbName,strSql).Tables[0];
           
            Dictionary<string, string> tabNames = new Dictionary<string, string>();
            DataTable dt = Query(DbName, strSql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string caption = null;
                if (dr[1] !=DBNull.Value )
                    caption = dr[1].ToString () ;
                tabNames.Add(dr[0].ToString(), caption);
            }
           
            return tabNames;
            
        }
        public List<string> GetTablesSP(string DbName)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@table_name", SqlDbType.NVarChar,384),
					new SqlParameter("@table_owner", SqlDbType.NVarChar,384),
                    new SqlParameter("@table_qualifier", SqlDbType.NVarChar,384),
                    new SqlParameter("@table_type", SqlDbType.VarChar,100)
            };
            parameters[0].Value = null;
            parameters[1].Value = null;
            parameters[2].Value = null;
            parameters[3].Value = "'TABLE'";

            DataSet ds = RunProcedure(DbName, "sp_tables", parameters, "ds");
            List<string> tabNames = new List<string>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                //dt.Columns["TABLE_QUALIFIER"].ColumnName = "db";
                //dt.Columns["TABLE_OWNER"].ColumnName = "cuser";
                //dt.Columns["TABLE_NAME"].ColumnName = "name";
                //dt.Columns["TABLE_TYPE"].ColumnName = "type";
                //dt.Columns["REMARKS"].ColumnName = "remarks";
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    tabNames.Add(dt.Rows[n]["TABLE_NAME"].ToString());
                }
                return tabNames;
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
            if (isdbosp)
            {
                return GetVIEWsSP(DbName);
            }
			string strSql="select [name] from sysobjects where xtype='V' and [name]<>'syssegments' and [name]<>'sysconstraints' order by [name]";//order by id
			return Query(DbName,strSql).Tables[0];
		}
        /// <summary>
        /// 得到数据库的所有视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetVIEWsSP(string DbName)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@table_name", SqlDbType.NVarChar,384),
					new SqlParameter("@table_owner", SqlDbType.NVarChar,384),
                    new SqlParameter("@table_qualifier", SqlDbType.NVarChar,384),
                    new SqlParameter("@table_type", SqlDbType.VarChar,100)
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
            if (isdbosp)
            {
                return GetTabViewsSP(DbName);
            }
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select [name],sysobjects.xtype type from sysobjects ");
            strSql.Append("where (xtype='U' or xtype='V' or xtype='P') ");
			strSql.Append("and [name]<>'dtproperties' and [name]<>'syssegments' and [name]<>'sysconstraints' ");
			strSql.Append("order by xtype,[name]");			
			return Query(DbName,strSql.ToString()).Tables[0];
		}
        /// <summary>
        /// 得到数据库的所有表和视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetTabViewsSP(string DbName)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@table_name", SqlDbType.NVarChar,384),
					new SqlParameter("@table_owner", SqlDbType.NVarChar,384),
                    new SqlParameter("@table_qualifier", SqlDbType.NVarChar,384),
                    new SqlParameter("@table_type", SqlDbType.VarChar,100)
            };
            parameters[0].Value = null;
            parameters[1].Value = null;
            parameters[2].Value = null;
            parameters[3].Value = "'TABLE','VIEW'";

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
        /// 得到数据库的所有存储过程名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetProcs(string DbName)
        {
            string strSql = "select [name] from sysobjects where xtype='P'and [name]<>'dtproperties' order by [name]";//order by id
            return Query(DbName, strSql).Tables[0];
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select sysobjects.[name] name,sysusers.name cuser,");
			strSql.Append("sysobjects.xtype type,sysobjects.crdate dates ");
			strSql.Append("from sysobjects,sysusers ");
			strSql.Append("where sysusers.uid=sysobjects.uid ");
			strSql.Append("and sysobjects.xtype='U' ");
			strSql.Append("and  sysobjects.[name]<>'dtproperties' ");
            strSql.Append("order by sysobjects.[name] ");
            //strSql.Append("order by sysobjects.id");
            //return Query(DbName,strSql.ToString()).Tables[0];

            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            SqlDataReader reader = ExecuteReader(DbName, strSql.ToString());
            while (reader.Read())
            {
                tab = new TableInfo();
                tab.TabName = reader.GetString(0);
                tab.TabDate = reader.GetValue(3).ToString();
                tab.TabType = reader.GetString(2);
                tab.TabUser = reader.GetString(1);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select sysobjects.[name] name,sysusers.name cuser,");
			strSql.Append("sysobjects.xtype type,sysobjects.crdate dates ");
			strSql.Append("from sysobjects,sysusers ");
			strSql.Append("where sysusers.uid=sysobjects.uid ");
			strSql.Append("and sysobjects.xtype='V' ");
			strSql.Append("and sysobjects.[name]<>'syssegments' and sysobjects.[name]<>'sysconstraints'  ");
            //strSql.Append("order by sysobjects.id");
            strSql.Append("order by sysobjects.[name] ");
            //return Query(DbName,strSql.ToString()).Tables[0];

            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            SqlDataReader reader = ExecuteReader(DbName, strSql.ToString());
            while (reader.Read())
            {
                tab = new TableInfo();
                tab.TabName = reader.GetString(0);
                tab.TabDate = reader.GetValue(3).ToString();
                tab.TabType = reader.GetString(2);
                tab.TabUser = reader.GetString(1);
                tablist.Add(tab);
            }
            reader.Close();
            return tablist;
		}
		/// <summary>
		/// 得到数据库的所有表和视图的详细信息
		/// </summary>
		/// <param name="DbName">数据库</param>
		/// <returns></returns>
        public List<TableInfo> GetTabViewsInfo(string DbName)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select sysobjects.[name] name,sysusers.name cuser,");
			strSql.Append("sysobjects.xtype type,sysobjects.crdate dates ");
			strSql.Append("from sysobjects,sysusers ");
			strSql.Append("where sysusers.uid=sysobjects.uid ");
            strSql.Append("and (sysobjects.xtype='U' or sysobjects.xtype='V' or sysobjects.xtype='P') ");
			strSql.Append("and sysobjects.[name]<>'dtproperties' and sysobjects.[name]<>'syssegments' and sysobjects.[name]<>'sysconstraints'  ");
            //strSql.Append("order by sysobjects.id");
            strSql.Append("order by sysobjects.xtype,sysobjects.[name] ");
            //return Query(DbName,strSql.ToString()).Tables[0];

            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            SqlDataReader reader = ExecuteReader(DbName, strSql.ToString());
            while (reader.Read())
            {
                tab = new TableInfo();
                tab.TabName = reader.GetString(0);
                tab.TabDate = reader.GetValue(3).ToString();
                tab.TabType = reader.GetString(2);
                tab.TabUser = reader.GetString(1);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sysobjects.[name] name,sysusers.name cuser,");
            strSql.Append("sysobjects.xtype type,sysobjects.crdate dates ");
            strSql.Append("from sysobjects,sysusers ");
            strSql.Append("where sysusers.uid=sysobjects.uid ");
            strSql.Append("and sysobjects.xtype='P' ");
            //strSql.Append("and  sysobjects.[name]<>'dtproperties' ");
            //strSql.Append("order by sysobjects.id");
            strSql.Append("order by sysobjects.[name] ");
            //return Query(DbName, strSql.ToString()).Tables[0];

            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            SqlDataReader reader = ExecuteReader(DbName, strSql.ToString());
            while (reader.Read())
            {
                tab = new TableInfo();
                tab.TabName = reader.GetString(0);
                tab.TabDate = reader.GetValue(3).ToString();
                tab.TabType = reader.GetString(2);
                tab.TabUser = reader.GetString(1);
                tablist.Add(tab);
            }
            reader.Close();
            return tablist;
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
			try
			{
                DataTable tbl = Query(DbName, "select * from " + TableName+" where 1 = 0 ").Tables[0];
                if (isdbosp)
                {
                    return GetColumnListSP(DbName, TableName);
                }
				StringBuilder strSql=new StringBuilder();
				strSql.Append("Select ");
                strSql.Append("a.colorder as colorder,");
				strSql.Append("a.name as ColumnName,");
				strSql.Append("b.name as TypeName ");
				strSql.Append(" from syscolumns a, systypes b, sysobjects c ");
				strSql.Append(" where a.xtype = b.xusertype ");
				strSql.Append("and a.id = c.id ");
//				strSql.Append("and c.xtype='U' ");
				strSql.Append("and c.name ='"+TableName+"'");
				strSql.Append(" order by a.colorder");
			
                //return Query(DbName,strSql.ToString()).Tables[0];
                ArrayList colexist = new ArrayList();//是否已经存在
                List<ColumnInfo> collist = new List<ColumnInfo>();
                ColumnInfo col;
                SqlDataReader reader = ExecuteReader(DbName,strSql.ToString());
                while (reader.Read())
                {
                    col = new ColumnInfo();
                    col.Colorder = reader.GetValue(0).ToString();
                    col.ColumnName = reader.GetString(1);
                    col.TypeName = reader.GetString(2);
                    col.ColumnType = tbl.Columns[col.ColumnName].DataType;
                    col.Length = "";
                    col.Preci = "";
                    col.Scale = "";
                    col.IsPK = false;
                    col.cisNull = false;
                    col.DefaultVal = "";
                    col.IsIdentity =false;
                    col.DeText = "";                    
                    if (!colexist.Contains(col.ColumnName))
                    {
                        collist.Add(col);
                        colexist.Add(col.ColumnName);
                    }
                }
                reader.Close();
                return collist;

			}
			catch(System.Exception)
			{				
				return null;
			}

		}
        public List<ColumnInfo> GetColumnListSP(string DbName, string TableName)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@table_name", SqlDbType.NVarChar,384),
					new SqlParameter("@table_owner", SqlDbType.NVarChar,384),
                    new SqlParameter("@table_qualifier", SqlDbType.NVarChar,384),
                    new SqlParameter("@column_name", SqlDbType.VarChar,100)
            };
            parameters[0].Value = TableName;
            parameters[1].Value = null;
            parameters[2].Value = null;
            parameters[3].Value = null;

            DataSet ds = RunProcedure(DbName, "sp_columns", parameters, "ds");
            int n = ds.Tables.Count;
            if (n > 0)
            {
                DataTable dt = ds.Tables[0];
                int r = dt.Rows.Count;
                DataTable dtkey = CreateColumnTable();
                for (int m = 0; m < r; m++)
                {
                    DataRow nrow = dtkey.NewRow();
                    nrow["colorder"] = dt.Rows[m]["ORDINAL_POSITION"];
                    nrow["ColumnName"] = dt.Rows[m]["COLUMN_NAME"];
                    string tn = dt.Rows[m]["TYPE_NAME"].ToString().Trim();
                    nrow["TypeName"] = (tn == "int identity") ? "int" : tn;
                    nrow["Length"] = dt.Rows[m]["LENGTH"];
                    nrow["Preci"] = dt.Rows[m]["PRECISION"];
                    nrow["Scale"] = dt.Rows[m]["SCALE"];
                    nrow["IsIdentity"] = (tn == "int identity") ? "√" : "";
                    nrow["isPK"] = "";
                    nrow["cisNull"] = (dt.Rows[m]["NULLABLE"].ToString().Trim() == "1") ? "√" : "";
                    nrow["defaultVal"] = dt.Rows[m]["COLUMN_DEF"];
                    nrow["deText"] = dt.Rows[m]["REMARKS"];
                    dtkey.Rows.Add(nrow);
                }
                return LTP.CodeHelper.CodeCommon.GetColumnInfos(dtkey);
            }
            else
            {
                return null;
            }   

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
            DataTable tbl = Query(DbName, "select * from " + TableName + " where 1 = 0 ").Tables[0];
            if (isdbosp)
            {               
               return GetColumnInfoListSP(DbName, TableName);                
            }            
			StringBuilder strSql=new StringBuilder();					            
			strSql.Append("SELECT ");
			strSql.Append("colorder=C.column_id,");
			strSql.Append("ColumnName=C.name,");
			strSql.Append("TypeName=T.name, ");
            //strSql.Append("Length=C.max_length, ");
            strSql.Append("Length=CASE WHEN T.name='nchar' THEN C.max_length/2 WHEN T.name='nvarchar' THEN C.max_length/2 ELSE C.max_length END,");
			strSql.Append("Preci=C.precision, ");
			strSql.Append("Scale=C.scale, ");
			strSql.Append("IsIdentity=CASE WHEN C.is_identity=1 THEN N'√'ELSE N'' END,");
			strSql.Append("isPK=ISNULL(IDX.PrimaryKey,N''),");			
			
			strSql.Append("Computed=CASE WHEN C.is_computed=1 THEN N'√'ELSE N'' END, ");
			strSql.Append("IndexName=ISNULL(IDX.IndexName,N''), ");
			strSql.Append("IndexSort=ISNULL(IDX.Sort,N''), ");
			strSql.Append("Create_Date=O.Create_Date, ");
			strSql.Append("Modify_Date=O.Modify_date, ");

			strSql.Append("cisNull=CASE WHEN C.is_nullable=1 THEN N'√'ELSE N'' END, ");
			strSql.Append("defaultVal=ISNULL(D.definition,N''), ");
			strSql.Append("deText=ISNULL(PFD.[value],N'') ");
			
			strSql.Append("FROM sys.columns C ");
			strSql.Append("INNER JOIN sys.objects O ");
			strSql.Append("ON C.[object_id]=O.[object_id] ");
            strSql.Append("AND (O.type='U' or O.type='V') ");
			strSql.Append("AND O.is_ms_shipped=0 ");
			strSql.Append("INNER JOIN sys.types T ");
			strSql.Append("ON C.user_type_id=T.user_type_id ");
			strSql.Append("LEFT JOIN sys.default_constraints D ");
			strSql.Append("ON C.[object_id]=D.parent_object_id ");
			strSql.Append("AND C.column_id=D.parent_column_id ");
			strSql.Append("AND C.default_object_id=D.[object_id] ");
			strSql.Append("LEFT JOIN sys.extended_properties PFD ");
			strSql.Append("ON PFD.class=1  ");
			strSql.Append("AND C.[object_id]=PFD.major_id  ");
			strSql.Append("AND C.column_id=PFD.minor_id ");
//			strSql.Append("--AND PFD.name='Caption'  -- 字段说明对应的描述名称(一个字段可以添加多个不同name的描述) ");
			strSql.Append("LEFT JOIN sys.extended_properties PTB ");
			strSql.Append("ON PTB.class=1 ");
			strSql.Append("AND PTB.minor_id=0  ");
			strSql.Append("AND C.[object_id]=PTB.major_id ");
//			strSql.Append("-- AND PFD.name='Caption'  -- 表说明对应的描述名称(一个表可以添加多个不同name的描述)   ");
			strSql.Append("LEFT JOIN ");// -- 索引及主键信息
			strSql.Append("( ");
			strSql.Append("SELECT  ");
			strSql.Append("IDXC.[object_id], ");
			strSql.Append("IDXC.column_id, ");
			strSql.Append("Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending') ");
			strSql.Append("WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END, ");
			strSql.Append("PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'√'ELSE N'' END, ");
			strSql.Append("IndexName=IDX.Name ");
			strSql.Append("FROM sys.indexes IDX ");
			strSql.Append("INNER JOIN sys.index_columns IDXC ");
			strSql.Append("ON IDX.[object_id]=IDXC.[object_id] ");
			strSql.Append("AND IDX.index_id=IDXC.index_id ");
			strSql.Append("LEFT JOIN sys.key_constraints KC ");
			strSql.Append("ON IDX.[object_id]=KC.[parent_object_id] ");
			strSql.Append("AND IDX.index_id=KC.unique_index_id ");
			strSql.Append("INNER JOIN  ");// 对于一个列包含多个索引的情况,只显示第1个索引信息
			strSql.Append("( ");
			strSql.Append("SELECT [object_id], Column_id, index_id=MIN(index_id) ");
			strSql.Append("FROM sys.index_columns ");
			strSql.Append("GROUP BY [object_id], Column_id ");
			strSql.Append(") IDXCUQ ");
			strSql.Append("ON IDXC.[object_id]=IDXCUQ.[object_id] ");
			strSql.Append("AND IDXC.Column_id=IDXCUQ.Column_id ");
			strSql.Append("AND IDXC.index_id=IDXCUQ.index_id ");
			strSql.Append(") IDX ");
			strSql.Append("ON C.[object_id]=IDX.[object_id] ");
			strSql.Append("AND C.column_id=IDX.column_id  ");
		
			strSql.Append("WHERE O.name=N'"+TableName+"' ");
			strSql.Append("ORDER BY O.name,C.column_id  ");
            

            //return Query(DbName,strSql.ToString()).Tables[0];

            ArrayList colexist = new ArrayList();//是否已经存在

            List<ColumnInfo> collist = new List<ColumnInfo>();
            ColumnInfo col;
            SqlDataReader reader = ExecuteReader(DbName, strSql.ToString()); 
            while (reader.Read())
            {
                col = new ColumnInfo();
                col.Colorder = reader.GetValue(0).ToString();
                col.ColumnName = reader.GetString(1);
                col.TypeName = reader.GetString(2);
                col.ColumnType = tbl.Columns[col.ColumnName].DataType;
                col.Length = reader.GetValue(3).ToString();
                col.Preci = reader.GetValue(4).ToString();
                col.Scale = reader.GetValue(5).ToString();
                col.IsIdentity = (reader.GetString(6) == "√") ? true : false;
                col.IsPK = (reader.GetString(7) == "√") ? true : false;
                col.cisNull = (reader.GetString(13) == "√") ? true : false; 
                col.DefaultVal = reader.GetString(14);
                col.DeText = reader.GetString(15);
                if (!colexist.Contains(col.ColumnName))
                {
                    collist.Add(col);
                    colexist.Add(col.ColumnName);
                }
            }
            reader.Close();
            return collist;
		}

        public List<ColumnInfo> GetColumnInfoListSP(string DbName, string TableName)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@table_name", SqlDbType.NVarChar,384),
					new SqlParameter("@table_owner", SqlDbType.NVarChar,384),
                    new SqlParameter("@table_qualifier", SqlDbType.NVarChar,384),
                    new SqlParameter("@column_name", SqlDbType.VarChar,100)
            };
            parameters[0].Value = TableName;
            parameters[1].Value = null;
            parameters[2].Value = null;
            parameters[3].Value = null;

            DataSet ds = RunProcedure(DbName, "sp_columns", parameters, "ds");
            int n = ds.Tables.Count;
            if (n > 0)
            {
                DataTable dt = ds.Tables[0];
                int r = dt.Rows.Count;
                DataTable dtkey = CreateColumnTable();
                for (int m = 0; m < r; m++)
                {
                    DataRow nrow = dtkey.NewRow();
                    nrow["colorder"] = dt.Rows[m]["ORDINAL_POSITION"];
                    nrow["ColumnName"] = dt.Rows[m]["COLUMN_NAME"];
                    string tn = dt.Rows[m]["TYPE_NAME"].ToString().Trim();
                    nrow["TypeName"] = (tn == "int identity") ? "int" : tn;
                    nrow["Length"] = dt.Rows[m]["LENGTH"];
                    nrow["Preci"] = dt.Rows[m]["PRECISION"];
                    nrow["Scale"] = dt.Rows[m]["SCALE"];
                    nrow["IsIdentity"] = (tn == "int identity") ? "√" : "";
                    nrow["isPK"] = "";
                    nrow["cisNull"] = (dt.Rows[m]["NULLABLE"].ToString().Trim() == "1") ? "√" : "";
                    nrow["defaultVal"] = dt.Rows[m]["COLUMN_DEF"];
                    nrow["deText"] = dt.Rows[m]["REMARKS"];
                    dtkey.Rows.Add(nrow);
                }
                return LTP.CodeHelper.CodeCommon.GetColumnInfos(dtkey);
            }
            else
            {
                return null;
            }           
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
		public DataTable GetKeyName(string DbName,string TableName)
		{
            DataTable dtkey = CreateColumnTable();
            //DataTable dt = GetColumnInfoList(DbName, TableName);

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

            #region 
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select * from ");
            //strSql.Append("( ");

            //strSql.Append("SELECT ");
            //strSql.Append("colorder=C.column_id,");
            //strSql.Append("ColumnName=C.name,");
            //strSql.Append("TypeName=T.name, ");
            //strSql.Append("Length=C.max_length, ");
            //strSql.Append("Preci=C.precision, ");
            //strSql.Append("Scale=C.scale, ");
            //strSql.Append("IsIdentity=CASE WHEN C.is_identity=1 THEN N'√'ELSE N'' END,");
            //strSql.Append("isPK=ISNULL(IDX.PrimaryKey,N''),");

            //strSql.Append("Computed=CASE WHEN C.is_computed=1 THEN N'√'ELSE N'' END, ");
            //strSql.Append("IndexName=ISNULL(IDX.IndexName,N''), ");
            //strSql.Append("IndexSort=ISNULL(IDX.Sort,N''), ");
            //strSql.Append("Create_Date=O.Create_Date, ");
            //strSql.Append("Modify_Date=O.Modify_date, ");

            //strSql.Append("cisNull=CASE WHEN C.is_nullable=1 THEN N'√'ELSE N'' END, ");
            //strSql.Append("defaultVal=ISNULL(D.definition,N''), ");
            //strSql.Append("deText=ISNULL(PFD.[value],N'') ");

            //strSql.Append("FROM sys.columns C ");
            //strSql.Append("INNER JOIN sys.objects O ");
            //strSql.Append("ON C.[object_id]=O.[object_id] ");
            //strSql.Append("AND O.type='U' ");
            //strSql.Append("AND O.is_ms_shipped=0 ");
            //strSql.Append("INNER JOIN sys.types T ");
            //strSql.Append("ON C.user_type_id=T.user_type_id ");
            //strSql.Append("LEFT JOIN sys.default_constraints D ");
            //strSql.Append("ON C.[object_id]=D.parent_object_id ");
            //strSql.Append("AND C.column_id=D.parent_column_id ");
            //strSql.Append("AND C.default_object_id=D.[object_id] ");
            //strSql.Append("LEFT JOIN sys.extended_properties PFD ");
            //strSql.Append("ON PFD.class=1  ");
            //strSql.Append("AND C.[object_id]=PFD.major_id  ");
            //strSql.Append("AND C.column_id=PFD.minor_id ");
            ////strSql.Append("--AND PFD.name='Caption'  -- 字段说明对应的描述名称(一个字段可以添加多个不同name的描述) ");
            //strSql.Append("LEFT JOIN sys.extended_properties PTB ");
            //strSql.Append("ON PTB.class=1 ");
            //strSql.Append("AND PTB.minor_id=0  ");
            //strSql.Append("AND C.[object_id]=PTB.major_id ");
            ////strSql.Append("-- AND PFD.name='Caption'  -- 表说明对应的描述名称(一个表可以添加多个不同name的描述)   ");
            //strSql.Append("LEFT JOIN ");// -- 索引及主键信息
            //strSql.Append("( ");
            //strSql.Append("SELECT  ");
            //strSql.Append("IDXC.[object_id], ");
            //strSql.Append("IDXC.column_id, ");
            //strSql.Append("Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending') ");
            //strSql.Append("WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END, ");
            //strSql.Append("PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'√'ELSE N'' END, ");
            //strSql.Append("IndexName=IDX.Name ");
            //strSql.Append("FROM sys.indexes IDX ");
            //strSql.Append("INNER JOIN sys.index_columns IDXC ");
            //strSql.Append("ON IDX.[object_id]=IDXC.[object_id] ");
            //strSql.Append("AND IDX.index_id=IDXC.index_id ");
            //strSql.Append("LEFT JOIN sys.key_constraints KC ");
            //strSql.Append("ON IDX.[object_id]=KC.[parent_object_id] ");
            //strSql.Append("AND IDX.index_id=KC.unique_index_id ");
            //strSql.Append("INNER JOIN  ");// 对于一个列包含多个索引的情况,只显示第1个索引信息
            //strSql.Append("( ");
            //strSql.Append("SELECT [object_id], Column_id, index_id=MIN(index_id) ");
            //strSql.Append("FROM sys.index_columns ");
            //strSql.Append("GROUP BY [object_id], Column_id ");
            //strSql.Append(") IDXCUQ ");
            //strSql.Append("ON IDXC.[object_id]=IDXCUQ.[object_id] ");
            //strSql.Append("AND IDXC.Column_id=IDXCUQ.Column_id ");
            //strSql.Append("AND IDXC.index_id=IDXCUQ.index_id ");
            //strSql.Append(") IDX ");
            //strSql.Append("ON C.[object_id]=IDX.[object_id] ");
            //strSql.Append("AND C.column_id=IDX.column_id  ");
            //strSql.Append("WHERE O.name=N'" + TableName + "' ");
            
            //strSql.Append(") Keyname ");
            //strSql.Append(" where isPK='√' or (IsIdentity='√' or colorder=1)");
            //return Query(DbName, strSql.ToString()).Tables[0];
            #endregion

        }
		#endregion

		#region 得到数据表里的数据 GetTabData(string DbName,string TableName)

		/// <summary>
		/// 得到数据表里的数据
		/// </summary>
		/// <param name="DbName"></param>
		/// <param name="TableName"></param>
		/// <returns></returns>
		public DataTable GetTabData(string DbName,string TableName)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select * from [" + TableName + "]");
			return Query(DbName,strSql.ToString()).Tables[0];
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
		public bool RenameTable(string DbName,string OldName,string NewName)
		{
			try
			{				
				SqlCommand dbCommand =OpenDB(DbName);				
				dbCommand.CommandText="EXEC sp_rename '"+OldName+"', '"+NewName+"'";
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
		public bool DeleteTable(string DbName,string TableName)
		{
			try
			{				
				SqlCommand dbCommand =OpenDB(DbName);				
				dbCommand.CommandText="DROP TABLE ["+TableName+"]";
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
				string strSql="execute master..sp_msgetversion ";//select @@version
				return Query("master",strSql).Tables[0].Rows[0][0].ToString();
			}
			catch//(System.Exception ex)
			{
				//string str=ex.Message;	
				return "";
			}	
		}

		#endregion


       
    }
}
