using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Represents a identifier for write access to a readonly <see cref="DependencyProperty"/>.
    /// </summary>
    /// <typeparam name="TPropertyType">The type of the property.</typeparam>
    public sealed class DependencyPropertyKey<TPropertyType> : DependencyPropertyKey
    {
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty"/> which is associated with the current key.
        /// </summary>
        public new DependencyProperty<TPropertyType> Property { get; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes the instance of the <see cref="DependencyPropertyKey"/> class.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty{TPropertyType}"/> which created the current key.</param>
        internal DependencyPropertyKey(DependencyProperty<TPropertyType> property) => Property = property ?? throw new ArgumentNullException(nameof(property));
        #endregion
        #region #### PROTECTED METHODS ##################################################
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty"/> which is associated to the current key.
        /// </summary>
        /// <returns>The <see cref="DependencyProperty"/> for the current key.</returns>
        protected override DependencyProperty GetProperty() => Property;
        #endregion
    }
}
