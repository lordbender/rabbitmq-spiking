using System;

namespace RabbitMqSpike
{
    [Serializable]
    public class MessageWrapper<TMessage>
    {
        public string Title { get; set; }

        public TMessage Message { get; set; }
    }
}