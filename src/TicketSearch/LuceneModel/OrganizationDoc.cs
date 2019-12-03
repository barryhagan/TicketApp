using Lucene.Net.Documents;
using System.Collections.Generic;
using System.Linq;
using TicketCore.Model;

namespace TicketSearch.LuceneModel
{
    internal static class OrganizationDoc
    {
        static OrganizationDoc()
        {
            var org = new Organization();
            SearchFields = new List<string>
                {
                    nameof(org._id),
                    nameof(org.url),
                    nameof(org.external_id),
                    nameof(org.name),
                    nameof(org.domain_names),
                    nameof(org.created_at),
                    nameof(org.details),
                    nameof(org.shared_tickets),
                    nameof(org.tags)
                };
        }

        public static List<string> SearchFields { get; private set; }

        public static Document GetSearchDoc(Organization org)
        {
            var searchDoc = new Document
            {
                new StringField(nameof(org._id), org._id.ToString(), Field.Store.YES),
                new StringField(InMemoryLuceneSearch.DOC_TYPE_FIELD, typeof(Organization).Name, Field.Store.YES),

                new TextField(nameof(org.url), org.url ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new TextField(nameof(org.external_id), org.external_id?.ToString("N") ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new TextField(nameof(org.name), org.name ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new TextField(nameof(org.details), org.details ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(org.shared_tickets), org.shared_tickets.ToString().ToLowerInvariant(), Field.Store.NO),
                new StringField(nameof(org.created_at), DateTools.DateToString(org.created_at.DateTime, DateTools.Resolution.MILLISECOND), Field.Store.NO),

                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, org.details ?? "", Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, org.name ?? "", Field.Store.NO),
            };

            if ((org.tags ?? new List<string>()).Any())
            {
                foreach (var tag in org.tags)
                {
                    searchDoc.Add(new TextField(nameof(org.tags), tag, Field.Store.NO));
                    searchDoc.Add(new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, tag, Field.Store.NO));
                }
            }
            else
            {
                searchDoc.Add(new StringField(nameof(org.tags), InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO));
            }

            foreach (var domain in org.domain_names ?? new List<string>())
            {
                searchDoc.Add(new TextField(nameof(org.domain_names), domain, Field.Store.NO));
                searchDoc.Add(new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, domain, Field.Store.NO));
            }


            return searchDoc;
        }

    }
}
