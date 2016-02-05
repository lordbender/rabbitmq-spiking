using System;
using System.Collections.Generic;

using RabbitMqSpike.Models;

namespace RabbitMqSpike.Services.Contracts
{
    public interface IQueueService : IDisposable
    {
        void CreateQueue(string routingKey = "default", bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> paramiters = null);

        void EnqueueObject<TMessage>(MessageWrapper<TMessage> message, string routingKey = "default") where TMessage : class;

        MessageWrapper<TMessage> DequeueObject<TMessage>(string routingKey = "default") where TMessage : class;

        void EnqueueInt(int message, string routingKey = "default");

        int DequeueInt(string routingKey = "default");

        void EnqueueString(string message, string routingKey = "default");

        string DequeueString(string routingKey = "default");
    }
}