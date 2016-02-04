using System.Text;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqSpike
{
    public class QueueService
    {

        private readonly ConnectionFactory _connectionFactory;

        public QueueService(string hostName = "localhost")
        {
            _connectionFactory = new ConnectionFactory { HostName = hostName };
        }

        public bool Send<TMessage>(MessageWrapper<TMessage> message, string queueName = "default") where TMessage : class
        {
           
            using (var connection = _connectionFactory.CreateConnection())
            
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queueName, false, false, false, null);

                var serializedMessage = JsonConvert.SerializeObject(message);

                var body = Encoding.UTF8.GetBytes(serializedMessage);

                channel.BasicPublish("", queueName, null, body);
                return true;
            }
            
        }

        public MessageWrapper<TMessage> Send<TMessage>(string queueName = "default") where TMessage : class
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                MessageWrapper<TMessage> returnObj = null;

                channel.QueueDeclare("hello", false, false, false, null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    /*Grab the message back into an object*/
                    var messageObject = JsonConvert.DeserializeObject<MessageWrapper<TMessage>>(message);
                    
                    returnObj = messageObject;
                };

                channel.BasicConsume("hello", true, consumer);
                
                return returnObj;
            }

        }
    }
}