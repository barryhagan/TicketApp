using System;

namespace TicketCore.Exceptions
{
    public class ObjectNotFoundException<T, TKey> : TicketAppException
    {
        public TKey Id { get; private set; }

        public ObjectNotFoundException() { }

        public ObjectNotFoundException(string message) : base(message) { }

        public ObjectNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public ObjectNotFoundException(TKey id, string message) : this(id, message, null) { }

        public ObjectNotFoundException(TKey id, string message, Exception innerException) : base(message, innerException)
        {
            Id = id;
        }
    }
}
