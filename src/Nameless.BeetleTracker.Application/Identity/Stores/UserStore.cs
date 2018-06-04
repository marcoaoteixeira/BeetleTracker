using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nameless.BeetleTracker.Data;
using Nameless.BeetleTracker.Identity.Stores.Resources.User;
using Newtonsoft.Json;

namespace Nameless.BeetleTracker.Identity.Stores {

    /// <summary>
    /// Implementation of <see cref="IUserStore{TUser}"/> and affinities.
    /// </summary>
    /// <typeparam name="TUser">Type of <see cref="IUser"/>.</typeparam>
    public sealed class UserStore<TUser> : IUserLoginStore<TUser>
        , IUserClaimStore<TUser>
        , IUserRoleStore<TUser>
        , IUserPasswordStore<TUser>
        , IUserSecurityStampStore<TUser>
        , IQueryableUserStore<TUser>
        , IUserStore<TUser>
        , IUserLockoutStore<TUser, string>
        , IUserEmailStore<TUser>
        , IUserPhoneNumberStore<TUser>
        , IUserTwoFactorStore<TUser, string>
        , IDisposable
        where TUser : IdentityUser {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Private Fields

        private bool _disposed;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="UserStore{TUser}"/>.
        /// </summary>
        /// <param name="database">The database.</param>
        public UserStore(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region Destructors

        /// <summary>
        /// Destructor.
        /// </summary>
        ~UserStore() {
            Dispose(false);
        }

        #endregion Destructors

        #region Private Methods

        private void Dispose(bool disposing) {
            _disposed = true;
        }

        private void PreventAccessAfterDispose() {
            if (_disposed) {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Retrieves all users given a where clause.
        /// </summary>
        /// <param name="where">Where clause.</param>
        /// <returns>All filtered users.</returns>
        public IEnumerable<TUser> GetUsers(Func<TUser, bool> where = null) {
            var users = _database.ExecuteReader(SQL.GetUsers, Mapper.MapUser<TUser>);
            return where != null
                ? users.Where(where).ToArray()
                : users.ToArray();
        }

        #endregion Public Methods

        #region IUserLoginStore<TUser,string> Members

        /// <inheritdoc/>
        public Task AddLoginAsync(TUser user, UserLoginInfo login) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));
            Prevent.ParameterNull(login, nameof(login));

            return Task.Factory.StartNew(() => {
                _database.ExecuteNonQuery(SQL.AddUserLogin, parameters: new[] {
                    Parameter.CreateInputParameter("UserId", user.Id, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(UserLoginInfo.LoginProvider), login.LoginProvider),
                    Parameter.CreateInputParameter(nameof(UserLoginInfo.ProviderKey), login.ProviderKey)
                });
            });
        }

        /// <inheritdoc/>
        public Task<TUser> FindAsync(UserLoginInfo login) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(login, nameof(login));

            return Task.Factory.StartNew(() => {
                var user = _database.ExecuteReaderSingle(SQL.FindUserByUserLogin, Mapper.MapUser<TUser>, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(UserLoginInfo.LoginProvider), login.LoginProvider),
                    Parameter.CreateInputParameter(nameof(UserLoginInfo.ProviderKey), login.ProviderKey)
                });
                return user;
            });
        }

        /// <inheritdoc/>
        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var userLogins = _database.ExecuteReader(SQL.GetUserLoginsByUser, Mapper.MapUserLogin, parameters: new[] {
                    Parameter.CreateInputParameter("UserId", user.Id)
                });
                return userLogins.ToList() as IList<UserLoginInfo>;
            });
        }

        /// <inheritdoc/>
        public Task RemoveLoginAsync(TUser user, UserLoginInfo login) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));
            Prevent.ParameterNull(login, nameof(login));

            return Task.Factory.StartNew(() => {
                _database.ExecuteNonQuery(SQL.RemoveUserLogin, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(UserLoginInfo.LoginProvider), login.LoginProvider),
                    Parameter.CreateInputParameter(nameof(UserLoginInfo.ProviderKey), login.ProviderKey)
                });
            });
        }

        #endregion IUserLoginStore<TUser,string> Members

        #region IUserStore<TUser,string> Members

        /// <inheritdoc/>
        public Task CreateAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var attribute = (string)JsonConvert.SerializeObject(user.Attributes);
                _database.ExecuteNonQuery(SQL.CreateUser, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), Guid.NewGuid(), DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityUser.UserName), user.UserName),
                    Parameter.CreateInputParameter(nameof(IdentityUser.FullName), user.FullName),
                    Parameter.CreateInputParameter(nameof(IdentityUser.AccessFailedCount), user.AccessFailedCount, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(IdentityUser.Email), user.Email),
                    Parameter.CreateInputParameter(nameof(IdentityUser.EmailConfirmed), user.EmailConfirmed, DbType.Boolean),
                    Parameter.CreateInputParameter(nameof(IdentityUser.LockoutEnabled), user.LockoutEnabled, DbType.Boolean),
                    Parameter.CreateInputParameter(nameof(IdentityUser.LockoutEndDateUtc), user.LockoutEndDateUtc, DbType.DateTime),
                    Parameter.CreateInputParameter(nameof(IdentityUser.PasswordHash), user.PasswordHash),
                    Parameter.CreateInputParameter(nameof(IdentityUser.PhoneNumber), user.PhoneNumber),
                    Parameter.CreateInputParameter(nameof(IdentityUser.PhoneNumberConfirmed), user.PhoneNumberConfirmed, DbType.Boolean),
                    Parameter.CreateInputParameter(nameof(IdentityUser.TwoFactorEnabled), user.TwoFactorEnabled, DbType.Boolean),
                    Parameter.CreateInputParameter(nameof(IdentityUser.SecurityStamp), user.SecurityStamp),
                    Parameter.CreateInputParameter(nameof(IdentityUser.ProfilePicture), user.ProfilePicture, DbType.Binary),
                    Parameter.CreateInputParameter(nameof(IdentityUser.Attributes), attribute),
                    Parameter.CreateInputParameter(nameof(IdentityUser.State), (int)user.State, DbType.Int32)
                });
            });
        }

        /// <inheritdoc/>
        public Task DeleteAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                _database.ExecuteNonQuery(SQL.DeleteUser, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityUser.State), (int)EntityState.Deleted, DbType.Int32)
                });
            });
        }

        /// <inheritdoc/>
        public Task<TUser> FindByIdAsync(string userId) {
            PreventAccessAfterDispose();

            return Task.Factory.StartNew(() => {
                var user = _database.ExecuteReaderSingle(SQL.FindUserById, Mapper.MapUser<TUser>, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), userId, DbType.Guid)
                });
                return user;
            });
        }

        /// <inheritdoc/>
        public Task<TUser> FindByNameAsync(string userName) {
            PreventAccessAfterDispose();

            return Task.Factory.StartNew(() => {
                var user = _database.ExecuteReaderSingle(SQL.FindUserByUserName, Mapper.MapUser<TUser>, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.UserName), userName)
                });
                return user;
            });
        }

        /// <inheritdoc/>
        public Task UpdateAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var attribute = (string)JsonConvert.SerializeObject(user.Attributes);
                _database.ExecuteNonQuery(SQL.CreateUser, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityUser.UserName), user.UserName),
                    Parameter.CreateInputParameter(nameof(IdentityUser.FullName), user.FullName),
                    Parameter.CreateInputParameter(nameof(IdentityUser.AccessFailedCount), user.AccessFailedCount, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(IdentityUser.Email), user.Email),
                    Parameter.CreateInputParameter(nameof(IdentityUser.EmailConfirmed), user.EmailConfirmed, DbType.Boolean),
                    Parameter.CreateInputParameter(nameof(IdentityUser.LockoutEnabled), user.LockoutEnabled, DbType.Boolean),
                    Parameter.CreateInputParameter(nameof(IdentityUser.LockoutEndDateUtc), user.LockoutEndDateUtc, DbType.DateTime),
                    Parameter.CreateInputParameter(nameof(IdentityUser.PasswordHash), user.PasswordHash),
                    Parameter.CreateInputParameter(nameof(IdentityUser.PhoneNumber), user.PhoneNumber),
                    Parameter.CreateInputParameter(nameof(IdentityUser.PhoneNumberConfirmed), user.PhoneNumberConfirmed, DbType.Boolean),
                    Parameter.CreateInputParameter(nameof(IdentityUser.TwoFactorEnabled), user.TwoFactorEnabled, DbType.Boolean),
                    Parameter.CreateInputParameter(nameof(IdentityUser.SecurityStamp), user.SecurityStamp),
                    Parameter.CreateInputParameter(nameof(IdentityUser.ProfilePicture), user.ProfilePicture, DbType.Binary),
                    Parameter.CreateInputParameter(nameof(IdentityUser.Attributes), attribute),
                    Parameter.CreateInputParameter(nameof(IdentityUser.State), (int)user.State, DbType.Int32)
                });
            });
        }

        #endregion IUserStore<TUser,string> Members

        #region IDisposable Members

        /// <inheritdoc/>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members

        #region IUserClaimStore<TUser,string> Members

        /// <inheritdoc/>
        public Task AddClaimAsync(TUser user, Claim claim) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));
            Prevent.ParameterNull(claim, nameof(claim));

            return Task.Factory.StartNew(() => {
                _database.ExecuteNonQuery(SQL.AddUserClaim, parameters: new[] {
                    Parameter.CreateInputParameter("UserId", user.Id, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityUserClaim.Id), Guid.NewGuid(), DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityUserClaim.Type), claim.Type),
                    Parameter.CreateInputParameter(nameof(IdentityUserClaim.Value), claim.Value)
                });
            });
        }

        /// <inheritdoc/>
        public Task<IList<Claim>> GetClaimsAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var claims = _database.ExecuteReader(SQL.GetUserClaims, Mapper.MapUserClaim, parameters: new[] {
                    Parameter.CreateInputParameter("UserId", user.Id)
                });
                return claims.ToList() as IList<Claim>;
            });
        }

        /// <inheritdoc/>
        public Task RemoveClaimAsync(TUser user, Claim claim) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));
            Prevent.ParameterNull(claim, nameof(claim));

            return Task.Factory.StartNew(() => {
                _database.ExecuteNonQuery(SQL.RemoveUserClaim, parameters: new[] {
                    Parameter.CreateInputParameter("UserId", user.Id, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityUserClaim.Type), claim.Type),
                    Parameter.CreateInputParameter(nameof(IdentityUserClaim.Value), claim.Value)
                });
            });
        }

        #endregion IUserClaimStore<TUser,string> Members

        #region IUserRoleStore<TUser,string> Members

        /// <inheritdoc/>
        public Task AddToRoleAsync(TUser user, string roleName) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));
            Prevent.ParameterNullOrWhiteSpace(roleName, nameof(roleName));

            return Task.Factory.StartNew(() => {
                var roleId = _database.ExecuteScalar(SQL.GetRoleIdByName, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityRole.Name), roleName)
                });

                if (roleId == null) {
                    throw new InvalidOperationException(string.Format(Properties.Resources.CouldNotFindRoleByItsName, roleName));
                }

                _database.ExecuteNonQuery(SQL.AddRoleToUser, parameters: new[] {
                    Parameter.CreateInputParameter("UserId", user.Id, DbType.Guid),
                    Parameter.CreateInputParameter("RoleId", roleId, DbType.Guid)
                });
            });
        }

        /// <inheritdoc/>
        public Task<IList<string>> GetRolesAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var roles = _database.ExecuteReader(SQL.GetUserRoles, _ => Mapper.MapString(_, nameof(IdentityRole.Name)), parameters: new[] {
                    Parameter.CreateInputParameter("UserId", user.Id)
                });
                return roles.ToList() as IList<string>;
            });
        }

        /// <inheritdoc/>
        public Task<bool> IsInRoleAsync(TUser user, string roleName) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));
            Prevent.ParameterNullOrWhiteSpace(roleName, nameof(roleName));

            return Task.Factory.StartNew(() => {
                var isInRole = _database.ExecuteScalar(SQL.IsUserInRole, parameters: new[] {
                    Parameter.CreateInputParameter("RoleName", roleName)
                });
                return (bool)isInRole;
            });
        }

        /// <inheritdoc/>
        public Task RemoveFromRoleAsync(TUser user, string roleName) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));
            Prevent.ParameterNullOrWhiteSpace(roleName, nameof(roleName));

            return Task.Factory.StartNew(() => {
                _database.ExecuteNonQuery(SQL.RemoveUserFromRole, parameters: new[] {
                    Parameter.CreateInputParameter("UserId", user.Id),
                    Parameter.CreateInputParameter("RoleName", roleName)
                });
            });
        }

        #endregion IUserRoleStore<TUser,string> Members

        #region IUserPasswordStore<TUser,string> Members

        /// <inheritdoc/>
        public Task<string> GetPasswordHashAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var hash = _database.ExecuteScalar(SQL.GetUserPasswordHash, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid)
                });
                return (string)hash;
            });
        }

        /// <inheritdoc/>
        public Task<bool> HasPasswordAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var hasPassword = _database.ExecuteScalar(SQL.UserHasPasswordHash, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid)
                });
                return (bool)hasPassword;
            });
        }

        /// <inheritdoc/>
        public Task SetPasswordHashAsync(TUser user, string passwordHash) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));
            Prevent.ParameterNullOrWhiteSpace(passwordHash, nameof(passwordHash));

            return Task.Factory.StartNew(() => {
                user.PasswordHash = passwordHash;
                _database.ExecuteNonQuery(SQL.SetUserPasswordHash, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityUser.PasswordHash), user.PasswordHash)
                });
            });
        }

        #endregion IUserPasswordStore<TUser,string> Members

        #region IUserSecurityStampStore<TUser,string> Members

        /// <inheritdoc/>
        public Task<string> GetSecurityStampAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var securityStamp = _database.ExecuteScalar(SQL.GetUserSecurityStamp, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid)
                });
                return (string)securityStamp;
            });
        }

        /// <inheritdoc/>
        public Task SetSecurityStampAsync(TUser user, string stamp) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));
            Prevent.ParameterNullOrWhiteSpace(stamp, nameof(stamp));

            return Task.Factory.StartNew(() => {
                user.SecurityStamp = stamp;
                _database.ExecuteNonQuery(SQL.SetUserSecurityStamp, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityUser.SecurityStamp), user.SecurityStamp)
                });
            });
        }

        #endregion IUserSecurityStampStore<TUser,string> Members

        #region IQueryableUserStore<TUser,string> Members

        /// <inheritdoc/>
        public IQueryable<TUser> Users {
            get { return GetUsers().AsQueryable(); }
        }

        #endregion IQueryableUserStore<TUser,string> Members

        #region IUserLockoutStore<TUser,string> Members

        /// <inheritdoc/>
        public Task<int> GetAccessFailedCountAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var accessFailedCount = _database.ExecuteScalar(SQL.GetUserAccessFailedCount, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid)
                });
                return (int)accessFailedCount;
            });
        }

        /// <inheritdoc/>
        public Task<bool> GetLockoutEnabledAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var lockoutEnabled = _database.ExecuteScalar(SQL.GetUserLockoutEnabled, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid)
                });
                return (bool)lockoutEnabled;
            });
        }

        /// <inheritdoc/>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var lockoutEndDate = _database.ExecuteScalar(SQL.GetUserLockoutEndDate, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid)
                });
                return lockoutEndDate != null
                    ? new DateTimeOffset(DateTime.SpecifyKind((DateTime)lockoutEndDate, DateTimeKind.Utc))
                    : DateTimeOffset.Now;
            });
        }

        /// <inheritdoc/>
        public Task<int> IncrementAccessFailedCountAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var accessFailedCount = _database.ExecuteScalar(SQL.IncrementUserAccessFailedCount, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid)
                });
                user.AccessFailedCount = (int)accessFailedCount;
                return user.AccessFailedCount;
            });
        }

        /// <inheritdoc/>
        public Task ResetAccessFailedCountAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                user.AccessFailedCount = 0;
                _database.ExecuteNonQuery(SQL.ResetUserAccessFailedCount, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid)
                });
            });
        }

        /// <inheritdoc/>
        public Task SetLockoutEnabledAsync(TUser user, bool enabled) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                user.LockoutEnabled = enabled;
                _database.ExecuteNonQuery(SQL.SetUserLockoutEnabled, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityUser.LockoutEnabled), user.LockoutEnabled, DbType.Boolean)
                });
            });
        }

        /// <inheritdoc/>
        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                user.LockoutEndDateUtc = lockoutEnd != DateTimeOffset.MinValue
                    ? lockoutEnd.UtcDateTime
                    : (DateTime?)null;
                _database.ExecuteNonQuery(SQL.SetUserLockoutEndDate, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityUser.LockoutEndDateUtc), user.LockoutEndDateUtc, DbType.DateTime)
                });
            });
        }

        #endregion IUserLockoutStore<TUser,string> Members

        #region IUserEmailStore<TUser,string> Members

        /// <inheritdoc/>
        public Task<TUser> FindByEmailAsync(string email) {
            PreventAccessAfterDispose();

            Prevent.ParameterNullOrWhiteSpace(email, nameof(email));

            return Task.Factory.StartNew(() => {
                var user = _database.ExecuteReaderSingle(SQL.FindUserByEmail, Mapper.MapUser<TUser>, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Email), email)
                });
                return user;
            });
        }

        /// <inheritdoc/>
        public Task<string> GetEmailAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var email = _database.ExecuteScalar(SQL.GetUserEmail, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id)
                });
                return (string)email;
            });
        }

        /// <inheritdoc/>
        public Task<bool> GetEmailConfirmedAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var emailConfirmed = _database.ExecuteScalar(SQL.GetUserEmailConfirmed, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id)
                });
                return (bool)emailConfirmed;
            });
        }

        /// <inheritdoc/>
        public Task SetEmailAsync(TUser user, string email) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));
            Prevent.ParameterNullOrWhiteSpace(email, nameof(email));

            return Task.Factory.StartNew(() => {
                user.Email = email;
                _database.ExecuteNonQuery(SQL.SetUserEmail, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id),
                    Parameter.CreateInputParameter(nameof(IdentityUser.Email), user.Email)
                });
            });
        }

        /// <inheritdoc/>
        public Task SetEmailConfirmedAsync(TUser user, bool confirmed) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                user.EmailConfirmed = confirmed;
                _database.ExecuteNonQuery(SQL.SetUserEmailConfirmed, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id),
                    Parameter.CreateInputParameter(nameof(IdentityUser.EmailConfirmed), user.EmailConfirmed)
                });
            });
        }

        #endregion IUserEmailStore<TUser,string> Members

        #region IUserPhoneNumberStore<TUser,string> Members

        /// <inheritdoc/>
        public Task<string> GetPhoneNumberAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var phoneNumber = _database.ExecuteScalar(SQL.GetUserPhoneNumber, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id)
                });
                return (string)phoneNumber;
            });
        }

        /// <inheritdoc/>
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var phoneNumberConfirmed = _database.ExecuteScalar(SQL.GetUserPhoneNumberConfirmed, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id)
                });
                return (bool)phoneNumberConfirmed;
            });
        }

        /// <inheritdoc/>
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                user.PhoneNumber = phoneNumber;
                _database.ExecuteNonQuery(SQL.SetUserPhoneNumber, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id),
                    Parameter.CreateInputParameter(nameof(IdentityUser.PhoneNumber), user.PhoneNumber)
                });
            });
        }

        /// <inheritdoc/>
        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                user.PhoneNumberConfirmed = confirmed;
                _database.ExecuteNonQuery(SQL.SetUserPhoneNumberConfirmed, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id),
                    Parameter.CreateInputParameter(nameof(IdentityUser.PhoneNumberConfirmed), user.PhoneNumberConfirmed)
                });
            });
        }

        #endregion IUserPhoneNumberStore<TUser,string> Members

        #region IUserTwoFactorStore<TUser,string> Members

        /// <inheritdoc/>
        public Task<bool> GetTwoFactorEnabledAsync(TUser user) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                var twoFactorEnabled = _database.ExecuteScalar(SQL.GetUserTwoFactorEnabled, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id)
                });
                return (bool)twoFactorEnabled;
            });
        }

        /// <inheritdoc/>
        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(user, nameof(user));

            return Task.Factory.StartNew(() => {
                user.TwoFactorEnabled = enabled;
                _database.ExecuteNonQuery(SQL.SetUserTwoFactorEnabled, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(IdentityUser.Id), user.Id),
                    Parameter.CreateInputParameter(nameof(IdentityUser.TwoFactorEnabled), user.TwoFactorEnabled)
                });
            });
        }

        #endregion IUserTwoFactorStore<TUser,string> Members
    }
}