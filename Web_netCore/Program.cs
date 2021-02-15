using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web_netCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();//��ע���ڴ棬����������
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args) //����Ĭ�ϵ�builder ��ɸ�������

            //ָ��һ��web �������� kestrel
            .ConfigureLogging((context, loggingBuilder) =>
            {
                //��־��
                //����־�й��˵� �����ռ� ,ϵͳ��־
                loggingBuilder.AddFilter("System", LogLevel.Warning);
                loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);
                loggingBuilder.AddLog4Net();
            })

                   
                .ConfigureWebHostDefaults(webBuilder => //ָ��web������
                {



                    webBuilder.UseStartup<Startup>();
                });
    }
}
