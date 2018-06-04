using System;

namespace Nameless.BeetleTracker.Text {

    /// <summary>
    /// Exception for expression property not found.
    /// </summary>
    public class ExpressionPropertyNotFoundException : Exception {

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ExpressionPropertyNotFoundException"/>.
        /// </summary>
        public ExpressionPropertyNotFoundException()
            : base(Properties.Resources.ExpressionPropertyNotFound) { }

        /// <summary>
        /// Initializes a new instance of <see cref="ExpressionPropertyNotFoundException"/>.
        /// </summary>
        /// <param name="expression">The expression.</param>
		public ExpressionPropertyNotFoundException(string expression)
            : base(string.Format(Properties.Resources.ExpressionPropertyNotFoundMF, expression)) { }

        /// <summary>
        /// Initializes a new instance of <see cref="ExpressionPropertyNotFoundException"/>.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="inner">The inner exception, if exists.</param>
		public ExpressionPropertyNotFoundException(string expression, Exception inner)
            : base(string.Format(Properties.Resources.ExpressionPropertyNotFoundMF, expression), inner) { }

        #endregion Public Constructors
    }
}