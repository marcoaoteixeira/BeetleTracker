using System;

namespace Nameless.BeetleTracker.IoC {

    /// <summary>
    /// Attribute used to wire services via property injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class PropertyAutoWireAttribute : Attribute {
    }
}