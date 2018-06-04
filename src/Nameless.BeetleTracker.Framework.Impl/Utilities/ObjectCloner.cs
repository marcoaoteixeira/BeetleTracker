using Newtonsoft.Json;

namespace Nameless.BeetleTracker.Utility {

    /// <summary>
    /// Reference article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx.
    /// Provides a method for performing a deep copy of an object.
    /// </summary>
    public static class ObjectCloner {

        #region Public Static Methods

        /// <summary>
        /// Perform a deep Copy of the object, using JSON as a serialization method. NOTE: Private members are not cloned using this method.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="instance">The object to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T instance) {
            // Don't serialize a null object, simply return the default for that object.
            if (ReferenceEquals(instance, null)) { return default(T); }

            // Initialize inner objects individually, for example in default constructor
            // some list property initialized with some values, but in 'instance' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var settings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            var value = JsonConvert.SerializeObject(instance);

            return JsonConvert.DeserializeObject<T>(value, settings);
        }

        #endregion Public Static Methods
    }
}