using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Specifies how a binding is handled for a property.
    /// </summary>
    public enum BindingMode
    {
        /// <summary>
        /// Changes in one of the affected properties causes an update in the other.
        /// </summary>
        TwoWay = 0,
        /// <summary>
        /// Changes in the source property cause a update in the target property.
        /// </summary>
        OneWay = 1,
        /// <summary>
        /// Specifies that the default binding mode of the target property should be used. When used during the declaration of a <see cref="DependencyProperty"/> it defaults to <see cref="OneWay"/>.
        /// </summary>
        Default = 4
    }
}
