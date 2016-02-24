using System;
using System.Timers;

using RabbitMqSpike.Contracts;
using RabbitMqSpike.Service;

namespace RabbitMqSpike.Sender
{
    public class ObjectQueueSender<TMessageShape> : IDisposable where TMessageShape : class
    {
        private readonly string _hostName;
        private readonly string _routingKey;
        private readonly Timer _timer;

        public ObjectQueueSender(long intervalInSeconds, string hostName = "localhost", string routingKey = "AutoStartServiceQueue")
        {
            _hostName = hostName;
            _routingKey = routingKey;
            _timer = new Timer(60*60*intervalInSeconds) {Enabled = false};

            /*Set the timer target!!!*/
            _timer.Elapsed += Tartget;

            Console.WriteLine("ObjectQueueObserver Created Timer Tick at {0}, looking in {1} Queue for work...", 60*60*intervalInSeconds, _hostName);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public void Start()
        {
            _timer.Start();
        }

        private void Tartget(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            Console.WriteLine("ObjectQueueSender Timer Tick, loading items into {0} Queue for work...", _routingKey);

            try
            {
                using (IQueueService service = new QueueService(_hostName))
                {
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
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);

                //Eat it, Log it, but DO NOT THROW IT...
            }

            _timer.Start();
        }
    }
}