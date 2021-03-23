using AzureQueueClient.Interfaces;
using AzureSmtpQueueBackgroundService.DependencyInjection;
using Core.Base64ToString;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smtp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureSmtpQueueBackgroundService
{
    public class SendSmsWorker : BackgroundService
    {
        public Encode encode;
        public static Dependency dependency;
        public IQueueClient queueClient { get; set; }
        public ISmtp smtpClient { get; set; }
        public IServiceProvider serviceProvider { get; set; }

        public SendSmsWorker()
        {
            encode = new Encode();
            dependency = new Dependency();

            serviceProvider = dependency.Dependencies();

            queueClient = serviceProvider.GetRequiredService<IQueueClient>();
            smtpClient = serviceProvider.GetRequiredService<ISmtp>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var queueName = Console.ReadLine();
                var emailTo = Console.ReadLine();

                queueClient.DequeueMultipleMessages(queueName,emailTo);

                var queueLenght = queueClient.GetQueueLength(queueName);

                if(queueLenght == 0)
                {
                    queueClient.DeleteQueue(queueName);
                }

                Console.WriteLine("Messages Deleted");

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
