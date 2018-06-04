using Lucene.Net.Analysis.Standard;

namespace Nameless.BeetleTracker.Search {

    /// <summary>
    /// Default implementation of <see cref="IAnalyzerSelector"/>.
    /// </summary>
    public sealed class AnalyzerSelector : IAnalyzerSelector {

        #region ILuceneAnalyzerSelector Members

        /// <inheritdoc />
        public AnalyzerSelectorResult GetLuceneAnalyzer(string indexName) {
            return new AnalyzerSelectorResult {
                Priority = -5,
                Analyzer = new StandardAnalyzer(IndexProvider.Version)
            };
        }

        #endregion ILuceneAnalyzerSelector Members
    }
}