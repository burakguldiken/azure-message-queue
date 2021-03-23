using Azure.Storage.Queues;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureQueueClient.Interfaces
{
    public interface IQueueClient
    {
        /// <summary>
        /// Create new queue client
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        QueueClient CreateQueueClient(string queueName);
        /// <summary>
        /// Create new queue
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        bool CreateQueue(string queueName);
        /// <summary>
        /// Add a new message in the queue
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="message"></param>
        void InsertMessage(string queueName, string message);
        /// <summary>
        /// Update an existing message in the queue
        /// </summary>
        /// <param name="queueName"></param>
        void UpdateMessage(string queueName);
        /// <summary>
        /// Process and remove a message from the queue
        /// </summary>
        /// <param name="queueName"></param>
        void DequeueMessage(string queueName, string messageId, string popReceipt);
        /// <summary>
        /// Process and remove multiple messages from the queue
        /// </summary>
        /// <param name="queueName"></param>
        void DequeueMultipleMessages(string queueName,string to);
        /// <summary>
        /// Get the approximate number of messages in the queue
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        int GetQueueLength(string queueName);
        /// <summary>
        /// Delete the queue
        /// </summary>
        /// <param name="queueName"></param>
        void DeleteQueue(string queueName);
    }
}
