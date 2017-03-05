using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Provides information about a <see cref="DependencyProperty{TProperty}"/> whichs value has been changed.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property which has been changed.</typeparam>
    public sealed class PropertyChangedEventArgs<TProperty> : PropertyChangedEventArgs
    {
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the new value of the property.
        /// </summary>
        public new TProperty NewValue { get; }
        /// <summary>
        /// Retrieves the old value of the property.
        /// </summary>
        public new TProperty OldValue { get; }
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty{TProperty}"/> which has been changed.
        /// </summary>
        public new DependencyProperty<TProperty> Property { get; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Creates a new instance of the <see cref="PropertyChangedEventArgs{TProperty}"/>.
        /// </summary>
        /// <param name="recusivePropertyChanges">The number of recusive property changes when this args have been created.</param>
        /// <param name="property">The <see cref="DependencyProperty{TProperty}"/> which has been changed.</param>
        /// <param name="newValue">The new value of the property.</param>
        /// <param name="oldValue">The old value of the property.</param>
        internal PropertyChangedEventArgs(Int16 recusivePropertyChanges, DependencyProperty<TProperty> property,  TProperty newValue, TProperty oldValue)
            : base(recusivePropertyChanges)
        {
            Property = property; 
            NewValue = newValue;
            OldValue = oldValue;
        }
        #endregion
        #region #### METHODS ############################################################
        /// <summary>
        /// Retrieves the new value of the property.
        /// </summary>
        /// <returns>The new value of the property.</returns>
        protected override Object GetNewValue() => NewValue;
        /// <summary>
        /// Retrieves the old value of the property.
        /// </summary>
        /// <returns>The old value of the property.</returns>
        protected override Object GetOldValue() => OldValue;
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty"/> which has been changed.
        /// </summary>
        /// <returns>The <see cref="DependencyProperty"/> which has been changed.</returns>
        protected override DependencyProperty GetProperty() => Property;
        #endregion
    }
}
