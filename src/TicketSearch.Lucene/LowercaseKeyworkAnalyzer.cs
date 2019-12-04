using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Util;

namespace TicketSearch.Lucene
{

    /// <summary>
    /// Custom analyzer for case insensitive indexing of keyword fields (no tokenizing)
    /// </summary>
    internal class LowercaseKeywordAnalyzer : Analyzer
    {
        private readonly LuceneVersion version;
        public LowercaseKeywordAnalyzer(LuceneVersion version)
        {
            this.version = version;
        }

        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            var tokenizer = new KeywordTokenizer(reader);
            TokenStream result = new LowerCaseFilter(version, tokenizer);
            return new TokenStreamComponents(tokenizer, result);
        }
    }
}
