using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SiHan.Asp.Common.Upload
{
    /// <summary>
    /// 上传服务
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        Task<UploadedFile> UploadAsync(IFormFile formFile);

        /// <summary>
        /// 删除附件文件
        /// </summary>
        /// <param name="fileName"></param>
        void Delete(string fileName);

        /// <summary>
        /// 获取附件的URL路径
        /// </summary>
        string GetUrl(string fileName);
    }
}
