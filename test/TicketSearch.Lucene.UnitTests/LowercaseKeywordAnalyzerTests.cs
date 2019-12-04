using System.Collections.Generic;
using System.Linq;
using LuceneNet = Lucene.Net;
using Lucene.Net.Analysis.TokenAttributes;
using Xunit;

namespace TicketSearch.Lucene.UnitTests
{

    public class LowercaseKeywordAnalyzerTests
    {
        [Fact]
        public void analyzer_uses_correct_tokenizers()
        {
            var analyzer = new LowercaseKeywordAnalyzer(LuceneNet.Util.LuceneVersion.LUCENE_48);

            var stream = analyzer.GetTokenStream("default", "TEST MY KEYWORD");
            stream.Reset();

            var tokens = new List<string>();
            while (stream.IncrementToken())
            {
                var attribute = stream.GetAttribute<ICharTermAttribute>();
                tokens.Add(attribute.ToString());
            }
            Assert.Single(tokens);
            Assert.Equal("test my keyword", tokens.Single());
        }
    }
}
