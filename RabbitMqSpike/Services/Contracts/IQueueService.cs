using System;
using System.Collections.Generic;

using RabbitMqSpike.Models;

namespace RabbitMqSpike.Services.Contracts
{
    public interface IQueueService : IDisposable
    {
        void CreateQueue(string routingKey = "default", bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> paramiters = null);
        MessageWrapper<TMessage> Receive<TMessage>(string routingKey = "default") where TMessage : class;
        void Send<TMessage>(MessageWrapper<TMessage> message, string routingKey = "default") where TMessage : class;
    }
}