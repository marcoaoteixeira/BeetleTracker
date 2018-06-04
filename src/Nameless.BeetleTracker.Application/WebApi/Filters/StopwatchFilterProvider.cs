using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Nameless.BeetleTracker.WebApi.Filters {

    /// <summary>
    /// Implementation of <see cref="IFilterProvider"/> to use <see cref="System.Diagnostics.Stopwatch"/>.
    /// </summary>
    public class StopwatchFilterProvider : IFilterProvider {

        #region IFilterProvider Members

        /// <inheritdoc />
        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
            => new[] { new FilterInfo(new StopwatchActionFilter(), FilterScope.Global) };

        #endregion IFilterProvider Members
    }
}