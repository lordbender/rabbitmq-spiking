using System;
using System.Threading;

using Newtonsoft.Json;

using RabbitMqSpike.Contracts;
using RabbitMqSpike.Listener;
using RabbitMqSpike.Service;

namespace RabbitMqSpike
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Console.ForegroundColor = ConsoleColor.Cyan;

            ////Your friendly neighborhood queue wrapper!
            using (IQueueService service = new QueueService("localhost"))
            {
                //Send a few message Object to the Queue.
                for (var i = 0; i < 10; i++)
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
                //Get the next available message from the Queue.
                for (var i = 0; i < 10; i++)
                {
                    Thread.Sleep(new TimeSpan(0, 0, 0, 0, 500));
                    var message = service.DequeueObject<SomeMessage>("objectQueue");

                    //Prove the message was read...
                    Console.WriteLine("\tComplex Object Message Title: {0}", message.Title);
                    Console.WriteLine("\tProcessing Complex Object: {0}\n\n",
                        JsonConvert.SerializeObject(message.Message));
                }

                /*Simple string example*/
                service.EnqueueString("Awesome Queue Man!!!", "stringQueue");
                var stringMessage = service.DequeueString("stringQueue");
                Console.WriteLine("\tSimple string message retrieved: {0}\n\n", stringMessage);

                /*Simple int/long example (writing an id for a work type for example)*/
                service.EnqueueInt(1234322);
                var intMessage = service.DequeueInt();
                Console.WriteLine("\tSimple int/long message retrieved: {0}\n\n", intMessage);

                //Send a few message Object to the Queue to test the AutoStart Emulator.
                for (var i = 0; i < 10; i++)
                    service.EnqueueObject(new MessageWrapper<SomeMessage>
                    {
                        Title = "Test IIS AutoStart Client Message",
                        Message = new SomeMessage
                        {
                            SomeProp = "Testing AutoStart",
                            SomeOtherProp = DateTime.Now
                        }
                    }, "AutoStartServiceQueue");
            }

            //Emulate AutoStart IIS Service:
            using (var autoStart = new ObjectQueueObserver<SomeMessage>(2 /*Tick Every 2 Seconds*/, "localhost", "AutoStartServiceQueue"))
            {
                autoStart.Start();

                //See ya later.
                Console.WriteLine("Hit enter to exit...");
                Console.ReadLine();
            }
        }
    }
}