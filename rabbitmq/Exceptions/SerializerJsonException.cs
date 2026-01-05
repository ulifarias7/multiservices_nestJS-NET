using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMq.Exceptions
{
    public class SerializerJsonException : Exception
    {
        public SerializerJsonException(string message) : base (message) { }

        public SerializerJsonException(string message, Exception exception)
           : base(message, exception) { }
    }
}
