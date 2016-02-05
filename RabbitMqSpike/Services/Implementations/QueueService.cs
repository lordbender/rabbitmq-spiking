using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using RabbitMqSpike.Models;
using RabbitMqSpike.Services.Contracts;

using RabbitMQ.Client;

namespace RabbitMqSpike.Services.Implementations
{
    public class QueueService : IQueueService
    {
        private readonly ConnectionFactory _connectionFactory;

        public QueueService(string hostName, string username = "guest", string password = "guest", int port = 5672)
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = username,
                Password = password,
                Port = port
            };
        }

        public void CreateQueue(string routingKey = "default", bool durable = true, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> paramiters = null)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
                channel.QueueDeclare(routingKey, durable, exclusive, autoDelete, paramiters);
        }

        public void EnqueueObject<TMessage>(MessageWrapper<TMessage> message, string routingKey = "objectQueue") where TMessage : class
        {
            //Make sure the queue Exists before writing to it.
            CreateQueue(routingKey);

            //Enqueue the message as a string.
            EnqueueString(JsonConvert.SerializeObject(message), routingKey);
        }

        public MessageWrapper<TMessage> DequeueObject<TMessage>(string routingKey = "objectQueue") where TMessage : class
        {
            return JsonConvert.DeserializeObject<MessageWrapper<TMessage>>(DequeueString(routingKey));
        }

        public void EnqueueInt(long message, string routingKey = "intQueue")
        {
            //Make sure the queue Exists before writing to it.
            CreateQueue(routingKey);
            EnqueueString(Convert.ToString(message), routingKey);
        }

        public int DequeueInt(string routingKey = "intQueue")
        {
            return int.Parse(DequeueString(routingKey));
        }

        public void EnqueueString(string message, string routingKey = "stringQueue")
        {
            //Make sure the queue Exists before writing to it.
            CreateQueue(routingKey);

            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", routingKey, null, body);
            }
        }

        public string DequeueString(string routingKey = "stringQueue")
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // channel.QueueDeclare(routingKey, false, false, false, null);
                var result = channel.BasicGet(routingKey, true);
                return Encoding.UTF8.GetString(result.Body);
            }
        }

        public void Dispose()
        {
            //Do cool Unmanaged Disposible stuffs here as needed...
        }
    }
}