using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Represents a identifier for write access to a readonly <see cref="DependencyProperty"/>.
    /// </summary>
    public abstract partial class DependencyPropertyKey
    {
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty"/> which is associated with the current key.
        /// </summary>
        public DependencyProperty Property => GetProperty();
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes the instance of the <see cref="DependencyPropertyKey"/> class.
        /// </summary>
        internal DependencyPropertyKey() { }
        #endregion
        #region #### PROTECTED METHODS ##################################################
        /// <summary>
        /// Retrieves the <see cref="DependencyProperty"/> which is associated to the current key.
        /// </summary>
        /// <returns>The <see cref="DependencyProperty"/> for the current key.</returns>
        protected abstract DependencyProperty GetProperty();
        #endregion
    }
}
