using Lucene.Net.Documents;
using System.Collections.Generic;

namespace TicketSearch.Lucene.SearchTransformers
{
    internal interface ISearchTransformer<T>
    {
        List<string> SearchFields { get; }
        Document Transform(T model);
    }
}
