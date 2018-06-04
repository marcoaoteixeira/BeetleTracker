using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Extension methods for <see cref="IEnumerable"/> and <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtension {

        #region Public Static Methods

        /// <summary>
        /// Interact through an instance of <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The enumerable argument type.</typeparam>
        /// <param name="source">An instance of <see cref="IEnumerable{T}"/>.</param>
        /// <param name="action">The iterator action.</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="source"/> or <paramref name="action"/> were <c>null</c>.
        /// </exception>
        public static void Each<T>(this IEnumerable<T> source, Action<T> action) {
            Prevent.ParameterNull(action, nameof(action));

            Each(source, (current, _) => action(current));
        }

        /// <summary>
        /// Interact through an instance of <see cref="IEnumerable{T}"/>.
        /// And pass an index value to the iterator action.
        /// </summary>
        /// <typeparam name="T">The enumerable argument type.</typeparam>
        /// <param name="source">An instance of <see cref="IEnumerable{T}"/>.</param>
        /// <param name="action">The iterator action.</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="source"/> or <paramref name="action"/> were <c>null</c>.
        /// </exception>
        public static void Each<T>(this IEnumerable<T> source, Action<T, int> action) {
            Prevent.ParameterNull(action, nameof(action));

            if (source == null) { return; }

            var counter = 0;
            using (var enumerator = source.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    action(enumerator.Current, counter++);
                }
            }
        }

        /// <summary>
        /// Interact through an instance of <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="source">An instance of <see cref="IEnumerable"/>.</param>
        /// <param name="action">The iterator action.</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="source"/> or <paramref name="action"/> were <c>null</c>.
        /// </exception>
        public static void Each(this IEnumerable source, Action<object> action) {
            Prevent.ParameterNull(action, nameof(action));

            Each(source, (current, _) => action(current));
        }

        /// <summary>
        /// Interact through an instance of <see cref="IEnumerable"/>.
        /// And pass an index value to the iterator action.
        /// </summary>
        /// <param name="source">An instance of <see cref="IEnumerable"/>.</param>
        /// <param name="action">The iterator action.</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="source"/> or <paramref name="action"/> were <c>null</c>.
        /// </exception>
        public static void Each(this IEnumerable source, Action<object, int> action) {
            Prevent.ParameterNull(action, nameof(action));

            if (source == null) { return; }

            var counter = 0;
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext()) {
                action(enumerator.Current, counter++);
            }

            var disposable = enumerator as IDisposable;
            if (disposable != null) { disposable.Dispose(); }
        }

        /// <summary>
        /// Checks if an <see cref="IEnumerable"/> is empty.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable"/> instance.</param>
        /// <returns><c>true</c>, if is empty, otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="source"/> is <c>null</c>.</exception>
        public static bool IsNullOrEmpty(this IEnumerable source) {
            if (source == null) { return true; }

            // Costs O(1)
            var collection = source as ICollection;
            if (collection != null) { return collection.Count == 0; }

            // Costs O(N)
            var enumerator = source.GetEnumerator();
            var canMoveNext = enumerator.MoveNext();
            var disposable = enumerator as IDisposable;
            if (disposable != null) { disposable.Dispose(); }

            return !canMoveNext;
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> instance into a <see cref="IReadOnlyCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable.</typeparam>
        /// <param name="source">The source <see cref="IEnumerable{T}"/>.</param>
        /// <returns>An <see cref="IReadOnlyCollection{T}"/> instance.</returns>
        public static IList<T> ToReadOnlyCollection<T>(this IEnumerable<T> source) => new ReadOnlyCollection<T>((source ?? Enumerable.Empty<T>()).ToList());

        /// <summary>
        /// Selects distinct the source <see cref="IEnumerable{T}"/> by an expression.
        /// </summary>
        /// <typeparam name="TSource">Type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <param name="source">The source <see cref="IEnumerable{T}"/>.</param>
        /// <param name="keySelector">The key selector function.</param>
        /// <returns>A filtered collection.</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            var seenKeys = new HashSet<TKey>();

            foreach (var element in source) {
                if (seenKeys.Add(keySelector(element))) {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Orders an enumerable by its argument type field, specified as <see cref="string"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="fieldName">The field name in the enumerable type.</param>
        /// <returns>The ordered result.</returns>
        public static IEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string fieldName) => ExecuteOrderBy(source, fieldName, ascending: true);

        /// <summary>
        /// Orders, descending, an enumerable by its argument type field, specified as <see cref="string"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="fieldName">The field name in the enumerable type.</param>
        /// <returns>The ordered result.</returns>
        public static IEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, string fieldName) => ExecuteOrderBy(source, fieldName, ascending: false);

        #endregion Public Static Methods

        #region Private Static Methods

        private static IEnumerable<TSource> ExecuteOrderBy<TSource>(IEnumerable<TSource> source, string fieldName, bool ascending = true) {
            var query = source.AsQueryable();

            var type = typeof(TSource);
            var property = type.GetTypeInfo().GetProperty(fieldName);
            var parameter = Expression.Parameter(type, "_");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), ascending ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending), new Type[] { type, property.PropertyType }, query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<TSource>(resultExpression);
        }

        #endregion Private Static Methods
    }
}