using System.Collections.Generic;

namespace TicketCore.Interfaces
{
    public interface IDataLoader
    {
        IEnumerable<T> LoadObjects<T>();
    }
}
