using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Extension methods for <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionExtension {

        #region Public Static Methods

        /// <summary>
        /// Returns <c>true</c> if is a fatal exception.
        /// </summary>
        /// <param name="source">The source <see cref="Exception"/>.</param>
        /// <returns><c>true</c> if is fatal, otherwise, <c>false</c>.</returns>
        public static bool IsFatal(this Exception source) => source is FatalException ||
            source is StackOverflowException ||
            source is OutOfMemoryException ||
            source is AccessViolationException ||
            source is AppDomainUnloadedException ||
            source is ThreadAbortException ||
            source is SecurityException ||
            source is SEHException;

        #endregion Public Static Methods
    }
}