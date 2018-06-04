using System.Collections;
using Microsoft.AspNet.Identity;
using Nameless.BeetleTracker.Dynamic;

namespace Nameless.BeetleTracker.Identity {

    /// <summary>
    /// Default implementation of <see cref="IRole"/>.
    /// </summary>
    public class IdentityRole : IRole {

        #region Private Read-Only Fields

        private readonly string _id;
        private readonly IDictionary _attributes;

        #endregion Private Read-Only Fields

        #region Internal Constructors

        internal IdentityRole(string id = null, IDictionary attributes = null) {
            _id = id;
            _attributes = attributes ?? new Hashtable();
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the entity state.
        /// </summary>
        public EntityState State { get; set; }

        /// <summary>
        /// Gets or sets the identity role attributes
        /// </summary>
        public dynamic Attributes {
            get { return new HashtableDynamicObject(_attributes); }
        }

        #endregion Public Properties

        #region IRole Members

        /// <inheritdoc/>
        public string Id {
            get { return _id; }
        }

        /// <inheritdoc/>
        public string Name { get; set; }

        #endregion IRole Members
    }
}