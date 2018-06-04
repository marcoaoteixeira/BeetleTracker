using System.Threading.Tasks;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Extension methods for <see cref="Task"/>
    /// </summary>
    public static class TaskExtension {

        #region Public Static Methods

        /// <summary>
        /// Waits the task completation and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The <see cref="Task{TResult}"/> instance.</param>
        /// <returns>The <see cref="Task{TResult}"/> result.</returns>
        public static TResult WaitWithResult<TResult>(this Task<TResult> source) {
            if (source == null) { return default(TResult); }

            source.Wait();

            return source.Result;
        }

        #endregion Public Static Methods
    }
}