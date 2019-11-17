using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SiHan.Libs.Utils.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SiHan.Asp.Common.Upload
{
    /// <summary>
    /// 默认上传服务
    /// </summary>
    public class DefaultUploadService : IUploadService
    {

        protected IHostingEnvironment HostingEnvironment { get; set; }

        public DefaultUploadService(IHostingEnvironment environment)
        {            
            this.HostingEnvironment = environment;
        }

        /// <summary>
        /// 获取上传目录的名称
        /// </summary>
        public virtual string GetDirectoryName()
        {
            return "upload";
        }

        /// <summary>
        /// 获取上传目录的磁盘路径
        /// </summary>
        public virtual string GetDirectoryPath()
        {
            string rootPath = this.HostingEnvironment.WebRootPath;
            string uploadPath = Path.Combine(rootPath, this.GetDirectoryName());
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            return uploadPath;
        }


        /// <summary>
        /// 处理上传文件(将上传文件拷贝到上传目录)
        /// </summary>
        public virtual async Task<UploadedFile> UploadAsync(IFormFile formFile)
        {
            if (formFile == null)
            {
                throw new Exception("上传文件为空");
            }
            string oldName = formFile.FileName; //原始文件名
            string extName = Path.GetExtension(oldName); // 获取扩展名
            string newName = this.GetNewFileName(formFile) + extName; //构建新的文件名
            string fullName = Path.Combine(this.GetDirectoryPath(), newName); // 构建新文件的磁盘路径
            using (var stream = new FileStream(fullName, FileMode.Create)) // 开启新文件的流
            {
                await formFile.CopyToAsync(stream); //将上传文件拷贝到新的文件流
            }
            UploadedFile uploadedFile = new UploadedFile //构建上传文件的信息
            {
                ExtName = extName,
                FileName = newName,
                OldName = oldName,
                Size = formFile.Length,
                FullName = fullName
            };
            return uploadedFile;
        }

        /// <summary>
        /// 生成新的上传文件名称（不含扩展名）(默认是含字母和数字的8位随机名称)
        /// </summary>
        protected virtual string GetNewFileName(IFormFile formFile)
        {
            return RandomHelper.GetLetters(8, "abcdefghijklmnopqrstuvwxyz0123456789");
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        public virtual void Delete(string fileName)
        {
            string fullName = Path.Combine(this.GetDirectoryPath(), fileName);
            FileInfo file = new FileInfo(fullName);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        /// <summary>
        /// 获取附件的URL路径
        /// </summary>
        public virtual string GetUrl(string fileName)
        {
            return "/" + this.GetDirectoryName() + "/" + fileName;
        }
    }
}
