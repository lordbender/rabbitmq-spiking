using System.Text;

using Newtonsoft.Json;

using RabbitMQ.Client;

namespace RabbitMqSpike
{
    public class QueueService
    {
        private readonly ConnectionFactory _connectionFactory;

        public QueueService(string hostName = "localhost")
        {
            _connectionFactory = new ConnectionFactory {HostName = hostName};
        }

        public void Send<TMessage>(MessageWrapper<TMessage> message, string routingKey = "default") where TMessage : class
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(routingKey, false, false, false, null);

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
    }
}