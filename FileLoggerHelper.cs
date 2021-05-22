using System;
using System.Collections.Generic;
using System.Text;

namespace FileLogger
{
    public class FileLoggerHelper
    {
        /// <summary>
        /// 日志最后一行加入日志记录日期及时间
        /// </summary>
        public bool IsLogTime { get; set; } = true;
        /// <summary>
        /// 自定义子目录，会覆盖配置中的子目录选项
        /// </summary>
        public string CustomSubPath { get; set; }
        /// <summary>
        /// 自定义日志文件名称，会覆盖配置中的默认文件名选项
        /// </summary>
        public string CustomFileName { get; set; }
    }
}