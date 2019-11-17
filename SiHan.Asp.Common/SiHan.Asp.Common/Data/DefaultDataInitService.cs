using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Data
{
    /// <summary>
    /// 默认数据初始化服务
    /// </summary>
    public class DefaultDataInitService : IDataInitService
    {
        /// <summary>
        /// 默认不播种
        /// </summary>
        public void Seed()
        {
            // 默认不播种，不进行任何操作
        }
    }
}
