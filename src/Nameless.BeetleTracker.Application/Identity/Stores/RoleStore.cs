using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nameless.BeetleTracker.Data;
using Nameless.BeetleTracker.Identity.Stores.Resources.Role;
using Newtonsoft.Json;

namespace Nameless.BeetleTracker.Identity.Stores {

    /// <summary>
    /// Default implementation of <see cref="IRoleStore{TRole}"/>.
    /// </summary>
    /// <typeparam name="TRole">Type of the role.</typeparam>
    public sealed class RoleStore<TRole> : IRoleStore<TRole>
        , IQueryableRoleStore<TRole>
        , IDisposable
        where TRole : IdentityRole {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Private Fields

        private bool _disposed;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="RoleStore{TRole}"/>.
        /// </summary>
        /// <param name="database">An instance of <see cref="IDatabase"/>.</param>
        public RoleStore(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region Destructors

        /// <summary>
        /// Destructor
        /// </summary>
        ~RoleStore() {
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
        /// Retrieves all roles.
        /// </summary>
        /// <param name="where">The where clause.</param>
        /// <returns>A collection of <typeparamref name="TRole"/>.</returns>
        public IEnumerable<TRole> GetRoles(Func<TRole, bool> where = null) {
            var roles = _database.ExecuteReader(SQL.ListAll, Mapper.MapRole<TRole>);

            return where != null
                ? roles.Where(where).ToArray()
                : roles.ToArray();
        }

        #endregion Public Methods

        #region IRoleStore<TRole,string> Members

        /// <inheritdoc/>
        public Task CreateAsync(TRole role) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(role, nameof(role));

            return Task.Factory.StartNew(() => {
                var attributes = JsonConvert.SerializeObject(role.Attributes);
                _database.ExecuteNonQuery(SQL.Create, parameters: new Parameter[] {
                    Parameter.CreateInputParameter(nameof(IdentityRole.Id), role.Id, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityRole.Name), role.Name),
                    Parameter.CreateInputParameter(nameof(IdentityRole.Attributes), attributes)
                });
            });
        }

        /// <inheritdoc/>
        public Task DeleteAsync(TRole role) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(role, nameof(role));

            return Task.Factory.StartNew(() => {
                _database.ExecuteNonQuery(SQL.Delete, parameters: new Parameter[] {
                    Parameter.CreateInputParameter(nameof(IdentityRole.Id), role.Id, DbType.Guid)
                });
            });
        }

        /// <inheritdoc/>
        public Task<TRole> FindByIdAsync(string roleId) {
            PreventAccessAfterDispose();

            Prevent.ParameterNullOrWhiteSpace(roleId, nameof(roleId));

            return Task.Factory.StartNew(() => {
                var role = _database.ExecuteReaderSingle(SQL.FindById, Mapper.MapRole<TRole>, parameters: new Parameter[] {
                    Parameter.CreateInputParameter(nameof(IdentityRole.Id), roleId, DbType.Guid)
                });
                return role;
            });
        }

        /// <inheritdoc/>
        public Task<TRole> FindByNameAsync(string roleName) {
            PreventAccessAfterDispose();

            Prevent.ParameterNullOrWhiteSpace(roleName, nameof(roleName));

            return Task.Factory.StartNew(() => {
                var role = _database.ExecuteReaderSingle(SQL.FindByName, Mapper.MapRole<TRole>, parameters: new Parameter[] {
                    Parameter.CreateInputParameter(nameof(IdentityRole.Name), roleName)
                });
                return role;
            });
        }

        /// <inheritdoc/>
        public Task UpdateAsync(TRole role) {
            PreventAccessAfterDispose();

            Prevent.ParameterNull(role, nameof(role));

            return Task.Factory.StartNew(() => {
                var attributes = JsonConvert.SerializeObject(role.Attributes);
                _database.ExecuteNonQuery(SQL.Update, parameters: new Parameter[] {
                    Parameter.CreateInputParameter(nameof(IdentityRole.Id), role.Id, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(IdentityRole.Name), role.Name),
                    Parameter.CreateInputParameter(nameof(IdentityRole.Attributes), attributes)
                });
            });
        }

        #endregion IRoleStore<TRole,string> Members

        #region IDisposable Members

        /// <inheritdoc/>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members

        #region IQueryableRoleStore<TRole,string> Members

        /// <inheritdoc/>
        public IQueryable<TRole> Roles {
            get { return GetRoles().AsQueryable(); }
        }

        #endregion IQueryableRoleStore<TRole,string> Members
    }
}