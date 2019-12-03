using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using TicketCore.Model;

namespace TicketSearch.Lucene.SearchModel
{
    internal static class UserDoc
    {
        static UserDoc()
        {
            var user = new User();
            SearchFields = new List<string>
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

        public static List<string> SearchFields { get; private set; }

        public static Document GetSearchDoc(User user)
        {
            var searchDoc = new Document
            {
                new StringField(nameof(user._id), user._id.ToString(), Field.Store.YES),
                new StringField(InMemoryLuceneSearch.DOC_TYPE_FIELD, typeof(User).Name.ToLowerInvariant(), Field.Store.YES),

                new TextField(nameof(user.name), user.name ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new TextField(nameof(user.url), user.url ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(user.external_id), user.external_id?.ToString().ToLowerInvariant() ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new TextField(nameof(user.alias), user.alias ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(user.active), user.active.ToString().ToLowerInvariant(), Field.Store.NO),
                new StringField(nameof(user.verified), user.verified.ToString().ToLowerInvariant(), Field.Store.NO),
                new StringField(nameof(user.shared), user.shared.ToString().ToLowerInvariant(), Field.Store.NO),
                new StringField(nameof(user.locale), user.locale?.ToLowerInvariant() ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(user.timezone), user.timezone?.ToLowerInvariant() ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(user.last_login_at), DateTools.DateToString(user.last_login_at.DateTime, DateTools.Resolution.MILLISECOND), Field.Store.NO),
                new StringField(nameof(user.created_at), DateTools.DateToString(user.created_at.DateTime, DateTools.Resolution.MILLISECOND), Field.Store.NO),
                new StringField(nameof(user.phone), user.phone ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new TextField(nameof(user.signature), user.signature ?? InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO),
                new StringField(nameof(user.organization_id), user.organization_id.ToString(), Field.Store.NO),
                new StringField(nameof(user.suspended), user.suspended.ToString().ToLowerInvariant(), Field.Store.NO),
                new StringField(nameof(user.role), user.role?.ToLowerInvariant(), Field.Store.NO),

                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, user.name ?? "", Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, user.alias ?? "", Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, user.signature ?? "", Field.Store.NO),
                new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, user.external_id?.ToString().ToLowerInvariant() ?? "", Field.Store.NO)
            };

            if (!string.IsNullOrEmpty(user.email))
            {
                searchDoc.Add(new TextField(nameof(user.email), user.email, Field.Store.NO));
                searchDoc.Add(new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, user.email, Field.Store.NO));

                var domainSeparator = user.email.LastIndexOf('@');
                if(domainSeparator > 0)
                {
                    var domain = user.email.Substring(domainSeparator + 1);
                    searchDoc.Add(new TextField(nameof(user.email), domain, Field.Store.NO));
                    searchDoc.Add(new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, domain, Field.Store.NO));
                }
            }
            else
            {
                searchDoc.Add(new TextField(nameof(user.email), InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO));

            }


            if ((user.tags ?? new List<string>()).Any())
            {
                foreach (var tag in user.tags)
                {
                    searchDoc.Add(new TextField(nameof(user.tags), tag, Field.Store.NO));
                    searchDoc.Add(new TextField(InMemoryLuceneSearch.GLOBAL_SEARCH_FIELD, tag, Field.Store.NO));
                }
            }
            else
            {
                searchDoc.Add(new TextField(nameof(user.tags), InMemoryLuceneSearch.EMPTY_VALUE, Field.Store.NO));
            }

            return searchDoc;
        }
    }
}
