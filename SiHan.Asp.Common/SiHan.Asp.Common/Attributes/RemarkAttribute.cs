using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Attributes
{
    /// <summary>
    /// 备注说明特性（用于代码生成器）
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RemarkAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描叙（用于生成文本框水印文字）
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 排序，默认为0
        /// </summary>
        public int Order { get; set; } = 0;
        /// <summary>
        /// 是否必填（默认为false）
        /// </summary>
        public bool IsRequired { get; set; } = false;
    }
}



