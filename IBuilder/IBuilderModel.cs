using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using LTP.IDBO;
using LTP.CodeHelper;
namespace LTP.IBuilder
{
    /// <summary>
    /// Model代码构造器接口
    /// </summary>
    public interface IBuilderModel
    {
        #region 公有属性   
        /// <summary>
        /// model类名
        /// </summary>
        string ModelName
        {
            set;
            get;
        }
        /// <summary>
        /// 顶级命名空间名 
        /// </summary>
        string NameSpace
        {
            set;
            get;
        }
        /// <summary>
        /// 实体类的命名空间
        /// </summary>
        string Modelpath
        {
            set;
            get;
        }
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        List<ColumnInfo> Fieldlist
        {
            set;
            get;
        }

        /// <summary>
        /// 表名(表名,表說明)
        /// </summary>
        SimplePair TableName
        {
            get;
            set;
        }

        #endregion
        
        #region 生成完整单个Model类
        /// <summary>
        /// 生成完整单个Model类
        /// </summary>		
        string CreatModel();       
        #endregion

        #region 生成Model属性部分
        /// <summary>
        /// 生成实体类的属性
        /// </summary>
        /// <returns></returns>
        string CreatModelMethod();      
        #endregion
    }

    public class SimplePair
    {
        public SimplePair()
        { }
        public SimplePair(string name, string caption)
        {
            Name = name;
            Caption = caption;
        }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 說明
        /// </summary>
        public string Caption { get; set; }
        public override string ToString()
        {
            return string.Format("{0}:{1}", Name, Caption).Replace ("\n","") ;
        }
    }
}
