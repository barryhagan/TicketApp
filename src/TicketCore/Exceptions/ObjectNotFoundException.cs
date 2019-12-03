using System;

namespace TicketCore.Exceptions
{
    public class ObjectNotFoundException<T,TKey> : TicketAppException
    {
        public TKey Id { get; private set; }

        public ObjectNotFoundException(TKey id, string message) : this(id, message, null)
        {

        }

        public ObjectNotFoundException(TKey id, string message, Exception innerException) : base(message, innerException)
        {
            Id = id;
        }
    }
}
