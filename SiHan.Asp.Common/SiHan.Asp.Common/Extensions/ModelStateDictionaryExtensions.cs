using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiHan.Asp.Common.Extensions
{
    /// <summary>
    /// ModelStateDictionary扩展类
    /// </summary>
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// 从模型状态中获取错误信息
        /// </summary>
        public static Dictionary<string, string> GetAllErrors(this ModelStateDictionary modelState)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (!modelState.IsValid)
            {
                var errorList = modelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                foreach (var item in errorList)
                {
                    if (item.Value.Length > 0)
                    {
                        errors.Add(item.Key, item.Value[0]);
                    }
                }
            }
            return errors;
        }
    }
}
