using System;
using System.Linq.Expressions;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Extension methods for <see cref="Expression"/>.
    /// </summary>
    public static class ExpressionExtension {

        #region Public Static Methods

        /// <summary>
        /// Retrieves the member info.
        /// </summary>
        /// <param name="source">The method.</param>
        /// <returns>
        /// An instance of <see cref="MemberExpression"/> representing the <paramref name="source"/> expression.
        /// </returns>
        /// <remarks>Used to get property information</remarks>
        public static MemberExpression GetMemberInfo(this Expression source) {
            if (source == null) { return null; }

            var lambda = source as LambdaExpression;
            if (lambda == null) {
                throw new ArgumentException(Properties.Resources.NotALambdaExpression, nameof(source));
            }

            MemberExpression member = null;
            switch (lambda.Body.NodeType) {
                case ExpressionType.Convert:
                    member = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
                    break;

                case ExpressionType.MemberAccess:
                    member = lambda.Body as MemberExpression;
                    break;
            }

            if (member == null) {
                throw new InvalidOperationException(Properties.Resources.NotAMemberExpression);
            }

            return member;
        }

        /// <summary>
        /// Retrieves member name by a function expression.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="source">The source function.</param>
        /// <returns>The name of the member.</returns>
        public static string GetMemberName<T>(this Expression<Func<T, object>> source) {
            if (source == null) { return null; }

            return GetExpressionMemberName(source.Body);
        }

        /// <summary>
        /// Retrieves member name by a expression.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="source">The source expression.</param>
        /// <returns>The name of the member.</returns>
        public static string GetMemberName<T>(this Expression<Action<T>> source) {
            if (source == null) { return null; }

            return GetExpressionMemberName(source.Body);
        }

        #endregion Public Static Methods

        #region Private Static Methods

        private static string GetExpressionMemberName(Expression expression) {
            if (expression is MemberExpression) {
                return ((MemberExpression)expression).Member.Name;
            }

            if (expression is MethodCallExpression) {
                return ((MethodCallExpression)expression).Method.Name;
            }

            if (expression is UnaryExpression) {
                return GetUnaryExpressionMemberName(((UnaryExpression)expression));
            }

            throw new ArgumentException(Properties.Resources.InvalidExpression, nameof(expression));
        }

        private static string GetUnaryExpressionMemberName(UnaryExpression expression) => expression.Operand is MethodCallExpression
            ? ((MethodCallExpression)expression.Operand).Method.Name
            : ((MemberExpression)expression.Operand).Member.Name;

        #endregion Private Static Methods
    }
}