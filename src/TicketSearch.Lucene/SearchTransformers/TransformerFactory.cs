using TicketCore.Exceptions;
using TicketCore.Model;

namespace TicketSearch.Lucene.SearchTransformers
{
    internal static class TransformerFactory
    {
        public static ISearchTransformer<T> LoadTransformer<T>()
        {
            switch (typeof(T).Name)
            {
                case nameof(Organization):
                    return new OrganizationTransformer() as ISearchTransformer<T>;
                case nameof(Ticket):
                    return new TicketTransformer() as ISearchTransformer<T>;
                case nameof(User):
                    return new UserTransformer() as ISearchTransformer<T>;
                default:
                    throw new TicketAppException($"There is no search transformer available for document type {typeof(T).Name}");
            }
        }
    }
}
