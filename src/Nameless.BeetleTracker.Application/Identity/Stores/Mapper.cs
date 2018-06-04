using System.Collections;
using System.Data;
using System.Security.Claims;
using Nameless.BeetleTracker.Data;
using Newtonsoft.Json;

namespace Nameless.BeetleTracker.Identity.Stores {

    internal static class Mapper {

        #region Internal Static Methods

        internal static TUser MapUser<TUser>(IDataReader reader) where TUser : IdentityUser {
            if (reader == null) { return default(TUser); }
            var id = reader.GetStringOrDefault(nameof(IdentityUser.Id));
            var attributes = reader.GetStringOrDefault(nameof(IdentityUser.Attributes));
            var hashtable = JsonConvert.DeserializeObject<Hashtable>(attributes);
            return (TUser)new IdentityUser(id, hashtable) {
                UserName = reader.GetStringOrDefault(nameof(IdentityUser.UserName)),
                FullName = reader.GetStringOrDefault(nameof(IdentityUser.FullName)),
                AccessFailedCount = reader.GetInt32OrDefault(nameof(IdentityUser.AccessFailedCount)),
                Email = reader.GetStringOrDefault(nameof(IdentityUser.Email)),
                EmailConfirmed = reader.GetBooleanOrDefault(nameof(IdentityUser.EmailConfirmed)),
                LockoutEnabled = reader.GetBooleanOrDefault(nameof(IdentityUser.LockoutEnabled)),
                LockoutEndDateUtc = reader.GetDateTimeOrDefault(nameof(IdentityUser.LockoutEndDateUtc)),
                PasswordHash = reader.GetStringOrDefault(nameof(IdentityUser.PasswordHash)),
                PhoneNumber = reader.GetStringOrDefault(nameof(IdentityUser.PhoneNumber)),
                PhoneNumberConfirmed = reader.GetBooleanOrDefault(nameof(IdentityUser.PhoneNumberConfirmed)),
                TwoFactorEnabled = reader.GetBooleanOrDefault(nameof(IdentityUser.TwoFactorEnabled)),
                SecurityStamp = reader.GetStringOrDefault(nameof(IdentityUser.SecurityStamp)),
                ProfilePicture = reader.GetBlobOrDefault(nameof(IdentityUser.ProfilePicture)),
                State = (EntityState)reader.GetInt32OrDefault(nameof(IdentityUser.State))
            };
        }

        internal static TRole MapRole<TRole>(IDataReader reader) where TRole : IdentityRole {
            if (reader == null) { return default(TRole); }
            var id = reader.GetStringOrDefault(nameof(IdentityRole.Id));
            var attributes = reader.GetStringOrDefault(nameof(IdentityRole.Attributes));
            var hashtable = JsonConvert.DeserializeObject<Hashtable>(attributes);
            return (TRole)new IdentityRole(id, attributes: hashtable) {
                Name = reader.GetStringOrDefault(nameof(IdentityRole.Name)),
                State = (EntityState)reader.GetInt32OrDefault(nameof(IdentityRole.State))
            };
        }

        internal static IdentityUserLogin MapUserLogin(IDataReader reader) {
            if (reader == null) { return null; }
            return new IdentityUserLogin {
                LoginProvider = reader.GetStringOrDefault(nameof(IdentityUserLogin.LoginProvider)),
                ProviderKey = reader.GetStringOrDefault(nameof(IdentityUserLogin.ProviderKey))
            };
        }

        internal static Claim MapUserClaim(IDataReader reader) {
            if (reader == null) { return null; }
            return new Claim(
                type: reader.GetStringOrDefault(nameof(IdentityUserClaim.Type)), value:
                reader.GetStringOrDefault(nameof(IdentityUserClaim.Value))
            );
        }

        internal static string MapString(IDataReader reader, string columnName) {
            if (reader == null) { return null; }
            return reader.GetStringOrDefault(columnName);
        }

        #endregion Internal Static Methods
    }
}