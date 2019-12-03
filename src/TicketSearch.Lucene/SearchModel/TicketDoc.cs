using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using TicketCore.Model;

namespace TicketSearch.Lucene.SearchModel
{
    internal static class TicketDoc
    {
        static TicketDoc()
        {
            var ticket = new Ticket();
            SearchFields = new List<string>
                {
                    nameof(ticket._id),
                    nameof(ticket.url),
                    nameof(ticket.external_id),
                    nameof(ticket.created_at),
                    nameof(ticket.type),
                    nameof(ticket.subject),
                    nameof(ticket.description),
                    nameof(ticket.priority),
                    nameof(ticket.status),
                    nameof(ticket.submitter_id),
                    nameof(ticket.assignee_id),
                    nameof(ticket.organization_id),
                    nameof(ticket.tags),
                    nameof(ticket.has_incidents),
                    nameof(ticket.due_at),
                    nameof(ticket.via),
                };
        }

        public static List<string> SearchFields { get; private set; }

        public static Document GetSearchDoc(Ticket ticket)
        {
            var searchDoc = new Document
            {
                new StringField(nameof(ticket._id), ticket._id.ToString().ToLowerInvariant(), Field.Store.YES),
                new StringField(InMemoryLuceneSearch.DOC_TYPE_FIELD, typeof(Ticket).Name.ToLowerInvariant(), Field.Store.YES),

                new TextField(nameof(ticket.url), ticket.url ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(ticket.external_id), ticket.external_id?.ToString().ToLowerInvariant() ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(ticket.type), ticket.type ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new TextField(nameof(ticket.subject), ticket.subject ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new TextField(nameof(ticket.description), ticket.description ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(ticket.priority), ticket.priority ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(ticket.status), ticket.status ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(ticket.submitter_id), ticket.submitter_id.ToString(), Field.Store.NO),
                new StringField(nameof(ticket.assignee_id), ticket.assignee_id.ToString(), Field.Store.NO),
                new StringField(nameof(ticket.organization_id), ticket.organization_id.ToString(), Field.Store.NO),
                new StringField(nameof(ticket.due_at), DateTools.DateToString(ticket.due_at.DateTime, DateTools.Resolution.MILLISECOND), Field.Store.NO),
                new StringField(nameof(ticket.created_at), DateTools.DateToString(ticket.created_at.DateTime, DateTools.Resolution.MILLISECOND), Field.Store.NO),
                new StringField(nameof(ticket.via), ticket.via ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(ticket.has_incidents), ticket.has_incidents.ToString().ToLowerInvariant(), Field.Store.NO),

                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, ticket.subject ?? "", Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, ticket.description ?? "", Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, ticket.external_id?.ToString().ToLowerInvariant() ?? "", Field.Store.NO)
            };


            if ((ticket.tags ?? new List<string>()).Any())
            {
                foreach (var tag in ticket.tags)
                {
                    searchDoc.Add(new TextField(nameof(ticket.tags), tag, Field.Store.NO));
                    searchDoc.Add(new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, tag, Field.Store.NO));
                }
            }
            else
            {
                searchDoc.Add(new TextField(nameof(ticket.tags), InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO));
            }

            return searchDoc;
        }
    }
}
