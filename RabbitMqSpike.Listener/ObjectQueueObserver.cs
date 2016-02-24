using System;
using System.Timers;

using RabbitMqSpike.Contracts;
using RabbitMqSpike.Service;

namespace RabbitMqSpike.Listener
{
    public class ObjectQueueObserver<TMessageShape> : IDisposable where TMessageShape : class
    {
        private readonly string _hostName;
        private readonly string _routingKey;
        private readonly Timer _timer;

        public ObjectQueueObserver(long intervalInSeconds, string hostName = "localhost", string routingKey = "AutoStartServiceQueue")
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
            Console.WriteLine("ObjectQueueObserver Timer Tick, looking in {0} Queue for work...", _routingKey);

            try
            {
                using (IQueueService service = new QueueService(_hostName))
                {
                    var workItem = service.DequeueObject<TMessageShape>(_routingKey);

                    if (workItem != null)
                    {
                        Console.WriteLine("ObjectQueueObserver Timer Tick, Work Item FOUND in the {0} Queue, Sending to to correct handler", _routingKey);

                        //Do some Work Based on the Message Type....
                        Console.WriteLine("Message Title {0}, Infer Work Type For MessageType {1}", workItem.Title,
                            typeof (TMessageShape));
                    }
                    else
                    {
                        Console.WriteLine("ObjectQueueObserver Timer Tick, No messages in the {0} Queue at this time...", _routingKey);
                    }
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