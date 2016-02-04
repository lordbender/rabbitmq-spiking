using System;
using System.Text;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqSpike
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);

                    var message = JsonConvert.SerializeObject(new MessageWrapper<SomeMessage>
                    {
                        Title = "Hello World",
                        Message = new SomeMessage
                        {
                            SomeProp = "Cool Data",
                            SomeOtherProp = DateTime.Now
                        }
                    });

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", "hello", null, body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }

                Console.WriteLine("Data Written to Queue. Press [enter] to Continue.");
                Console.ReadLine();
            }

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("hello", false, false, false, null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    /*Grab the message back into an object*/
                    var messageObject = JsonConvert.DeserializeObject<MessageWrapper<SomeMessage>>(message);

                    Console.WriteLine(" [x] Received {0}", messageObject.Message.SomeProp);
                };

                channel.BasicConsume("hello", true, consumer);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }

    [Serializable]
    public class MessageWrapper<TMessage>
    {
        public string Title { get; set; }

        public TMessage Message { get; set; }
    }

    [Serializable]
    public class SomeMessage
    {
        public string SomeProp { get; set; }

        public DateTime SomeOtherProp { get; set; }
    }
}