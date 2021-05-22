# FileLogger
### 扩展ILogger，将日志写入文件中，写入全部日志等级。

### 配置说明
#### 以下配置可以根据需要留空或删除

```json
"FileLogging": {
    //默认日志文件名称，空白为年月日：20210101.log
    "DefaultFileName": "",
    //默认日志目录，空白为：Log
    "DefaultPath": "Log",
    //三种子目录建立规则Day(20210101)，Week(202101W)，Month(202101M)，空白为：Day。
    "SubPath": "",
    //日志文件最大值，单位MB，限制1-50MB，删除该项默认为：5MB。
    "MaxSize": 5
}
 ```
 
 ### 注入及使用
 #### 依赖注入
 
 ```cs
 //引用
 using FileLogger.Extensions;
 
 //ConfigureServices注入
  services.AddLogging(builder =>
  {
    builder.AddFileLogger(Configuration);
  });
 ```
 
 ### 使用
 
 ```cs

 private readonly ILogger<HomeController> _logger;
       
       //控制器
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //只保存日志内容
            _logger.FileLogInformation("home information log test!");
            //保存日志及消息
            _logger.FileLogInformation("home information log test!",new Exception("exception message"));
            //带日志记录助手
              _logger.FileLogInformation("home information log test!", new Exception("exception message"),
                helper =>
                {
                    //自定义子目录，会覆盖配置中的子目录选项
                    helper.CustomSubPath = "subPath";
                    //自定义日志文件名称，会覆盖配置中的默认文件名选项
                    helper.CustomFileName = "filename.log";
                    //日志最后一行加入日志记录日期及时间
                    helper.IsLogTime = false;
                });
            return View();
        }
 ```
 
 
 
