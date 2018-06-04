using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nameless.BeetleTracker.Resources;
using Newtonsoft.Json;

namespace Nameless.BeetleTracker.Models.Mvc.Membership {
    public class UserViewModel {
        #region Public Properties

        [JsonProperty(PropertyName = "id")]
        [HiddenInput]
        public Guid ID { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        [Display(ResourceType = typeof(Displays), Name = "FullName")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 6)]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "accessFailedCount")]
        [Display(ResourceType = typeof(Displays), Name = "AccessFailedCount")]
        public int AccessFailedCount { get; set; }

        [JsonProperty(PropertyName = "email")]
        [Display(ResourceType = typeof(Displays), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "EmailAddress")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "emailConfirmed")]
        [Display(ResourceType = typeof(Displays), Name = "EmailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [JsonProperty(PropertyName = "lockoutEnabled")]
        [Display(ResourceType = typeof(Displays), Name = "LockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [JsonProperty(PropertyName = "lockoutEndDateUtc")]
        [Display(ResourceType = typeof(Displays), Name = "LockoutEndDateUtc")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [JsonProperty(PropertyName = "phoneNumber")]
        [Display(ResourceType = typeof(Displays), Name = "PhoneNumber")]
        [Phone(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Phone")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "phoneNumberConfirmed")]
        [Display(ResourceType = typeof(Displays), Name = "PhoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [JsonProperty(PropertyName = "twoFactorEnabled")]
        [Display(ResourceType = typeof(Displays), Name = "TwoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        [JsonProperty(PropertyName = "profilePicture")]
        [Display(ResourceType = typeof(Displays), Name = "ProfilePicture")]
        public string ProfilePicture { get; set; }

        [JsonProperty(PropertyName = "roles")]
        [Display(ResourceType = typeof(Displays), Name = "Roles")]
        public IList<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();

        [JsonProperty(PropertyName = "claims")]
        [Display(ResourceType = typeof(Displays), Name = "Claims")]
        public IList<UserClaimViewModel> Claims { get; set; } = new List<UserClaimViewModel>();

        [JsonProperty(PropertyName = "logins")]
        [Display(ResourceType = typeof(Displays), Name = "Logins")]
        public IList<UserLoginViewModel> Logins { get; set; } = new List<UserLoginViewModel>();
        
        [JsonProperty(PropertyName = "occupation")]
        [Display(ResourceType = typeof(Displays), Name = "Occupation")]
        [StringLength(256, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength")]
        public string Occupation { get; set; }

        #endregion
    }
}