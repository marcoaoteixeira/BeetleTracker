﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nameless.BeetleTracker.EventSourcing.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SQL {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SQL() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Nameless.BeetleTracker.EventSourcing.Resources.SQL", typeof(SQL).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Events VALUES (
        ///    @AggregateID,
        ///    @Version,
        ///    @Payload,
        ///    @TimeStamp,
        ///    @EventType
        ///);.
        /// </summary>
        internal static string CreateEvent {
            get {
                return ResourceManager.GetString("CreateEvent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Events VALUES (
        ///    @AggregateID,
        ///    @Version,
        ///    @Payload,
        ///    @TimeStamp,
        ///    @EventType
        ///);.
        /// </summary>
        internal static string CreateSnapshot {
            get {
                return ResourceManager.GetString("CreateSnapshot", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    AggregateID,
        ///    Version,
        ///    Payload,
        ///    TimeStamp,
        ///    EventType
        ///FROM Events (NOLOCK)
        ///WHERE
        ///    AggregateID = @AggregateID
        ///AND Version &gt;= @Version;.
        /// </summary>
        internal static string ListEvents {
            get {
                return ResourceManager.GetString("ListEvents", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    AggregateID,
        ///    Version,
        ///    Payload,
        ///    TimeStamp,
        ///    EventType
        ///FROM Snapshots (NOLOCK)
        ///WHERE
        ///    AggregateID = @AggregateID
        ///AND Version &gt;= @Version;.
        /// </summary>
        internal static string ListSnapshots {
            get {
                return ResourceManager.GetString("ListSnapshots", resourceCulture);
            }
        }
    }
}
