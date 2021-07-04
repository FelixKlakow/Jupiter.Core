using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Jupiter.Reflection;

namespace Jupiter
{
    /// <summary>
    /// Provides a implementation of the <see cref="IDependencyObject"/> interface but does not expose the <see cref="IDependencyObject"/> interface.
    /// </summary>
    /// <remarks>The <see cref="DependencyObjectContainer"/> class can be used as base class for dependency objects or as container for objects which implement the <see cref="IDependencyObject"/> interface.</remarks>
    public class DependencyObjectContainer
    {
        #region #### VARIABLES ##########################################################
        // TODO : Dictionary should be replaced later by a faster mechanism
        readonly Dictionary<DependencyProperty, PropertyValueStorage> _Values = new Dictionary<DependencyProperty, PropertyValueStorage>();
        readonly DependencyType _DependencyType;
        #endregion
        #region #### EVENTS #############################################################
        /// <summary>
        /// Occurs when any <see cref="DependencyProperty"/> has been changed.
        /// </summary>
        public event GenericEventHandler<IDependencyObject, PropertyChangedEventArgs> PropertyChanged;
        /// <summary>
        /// Occurs when the <see cref="DependencyExtension"/> of any <see cref="DependencyProperty"/> has been changed.
        /// </summary>
        public event GenericEventHandler<IDependencyObject, PropertyExtensionChangedEventArgs> PropertyExtensionChanged;
        #endregion
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves a enumeration with <see cref="DependencyProperty"/> objects which has been attached to the current object.
        /// </summary>
        public IEnumerable<DependencyProperty> AttachedProperties => _Values.Keys.Where(p => p.IsAttachement).ToArray();
        /// <summary>
        /// Retrieves the object whichs dependency proeprties should be represented.
        /// </summary>
        internal IDependencyObject RepresentedObject { get; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyObjectContainer"/> class.
        /// </summary>
        internal DependencyObjectContainer()
        {
            RepresentedObject = (this as IDependencyObject) ?? throw new NotSupportedException("Object must inherit from IDependencyObject");
            _DependencyType = DependencyType.GetDependencyType(RepresentedObject.GetType().GetTypeInfo(), true);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyObjectContainer"/> class.
        /// </summary>
        /// <param name="representedObject">The <see cref="IDependencyObject"/> type whichs dependency propertys should be represented.</param>
        public DependencyObjectContainer(IDependencyObject representedObject)
        {
            RepresentedObject = representedObject ?? throw new ArgumentNullException(nameof(representedObject));
            _DependencyType = DependencyType.GetDependencyType(RepresentedObject.GetType().GetTypeInfo(), true);
        }
        #endregion
        #region #### METHODS ############################################################
        /// <summary>
        /// Checks whether the access to the property is allowed for the state of the current object.
        /// </summary>
        /// <param name="property">The property to check.</param>
        /// <param name="exception">A custom exception which should be thrown instead of the default exception.</param>
        /// <returns>True if access is allowed; otherwise false.</returns>
        protected virtual Boolean CheckPropertyAccess(DependencyProperty property, out Exception exception)
        {
            exception = null;
            return true;
        }
        /// <summary>
        /// Gets the storage for the specified property.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> to get the storage for.</param>
        /// <param name="create">Determines if the storage should be created when not found.</param>
        /// <param name="checkAccess">Determines if the property should be checked for access before getting the storage.</param>
        /// <returns>The <see cref="PropertyValueStorage"/> for the specified property; null when no storage could be found and should not be created.</returns>
        PropertyValueStorage GetStorage(DependencyProperty property, Boolean create, Boolean checkAccess)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            // Check if the proeprty is accessible without a key
            if (checkAccess && property.IsReadonly) throw new ArgumentException("Property is readonly and a key is required for write access");

            return GetStorage(property, create);
        }
        /// <summary>
        /// Gets the storage for the specified property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The <see cref="DependencyProperty"/> to get the storage for.</param>
        /// <param name="create">Determines if the storage should be created when not found.</param>
        /// <param name="checkAccess">Determines if the property should be checked for access before getting the storage.</param>
        /// <returns>The <see cref="PropertyValueStorage"/> for the specified property; null when no storage could be found and should not be created.</returns>
        PropertyValueStorage<TProperty> GetStorage<TProperty>(DependencyProperty<TProperty> property, Boolean create, Boolean checkAccess)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            // Check if the proeprty is accessible without a key
            if (checkAccess && property.IsReadonly) throw new ArgumentException("Property is readonly and a key is required for write access");

            return (PropertyValueStorage<TProperty>)GetStorage(property, create);
        }
        /// <summary>
        /// Get the storage for the specified property.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> to get the storage for.</param>
        /// <param name="create">Determines if the storage should be created when not found.</param>
        /// <returns>The <see cref="PropertyValueStorage"/> for the specified property; null when no storage could be found and should not be created.</returns>
        PropertyValueStorage GetStorage(DependencyProperty property, Boolean create)
        {
            // Check if access is allowed
            if (!CheckPropertyAccess(property, out Exception customException)) throw (customException ?? new InvalidOperationException("Access forbidden"));

#if DEBUG
            // In debug mode we check if the property is a valid for the current object
            if (!IsValidProperty(property)) throw new InvalidOperationException($"Property '{property}' is not valid for the current object");

#endif
            if (!_Values.TryGetValue(property, out PropertyValueStorage result) && create)
            {
                // For performance reasons we check in release mode only if the property is valid when we create a new storage
                if (!IsValidProperty(property)) throw new InvalidOperationException($"Property '{property}' is not valid for the current object");

                // Create a new value for the property
                result = property.CreateValue(RepresentedObject);
                // Add the value to the dictionary
                _Values.Add(property, result);
            }

            return result;
        }
        /// <summary>
        /// Tests whether the property is valid for the current <see cref="DependencyObjectContainer"/>.
        /// </summary>
        /// <param name="property">The property to test.</param>
        /// <returns>True if the property is valid; otherwise false.</returns>
        Boolean IsValidProperty(DependencyProperty property) => property.IsAttachement || _DependencyType.IsSubclassOf(property.DependencyType);
        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="PropertyChangedEventArgs"/> which should be used for the event.</param>
        internal void OnPropertyChange(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(RepresentedObject, args);
        /// <summary>
        /// Raises the <see cref="PropertyExtensionChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="PropertyChangedEventArgs"/> which should be used for the event.</param>
        internal void OnExtensionChange(PropertyExtensionChangedEventArgs args) => PropertyExtensionChanged?.Invoke(RepresentedObject, args);
        #endregion
        #region #### ADD CHANGE METHODS #################################################
        /// <summary>
        /// Adds a change handler for the specified <see cref="DependencyProperty"/>.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The <see cref="DependencyProperty"/> to add the change handler to.</param>
        /// <param name="handler">The handler for the change event.</param>
        public void AddChangeHandler<TProperty>(DependencyProperty<TProperty> property, GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            GetStorage<TProperty>(property, true, false).AddChangeHandler(handler);
        }
        /// <summary>
        /// Removes a change handler from the specified <see cref="DependencyProperty"/>.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The <see cref="DependencyProperty"/> to remove the change handler from.</param>
        /// <param name="handler">The handler of the change event which should be removed.</param>
        public void RemoveChangeHandler<TProperty>(DependencyProperty<TProperty> property, GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            GetStorage<TProperty>(property, false, false)?.RemoveChangeHandler(handler);
        }
        /// <summary>
        /// Adds a change handler for the specified <see cref="DependencyProperty"/>.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> to add the change handler to.</param>
        /// <param name="handler">The handler for the change event.</param>
        public void AddChangeHandler(DependencyProperty property, GenericEventHandler<IDependencyObject, PropertyChangedEventArgs> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            GetStorage(property, true, false).AddChangeHandler(handler);
        }
        /// <summary>
        /// Removes a change handler from the specified <see cref="DependencyProperty"/>.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> to remove the change handler from.</param>
        /// <param name="handler">The handler of the change event which should be removed.</param>
        public void RemoveChangeHandler(DependencyProperty property, GenericEventHandler<IDependencyObject, PropertyChangedEventArgs> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            GetStorage(property, false, false)?.RemoveChangeHandler(handler);
        }
        #endregion
        #region #### MARKUP METHODS #####################################################
        /// <summary>
        /// Set the value of the target property to a value provided by a <see cref="DependencyExpression"/> which is generated by the <see cref="DependencyExtension"/>.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> whichs value should be set.</param>
        /// <param name="extension">The <see cref="DependencyExtension"/> which provides provides the <see cref="DependencyExpression"/>.</param>
        public void SetValueExtension(DependencyProperty property, DependencyExtension extension) => GetStorage(property, extension != null, true)?.SetExtension(this, extension);
        /// <summary>
        /// Set the value of the target property to a value provided by a <see cref="DependencyExpression"/> which is generated by the <see cref="DependencyExtension"/>.
        /// </summary>
        /// <param name="propertyKey">The <see cref="DependencyPropertyKey"/> of the <see cref="DependencyProperty"/> whichs value should be set.</param>
        /// <param name="extension">The <see cref="DependencyExtension"/> which provides provides the <see cref="DependencyExpression"/>.</param>
        public void SetValueExtension(DependencyPropertyKey propertyKey, DependencyExtension extension)
        {
            if (propertyKey == null) throw new ArgumentNullException(nameof(propertyKey));

            GetStorage(propertyKey.Property, extension != null, false).SetExtensionOverride(this, extension);
        }
        /// <summary>
        /// Set the value of the target property to a value provided by a <see cref="DependencyExpression"/> which is generated by the <see cref="DependencyExtension"/> which overrides the current extension.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> whichs value should be set.</param>
        /// <param name="extension">The <see cref="DependencyExtension"/> which provides provides the <see cref="DependencyExpression"/>.</param>
        public void SetValueExtensionOverride(DependencyProperty property, DependencyExtension extension) => GetStorage(property, extension != null, true)?.SetExtensionOverride(this, extension);
        /// <summary>
        /// Set the value of the target property to a value provided by a <see cref="DependencyExpression"/> which is generated by the <see cref="DependencyExtension"/> which overrides the current extension.
        /// </summary>
        /// <param name="propertyKey">The <see cref="DependencyPropertyKey"/> of the <see cref="DependencyProperty"/> whichs value should be set.</param>
        /// <param name="extension">The <see cref="DependencyExtension"/> which provides provides the <see cref="DependencyExpression"/>.</param>
        public void SetValueExtensionOverride(DependencyPropertyKey propertyKey, DependencyExtension extension)
        {
            if (propertyKey == null) throw new ArgumentNullException(nameof(propertyKey));

            GetStorage(propertyKey.Property, extension != null, false)?.SetExtensionOverride(this, extension);
        }
        /// <summary>
        /// Gets the extension value of the specified <see cref="DependencyProperty"/>.
        /// </summary>
        /// <param name="property">The property to get the extension for.</param>
        /// <returns>The <see cref="DependencyExtension"/> which is assigned to the property; If no <see cref="DependencyExtension"/> is assigned it returns null.</returns>
        public DependencyExtension GetValueExtension(DependencyProperty property) => GetStorage(property, false, false)?.Expression?.ExpressionTemplate;
        /// <summary>
        /// Gets the <see cref="DependencyExpression"/> which has been created by the assigned <see cref="DependencyExtension"/>.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> for which the <see cref="DependencyExpression"/> should be retrieved.</param>
        /// <returns>The <see cref="DependencyExpression"/> of the property; or null if no <see cref="DependencyExtension"/> has been assigned.</returns>
        public DependencyExpression GetValueExpression(DependencyProperty property) => GetStorage(property, false, false)?.Expression;
        /// <summary>
        /// Gets the extension override value of the specified <see cref="DependencyProperty"/>.
        /// </summary>
        /// <param name="property">The property to get the extension for.</param>
        /// <returns>The <see cref="DependencyExtension"/> which is assigned to the property; If no <see cref="DependencyExtension"/> is assigned it returns null.</returns>
        public DependencyExtension GetValueExtensionOverride(DependencyProperty property) => GetStorage(property, false, false)?.OverrideExpression?.ExpressionTemplate;
        /// <summary>
        /// Gets the <see cref="DependencyExpression"/> which has been created by the assigned <see cref="DependencyExtension"/> which is overriding the value.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> for which the <see cref="DependencyExpression"/> should be retrieved.</param>
        /// <returns>The <see cref="DependencyExpression"/> of the property; or null if no <see cref="DependencyExtension"/> has been assigned.</returns>
        public DependencyExpression GetValueExpressionOverride(DependencyProperty property) => GetStorage(property, false, false)?.OverrideExpression;
        #endregion
        #region #### PROPERTY METHODS ###################################################
        /// <summary>
        /// Coerces the value of the specified property.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> to coerce.</param>
        public void CoerceValue(DependencyProperty property)
        {
            PropertyValueStorage storage = GetStorage(property, true, false);

            // In any case we need to coerce the property value
            storage.CoerceValue(this);
            // Check if we can remove the storage again
            if (storage.IsRemoveable &&
                Equals(storage.BaseValue, property.GetPropertyDefault(RepresentedObject)) && Equals(storage.Value, storage.BaseValue) &&
                _Values.TryGetValue(property, out PropertyValueStorage currentStorage) && currentStorage == storage)
            {
                // Remove the property again
                _Values.Remove(property);
            }
        }
        /// <summary>
        /// Clears the value of the property to the default value.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> to clear.</param>
        public void ClearValue(DependencyProperty property)
        {
            PropertyValueStorage storage = GetStorage(property, false, true);
            if (storage != null)
            {
                // Get the default value of the property
                Object defaultValue = property.GetPropertyDefault(RepresentedObject);
                // Set the default value to trigger events
                storage.SetValue(this, defaultValue);

                // Check if the storage is still existing and can be removed ( to keep the object small )
                // TODO : This could be much faster when using something else than a dictionary
                if (storage.IsRemoveable && Equals(storage.BaseValue, defaultValue) && Equals(storage.Value, storage.BaseValue) &&
                    _Values.TryGetValue(property, out PropertyValueStorage currentStorage) && currentStorage == storage)
                {
                    // Try to remove the property from the container
                    _Values.Remove(property);
                }
            }
        }
        /// <summary>
        /// Clears the value of the property to the default value.
        /// </summary>
        /// <param name="propertyKey">The <see cref="DependencyPropertyKey"/> of the <see cref="DependencyProperty"/> which should be cleared.</param>
        public void ClearValue(DependencyPropertyKey propertyKey)
        {
            if (propertyKey == null) throw new ArgumentNullException(nameof(propertyKey));

            DependencyProperty property = propertyKey.Property;
            PropertyValueStorage storage = GetStorage(property, false, false);
            if (storage != null)
            {
                // Get the default value of the property
                Object defaultValue = property.GetPropertyDefault(RepresentedObject);
                // Set the default value to trigger events
                storage.SetValue(this, defaultValue);

                // Check if the storage is still existing and can be removed ( to keep the object small )
                // TODO : This could be much faster when using something else than a dictionary
                if (storage.IsRemoveable && storage.BaseValue == defaultValue &&
                    _Values.TryGetValue(property, out PropertyValueStorage currentStorage) && currentStorage == storage)
                {
                    // Try to remove the property from the container
                    _Values.Remove(property);
                }
            }
        }
        /// <summary>
        /// Sets the value of the specified property.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> whichs value should be set.</param>
        /// <param name="value">The new value of the property.</param>
        public void SetValue(DependencyProperty property, Object value)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            GetStorage(property, true, true).SetValue(this, value);
        }
        /// <summary>
        /// Sets the value of the specified property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The <see cref="DependencyProperty{TPropertyType}"/> whichs value should be set.</param>
        /// <param name="value">The new value of the property.</param>
        public void SetValue<TProperty>(DependencyProperty<TProperty> property, TProperty value)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            GetStorage<TProperty>(property, true, true).SetValue(this, value);
        }
        /// <summary>
        /// Sets the value of the specified property.
        /// </summary>
        /// <param name="propertyKey">The <see cref="DependencyPropertyKey"/> of the <see cref="DependencyProperty"/> whichs value should be set.</param>
        /// <param name="value">The new value of the property.</param>
        public void SetValue(DependencyPropertyKey propertyKey, Object value)
        {
            if (propertyKey == null) throw new ArgumentNullException(nameof(propertyKey));

            GetStorage(propertyKey.Property, true, false).SetValue(this, value);
        }
        /// <summary>
        /// Sets the value of the specified property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyKey">The <see cref="DependencyPropertyKey{TPropertyType}"/> of the <see cref="DependencyProperty"/>  whichs value should be set.</param>
        /// <param name="value">The new value of the property.</param>
        public void SetValue<TProperty>(DependencyPropertyKey<TProperty> propertyKey, TProperty value)
        {
            if (propertyKey == null) throw new ArgumentNullException(nameof(propertyKey));

            GetStorage(propertyKey.Property, true, false).SetValue(this, value);
        }
        /// <summary>
        /// Gets the value of the dependency property.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> to get the value for.</param>
        /// <returns>The value of the property.</returns>
        public Object GetValue(DependencyProperty property)
        {
            PropertyValueStorage storage = GetStorage(property, false, false);
            if (storage == null)
            {
                if (property.IsUsingValueFactory)
                {
                    storage = GetStorage(property, true, false);
                }
                else return property.GetPropertyDefault(RepresentedObject);
            }
            return storage.Value;
        }
        /// <summary>
        /// Gets the value of the dependency property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The <see cref="DependencyProperty{TPropertyType}"/> to get the value for.</param>
        /// <returns>The value of the property.</returns>
        public TProperty GetValue<TProperty>(DependencyProperty<TProperty> property)
        {
            PropertyValueStorage<TProperty> storage = GetStorage(property, false, false);
            if (storage == null)
            {
                if (property.IsUsingValueFactory)
                {
                    storage = GetStorage(property, true, false);
                }
                else return property.GetPropertyDefault(null);
            }
            return storage.Value;
        }
        /// <summary>
        /// Gets the base value of the specified property.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> to get the base value for.</param>
        /// <returns>The base value of the property.</returns>
        public Object GetBaseValue(DependencyProperty property)
        {
            PropertyValueStorage storage = GetStorage(property, false, false);
            if (storage == null)
            {
                if (property.IsUsingValueFactory)
                {
                    storage = GetStorage(property, true, false);
                }
                else return property.GetPropertyDefault(null);
            }
            return storage.BaseValue;
        }
        /// <summary>
        /// Gets the base value of the specified property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The <see cref="DependencyProperty{TPropertyType}"/> to get the base value for.</param>
        /// <returns>The base value of the property.</returns>
        public TProperty GetBaseValue<TProperty>(DependencyProperty<TProperty> property)
        {
            PropertyValueStorage<TProperty> storage = GetStorage(property, false, false);
            if (storage == null)
            {
                if (property.IsUsingValueFactory)
                {
                    storage = GetStorage(property, true, false);
                }
                else return property.GetPropertyDefault(null);
            }
            return storage.BaseValue;
        }
        #endregion
        #region #### NESTED TYPES #######################################################
        /// <summary>
        /// Represents the base class for property values which are stored in this class.
        /// </summary>
        internal abstract class PropertyValueStorage
        {
            #region #### VARIABLES ##########################################################
            protected Int16 _ChangeCount;
            #endregion
            #region #### PROPERTIES #########################################################
            /// <summary>
            /// Specifies if the property value can be removed.
            /// </summary>
            public abstract Boolean IsRemoveable { get; }
            /// <summary>
            /// Retrieves the current value of the represented <see cref="DependencyProperty"/>.
            /// </summary>
            public Object Value => GetValue();
            /// <summary>
            /// Retrieves the current base value of the represented <see cref="DependencyProperty"/>.
            /// </summary>
            public Object BaseValue => GetBaseValue();
            /// <summary>
            /// Retrieves the current expression of the represented <see cref="DependencyProperty"/>.
            /// </summary>
            public DependencyExpression Expression => GetExpression();
            /// <summary>
            /// Retrieves the current override expression of the represented <see cref="DependencyProperty"/>.
            /// </summary>
            public DependencyExpression OverrideExpression => GetOverrideExpression();
            #endregion
            #region #### PRIVATE METHODS ####################################################
            /// <summary>
            /// Gets the value of the represented property.
            /// </summary>
            /// <returns>Returns the value of the current property.</returns>
            protected abstract Object GetValue();
            /// <summary>
            /// Gets the base value of the represented property.
            /// </summary>
            /// <returns>Returns the base value of the current property.</returns>
            protected abstract Object GetBaseValue();
            /// <summary>
            /// Gets the expression of the represented property.
            /// </summary>
            /// <returns>Returns the expression of the current property.</returns>
            protected abstract DependencyExpression GetExpression();
            /// <summary>
            /// Gets the override expression of the represented property.
            /// </summary>
            /// <returns>Returns the override expression of the current property.</returns>
            protected abstract DependencyExpression GetOverrideExpression();
            #endregion
            #region #### PUBLIC METHODS #####################################################
            /// <summary>
            /// Sets the markup value of the property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            /// <param name="value">The value to set.</param>
            internal abstract void SetMarkupValue(DependencyObjectContainer container, Object value);
            /// <summary>
            /// Set the <see cref="DependencyExtension"/> of the current property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            /// <param name="extension">The extension which should be set.</param>
            public abstract void SetExtension(DependencyObjectContainer container, DependencyExtension extension);
            /// <summary>
            /// Set the <see cref="DependencyExtension"/> override of the current property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            /// <param name="extension">The extension which should be set.</param>
            public abstract void SetExtensionOverride(DependencyObjectContainer container, DependencyExtension extension);
            /// <summary>
            /// Coerces the value of the property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            public abstract void CoerceValue(DependencyObjectContainer container);
            /// <summary>
            /// Sets the value of the property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            /// <param name="value">The value to set.</param>
            public abstract void SetValue(DependencyObjectContainer container, Object value);
            /// <summary>
            /// Adds a event handler to the represented <see cref="DependencyProperty"/>.
            /// </summary>
            /// <param name="handler">The event handler which should be added.</param>
            public abstract void AddChangeHandler(GenericEventHandler<IDependencyObject, PropertyChangedEventArgs> handler);
            /// <summary>
            /// Removes a event handler from the represented <see cref="DependencyProperty"/>.
            /// </summary>
            /// <param name="handler">The event handler which should be removed.</param>
            public abstract void RemoveChangeHandler(GenericEventHandler<IDependencyObject, PropertyChangedEventArgs> handler);
            #endregion
        }
        /// <summary>
        /// Represents a property value which has no explicit base value and is used for properties which do not coerce.
        /// </summary>
        internal class PropertyValueStorage<TProperty> : PropertyValueStorage
        {
            #region #### VARIABLES ##########################################################
            protected readonly DependencyProperty<TProperty> _Property;
            protected TProperty _Value;
            protected DependencyExpression _Expression;
            protected GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>> _ChangeHandler;
            #endregion
            #region #### PROPERTIES #########################################################
            /// <summary>
            /// Specifies if the property value can be removed.
            /// </summary>
            public override Boolean IsRemoveable => _ChangeHandler == null;
            /// <summary>
            /// Retrieves the current value of the represented <see cref="DependencyProperty"/>.
            /// </summary>
            public new TProperty Value => _Value;
            /// <summary>
            /// Retrieves the current base value of the represented <see cref="DependencyProperty"/>.
            /// </summary>
            public new virtual TProperty BaseValue => _Value;
            #endregion
            #region #### CTOR ###############################################################
            public PropertyValueStorage(DependencyProperty<TProperty> property, IDependencyObject owner)
            {
                _Property = property;
                _Value = property.GetPropertyDefault(owner);
            }
            #endregion
            #region #### PRIVATE METHODS ####################################################
            /// <summary>
            /// Gets the value of the represented property.
            /// </summary>
            /// <returns>Returns the value of the current property.</returns>
            protected override Object GetValue() => _Value;
            /// <summary>
            /// Gets the base value of the represented property.
            /// </summary>
            /// <returns>Returns the base value of the current property.</returns>
            protected override Object GetBaseValue() => _Value;
            /// <summary>
            /// Gets the expression of the represented property.
            /// </summary>
            /// <returns>Returns the expression of the current property.</returns>
            protected override DependencyExpression GetExpression() => DependencyExpression.IsEmpty(_Expression) ? null : _Expression;
            /// <summary>
            /// Gets the override expression of the represented property.
            /// </summary>
            /// <returns>Returns the override expression of the current property.</returns>
            protected override DependencyExpression GetOverrideExpression() => _Expression?.OverrideExpression;
            #endregion
            #region #### PUBLIC METHODS #####################################################
            /// <summary>
            /// Sets the markup value of the property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            /// <param name="value">The value to set.</param>
            internal override void SetMarkupValue(DependencyObjectContainer container, Object value) => _Property.SetMarkupValue(container, ref _ChangeCount, ref _ChangeHandler, ref _Value, (TProperty)value);
            /// <summary>
            /// Set the <see cref="DependencyExtension"/> of the current property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            /// <param name="extension">The extension which should be set.</param>
            public override void SetExtension(DependencyObjectContainer container, DependencyExtension extension)
            {
                DependencyExpression oldExpression = _Expression;
                DependencyExpression overrideExpression = oldExpression?.OverrideExpression;

                if (oldExpression?.ExpressionTemplate != extension)
                {
                    try
                    {
                        // Releases the expression which was previous bound to the property.
                        _Expression?.Release();
                        _Expression = null;

                        // Create the expression for the markup
                        _Expression = extension?.CreateExpression(_Property);
                        _Expression?.Initialize(container, this);
                    }
                    catch (Exception)
                    {
                        _Expression = null;

                        if (oldExpression != null && !DependencyExpression.IsEmpty(oldExpression))
                            // We still need to inform the user that the markup changed
                            container.OnExtensionChange(new PropertyExtensionChangedEventArgs(_Property, null, oldExpression?.ExpressionTemplate));
                        throw;
                    }


                    // If override expression is not null we need to reassign the override expression
                    if (overrideExpression != null)
                    {
                        if (_Expression == null)
                        {
                            _Expression = DependencyExpression.CreateEmpty(_Property);
                            _Expression.Initialize(container, this);
                        }

                        _Expression.OverrideExpression = overrideExpression;
                    }

                    // Check if the old expression is a empty expression, if so return null
                    if (DependencyExpression.IsEmpty(oldExpression)) oldExpression = null;

                    if (!DependencyExpression.IsEmpty(oldExpression) || _Expression != null)
                        // No exception thrown still make the update
                        container.OnExtensionChange(new PropertyExtensionChangedEventArgs(_Property, _Expression?.ExpressionTemplate, oldExpression?.ExpressionTemplate));

                    // Coerces the value of the existing base value
                    if (_Expression == null) CoerceValue(container);
                }
            }
            /// <summary>
            /// Set the <see cref="DependencyExtension"/> override of the current property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            /// <param name="extension">The extension which should be set.</param>
            public override void SetExtensionOverride(DependencyObjectContainer container, DependencyExtension extension)
            {
                // Check if expression is null, if so we've to set a temporary override value
                if (_Expression == null)
                {
                    if (extension == null) return;

                    _Expression = DependencyExpression.CreateEmpty(_Property);
                    _Expression.Initialize(container, this);
                }
                else
                {
                    _Expression.OverrideExpression?.Release();
                    _Expression.OverrideExpression = null;

                    if (extension == null)
                    {
                        CoerceValue(container);
                        return;
                    }
                }



                // Create a expression for the overriding extension
                DependencyExpression expression = extension.CreateExpression(_Property);
                expression.Initialize(container, this);
                // Set a override expression
                _Expression.OverrideExpression = expression;
            }
            /// <summary>
            /// Coerces the value of the property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            public override void CoerceValue(DependencyObjectContainer container) { }
            /// <summary>
            /// Sets the value of the property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            /// <param name="value">The value to set.</param>
            public override sealed void SetValue(DependencyObjectContainer container, Object value) => SetValue(container, (TProperty)value);
            /// <summary>
            /// Sets the value of the property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            /// <param name="value">The value to set.</param>
            public virtual void SetValue(DependencyObjectContainer container, TProperty value) => _Property.SetValue(container, ref _ChangeCount, ref _ChangeHandler, ref _Expression, ref _Value, ref _Value, value);
            /// <summary>
            /// Adds a event handler to the represented <see cref="DependencyProperty"/>.
            /// </summary>
            /// <param name="handler">The event handler which should be added.</param>
            public override void AddChangeHandler(GenericEventHandler<IDependencyObject, PropertyChangedEventArgs> handler) => AddChangeHandler(ConvertDelegate<GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>>>(handler));
            /// <summary>
            /// Removes a event handler from the represented <see cref="DependencyProperty"/>.
            /// </summary>
            /// <param name="handler">The event handler which should be removed.</param>
            public override void RemoveChangeHandler(GenericEventHandler<IDependencyObject, PropertyChangedEventArgs> handler) => RemoveChangeHandler(ConvertDelegate<GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>>>(handler));
            /// <summary>
            /// Adds a event handler to the represented <see cref="DependencyProperty"/>.
            /// </summary>
            /// <param name="handler">The event handler which should be added.</param>
            public void AddChangeHandler(GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>> handler) => _ChangeHandler += handler;
            /// <summary>
            /// Removes a event handler from the represented <see cref="DependencyProperty"/>.
            /// </summary>
            /// <param name="handler">The event handler which should be removed.</param>
            public void RemoveChangeHandler(GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<TProperty>> handler) => _ChangeHandler -= handler;
            #endregion
            #region #### PRIVATE METHODS ####################################################
            /// <summary>
            /// Converts a delegate into another delegate when possible.
            /// </summary>
            /// <typeparam name="TDelegate">The delegate type to convert the delegate to.</typeparam>
            /// <param name="source">The source delegate to convert.</param>
            /// <returns>The converted delegate.</returns>
            static TDelegate ConvertDelegate<TDelegate>(Delegate source) => (TDelegate)(Object)source.GetMethodInfo().CreateDelegate(TypeOf<TDelegate>.Type, source.Target);
            #endregion
        }
        /// <summary>
        /// Represents a property value which has a explicit base value and is used for properties which coerce the base value.
        /// </summary>
        internal sealed class PropertyDualValueStorage<TProperty> : PropertyValueStorage<TProperty>
        {
            #region #### VARIABLES ##########################################################
            TProperty _BaseValue;
            #endregion
            #region #### PROPERTIES #########################################################
            /// <summary>
            /// Retrieves the current base value of the represented <see cref="DependencyProperty"/>.
            /// </summary>
            public override TProperty BaseValue => _BaseValue;
            #endregion
            #region #### CTOR ###############################################################
            public PropertyDualValueStorage(DependencyProperty<TProperty> property, IDependencyObject owner)
                : base(property, owner) => _BaseValue = _Value;
            #endregion
            #region #### PUBLIC METHODS #####################################################
            /// <summary>
            /// Coerces the value of the property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            public override void CoerceValue(DependencyObjectContainer container) => _Property.CoerceValue(container, ref _ChangeCount, ref _ChangeHandler, _Expression, _BaseValue, ref _Value);
            /// <summary>
            /// Sets the value of the property.
            /// </summary>
            /// <param name="container">The container of the <see cref="DependencyProperty"/>.</param>
            /// <param name="value">The value to set.</param>
            public override void SetValue(DependencyObjectContainer container, TProperty value) => _Property.SetValue(container, ref _ChangeCount, ref _ChangeHandler, ref _Expression, ref _BaseValue, ref _Value, value);
            #endregion
            #region #### PRIVATE METHODS ####################################################
            /// <summary>
            /// Gets the base value of the represented property.
            /// </summary>
            /// <returns>Returns the base value of the current property.</returns>
            protected override Object GetBaseValue() => _BaseValue;
            /// <summary>
            /// Converts a delegate into another delegate when possible.
            /// </summary>
            /// <typeparam name="TDelegate">The delegate type to convert the delegate to.</typeparam>
            /// <param name="source">The source delegate to convert.</param>
            /// <returns>The converted delegate.</returns>
            static TDelegate ConvertDelegate<TDelegate>(Delegate source) => (TDelegate)(Object)source.GetMethodInfo().CreateDelegate(TypeOf<TDelegate>.Type, source.Target);
            #endregion
        }
        #endregion
    }
}
