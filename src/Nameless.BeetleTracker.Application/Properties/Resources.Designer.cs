﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nameless.BeetleTracker.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Nameless.BeetleTracker.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Model contains null values..
        /// </summary>
        internal static string CheckModelForNullAttributeErrorMessage {
            get {
                return ResourceManager.GetString("CheckModelForNullAttributeErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find role by its name. Role name: {0}..
        /// </summary>
        internal static string CouldNotFindRoleByItsName {
            get {
                return ResourceManager.GetString("CouldNotFindRoleByItsName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your security code is {0}.
        /// </summary>
        internal static string EmailTokenProviderMessageFormat {
            get {
                return ResourceManager.GetString("EmailTokenProviderMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Security Code.
        /// </summary>
        internal static string EmailTokenProviderSubject {
            get {
                return ResourceManager.GetString("EmailTokenProviderSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your security code is {0}.
        /// </summary>
        internal static string PhoneNumberTokenProviderMessageFormat {
            get {
                return ResourceManager.GetString("PhoneNumberTokenProviderMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find codec for image..
        /// </summary>
        internal static string WebHelperEncodeImageBase64CodecNotFound {
            get {
                return ResourceManager.GetString("WebHelperEncodeImageBase64CodecNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find image file..
        /// </summary>
        internal static string WebHelperEncodeImageBase64FileNotFound {
            get {
                return ResourceManager.GetString("WebHelperEncodeImageBase64FileNotFound", resourceCulture);
            }
        }
    }
}
