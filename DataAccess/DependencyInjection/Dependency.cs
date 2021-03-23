using AzureQueueClient.Interfaces;
using AzureQueueClient.Services;
using Microsoft.Extensions.DependencyInjection;
using Smtp.Interfaces;
using Smtp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureSmtpQueueBackgroundService.DependencyInjection
{
    public class Dependency
    {
        public static IServiceProvider serviceProvider { get; set; }

        public IServiceProvider Dependencies()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<IQueueClient,SQueueClient>()
                .AddSingleton<ISmtp,SSmtp>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
