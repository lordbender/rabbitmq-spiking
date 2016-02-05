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

        public void Send<TMessage>(MessageWrapper<TMessage> message, string routingKey = "default") where TMessage : class
        {
            //Make sure the queue Exists before writing to it.
            CreateQueue(routingKey);

            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var serializedMessage = JsonConvert.SerializeObject(message);

                var body = Encoding.UTF8.GetBytes(serializedMessage);
                channel.BasicPublish("", routingKey, null, body);
            }
        }

        public MessageWrapper<TMessage> Receive<TMessage>(string routingKey = "default") where TMessage : class
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // channel.QueueDeclare(routingKey, false, false, false, null);
                var result = channel.BasicGet(routingKey, true);
                var message = Encoding.UTF8.GetString(result.Body);
                return JsonConvert.DeserializeObject<MessageWrapper<TMessage>>(message);
            }
        }

        public void Dispose()
        {
            //Do cool Unmanaged Disposible stuffs here as needed...
        }
    }
}