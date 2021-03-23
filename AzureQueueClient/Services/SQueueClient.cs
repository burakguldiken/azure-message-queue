using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using AzureQueueClient.Interfaces;
using Core.Base64ToString;
using Core.EnvironmentManager;
using Smtp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureQueueClient.Services
{
    public class SQueueClient : IQueueClient
    {
        Decode _decode; 
        Connection _connection;

        ISmtp _smtp;

        public SQueueClient(ISmtp smtp)
        {
            _connection = Connection.CreateConnectionInstance;
            _smtp = smtp;
        }

        public bool CreateQueue(string queueName)
        {
            try
            {
                QueueClient queueClient = CreateQueueClient(queueName);

                queueClient.CreateIfNotExists();

                if (queueClient.Exists())
                {
                    Console.WriteLine($"Queue created: '{queueClient.Name}'");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Client not created!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public QueueClient CreateQueueClient(string queueName)
        {
            string connectionString = _connection.connString;
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            return queueClient;
        }

        public void DeleteQueue(string queueName)
        {
            QueueClient queueClient = CreateQueueClient(queueName);

            if (queueClient.Exists())
            {
                queueClient.Delete();
            }

            Console.WriteLine($"Queue deleted: '{queueClient.Name}'");
        }

        public void DequeueMessage(string queueName, string messageId, string popReceipt)
        {
            QueueClient queueClient = CreateQueueClient(queueName);

            if (queueClient.Exists())
            {
                QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

                queueClient.DeleteMessage(messageId, popReceipt);

                Console.WriteLine($"Dequeued message Id: '{messageId}");
            }
        }

        public void DequeueMultipleMessages(string queueName,string to)
        {
            QueueClient queueClient = CreateQueueClient(queueName);

            if (queueClient.Exists())
            {
                QueueMessage[] receivedMessages = queueClient.ReceiveMessages(20, TimeSpan.FromMinutes(1));

                foreach (QueueMessage message in receivedMessages)
                {
                    string decodedMessage = Decode.Base64Decode(message.MessageText);

                    Console.WriteLine($"Deleted message: '{decodedMessage}'");

                    queueClient.DeleteMessage(message.MessageId, message.PopReceipt);

                    _smtp.SendEmail(decodedMessage, to);
                }
            }
        }

        public int GetQueueLength(string queueName)
        {
            QueueClient queueClient = CreateQueueClient(queueName);

            int cachedMessagesCount = 0;

            if (queueClient.Exists())
            {
                QueueProperties properties = queueClient.GetProperties();

                cachedMessagesCount = properties.ApproximateMessagesCount;

                Console.WriteLine($"Number of messages in queue: {cachedMessagesCount}");
            }

            return cachedMessagesCount;
        }

        public void InsertMessage(string queueName, string message)
        {
            QueueClient queueClient = CreateQueueClient(queueName);

            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                queueClient.SendMessage(message);
            }

            var decodedMessage = Decode.Base64Decode(message);

            Console.WriteLine($"Inserted: {decodedMessage}");
        }

        public void UpdateMessage(string queueName)
        {

            QueueClient queueClient = CreateQueueClient(queueName);

            if (queueClient.Exists())
            {
                QueueMessage[] message = queueClient.ReceiveMessages();

                queueClient.UpdateMessage(message[0].MessageId,
                        message[0].PopReceipt,
                        "Updated contents",
                        TimeSpan.FromSeconds(10.0)
                    );
            }
        }
    }
}
