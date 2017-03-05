using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Provides information about a single property in the dependency property system.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    public abstract class DependencyProperty<TProperty> : DependencyProperty
    {
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyProperty{TPropertyType}"/> class.
        /// </summary>
        /// <param name="declaringType">The type which declares the property.</param>
        /// <param name="ownerType">The type which can own this property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="defaultBindingMode">The default <see cref="BindingMode"/> for this property.</param>
        /// <param name="isAttachement">Specifies if the property is a attachement.</param>
        /// <param name="isReadonly">Specifies if the property is readonly and a key is required to write the property.</param>
        /// <param name="isUsingValueFactory">Specifies if the property is using a value factory for the initial value.</param>
        /// <param name="isFastProperty">Specifies if the property is accessed in a special behaviour.</param>
        internal DependencyProperty(TypeInfo declaringType, TypeInfo ownerType, String name, BindingMode defaultBindingMode, Boolean isAttachement, Boolean isUsingValueFactory)
            : base(declaringType, ownerType, Reflection.TypeOf<TProperty>.TypeInfo, name, defaultBindingMode, isAttachement, isUsingValueFactory)
        {
        }
        #endregion
        #region #### PUBLIC METHODS #####################################################
        /// <summary>
        /// Retrieves the default value of the current <see cref="DependencyProperty"/>.
        /// </summary>
        /// <param name="owner">The onwer of the dependency property.</param>
        /// <returns>Retrieves the default value of the property.</returns>
        internal abstract new TProperty GetPropertyDefault(IDependencyObject owner);
        /// <summary>
        /// Sets the value of the property to the new value.
        /// </summary>
        /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
        /// <param name="changeCount">The change count of the current property.</param>
        /// <param name="value">The current value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        internal abstract void SetMarkupValue(DependencyObjectContainer container, ref Int16 changeCount, ref GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>> changeHandler, ref TProperty value, TProperty newValue);
        /// <summary>
        /// Sets the value of the property to the new value.
        /// </summary>
        /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
        /// <param name="changeCount">The change count of the current property.</param>
        /// <param name="expression">The <see cref="DependencyExpression"/> which is assigned to the property or null if no expression is assigned.</param>
        /// <param name="baseValue">The base value of the property.</param>
        /// <param name="value">The current value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        internal abstract void SetValue(DependencyObjectContainer container, ref Int16 changeCount, ref GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>> changeHandler, ref DependencyExpression expression, ref TProperty baseValue, ref TProperty value, TProperty newValue);
        /// <summary>
        /// Coerces the current value of the dependency object.
        /// </summary>
        /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
        /// <param name="changeCount">The change count of the current property.</param>
        /// <param name="expression">The <see cref="DependencyExpression"/> which is assigned to the property or null if no expression is assigned.</param>
        /// <param name="baseValue">The base value of the property.</param>
        /// <param name="value">The current value of the property.</param>
        internal abstract void CoerceValue(DependencyObjectContainer container, ref Int16 changeCount, ref GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>> changeHandler, DependencyExpression expression, TProperty baseValue, ref TProperty value);
        #endregion
        #region #### PRIVATE METHODS ####################################################
        #endregion
        #region #### NESTED TYPES #######################################################
        #endregion
    }
}
