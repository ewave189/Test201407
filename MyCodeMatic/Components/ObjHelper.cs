using System;
using System.Collections.Generic;
using System.Text;

namespace Codematic
{
    /// <summary>
    /// 对象接口创建辅助类
    /// </summary>
    class ObjHelper
    {

        public ObjHelper()
        {
        }

        //创建数据库信息类接口
        public static LTP.IDBO.IDbObject CreatDbObj(string longservername)
        {
            LTP.CmConfig.DbSettings dbset = LTP.CmConfig.DbConfig.GetSetting(longservername);
            LTP.IDBO.IDbObject dbobj = LTP.DBFactory.DBOMaker.CreateDbObj(dbset.DbType);
            dbobj.DbConnectStr = dbset.ConnectStr;            
            return dbobj;
        }
    
        //创建脚本接口
        public static LTP.IDBO.IDbScriptBuilder CreatDsb(string longservername)
        {
            LTP.CmConfig.DbSettings dbset = LTP.CmConfig.DbConfig.GetSetting(longservername);
            LTP.IDBO.IDbScriptBuilder dsb = LTP.DBFactory.DBOMaker.CreateScript(dbset.DbType);
            dsb.DbConnectStr = dbset.ConnectStr;
            return dsb;
        }

        //创建代码生成类接口
        public static LTP.CodeBuild.CodeBuilders CreatCB(string longservername)
        {
            //LTP.CmConfig.DbSettings dbset = LTP.CmConfig.DbConfig.GetSetting(longservername);
            LTP.CodeBuild.CodeBuilders cb = new LTP.CodeBuild.CodeBuilders(CreatDbObj(longservername));// LTP.CodeBuild.CodeBuilders(dbset.DbType);
            //cb.DbConnectStr = dbset.ConnectStr;
            return cb;
        }
    }

     
}
