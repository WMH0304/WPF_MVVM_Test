using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web_netCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();//标注块内存，并运行起来
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args) //创建默认的builder 完成各种配置

            //指定一个web 服务器， kestrel
            .ConfigureLogging((context, loggingBuilder) =>
            {
                //日志包
                //在日志中过滤掉 命名空间 ,系统日志
                loggingBuilder.AddFilter("System", LogLevel.Warning);
                loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);
                loggingBuilder.AddLog4Net();
            })

                   
                .ConfigureWebHostDefaults(webBuilder => //指定web服务器
                {



                    webBuilder.UseStartup<Startup>();
                });
    }
}
