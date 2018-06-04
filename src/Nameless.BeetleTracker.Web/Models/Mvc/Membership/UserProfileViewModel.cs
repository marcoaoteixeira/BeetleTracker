using System;
using Newtonsoft.Json;

namespace Nameless.BeetleTracker.Models.Mvc.Membership {
    public class UserProfileViewModel {
        #region Public Properties

        [JsonProperty(PropertyName = "id")]
        public Guid ID { get; set; }
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "profilePicture")]
        public string ProfilePicture { get; set; }
        [JsonProperty(PropertyName = "occupation")]
        public string Occupation { get; set; }

        #endregion
    }
}