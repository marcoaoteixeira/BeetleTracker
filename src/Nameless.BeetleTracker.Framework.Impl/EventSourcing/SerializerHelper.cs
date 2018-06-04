using System;
using System.Text;
using Newtonsoft.Json;

namespace Nameless.BeetleTracker.EventSourcing {

    internal static class SerializerHelper {

        #region Internal Static Methods

        internal static byte[] Serialize(object obj) {
            var json = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(json);
        }

        internal static T Deserialize<T>(byte[] payload) {
            var json = Encoding.UTF8.GetString(payload);

            return JsonConvert.DeserializeObject<T>(json);
        }

        internal static object Deserialize(byte[] payload, string typeFullName) {
            var json = Encoding.UTF8.GetString(payload);
            var type = Type.GetType(typeFullName);

            return JsonConvert.DeserializeObject(json, type);
        }

        #endregion Internal Static Methods
    }
}