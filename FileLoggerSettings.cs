using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileLogger
{
    /// <summary>
    /// 子目录建立规则
    /// </summary>
    public enum SubPath
    {
        /// <summary>
        /// 按天分类日志。格式：20210522
        /// </summary>
        Day,

        /// <summary>
        /// 按周分类日志。格式：202105W（2021年第5周）
        /// </summary>
        Week,

        /// <summary>
        /// 按月分类日志。格式：202105M（2021年5月）
        /// </summary>
        Month
    }

    public class FileLoggerSettings
    {
        /// <summary>
        /// 默认保存目录
        /// </summary>
        public string DefaultPath { get; set; }

        /// <summary>
        /// 子目录建立规制
        /// </summary>
        public string SubPath { get; set; }

        /// <summary>
        /// 默认日志文件。如果：SubPath为Empty或DefaultFileName为空白时自动转换为日期格式。否则为该文件名。
        /// </summary>
        public string DefaultFileName { get; set; }

        /// <summary>
        /// 日志文件最大值，如果超过该值会自动添加.1后缀开始的分段日志。未指定时默认为5M。最高50M；
        /// </summary>
        public int MaxSize { get; set; }
    }
}