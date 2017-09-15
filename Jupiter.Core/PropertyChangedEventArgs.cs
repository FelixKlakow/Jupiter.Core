using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Provides information about a <see cref="DependencyProperty"/> whichs value has been changed.
    /// </summary>
    public abstract class PropertyChangedEventArgs : EventArgs
    {
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the new value of the property.
        /// </summary>
        public Object NewValue { get => GetNewValue(); }
        /// <summary>
        /// Retrieves the old value of the property.
        /// </summary>
        public Object OldValue { get => GetOldValue(); }
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty"/> which has been changed.
        /// </summary>
        public DependencyProperty Property { get => GetProperty(); }
        /// <summary>
        /// Specifies if the <see cref="PropertyChangedEventArgs"/> has been created for the first change on the current stack.
        /// </summary>
        public Boolean IsBaseChange { get; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedEventArgs"/>.
        /// </summary>
        /// <param name="recusivePropertyChanges">The number of recusive property changes when this args have been created.</param>
        internal PropertyChangedEventArgs(Int16 recusivePropertyChanges) => IsBaseChange = recusivePropertyChanges == 1;
        #endregion
        #region #### METHODS ############################################################
        /// <summary>
        /// Retrieves the new value of the property.
        /// </summary>
        /// <returns>The new value of the property.</returns>
        protected abstract Object GetNewValue();
        /// <summary>
        /// Retrieves the old value of the property.
        /// </summary>
        /// <returns>The old value of the property.</returns>
        protected abstract Object GetOldValue();
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty"/> which has been changed.
        /// </summary>
        /// <returns>The <see cref="DependencyProperty"/> which has been changed.</returns>
        protected abstract DependencyProperty GetProperty();
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString() => $"Property={Property.Name} NewValue={GetNewValue()} OldValue={GetOldValue()}";
        #endregion
    }
}
