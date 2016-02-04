using System;
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

        public bool Send<TMessage>(MessageWrapper<TMessage> message, string routingKey = "default") where TMessage : class
        {
           
            using (var connection = _connectionFactory.CreateConnection())
            
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: routingKey, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var serializedMessage = JsonConvert.SerializeObject(message);

                var body = Encoding.UTF8.GetBytes(serializedMessage);

                channel.BasicPublish(exchange: "", routingKey: routingKey, basicProperties: null, body: body);

                return true;
            }
            
        }

        public MessageWrapper<TMessage> Receive<TMessage>(string routingKey = "default") where TMessage : class
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                MessageWrapper<TMessage> returnObj = null;

                channel.QueueDeclare(queue: routingKey, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    /*Grab the message back into an object*/
                    var messageObject = JsonConvert.DeserializeObject<MessageWrapper<TMessage>>(message);
                    
                    returnObj = messageObject;

                    Console.WriteLine(" [x] Received {0}", message);
                };

                channel.BasicConsume(queue: routingKey, noAck: true, consumer: consumer);

                return returnObj;
            }

        }
    }
}