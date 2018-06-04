using System;
using System.Linq.Expressions;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Nameless.BeetleTracker.Mvc {

    /// <summary>
    /// Extension methods for <see cref="HtmlHelper"/>
    /// </summary>
    public static class HtmlHelperExtension {

        #region Public Static Methods

        /// <summary>
        /// Renders the specified partial view by using the specified HTML helper.
        /// If the constraint returns <c>true</c>.
        /// </summary>
        /// <param name="source">The HTML helper.</param>
        /// <param name="constraint">The constraint.</param>
        /// <param name="partialViewName">The partial view name.</param>
        public static MvcHtmlString PartialOn(this HtmlHelper source, Expression<Func<bool>> constraint, string partialViewName) => PartialOn(source, constraint, partialViewName, null, null);

        /// <summary>
        /// Renders the specified partial view, passing it a copy of the current System.Web.Mvc.ViewDataDictionary
        /// object, but with the Model property set to the specified model. If the constraint returns <c>true</c>.
        /// </summary>
        /// <param name="source">The HTML helper.</param>
        /// <param name="constraint">The constraint.</param>
        /// <param name="partialViewName">The partial view name.</param>
        /// <param name="model">The partial view model.</param>
        public static MvcHtmlString PartialOn(this HtmlHelper source, Expression<Func<bool>> constraint, string partialViewName, object model) => PartialOn(source, constraint, partialViewName, model, null);

        /// <summary>
        /// Renders the specified partial view, replacing its ViewData property with the
        /// specified System.Web.Mvc.ViewDataDictionary object. If the constraint returns <c>true</c>.
        /// </summary>
        /// <param name="source">The HTML helper.</param>
        /// <param name="constraint">The constraint.</param>
        /// <param name="partialViewName">The partial view name.</param>
        /// <param name="viewData">The partial view data dictionary.</param>
        public static MvcHtmlString PartialOn(this HtmlHelper source, Expression<Func<bool>> constraint, string partialViewName, ViewDataDictionary viewData) => PartialOn(source, constraint, partialViewName, null, viewData);

        /// <summary>
        /// Renders the specified partial view, replacing the partial view's ViewData property
        /// with the specified System.Web.Mvc.ViewDataDictionary object and setting the Model
        /// property of the view data to the specified model. If the constraint returns <c>true</c>.
        /// </summary>
        /// <param name="source">The HTML helper.</param>
        /// <param name="constraint">The constraint.</param>
        /// <param name="partialViewName">The partial view name.</param>
        /// <param name="model">The partial view model.</param>
        /// <param name="viewData">The partial view dictionary.</param>
        public static MvcHtmlString PartialOn(this HtmlHelper source, Expression<Func<bool>> constraint, string partialViewName, object model, ViewDataDictionary viewData) {
            if (source == null) { return MvcHtmlString.Empty; }

            return constraint.Compile().Invoke()
                ? source.Partial(partialViewName, model, viewData)
                : MvcHtmlString.Empty;
        }

        /// <summary>
        /// Retrieves the <see cref="string"/> representation of the anti-forgery token.
        /// </summary>
        /// <returns>The anti-forgery token.</returns>
        public static MvcHtmlString GetAntiForgeryToken(this HtmlHelper source) {
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return MvcHtmlString.Create(string.Concat(cookieToken, ",", formToken));
        }

        #endregion Public Static Methods
    }
}