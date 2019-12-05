using System.Collections.Generic;
using Lucene.Net.Documents;
using TicketCore.Model;

namespace TicketSearch.Lucene.SearchTransformers
{
    internal class TicketTransformer : TransformerBase, ISearchTransformer<Ticket>
    {
        private static readonly List<string> searchFields;

        static TicketTransformer()
        {
            var ticket = new Ticket();
            searchFields = new List<string>
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

        public override List<string> SearchFields => searchFields;

        public Document Transform(Ticket ticket)
        {
            var searchDoc = new Document
            {
                new StringField(nameof(ticket._id), NormalizeForIndex(ticket._id), Field.Store.YES),
                new StringField(InMemoryLuceneSearch.DOC_TYPE_FIELD, NormalizeForIndex(typeof(Ticket).Name), Field.Store.YES),

                new StringField(nameof(ticket.assignee_id), NormalizeForIndex(ticket.assignee_id), Field.Store.NO),
                new StringField(nameof(ticket.created_at), DateTools.DateToString(ticket.created_at.DateTime, DateTools.Resolution.MILLISECOND), Field.Store.NO),
                new TextField(nameof(ticket.description), NormalizeForIndex(ticket.description), Field.Store.NO),
                new StringField(nameof(ticket.due_at), DateTools.DateToString(ticket.due_at.DateTime, DateTools.Resolution.MILLISECOND), Field.Store.NO),
                new StringField(nameof(ticket.external_id), NormalizeForIndex(ticket.external_id), Field.Store.NO),
                new StringField(nameof(ticket.has_incidents), NormalizeForIndex(ticket.has_incidents), Field.Store.NO),
                new StringField(nameof(ticket.priority), NormalizeForIndex(ticket.priority), Field.Store.NO),
                new StringField(nameof(ticket.organization_id), NormalizeForIndex(ticket.organization_id), Field.Store.NO),
                new StringField(nameof(ticket.status), NormalizeForIndex(ticket.status), Field.Store.NO),
                new TextField(nameof(ticket.subject), NormalizeForIndex(ticket.subject), Field.Store.NO),
                new StringField(nameof(ticket.submitter_id), NormalizeForIndex(ticket.submitter_id), Field.Store.NO),
                new StringField(nameof(ticket.type), NormalizeForIndex(ticket.type), Field.Store.NO),
                new TextField(nameof(ticket.url), NormalizeForIndex(ticket.url), Field.Store.NO),
                new StringField(nameof(ticket.via), NormalizeForIndex(ticket.via), Field.Store.NO),

                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(ticket.description), Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(ticket.subject), Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(ticket.external_id), Field.Store.NO),
            };

            AddTagFields(searchDoc, ticket.tags);

            return searchDoc;
        }
    }
}
