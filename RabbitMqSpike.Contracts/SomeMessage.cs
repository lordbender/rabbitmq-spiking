using System;

namespace RabbitMqSpike.Contracts
{
    [Serializable]
    public class SomeMessage
    {
        public string SomeProp { get; set; }

        public DateTime SomeOtherProp { get; set; }
    }
}