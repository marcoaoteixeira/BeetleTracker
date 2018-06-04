using System.Collections.Generic;
using System.Web.Mvc;

namespace Nameless.BeetleTracker.Mvc.Filters {

    /// <summary>
    /// Implementation of <see cref="IFilterProvider"/> to use <see cref="System.Diagnostics.Stopwatch"/>.
    /// </summary>
    public class StopwatchFilterProvider : IFilterProvider {

        #region IFilterProvider Members

        /// <inheritdoc />
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
            => new[] { new Filter(new StopwatchActionFilter(), FilterScope.Global, 0) };

        #endregion IFilterProvider Members
    }
}