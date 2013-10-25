using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
namespace Codematic
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public static class LogInfo
    {
        static string logfilename = Application.StartupPath + "\\logInfo.txt";
        static string cmcfgfile = Application.StartupPath + @"\cmcfg.ini";
        

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="loginfo"></param>
        public static void WriteLog(string loginfo)
        {
            if (File.Exists(cmcfgfile))
            {
                LTP.Utility.INIFile cfgfile = new LTP.Utility.INIFile(cmcfgfile);
                string val = cfgfile.IniReadValue("loginfo", "save");
                if (val.Trim() == "1")
                {
                    StreamWriter sw = new StreamWriter(logfilename, true);
                    sw.WriteLine(DateTime.Now.ToString() + ":" + loginfo);
                    sw.Close();
                }
            }            
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="loginfo"></param>
        public static void WriteLog(System.Exception ex)
        {
            if (File.Exists(cmcfgfile))
            {
                LTP.Utility.INIFile cfgfile = new LTP.Utility.INIFile(cmcfgfile);
                string val = cfgfile.IniReadValue("loginfo", "save");
                if (val.Trim() == "1")
                {                    
                    StreamWriter sw = new StreamWriter(logfilename, true);
                    sw.WriteLine(DateTime.Now.ToString() + ":" );
                    sw.WriteLine("错误信息：" + ex.Message);
                    sw.WriteLine("Stack Trace:" + ex.StackTrace);
                    sw.WriteLine("");
                    sw.Close();
                }
            }            
        }
        
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="loginfo"></param>
        //public static void WriteLog(System.Reflection.MethodBase mb, System.Exception ex)
        //{
        //    if (File.Exists(cmcfgfile))
        //    {
        //        LTP.Utility.INIFile cfgfile = new LTP.Utility.INIFile(cmcfgfile);
        //        string val = cfgfile.IniReadValue("loginfo", "save");
        //        if (val.Trim() == "1")
        //        {                    
        //            string loginfo = mb.ReflectedType.FullName+mb.Name+"错误信息：\n\n" + ex.Message + "\n\n" + "Stack Trace:\n" + ex.StackTrace;
        //            StreamWriter sw = new StreamWriter(logfilename, true);
        //            sw.WriteLine(DateTime.Now.ToString() + ":" + loginfo);
        //            sw.Close();
        //        }
        //    }
        //}

    }
}
