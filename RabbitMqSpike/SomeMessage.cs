using System;

namespace RabbitMqSpike
{
    [Serializable]
    public class SomeMessage
    {
        public string SomeProp { get; set; }

        public DateTime SomeOtherProp { get; set; }
    }
}