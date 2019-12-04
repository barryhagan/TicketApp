using System;

namespace TicketCore.Exceptions
{
    public class TicketAppException : Exception
    {
        public TicketAppException() { }

        public TicketAppException(string message) : base(message) { }

        public TicketAppException(string message, Exception innerException) : base(message, innerException) { }
    }
}
