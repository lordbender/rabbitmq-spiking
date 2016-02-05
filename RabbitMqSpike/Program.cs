using System;

using Newtonsoft.Json;

using RabbitMqSpike.Models;
using RabbitMqSpike.Services.Implementations;

namespace RabbitMqSpike
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Your friendly neighborhood queue wrapper!
            using (var service = new QueueService())
            {
                //Send a message to the Queue.
                service.Send(new MessageWrapper<SomeMessage>
                {
                    Title = "Test Client Message",
                    Message = new SomeMessage
                    {
                        SomeProp = "Testing",
                        SomeOtherProp = DateTime.Now
                    }
                });

                //Would presumably be in some other application.
                //Get the next available message from the Queue
                var message = service.Receive<SomeMessage>();

                //Prove the message was read...
                Console.WriteLine("Message Title: {0}", message.Title);
                Console.WriteLine("Message Body: {0}\n\n", JsonConvert.SerializeObject(message.Message));
            }

            //See ya later.
            Console.WriteLine("Hit enter to exit...");
            Console.ReadLine();
        }
    }
}