using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtension {

        #region Private Static Read-Only Fields

        private static readonly Type[] WriteTypes = new[] {
            typeof(string),
            typeof(DateTime),
            typeof(Enum),
            typeof(decimal),
            typeof(Guid)
        };

        #endregion Private Static Read-Only Fields

        #region Public Static Methods

        /// <summary>
        /// Gets the default value for a given type, when instantiated.
        /// </summary>
        /// <param name="source">The type</param>
        /// <returns>The default value of the type, when instantiated.</returns>
        public static object GetDefault(this Type source) {
            if (source == null) { return null; }

            return source.IsValueType
                ? Activator.CreateInstance(source)
                : null;
        }

        /// <summary>
        /// Determines whether the <paramref name="genericType"/> is assignable from
        /// <paramref name="source"/> taking into account generic definitions
        /// </summary>
        /// <param name="source">The given type.</param>
        /// <param name="genericType">The generic type.</param>
        /// <returns><c>true</c> if <paramref name="genericType"/> is assignable from <paramref name="source"/>, otherwise, <c>false</c>.</returns>
        public static bool IsAssignableFromGenericType(this Type source, Type genericType) {
            if (source == null || genericType == null) { return false; }

            return source == genericType
              || MapsToGenericTypeDefinition(source, genericType)
              || HasInterfaceThatMapsToGenericTypeDefinition(source, genericType)
              || IsAssignableFromGenericType(source.GetTypeInfo().BaseType, genericType);
        }

        /// <summary>
        /// Verifies if the <see cref="Type"/> is an instance of <see cref="Nullable"/>.
        /// </summary>
        /// <param name="source">The source type.</param>
        /// <returns><c>true</c>, if is instance of <see cref="Nullable"/>, otherwise, <c>false</c>.</returns>
        public static bool IsNullable(this Type source) => source != null
            ? source.GetTypeInfo().IsGenericType && source.GetGenericTypeDefinition() == typeof(Nullable<>)
            : false;

        /// <summary>
        /// Can convert to <see cref="Nullable"/> type.
        /// </summary>
        /// <param name="source">The source type.</param>
        /// <returns><c>true</c>, if can convert, otherwise, <c>false</c>.</returns>
        public static bool AllowNull(this Type source) => source != null && (!source.GetTypeInfo().IsValueType || IsNullable(source));

        /// <summary>
		/// Retrieves the generic method associated to the source type.
		/// </summary>
		/// <param name="source">The source type.</param>
		/// <param name="name">The name of the method.</param>
		/// <param name="genericArgumentTypes">Method generic argument types, if any.</param>
		/// <param name="argumentTypes">Method argument types, if any.</param>
		/// <param name="returnType">Method return type.</param>
		/// <returns>Returns an instance of <see cref="MethodInfo"/> representing the generic method.</returns>
		public static MethodInfo GetGenericMethod(this Type source, string name, Type[] genericArgumentTypes, Type[] argumentTypes, Type returnType) {
            if (source == null) { return null; }

            Prevent.ParameterNullOrWhiteSpace(name, nameof(name));
            Prevent.ParameterNull(genericArgumentTypes, nameof(genericArgumentTypes));
            Prevent.ParameterNull(argumentTypes, nameof(argumentTypes));
            Prevent.ParameterNull(returnType, nameof(returnType));

            return source.GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                .Where(method =>
                    method.Name == name &&
                    method.GetGenericArguments().Length == genericArgumentTypes.Length &&
                    method.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(argumentTypes) &&
                    (method.ReturnType.GetTypeInfo().IsGenericType && !method.ReturnType.GetTypeInfo().IsGenericTypeDefinition ? returnType.GetGenericTypeDefinition() : method.ReturnType) == returnType)
                .Single()
                .MakeGenericMethod(genericArgumentTypes);
        }

        /// <summary>
        /// Returns a value that indicates whether the current type can be assigned to the
        /// specified type.
        /// </summary>
        /// <param name="source">The current type.</param>
        /// <param name="type">The specified type.</param>
        /// <returns>
        /// <c>true</c> if the current type can be assigned to the specified type;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableTo(this Type source, Type type) {
            if (source == null) { return false; }

            Prevent.ParameterNull(type, nameof(type));

            return type.GetTypeInfo().IsAssignableFrom(source.GetTypeInfo());
        }

        /// <summary>
        /// Returns a value that indicates whether the current type can be assigned to the
        /// specified type.
        /// </summary>
        /// <typeparam name="T">The specified type.</typeparam>
        /// <param name="source">The current type.</param>
        /// <returns>
        /// <c>true</c> if the current type can be assigned to the specified type;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableTo<T>(this Type source) => IsAssignableTo(source, typeof(T));

        /// <summary>
		/// Verifies if the given type is an anonymous type.
		/// </summary>
		/// <param name="source">The source type.</param>
		/// <returns><c>true</c> if anonymous type, otherwise, <c>false</c>.</returns>
		public static bool IsAnonymous(this Type source) {
            if (source == null) { return false; }

            var typeInfo = source.GetTypeInfo();

            return typeInfo.GetCustomAttribute<CompilerGeneratedAttribute>(inherit: false) != null &&
                typeInfo.IsGenericType &&
                typeInfo.Name.Contains("AnonymousType") &&
                (typeInfo.Name.StartsWith("<>") || typeInfo.Name.StartsWith("VB$"));
        }

        /// <summary>
        /// Verifies if the <paramref name="source"/> is a simple type.
        /// </summary>
        /// <param name="source">The source type.</param>
        /// <returns><c>true</c> if is simple type; otherwise, <c>false</c>.</returns>
        public static bool IsSimple(this Type source) => (source != null && (source.GetTypeInfo().IsPrimitive || WriteTypes.Contains(source)));

        #endregion Public Static Methods

        #region Private Static Methods

        private static bool MapsToGenericTypeDefinition(Type source, Type genericType) => genericType.GetTypeInfo().IsGenericTypeDefinition
            && source.GetTypeInfo().IsGenericType
            && source.GetGenericTypeDefinition() == genericType;

        private static bool HasInterfaceThatMapsToGenericTypeDefinition(Type source, Type genericType) => source.GetTypeInfo()
            .GetInterfaces()
            .Where(@interface => @interface.GetTypeInfo().IsGenericType)
            .Any(@interface => @interface.GetGenericTypeDefinition() == genericType);

        #endregion Private Static Methods
    }
}