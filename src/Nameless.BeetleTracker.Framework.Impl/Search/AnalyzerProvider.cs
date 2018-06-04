using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace Nameless.BeetleTracker.Search {

    /// <summary>
    /// Default implementation of <see cref="IAnalyzerProvider"/>.
    /// </summary>
    public sealed class AnalyzerProvider : IAnalyzerProvider {

        #region Private Read-Only Fields

        private IAnalyzerSelector[] _selectors;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="AnalyzerProvider"/>.
        /// </summary>
        /// <param name="selectors">A collection of <see cref="IAnalyzerSelector"/>.</param>
        public AnalyzerProvider(params IAnalyzerSelector[] selectors) {
            Prevent.ParameterNull(selectors, nameof(selectors));

            _selectors = selectors;
        }

        #endregion Public Constructors

        #region ILuceneAnalyzerProvider

        /// <inheritdoc />
        public Analyzer GetAnalyzer(string indexName) {
            var analyzer = _selectors
                .Select(_ => _.GetLuceneAnalyzer(indexName))
                .Where(_ => _ != null)
                .OrderByDescending(_ => _.Priority)
                .Select(_ => _.Analyzer)
                .FirstOrDefault();

            return analyzer == null
                ? new StandardAnalyzer(IndexProvider.Version)
                : analyzer;
        }

        #endregion ILuceneAnalyzerProvider
    }
}