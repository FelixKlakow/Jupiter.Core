using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Provides a template for coercing the base value of the <see cref="DependencyProperty"/>.
    /// </summary>
    /// <typeparam name="TPropertyOwner">The owner of the property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="owner">The owner of the property.</param>
    /// <param name="baseValue">The base value which should be coerced.</param>
    /// <returns></returns>
    public delegate TProperty CoerceValueDelegate<TPropertyOwner, TProperty>(TPropertyOwner owner, TProperty baseValue);
    /// <summary>
    /// Provides a template for creating a value for a property when the property is accessed the first time.
    /// </summary>
    /// <typeparam name="TPropertyOwner">The owner of the property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="owner">The owner of the property.</param>
    /// <returns></returns>
    public delegate TProperty CreateValueDelegate<TPropertyOwner, TProperty>(TPropertyOwner owner);
    /// <summary>
    /// Provides a template for validating the base value of a property.
    /// </summary>
    /// <typeparam name="TPropertyOwner">The owner of the property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="owner">The owner of the property.</param>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate Boolean PropertyValidationDelegate<TPropertyOwner, TProperty>(TPropertyOwner owner, TProperty value);
    /// <summary>
    /// Provides a template for a callback which is called before a property is changed.
    /// </summary>
    /// <typeparam name="TPropertyOwner">The owner of the property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="owner">The owner of the property.</param>
    /// <param name="args"></param>
    public delegate void PropertyChangingDelegate<TPropertyOwner, TProperty>(TPropertyOwner owner, PropertyChangingEventArgs<TProperty> args);
    /// <summary>
    /// Provides a template for a callback which is called when a property has been changed.
    /// </summary>
    /// <typeparam name="TPropertyOwner">The owner of the property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="owner">The owner of the property.</param>
    /// <param name="args"></param>
    public delegate void PropertyChangedDelegate<TPropertyOwner, TProperty>(TPropertyOwner owner, PropertyChangedEventArgs<TProperty> args);
}
