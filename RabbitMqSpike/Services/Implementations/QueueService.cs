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

        public QueueService(string hostName = "localhost", string username = "guest", string password = "guest")
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = username,
                Password = password
            };
        }

        public void CreateQueue(string routingKey = "default", bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> paramiters = null)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
                channel.QueueDeclare(routingKey, durable, exclusive, autoDelete, paramiters);
        }

        public void EnqueueObject<TMessage>(MessageWrapper<TMessage> message, string routingKey = "default") where TMessage : class
        {
            //Make sure the queue Exists before writing to it.
            CreateQueue(routingKey);

            //Enqueue the message as a string.
            EnqueueString(JsonConvert.SerializeObject(message), routingKey);
        }

        public MessageWrapper<TMessage> DequeueObject<TMessage>(string routingKey = "default") where TMessage : class
        {
            return JsonConvert.DeserializeObject<MessageWrapper<TMessage>>(DequeueString(routingKey));
        }

        public void EnqueueInt(int message, string routingKey = "default")
        {
            //Make sure the queue Exists before writing to it.
            CreateQueue(routingKey);

            throw new NotImplementedException();
        }

        public int DequeueInt(string routingKey = "default")
        {
            throw new NotImplementedException();
        }

        public void EnqueueString(string message, string routingKey = "default")
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

        public string DequeueString(string routingKey = "default")
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