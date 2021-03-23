using Microsoft.Extensions.Configuration;
using Smtp.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Smtp.Services
{
    public class SSmtp : ISmtp
    {
        public IConfiguration configuration = null;

        public string From { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }

        public SSmtp()
        {
            CreateConfiguration();
            From = configuration.GetValue<string>("Smtp:From");
            Host = configuration.GetValue<string>("Smtp:Host");
            Port = configuration.GetValue<int>("Smtp:Port");
            Password = configuration.GetValue<string>("Smtp:Password");
        }

        public IConfiguration CreateConfiguration()
        {
            if (configuration == null)
            {
                var builder = new ConfigurationBuilder().AddJsonFile($"EnvironmentManager/appsettings.json", true, true);
                configuration = builder.Build();
            }
            return configuration;
        }

        public bool SendEmail(string message, string to)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.Priority = MailPriority.High;
                mail.From = new MailAddress(From);
                mail.To.Add(to);
                mail.Subject = "Azure Send E-mail Operation";
                mail.Body = message;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(Host, Port))
                {
                    try
                    {
                        smtp.Credentials = new NetworkCredential(From, Password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);

                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }
    }
}
