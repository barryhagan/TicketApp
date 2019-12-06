using System.Collections.Generic;
using Lucene.Net.Documents;
using TicketCore.Model;

namespace TicketSearch.Lucene.SearchTransformers
{
    internal class OrganizationTransformer : TransformerBase,  ISearchTransformer<Organization>
    {
        private static readonly List<string> searchFields;

        static OrganizationTransformer()
        {
            var org = new Organization();
            searchFields = new List<string>
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

        public override List<string> SearchFields => searchFields;

        public Document Transform(Organization org)
        {
            var searchDoc = new Document
            {
                new StringField(nameof(org._id), NormalizeForIndex(org._id), Field.Store.YES),
                new StringField(InMemoryLuceneSearch.DOC_TYPE_FIELD, NormalizeForIndex(typeof(Organization).Name), Field.Store.YES),

                new StringField(nameof(org.created_at), DateTools.DateToString(org.created_at.DateTime, DateTools.Resolution.SECOND), Field.Store.NO),
                new TextField(nameof(org.details), NormalizeForIndex(org.details), Field.Store.NO),
                new StringField(nameof(org.external_id), NormalizeForIndex(org.external_id), Field.Store.NO),
                new TextField(nameof(org.name), NormalizeForIndex(org.name), Field.Store.NO),
                new StringField(nameof(org.shared_tickets), NormalizeForIndex(org.shared_tickets), Field.Store.NO),
                new TextField(nameof(org.url), NormalizeForIndex(org.url), Field.Store.NO),

                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(org.details), Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(org.name), Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(org.external_id), Field.Store.NO)
            };

            AddTagFields(searchDoc, org.tags);

            foreach (var domain in org.domain_names ?? new List<string>())
            {
                searchDoc.Add(new StringField(nameof(org.domain_names), NormalizeForIndex(domain), Field.Store.NO));
                searchDoc.Add(new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(domain), Field.Store.NO));
            }


            return searchDoc;
        }
    }
}
