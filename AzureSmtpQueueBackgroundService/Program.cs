using Core.Enum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureSmtpQueueBackgroundService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    args = new string[2];

                    args[0] = "2";

                    if (Convert.ToInt32(args[0]) == (int)EnumWorker.AddMessageWorker)
                    {
                        services.AddHostedService<AddMessageWorker>();
                    }
                    else if(Convert.ToInt32(args[0]) == (int)EnumWorker.SendSmsWorker)
                    {
                        services.AddHostedService<SendSmsWorker>();
                    }
                });
    }
}
