using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Represents the base class for 
    /// </summary>
    public abstract class DependencyExtension : DependencyObject
    {
        #region #### VARIABLES ##########################################################
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyExtension"/> class.
        /// </summary>
        protected DependencyExtension() { }
        #endregion
        #region #### PROTECTED / INTERNAL METHODS #######################################
        /// <summary>
        /// Creates a new <see cref="DependencyExpression"/> from the current <see cref="DependencyExtension"/> for the specified <see cref="DependencyProperty"/>.
        /// </summary>
        /// <param name="property">The property for which the expression should be created.</param>
        /// <returns>The expression which have been created.</returns>
        protected internal abstract DependencyExpression CreateExpression(DependencyProperty property);
        #endregion
    }
}
