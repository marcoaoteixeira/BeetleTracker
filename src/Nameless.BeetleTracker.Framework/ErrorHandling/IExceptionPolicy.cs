using System;

namespace Nameless.BeetleTracker.ErrorHandling {

    /// <summary>
    /// Defines methods to treat exceptions.
    /// </summary>
    public interface IExceptionPolicy {

        #region Methods

        /// <summary>
        /// Treats the exception or rethrow.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>/<c>false</c> if the exception should be rethrown by the caller, otherwise <c>true</c>.</returns>
        bool Handle(object sender, Exception exception);

        #endregion Methods
    }
}