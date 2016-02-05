using System;
using System.Collections.Generic;

using RabbitMqSpike.Models;

namespace RabbitMqSpike.Services.Contracts
{
    public interface IQueueService : IDisposable
    {
        /// <summary>
        /// Ensures that the Queue Exists 
        /// </summary>
        /// <param name="routingKey">Queue Name</param>
        /// <param name="durable">Durability (exchanges survive broker restart)</param>
        /// <param name="exclusive"></param>
        /// <param name="autoDelete">Auto-delete (exchange is deleted when all queues have finished using it)</param>
        /// <param name="paramiters">Arguments (these are broker-dependent)</param>
        void CreateQueue(string routingKey, bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> paramiters = null);

        void EnqueueObject<TMessage>(MessageWrapper<TMessage> message, string routingKey = "objectQueue") where TMessage : class;

        MessageWrapper<TMessage> DequeueObject<TMessage>(string routingKey = "objectQueue") where TMessage : class;

        void EnqueueInt(long message, string routingKey = "intQueue");

        int DequeueInt(string routingKey = "intQueue");

        void EnqueueString(string message, string routingKey = "stringQueue");

        string DequeueString(string routingKey = "stringQueue");
    }
}