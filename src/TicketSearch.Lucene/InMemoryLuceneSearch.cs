using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.Util;
using TicketCore.Dto;
using TicketCore.Interfaces;
using TicketCore.Model;
using TicketSearch.Lucene.SearchTransformers;

namespace TicketSearch.Lucene
{
    public class InMemoryLuceneSearch : ITicketSearch
    {
        private const LuceneVersion LUCENE_VERSION = LuceneVersion.LUCENE_48;
        public const string GLOBAL_SEARCH_FIELD = "_global";
        public const string DOC_ID_FIELD = "_id";
        public const string DOC_TYPE_FIELD = "_doc_type";
        public const string EMPTY_VALUE = "ISNULL";

        private readonly IndexWriter indexWriter;
        private readonly PerFieldAnalyzerWrapper analyzer;

        public InMemoryLuceneSearch()
        {
            var indexDirectory = new RAMDirectory();
            analyzer = new PerFieldAnalyzerWrapper(new LowercaseKeywordAnalyzer(LUCENE_VERSION), new Dictionary<string, Analyzer>()
            {
                { GLOBAL_SEARCH_FIELD, new StandardAnalyzer(LUCENE_VERSION)},
                { "alias", new StandardAnalyzer(LUCENE_VERSION)},
                { "description", new StandardAnalyzer(LUCENE_VERSION)},
                { "details", new StandardAnalyzer(LUCENE_VERSION)},
                { "email", new StandardAnalyzer(LUCENE_VERSION)},
                { "name", new StandardAnalyzer(LUCENE_VERSION) },
                { "signature", new StandardAnalyzer(LUCENE_VERSION)},
                { "subject", new StandardAnalyzer(LUCENE_VERSION)},
                { "tags", new StandardAnalyzer(LUCENE_VERSION)},
                { "url", new StandardAnalyzer(LUCENE_VERSION)},
            });
            
            var indexConfig = new IndexWriterConfig(LUCENE_VERSION, analyzer);
            indexWriter = new IndexWriter(indexDirectory, indexConfig);
        }

        public Task AddDocumentsAsync<T, TKey>(IEnumerable<T> docs) where T : ModelBase<TKey>
        {
            var transformer = TransformerFactory.LoadTransformer<T>();
            indexWriter.AddDocuments(docs.Select(doc => transformer.Transform(doc)));
            indexWriter.Flush(false, false);
            return Task.CompletedTask;
        }

        public Task<List<SearchHit>> SearchAsync(SearchInput searchInput)
        {
            var searchBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(searchInput.Search))
            {
                searchBuilder.Append(searchInput.Search.Replace("\"\"", EMPTY_VALUE));
            }

            if (!string.IsNullOrWhiteSpace(searchInput.DocType))
            {
                if (searchBuilder.Length > 0)
                {
                    searchBuilder.Append(" AND ");
                }
                searchBuilder.Append($"{DOC_TYPE_FIELD}:{searchInput.DocType}");
            }

            if (searchBuilder.Length == 0)
            {
                return Task.FromResult(new List<SearchHit>());
            }

            var searcher = new IndexSearcher(indexWriter.GetReader(false));
            var queryParser = new QueryParser(LUCENE_VERSION, GLOBAL_SEARCH_FIELD, analyzer);
            var query = queryParser.Parse(searchBuilder.ToString());           
            var searchHits = searcher.Search(query, 200);

            var results = new List<SearchHit>();

            foreach (var hit in searchHits.ScoreDocs)
            {
                var doc = searcher.Doc(hit.Doc);
                var docId = doc.GetField(DOC_ID_FIELD).GetStringValue();
                var docType = doc.GetField(DOC_TYPE_FIELD).GetStringValue();

                results.Add(new SearchHit
                {
                    DocId = docId,
                    DocType = docType,
                    Score = hit.Score
                });
            }

            return Task.FromResult(results);
        }

        public Task<List<string>> GetSearchFieldsAsync<T>()
        {
            var transformer = TransformerFactory.LoadTransformer<T>();
            return Task.FromResult(transformer.SearchFields);
        }
    }
}
