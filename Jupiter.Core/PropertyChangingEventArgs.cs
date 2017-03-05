using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Provides information about a <see cref="DependencyProperty"/> whichs value is changing.
    /// </summary>
    public abstract class PropertyChangingEventArgs : EventArgs
    {
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the current value of the property before the change.
        /// </summary>
        public Object Value { get { return GetValue(); } }
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty"/> which is changing.
        /// </summary>
        public DependencyProperty Property { get { return GetProperty(); } }
        /// <summary>
        /// Specifies if the <see cref="PropertyChangingEventArgs"/> has been created for the first change on the current stack.
        /// </summary>
        public Boolean IsBaseChange { get; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Creates a new instance of the <see cref="PropertyChangingEventArgs"/>.
        /// </summary>
        /// <param name="recusivePropertyChanges">The number of recusive property changes when this args have been created.</param>
        internal PropertyChangingEventArgs(Int16 recusivePropertyChanges)
        {
            IsBaseChange = recusivePropertyChanges == 1;
        }
        #endregion
        #region #### METHODS ############################################################
        /// <summary>
        /// Retrieves the current value of the property.
        /// </summary>
        /// <returns>The current value of the property.</returns>
        protected abstract Object GetValue();
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty"/> which has been changed.
        /// </summary>
        /// <returns>The <see cref="DependencyProperty"/> which has been changed.</returns>
        protected abstract DependencyProperty GetProperty();
        #endregion
    }
}
