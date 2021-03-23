using AzureQueueClient.Interfaces;
using AzureSmtpQueueBackgroundService.DependencyInjection;
using Core.Base64ToString;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Smtp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AzureSmtpQueueBackgroundService
{
    public class AddMessageWorker : BackgroundService
    {
        public static Dependency dependency;
        public IQueueClient queueClient { get; set; }
        public ISmtp smtpClient { get; set; }
        public IServiceProvider serviceProvider { get; set; }

        public AddMessageWorker()
        {
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

                queueClient.CreateQueue(queueName);

                for (int repeatCount = 0; repeatCount < 10; repeatCount++)
                {
                    var encodedMessage =  Encode.Base64Encode($"Hello {repeatCount}");

                    queueClient.InsertMessage(queueName, encodedMessage);
                }

                Console.WriteLine("Messages Added");

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
