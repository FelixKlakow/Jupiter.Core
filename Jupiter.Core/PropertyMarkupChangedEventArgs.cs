using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Provides information about a <see cref="DependencyProperty"/> whichs <see cref="DependencyExtension"/> has been changed.
    /// </summary>
    public sealed class PropertyExtensionChangedEventArgs : EventArgs
    {
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the new <see cref="DependencyExtension"/> of the property.
        /// </summary>
        public DependencyExtension NewValue { get; }
        /// <summary>
        /// Retrieves the old <see cref="DependencyExtension"/> of the property.
        /// </summary>
        public DependencyExtension OldValue { get; }
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty"/> whichs <see cref="DependencyExtension"/> has been changed.
        /// </summary>
        public DependencyProperty Property { get; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyExtensionChangedEventArgs"/>.
        /// </summary>
        /// <param name="property">The property which is affected.</param>
        /// <param name="newValue">The new markup value of the property.</param>
        /// <param name="oldValue">The old markup value of the property.</param>
        internal PropertyExtensionChangedEventArgs(DependencyProperty property, DependencyExtension newValue, DependencyExtension oldValue)
        {
            Property = property;
            NewValue = newValue;
            OldValue = oldValue;
        }
        #endregion
        #region #### METHODS ############################################################
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString() => $"Property={Property.Name} NewValue={NewValue} OldValue={OldValue}";
        #endregion
    }
}
