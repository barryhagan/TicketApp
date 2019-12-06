using System.Collections.Generic;
using Lucene.Net.Documents;
using TicketCore.Model;

namespace TicketSearch.Lucene.SearchTransformers
{
    internal class UserTransformer : TransformerBase, ISearchTransformer<User>
    {
        private static readonly List<string> searchFields;

        static UserTransformer()
        {
            var user = new User();
            searchFields = new List<string>
                {
                    nameof(user._id),
                    nameof(user.url),
                    nameof(user.external_id),
                    nameof(user.name),
                    nameof(user.alias),
                    nameof(user.created_at),
                    nameof(user.active),
                    nameof(user.verified),
                    nameof(user.shared),
                    nameof(user.locale),
                    nameof(user.timezone),
                    nameof(user.last_login_at),
                    nameof(user.email),
                    nameof(user.phone),
                    nameof(user.signature),
                    nameof(user.organization_id),
                    nameof(user.tags),
                    nameof(user.suspended),
                    nameof(user.role),
                };
        }

        public override List<string> SearchFields => searchFields;

        public Document Transform(User user)
        {
            var searchDoc = new Document
            {
                new StringField(nameof(user._id), NormalizeForIndex(user._id), Field.Store.YES),
                new StringField(InMemoryLuceneSearch.DOC_TYPE_FIELD, NormalizeForIndex(typeof(User).Name), Field.Store.YES),

                new StringField(nameof(user.active), NormalizeForIndex(user.active), Field.Store.NO),
                new TextField(nameof(user.alias), NormalizeForIndex(user.alias), Field.Store.NO),
                new StringField(nameof(user.created_at), DateTools.DateToString(user.created_at.DateTime, DateTools.Resolution.SECOND), Field.Store.NO),
                new StringField(nameof(user.email), NormalizeForIndex(user.email), Field.Store.NO),
                new StringField(nameof(user.external_id), NormalizeForIndex(user.external_id), Field.Store.NO),
                new StringField(nameof(user.last_login_at), DateTools.DateToString(user.last_login_at.DateTime, DateTools.Resolution.SECOND), Field.Store.NO),
                new StringField(nameof(user.locale), NormalizeForIndex(user.locale), Field.Store.NO),
                new TextField(nameof(user.name), NormalizeForIndex(user.name), Field.Store.NO),
                new StringField(nameof(user.organization_id), NormalizeForIndex(user.organization_id) , Field.Store.NO),
                new StringField(nameof(user.phone), NormalizeForIndex(user.phone), Field.Store.NO),
                new StringField(nameof(user.role), NormalizeForIndex(user.role), Field.Store.NO),
                new StringField(nameof(user.shared), NormalizeForIndex(user.shared), Field.Store.NO),
                new TextField(nameof(user.signature), NormalizeForIndex(user.signature), Field.Store.NO),
                new StringField(nameof(user.suspended), NormalizeForIndex(user.suspended), Field.Store.NO),
                new StringField(nameof(user.timezone), NormalizeForIndex(user.timezone), Field.Store.NO),
                new TextField(nameof(user.url), NormalizeForIndex(user.url), Field.Store.NO),
                new StringField(nameof(user.verified), NormalizeForIndex(user.verified), Field.Store.NO),

                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(user.alias), Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(user.email), Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(user.name), Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(user.signature), Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, NormalizeForIndex(user.external_id), Field.Store.NO)
            };

            AddTagFields(searchDoc, user.tags);

            return searchDoc;
        }
    }
}
