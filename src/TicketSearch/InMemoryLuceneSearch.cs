using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketCore.Interfaces;
using TicketCore.Model;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Search;
using System.Linq;
using Lucene.Net.QueryParsers.Classic;
using TicketSearch.LuceneModel;
using TicketCore.Exceptions;

namespace TicketSearch
{
    public class InMemoryLuceneSearch : ITicketSearch
    {
        private const LuceneVersion LUCENE_VERSION = LuceneVersion.LUCENE_48;
        public const string GLOBAL_SEARCH_FIELD = "global";
        public const string DOC_TYPE_FIELD = "_doc_type";
        public const string EMPTY_VALUE = "IS_EMPTY";

        private readonly IndexWriter indexWriter;
        private readonly StandardAnalyzer analyzer;

        public InMemoryLuceneSearch()
        {
            var indexDirectory = new RAMDirectory();
            analyzer = new StandardAnalyzer(LUCENE_VERSION);
            var indexConfig = new IndexWriterConfig(LUCENE_VERSION, analyzer);
            indexWriter = new IndexWriter(indexDirectory, indexConfig);
        }

        public Task AddDocuments<T, TKey>(IEnumerable<T> docs) where T : ModelBase<TKey>
        {
            switch (typeof(T).Name)
            {
                case nameof(Organization):
                    indexWriter.AddDocuments(docs.Select(d => OrganizationDoc.GetSearchDoc(d as Organization)));
                    break;
                case nameof(User):
                    indexWriter.AddDocuments(docs.Select(d => UserDoc.GetSearchDoc(d as User)));
                    break;
                case nameof(Ticket):
                    indexWriter.AddDocuments(docs.Select(d => TicketDoc.GetSearchDoc(d as Ticket)));
                    break;
                default:
                    //TODO: handle foreign docs
                    break;
            }

            indexWriter.Flush(false, false);
            return Task.CompletedTask;
        }

        public Task<GlobalSearchResult> Search(SearchInput searchInput)
        {
            var indexReader = indexWriter.GetReader(false); //don't care about deletes for this demo
            var searcher = new IndexSearcher(indexReader); 

            var queryParser = new QueryParser(LUCENE_VERSION, GLOBAL_SEARCH_FIELD, analyzer);
            var query = queryParser.Parse(searchInput.search);

            var searchHits = searcher.Search(query, 100);

            var results = new GlobalSearchResult();

            foreach (var hit in searchHits.ScoreDocs)
            {
                var doc = searcher.Doc(hit.Doc);
                var docId = doc.GetField("_id").GetStringValue();
                var docType = doc.GetField(DOC_TYPE_FIELD).GetStringValue();
                
                switch (docType)
                {
                    case nameof(Organization):
                        results.organizations.Add(new Organization { _id = Convert.ToInt32(docId) });
                        break;
                    case nameof(Ticket):
                        results.tickets.Add(new Ticket { _id = Guid.Parse(docId) });
                        break;
                    case nameof(User):
                        results.users.Add(new User { _id = Convert.ToInt32(docId) });
                        break;
                }
            }

            return Task.FromResult(results);
        }

        public Task<List<string>> GetSearchFields<T>()
        {
            switch (typeof(T).Name)
            {
                case nameof(Organization):
                    return Task.FromResult(OrganizationDoc.SearchFields);
                case nameof(Ticket):
                    return Task.FromResult(TicketDoc.SearchFields);
                case nameof(User):
                    return Task.FromResult(UserDoc.SearchFields);
                default:
                    throw new TicketAppException($"Search is not available for {typeof(T).Name}");
            }
        }
    }
}
