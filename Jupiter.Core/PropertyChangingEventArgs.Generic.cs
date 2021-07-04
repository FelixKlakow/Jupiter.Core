using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Provides information about a <see cref="DependencyProperty{TProperty}"/> whichs value is changing.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property which is changing.</typeparam>
    public sealed class PropertyChangingEventArgs<TProperty> : PropertyChangingEventArgs
    {
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the current value of the property before the change.
        /// </summary>
        public new TProperty Value { get; }
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty{TProperty}"/> which is changing.
        /// </summary>
        public new DependencyProperty<TProperty> Property { get; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Creates a new instance of the <see cref="PropertyChangingEventArgs{TProperty}"/>.
        /// </summary>
        /// <param name="recusivePropertyChanges">The number of recusive property changes when this args have been created.</param>
        /// <param name="property">The <see cref="DependencyProperty{TPropertyType}"/> which is changing.</param>
        /// <param name="value">The value of the property.</param>
        internal PropertyChangingEventArgs(Int16 recusivePropertyChanges, DependencyProperty<TProperty> property, TProperty value)
            : base(recusivePropertyChanges)
        {
            Property = property;
            Value = value;
        }
        #endregion
        #region #### METHODS ############################################################
        /// <summary>
        /// Retrieves the current value of the property.
        /// </summary>
        /// <returns>The current value of the property.</returns>
        protected override Object GetValue() => Value;
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty"/> which has been changed.
        /// </summary>
        /// <returns>The <see cref="DependencyProperty"/> which has been changed.</returns>
        protected override DependencyProperty GetProperty() => Property;
        #endregion
    }
}
