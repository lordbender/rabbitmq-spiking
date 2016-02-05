using System;

using Newtonsoft.Json;

using RabbitMqSpike.Models;
using RabbitMqSpike.Services.Contracts;
using RabbitMqSpike.Services.Implementations;

namespace RabbitMqSpike
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            //Your friendly neighborhood queue wrapper!
            using (IQueueService service = new QueueService("localhost"))
            {
                //Send a message Object to the Queue.
                service.EnqueueObject(new MessageWrapper<SomeMessage>
                {
                    Title = "Test Client Message",
                    Message = new SomeMessage
                    {
                        SomeProp = "Testing",
                        SomeOtherProp = DateTime.Now
                    }
                }, "objectQueue");

                //Would presumably be in some other application.
                //Get the next available message from the Queue
                var message = service.DequeueObject<SomeMessage>("objectQueue");

                //Prove the message was read...
                Console.WriteLine("\tComplex Object Message Title: {0}", message.Title);
                Console.WriteLine("\tComplex Object: {0}\n\n", JsonConvert.SerializeObject(message.Message));

                /*Simple string example*/
                service.EnqueueString("Awesome Queue Man!!!", "stringQueue");
                var stringMessage = service.DequeueString("stringQueue");
                Console.WriteLine("\tSimple string message retrieved: {0}\n\n", stringMessage);

                /*Simple int/long example (writing an id for a work type for example)*/
                service.EnqueueInt(1234322);
                var intMessage = service.DequeueInt();
                Console.WriteLine("\tSimple int/long message retrieved: {0}\n\n", intMessage);
            }

            //See ya later.
            Console.WriteLine("Hit enter to exit...");
            Console.ReadLine();
        }
    }
}