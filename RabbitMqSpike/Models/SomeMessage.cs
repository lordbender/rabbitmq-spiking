using System;

namespace RabbitMqSpike.Models
{
    [Serializable]
    public class SomeMessage
    {
        public string SomeProp { get; set; }

        public DateTime SomeOtherProp { get; set; }
    }
}