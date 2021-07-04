using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Jupiter.Reflection;

namespace Jupiter
{
    public partial class DependencyProperty
    {
        static readonly System.Text.RegularExpressions.Regex PropertyNameRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z_]\w*(\.[a-zA-Z_]\w*)*$");
        #region #### PUBLIC METHODS #####################################################
        public static DependencyProperty<TProperty> Create<TDeclarer, TProperty>(Expression<Func<TDeclarer, TProperty>> getter, TProperty defaultValue = default(TProperty), BindingMode bindingMode = BindingMode.Default, PropertyValidationDelegate<TDeclarer, TProperty> validation = null, PropertyChangingDelegate<TDeclarer, TProperty> propertyChanging = null, PropertyChangedDelegate<TDeclarer, TProperty> propertyChanged = null, CoerceValueDelegate<TDeclarer, TProperty> coerceValue = null)
            where TDeclarer : class, IDependencyObject
        {
            return new DependencyProperty<TProperty, TDeclarer>(TypeOf<TDeclarer>.TypeInfo, GetPropertyName<TDeclarer, TProperty>(getter), bindingMode, false, defaultValue, coerceValue, validation, propertyChanging, propertyChanged);
        }
        public static DependencyProperty<TProperty> Create<TDeclarer, TProperty>(Expression<Func<TDeclarer, TProperty>> getter, CreateValueDelegate<TDeclarer, TProperty> defaultValueFactory, BindingMode bindingMode = BindingMode.Default, PropertyValidationDelegate<TDeclarer, TProperty> validation = null, PropertyChangingDelegate<TDeclarer, TProperty> propertyChanging = null, PropertyChangedDelegate<TDeclarer, TProperty> propertyChanged = null, CoerceValueDelegate<TDeclarer, TProperty> coerceValue = null)
            where TDeclarer : class, IDependencyObject
        {
            return new DependencyProperty<TProperty, TDeclarer>(TypeOf<TDeclarer>.TypeInfo, GetPropertyName<TDeclarer, TProperty>(getter), bindingMode, false, defaultValueFactory, coerceValue, validation, propertyChanging, propertyChanged);
        }
        public static DependencyPropertyKey<TProperty> CreateReadonly<TDeclarer, TProperty>(Expression<Func<TDeclarer, TProperty>> getter, TProperty defaultValue = default(TProperty), BindingMode bindingMode = BindingMode.Default, PropertyValidationDelegate<TDeclarer, TProperty> validation = null, PropertyChangingDelegate<TDeclarer, TProperty> propertyChanging = null, PropertyChangedDelegate<TDeclarer, TProperty> propertyChanged = null, CoerceValueDelegate<TDeclarer, TProperty> coerceValue = null)
            where TDeclarer : class, IDependencyObject
        {
            new DependencyProperty<TProperty, TDeclarer>(TypeOf<TDeclarer>.TypeInfo, GetPropertyName<TDeclarer, TProperty>(getter), bindingMode, false, defaultValue, coerceValue, validation, propertyChanging, propertyChanged, out DependencyPropertyKey<TProperty> key);
            return key;
        }
        public static DependencyPropertyKey<TProperty> CreateReadonly<TDeclarer, TProperty>(Expression<Func<TDeclarer, TProperty>> getter, CreateValueDelegate<TDeclarer, TProperty> defaultValueFactory, BindingMode bindingMode = BindingMode.Default, PropertyValidationDelegate<TDeclarer, TProperty> validation = null, PropertyChangingDelegate<TDeclarer, TProperty> propertyChanging = null, PropertyChangedDelegate<TDeclarer, TProperty> propertyChanged = null, CoerceValueDelegate<TDeclarer, TProperty> coerceValue = null)
            where TDeclarer : class, IDependencyObject
        {
            new DependencyProperty<TProperty, TDeclarer>(TypeOf<TDeclarer>.TypeInfo, GetPropertyName<TDeclarer, TProperty>(getter), bindingMode, true, defaultValueFactory, coerceValue, validation, propertyChanging, propertyChanged, out DependencyPropertyKey<TProperty> key);
            return key;
        }
        public static DependencyProperty<TProperty> CreateAttachment<TDeclarer, TOwner, TProperty>(String name, TProperty defaultValue = default(TProperty), BindingMode bindingMode = BindingMode.Default, PropertyValidationDelegate<TOwner, TProperty> validation = null, PropertyChangingDelegate<TOwner, TProperty> propertyChanging = null, PropertyChangedDelegate<TOwner, TProperty> propertyChanged = null, CoerceValueDelegate<TOwner, TProperty> coerceValue = null)
            where TOwner : class, IDependencyObject
        {
            ValidatePropertyName(name);
            return new DependencyProperty<TProperty, TOwner>(TypeOf<TDeclarer>.TypeInfo, name, bindingMode, true, defaultValue, coerceValue, validation, propertyChanging, propertyChanged);
        }
        public static DependencyProperty<TProperty> CreateAttachment<TDeclarer, TOwner, TProperty>(String name, CreateValueDelegate<TOwner, TProperty> defaultValueFactory, BindingMode bindingMode = BindingMode.Default, PropertyValidationDelegate<TOwner, TProperty> validation = null, PropertyChangingDelegate<TOwner, TProperty> propertyChanging = null, PropertyChangedDelegate<TOwner, TProperty> propertyChanged = null, CoerceValueDelegate<TOwner, TProperty> coerceValue = null)
            where TOwner : class, IDependencyObject
        {
            ValidatePropertyName(name);
            return new DependencyProperty<TProperty, TOwner>(TypeOf<TDeclarer>.TypeInfo, name, bindingMode, true, defaultValueFactory, coerceValue, validation, propertyChanging, propertyChanged);
        }
        public static DependencyPropertyKey<TProperty> CreateReadonlyAttachment<TDeclarer, TOwner, TProperty>(String name, TProperty defaultValue = default(TProperty), BindingMode bindingMode = BindingMode.Default, PropertyValidationDelegate<TOwner, TProperty> validation = null, PropertyChangingDelegate<TOwner, TProperty> propertyChanging = null, PropertyChangedDelegate<TOwner, TProperty> propertyChanged = null, CoerceValueDelegate<TOwner, TProperty> coerceValue = null)
            where TOwner : class, IDependencyObject
        {
            ValidatePropertyName(name);
            new DependencyProperty<TProperty, TOwner>(TypeOf<TDeclarer>.TypeInfo, name, bindingMode, true, defaultValue, coerceValue, validation, propertyChanging, propertyChanged, out DependencyPropertyKey<TProperty> key);
            return key;
        }
        public static DependencyPropertyKey<TProperty> CreateReadonlyAttachment<TDeclarer, TOwner, TProperty>(String name, CreateValueDelegate<TOwner, TProperty> defaultValueFactory, BindingMode bindingMode = BindingMode.Default, PropertyValidationDelegate<TOwner, TProperty> validation = null, PropertyChangingDelegate<TOwner, TProperty> propertyChanging = null, PropertyChangedDelegate<TOwner, TProperty> propertyChanged = null, CoerceValueDelegate<TOwner, TProperty> coerceValue = null)
            where TOwner : class, IDependencyObject
        {
            ValidatePropertyName(name);
            new DependencyProperty<TProperty, TOwner>(TypeOf<TDeclarer>.TypeInfo, name, bindingMode, true, defaultValueFactory, coerceValue, validation, propertyChanging, propertyChanged, out DependencyPropertyKey<TProperty> key);
            return key;
        }
        #endregion
        #region #### PRIVATE METHODS ####################################################
        /// <summary>
        /// Validates the getter expression and retrieves the name of the accessed property.
        /// </summary>
        /// <typeparam name="TDeclarer">The declarer of the property.</typeparam>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="getter">The getter expression of the property.</param>
        /// <returns>The name of the property.</returns>
        static String GetPropertyName<TDeclarer, TProperty>(Expression<Func<TDeclarer, TProperty>> getter)
        {
            // Validate getter expression 
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            if (!(getter.Body is MemberExpression)) throw new ArgumentException(nameof(getter) + "." + nameof(getter.Body) + " is not a valid MemberExpression");

            // Cast body as member expression
            MemberExpression expression = (MemberExpression)getter.Body;
            // Validate member, must be a property
            if (!(expression.Member is PropertyInfo)) throw new ArgumentException("Expression must access a property");

            // Return the member name
            return expression.Member.Name;
        }
        /// <summary>
        /// Validates the name of the attachement property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        static void ValidatePropertyName(String name)
        {
            if (!PropertyNameRegex.IsMatch(name)) throw new ArgumentException(nameof(name));
        }
        #endregion
    }
}
