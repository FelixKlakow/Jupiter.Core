using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jupiter.Reflection
{
    /// <summary>
    /// Provides information about a property.
    /// </summary>
    public sealed class SharedPropertyInfo : SharedValueMemberInfo
    {
        #region #### VARIABLES ##########################################################
        readonly PropertyInfo _Property;
        #endregion
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the <see cref="Jupiter.DependencyProperty"/> of the property.
        /// </summary>
        public DependencyProperty DependencyProperty { get; }
        #endregion
        #region #### CTOR ###############################################################
        public SharedPropertyInfo(SharedReflectionManager reflectionManager, SharedTypeInfo declaringType, PropertyInfo property, DependencyProperty dependencyProperty, Object defaultValueSample)
            : base(reflectionManager, declaringType, property, dependencyProperty?.PropertyType.AsType() ?? property.PropertyType, CanWriteProperty(property, dependencyProperty), SampleDefault(property, dependencyProperty, defaultValueSample))
        {
            DependencyProperty = dependencyProperty;
            _Property = property;
        }
        #endregion
        #region #### PUBLIC #############################################################
        /// <summary>
        /// Gets the value of the member for the specified instance.
        /// </summary>
        /// <param name="instance">The instance to get the value for.</param>
        /// <returns>The value of the member.</returns>
        public override Object GetValue(Object instance)
        {
            if (DependencyProperty == null) return _Property.GetValue(instance);

            IDependencyObject dependencyObject = (IDependencyObject)instance;
            return dependencyObject.GetValueExtension(DependencyProperty) ?? dependencyObject.GetBaseValue(DependencyProperty);
        }
        /// <summary>
        /// Sets the value of the member for the specified instance.
        /// </summary>
        /// <param name="instance">The instance to set the value for.</param>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Object instance, Object value)
        {
            if (!CanWrite) throw new InvalidOperationException($"Property '{_Property.Name}' cannot be written");

            if (DependencyProperty == null) _Property.SetValue(instance, value);
            else ((IDependencyObject)instance).SetValue(DependencyProperty, value);
        }
        #endregion
        #region #### PRIVATE ############################################################
        /// <summary>
        /// Checks whether the property can be written.
        /// </summary>
        /// <param name="property">The property to check.</param>
        /// <param name="dependencyProperty">The <see cref="Jupiter.DependencyProperty"/> to check.</param>
        /// <returns>True if the property is can be written; otherwise false.</returns>
        static Boolean CanWriteProperty(PropertyInfo property, DependencyProperty dependencyProperty)
            => !(dependencyProperty?.IsReadonly ?? property.SetMethod?.IsPublic != true);
        /// <summary>
        /// Samples the default value of the property.
        /// </summary>
        /// <param name="property">The property to get the default value for.</param>
        /// <param name="dependencyProperty">The dependency property to get the value for if a dependency property exists for the property.</param>
        /// <param name="defaultValueSample">The object to sample the default value from.</param>
        /// <returns>The default value for the property.</returns>
        static Object SampleDefault(PropertyInfo property, DependencyProperty dependencyProperty, Object defaultValueSample)
        {
            try
            {
                if (dependencyProperty != null) dependencyProperty.GetPropertyDefault(null);
                return defaultValueSample == null ? null : property.GetValue(defaultValueSample);
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region #### NESTED TYPES #######################################################
        #endregion
    }
}