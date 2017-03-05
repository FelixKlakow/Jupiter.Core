using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Provides information about a single property in the dependency property system.
    /// </summary>
    sealed class DependencyProperty<TProperty, TOwner> : DependencyProperty<TProperty>
        where TOwner : IDependencyObject
    {
        #region #### VARIABLES ##########################################################
        readonly CoerceValueDelegate<TOwner, TProperty> _CoerceValueCallback;
        readonly PropertyValidationDelegate<TOwner, TProperty> _ValidationCallback;
        readonly PropertyChangingDelegate<TOwner, TProperty> _ChangingCallback;
        readonly PropertyChangedDelegate<TOwner, TProperty> _ChangedCallback;
        readonly CreateValueDelegate<TOwner, TProperty> _CreateValueCallback;
        readonly TProperty _DefaultValue;
        #endregion
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the key of the current property.
        /// </summary>
        internal override DependencyPropertyKey Key { get; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyProperty"/> class.
        /// </summary>
        /// <param name="declaringType">The type which declares the property.</param>
        /// <param name="ownerType">The type which can own this property.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="defaultBindingMode">The default <see cref="BindingMode"/> for this property.</param>
        /// <param name="isAttachement">Specifies if the property is a attachement.</param>
        /// <param name="defaultValue">The default value of the property.</param>
        /// <param name="coerceValueCallback">The callback for coercing the value.</param>
        /// <param name="validationCallback">The validation callback of the value.</param>
        /// <param name="changingCallback">The changing callback of the property.</param>
        /// <param name="changedCallback">The changed callback of the property.</param>
        internal DependencyProperty(TypeInfo declaringType, String name, BindingMode defaultBindingMode, Boolean isAttachement, TProperty defaultValue, CoerceValueDelegate<TOwner, TProperty> coerceValueCallback, PropertyValidationDelegate<TOwner, TProperty> validationCallback, PropertyChangingDelegate<TOwner, TProperty> changingCallback, PropertyChangedDelegate<TOwner, TProperty> changedCallback)
            : base(declaringType, Reflection.TypeOf<TOwner>.TypeInfo, name, defaultBindingMode, isAttachement, false)
        {
            _DefaultValue = defaultValue;
            _CoerceValueCallback = coerceValueCallback;
            _ValidationCallback = validationCallback;
            _ChangingCallback = changingCallback;
            _ChangedCallback = changedCallback;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyProperty"/> class.
        /// </summary>
        /// <param name="declaringType">The type which declares the property.</param>
        /// <param name="ownerType">The type which can own this property.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="defaultBindingMode">The default <see cref="BindingMode"/> for this property.</param>
        /// <param name="isAttachement">Specifies if the property is a attachement.</param>
        /// <param name="createValueCallback">The default value of the property.</param>
        /// <param name="coerceValueCallback">The callback for coercing the value.</param>
        /// <param name="validationCallback">The validation callback of the value.</param>
        /// <param name="changingCallback">The changing callback of the property.</param>
        /// <param name="changedCallback">The changed callback of the property.</param>
        internal DependencyProperty(TypeInfo declaringType, String name, BindingMode defaultBindingMode, Boolean isAttachement, CreateValueDelegate<TOwner, TProperty> createValueCallback, CoerceValueDelegate<TOwner, TProperty> coerceValueCallback, PropertyValidationDelegate<TOwner, TProperty> validationCallback, PropertyChangingDelegate<TOwner, TProperty> changingCallback, PropertyChangedDelegate<TOwner, TProperty> changedCallback)
            : base(declaringType, Reflection.TypeOf<TOwner>.TypeInfo, name, defaultBindingMode, isAttachement, false)
        {
            _CreateValueCallback = createValueCallback;
            _CoerceValueCallback = coerceValueCallback;
            _ValidationCallback = validationCallback;
            _ChangingCallback = changingCallback;
            _ChangedCallback = changedCallback;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyProperty"/> class.
        /// </summary>
        /// <param name="declaringType">The type which declares the property.</param>
        /// <param name="ownerType">The type which can own this property.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="defaultBindingMode">The default <see cref="BindingMode"/> for this property.</param>
        /// <param name="isAttachement">Specifies if the property is a attachement.</param>
        /// <param name="defaultValue">The default value of the property.</param>
        /// <param name="coerceValueCallback">The callback for coercing the value.</param>
        /// <param name="validationCallback">The validation callback of the value.</param>
        /// <param name="changingCallback">The changing callback of the property.</param>
        /// <param name="changedCallback">The changed callback of the property.</param>
        internal DependencyProperty(TypeInfo declaringType, String name, BindingMode defaultBindingMode, Boolean isAttachement, TProperty defaultValue, CoerceValueDelegate<TOwner, TProperty> coerceValueCallback, PropertyValidationDelegate<TOwner, TProperty> validationCallback, PropertyChangingDelegate<TOwner, TProperty> changingCallback, PropertyChangedDelegate<TOwner, TProperty> changedCallback, out DependencyPropertyKey<TProperty> key)
            : base(declaringType, Reflection.TypeOf<TOwner>.TypeInfo, name, defaultBindingMode, isAttachement, false)
        {
            _DefaultValue = defaultValue;
            _CoerceValueCallback = coerceValueCallback;
            _ValidationCallback = validationCallback;
            _ChangingCallback = changingCallback;
            _ChangedCallback = changedCallback;
            Key = key = new DependencyPropertyKey<TProperty>(this);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyProperty"/> class.
        /// </summary>
        /// <param name="declaringType">The type which declares the property.</param>
        /// <param name="ownerType">The type which can own this property.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="defaultBindingMode">The default <see cref="BindingMode"/> for this property.</param>
        /// <param name="isAttachement">Specifies if the property is a attachement.</param>
        /// <param name="createValueCallback">The default value of the property.</param>
        /// <param name="coerceValueCallback">The callback for coercing the value.</param>
        /// <param name="validationCallback">The validation callback of the value.</param>
        /// <param name="changingCallback">The changing callback of the property.</param>
        /// <param name="changedCallback">The changed callback of the property.</param>
        internal DependencyProperty(TypeInfo declaringType, String name, BindingMode defaultBindingMode, Boolean isAttachement, CreateValueDelegate<TOwner, TProperty> createValueCallback, CoerceValueDelegate<TOwner, TProperty> coerceValueCallback, PropertyValidationDelegate<TOwner, TProperty> validationCallback, PropertyChangingDelegate<TOwner, TProperty> changingCallback, PropertyChangedDelegate<TOwner, TProperty> changedCallback, out DependencyPropertyKey<TProperty> key)
            : base(declaringType, Reflection.TypeOf<TOwner>.TypeInfo, name, defaultBindingMode, isAttachement, false)
        {
            _CreateValueCallback = createValueCallback;
            _CoerceValueCallback = coerceValueCallback;
            _ValidationCallback = validationCallback;
            _ChangingCallback = changingCallback;
            _ChangedCallback = changedCallback;
            Key = key = new DependencyPropertyKey<TProperty>(this);
        }
        #endregion
        #region #### PUBLIC METHODS #####################################################
        #endregion
        #region #### PRIVATE METHODS ####################################################
        /// <summary>
        /// Retrieves the default value of the current <see cref="DependencyProperty"/>.
        /// </summary>
        /// <param name="owner">The onwer of the dependency property.</param>
        /// <returns>Retrieves the default value of the property.</returns>
        internal override TProperty GetPropertyDefault(IDependencyObject owner) => _CreateValueCallback == null ? _DefaultValue : _CreateValueCallback.Invoke((TOwner)owner);
        /// <summary>
        /// Retrieves the default value of the current <see cref="DependencyProperty"/>.
        /// </summary>
        /// <param name="owner">The onwer of the dependency property.</param>
        /// <returns>Retrieves the default value of the property.</returns>
        protected override Object GetDefault(IDependencyObject owner) => GetPropertyDefault(owner);
        /// <summary>
        /// Create a <see cref="DependencyObjectContainer.PropertyValueStorage"/> for the current property.
        /// </summary>
        /// <param name="representedObject">The object which the property should represent.</param>
        /// <returns>The <see cref="DependencyObjectContainer.PropertyValueStorage"/> for the current property.</returns>
        internal override DependencyObjectContainer.PropertyValueStorage CreateValue(IDependencyObject representedObject) => _CoerceValueCallback == null ? new DependencyObjectContainer.PropertyValueStorage<TProperty>(this, representedObject) : new DependencyObjectContainer.PropertyDualValueStorage<TProperty>(this, representedObject);
        /// <summary>
        /// Sets the value of the property to the new value.
        /// </summary>
        /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
        /// <param name="changeCount">The change count of the current property.</param>
        /// <param name="value">The current value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        internal override void SetMarkupValue(DependencyObjectContainer container, ref Int16 changeCount, ref GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>> changeHandler, ref TProperty value, TProperty newValue)
        {
            // Save previous change count, prevents issues when a ThreadAbortException is thrown
            Int16 previousChangeCount = changeCount;
            try
            {
                // Increase change count
                changeCount++;

                // Get the typed owner
                TOwner representedObject = (TOwner)container.RepresentedObject;
                // Copy old value for change events
                TProperty oldValue = value;


                // Validate the property value and throw a exception if the value is invalid
                if (_ValidationCallback?.Invoke(representedObject, newValue) == false) throw new ArgumentException($"Property value '{newValue}' is invalid for property {OwnerType.Name}.{Name}");

                // Coerce value to enable early check for a property change
                TProperty coercedValue = newValue;
                // Optional early exit if coercing is enabled and coerced and old value are the same
                if (_CoerceValueCallback == null || !Object.Equals(coercedValue = _CoerceValueCallback(representedObject, newValue), oldValue))
                {
                    // Notify that the property owner that the property is changing
                    _ChangingCallback?.Invoke(representedObject, new PropertyChangingEventArgs<TProperty>(changeCount, this, oldValue));

                    // Assign the new value to the property
                    value = coercedValue;

                    // Create a change event args once to avoid unnecessary copies
                    PropertyChangedEventArgs<TProperty> args = new PropertyChangedEventArgs<TProperty>(changeCount, this, coercedValue, oldValue);
                    // Raise the property event when existing
                    _ChangedCallback?.Invoke(representedObject, args);

                    // Only raise the 
                    if (changeCount == 1)
                    {
                        // Set the previous change count before raising the events, this prevents
                        changeCount = previousChangeCount;

                        // Raise the event on the property directly
                        changeHandler?.Invoke(representedObject, args);
                        // Raise the event on the owner object
                        container.OnPropertyChange(args);
                    }
                }
            }
            finally
            {
                // Set the previous change count
                changeCount = previousChangeCount;
            }
        }
        /// <summary>
        /// Sets the value of the property to the new value.
        /// </summary>
        /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
        /// <param name="changeCount">The change count of the current property.</param>
        /// <param name="expression">The <see cref="DependencyExpression"/> which is assigned to the property or null if no expression is assigned.</param>
        /// <param name="baseValue">The base value of the property.</param>
        /// <param name="value">The current value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        internal override void SetValue(DependencyObjectContainer container, ref Int16 changeCount, ref GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>> changeHandler, ref DependencyExpression expression, ref TProperty baseValue, ref TProperty value, TProperty newValue)
        {
            // Save previous change count, prevents issues when a ThreadAbortException is thrown
            Int16 previousChangeCount = changeCount;
            try
            {
                // Copy previous expression if set
                DependencyExpression oldExpression = expression;

                // Compare the new value with the existing base value to avoid redundant changes to the property value
                if (!Object.Equals(newValue, baseValue))
                {
                    // Increase change count
                    changeCount++;

                    // Get the typed owner
                    TOwner representedObject = (TOwner)container.RepresentedObject;
                    // Copy old value for change events
                    TProperty oldValue = value;


                    // Validate the property value and throw a exception if the value is invalid
                    if (_ValidationCallback?.Invoke(representedObject, newValue) == false) throw new ArgumentException($"Property value '{newValue}' is invalid for property {OwnerType.Name}.{Name}");

                    // We can now set the new base value anyway
                    baseValue = newValue;

                    // Coerce value to enable early check for a property change
                    TProperty coercedValue = newValue;
                    // Optional early exit if coercing is enabled and coerced and old value are the same
                    if (_CoerceValueCallback == null || !Object.Equals(coercedValue = _CoerceValueCallback(representedObject, newValue), oldValue))
                    {
                        // Notify that the property owner that the property is changing
                        _ChangingCallback?.Invoke(representedObject, new PropertyChangingEventArgs<TProperty>(changeCount, this, oldValue));

                        // Assign the new value to the property
                        value = coercedValue;

                        // Create a change event args once to avoid unnecessary copies
                        PropertyChangedEventArgs<TProperty> args = new PropertyChangedEventArgs<TProperty>(changeCount, this, coercedValue, oldValue);
                        // Raise the property event when existing
                        _ChangedCallback?.Invoke(representedObject, args);

                        // Only raise the 
                        if (changeCount == 1)
                        {
                            // Set the previous change count before raising the events, this prevents
                            changeCount = previousChangeCount;

                            // Raise the event on the property directly
                            changeHandler?.Invoke(representedObject, args);
                            // Raise the event on the owner object
                            container.OnPropertyChange(args);
                        }
                    }
                }

                // It does not matter if the value is different, the expression must be removed in case a value is set
                if (oldExpression != null && expression == oldExpression)
                {
                    // Set expression to null
                    expression = null;
                    // Raise the markup change event
                    container.OnExtensionChange(new PropertyExtensionChangedEventArgs(this, null, oldExpression.ExpressionTemplate));
                }
            }
            finally
            {
                // Set the previous change count
                changeCount = previousChangeCount;
            }
        }
        /// <summary>
        /// Coerces the current value of the dependency object.
        /// </summary>
        /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
        /// <param name="changeCount">The change count of the current property.</param>
        /// <param name="expression">The <see cref="DependencyExpression"/> which is assigned to the property or null if no expression is assigned.</param>
        /// <param name="baseValue">The base value of the property.</param>
        /// <param name="value">The current value of the property.</param>
        internal override void CoerceValue(DependencyObjectContainer container, ref Int16 changeCount, ref GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>> changeHandler, DependencyExpression expression, TProperty baseValue, ref TProperty value)
        {
            // Save previous change count, prevents issues when a ThreadAbortException is thrown
            Int16 previousChangeCount = changeCount;
            try
            {
                // Get the typed owner
                TOwner representedObject = (TOwner)container.RepresentedObject;

                // Store previous coerced value
                TProperty oldValue = value;

                // Coerce value to enable early check for a property change
                TProperty coercedValue;

                // Method should only work when we've something to coerce
                if (_CoerceValueCallback != null && !Object.Equals(coercedValue = _CoerceValueCallback(representedObject, baseValue), oldValue))
                {
                    // Increase change count
                    changeCount++;


                    // Notify that the property owner that the property is changing
                    _ChangingCallback?.Invoke(representedObject, new PropertyChangingEventArgs<TProperty>(changeCount, this, oldValue));

                    // Assign the new value to the property
                    value = coercedValue;

                    // Create a change event args once to avoid unnecessary copies
                    PropertyChangedEventArgs<TProperty> args = new PropertyChangedEventArgs<TProperty>(changeCount, this, coercedValue, oldValue);
                    // Raise the property event when existing
                    _ChangedCallback?.Invoke(representedObject, args);

                    // Only raise the 
                    if (changeCount == 1)
                    {
                        // Set the previous change count before raising the events, this prevents
                        changeCount = previousChangeCount;

                        // Raise the event on the property directly
                        changeHandler?.Invoke(representedObject, args);
                        // Raise the event on the owner object
                        container.OnPropertyChange(args);
                    }
                }
            }
            finally
            {
                // Set the previous change count
                changeCount = previousChangeCount;
            }
        }
        #endregion
        #region #### NESTED TYPES #######################################################
        #endregion
    }
}
