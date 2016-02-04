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
            var sendService = new QueueService();
           var sent = sendService.Send(new MessageWrapper<SomeMessage>
            {
                Title = "Test Client Message",
                Message = new SomeMessage
                {
                    SomeProp = "Testing", 
                    SomeOtherProp = DateTime.Now
                }
            });



            var receiveService = new QueueService();

            var message = receiveService.Receive<SomeMessage>();

            Console.ReadLine();
        }
    }


}