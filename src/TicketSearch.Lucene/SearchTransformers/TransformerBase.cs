
using Lucene.Net.Documents;
using System.Collections.Generic;
using System.Linq;

namespace TicketSearch.Lucene.SearchTransformers
{
    internal abstract class TransformerBase
    {
        private const string TAGS_FIELD = "tags";

        public abstract List<string> SearchFields { get; }

        protected string NormalizeForIndex<T>(T value)
        {
            return value?.ToString().ToLowerInvariant() ?? InMemoryLuceneSearch.EMPTY_VALUE;
        }

        protected void AddTagFields(Document searchDoc, List<string> tags)
        {
            if ((tags ?? new List<string>()).Any())
            {
                foreach (var tag in tags)
                {
                    searchDoc.Add(new TextField(TAGS_FIELD, NormalizeForIndex(tag), Field.Store.NO));
                    searchDoc.Add(new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(tag), Field.Store.NO));
                }
            }
            else
            {
                searchDoc.Add(new TextField(TAGS_FIELD, InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO));
            }
        }
    }
}
