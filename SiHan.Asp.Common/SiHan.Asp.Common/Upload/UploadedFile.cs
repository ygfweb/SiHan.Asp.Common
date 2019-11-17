using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Upload
{
    /// <summary>
    /// 已上传文件
    /// </summary>
    public class UploadedFile
    {
        /// <summary>
        /// 已上传文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件尺寸
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 原始文件名
        /// </summary>
        public string OldName { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string ExtName { get; set; }

        /// <summary>
        /// 含路径的完整文件名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 是否是图片
        /// </summary>
        public bool IsImage
        {
            get
            {
                if (ExtName == ".jpg" || ExtName == ".png" || ExtName == ".gif" || ExtName == ".jpeg")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
