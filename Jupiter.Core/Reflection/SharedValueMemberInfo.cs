using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Jupiter.Core.Reflection
{
    /// <summary>
    /// Provides information and access to a property or field of an object.
    /// </summary>
    public abstract class SharedValueMemberInfo : SharedMemberInfo
    {
        #region #### VARIABLES ##########################################################
        /// <summary>
        /// The default value of the member.
        /// </summary>
        Object _DefaultValue;
        #endregion
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Specifies if the member can be written.
        /// </summary>
        public Boolean CanWrite { get; }
        /// <summary>
        /// Retrieves the <see cref="SharedTypeInfo"/> for the value type.
        /// </summary>
        public SharedTypeInfo ValueType { get; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes a new instance of the <see cref="SharedValueMemberInfo"/>.
        /// </summary>
        /// <param name="reflectionManager">The <see cref="SharedReflectionManager"/> used for the reflection.</param>
        /// <param name="declaringType">The <see cref="SharedTypeInfo"/> of the declaring type.</param>
        /// <param name="member">The member which is represented.</param>
        /// <param name="valueType">The type of the value hold by the member.</param>
        /// <param name="canWrite">Specifies if the member can be written.</param>
        internal SharedValueMemberInfo(SharedReflectionManager reflectionManager, SharedTypeInfo declaringType, MemberInfo member, Type valueType, Boolean canWrite, Object defaultValue)
            : base(declaringType, member)
        {
            if (reflectionManager == null) throw new ArgumentNullException(nameof(reflectionManager));
            if (valueType == null) throw new ArgumentNullException(nameof(valueType));

            CanWrite = canWrite;
            ValueType = reflectionManager.GetInfo(valueType.GetTypeInfo());
            _DefaultValue = ((member.GetCustomAttribute<DefaultValueAttribute>() is DefaultValueAttribute defaultAttribute) ? defaultAttribute.Value : defaultValue) ??
                (ValueType.IsValueType ? Activator.CreateInstance(valueType) : null);
        }
        #endregion
        #region #### PUBLIC #############################################################
        /// <summary>
        /// Gets the value of the member for the specified instance.
        /// </summary>
        /// <param name="instance">The instance to get the value for.</param>
        /// <returns>The value of the member.</returns>
        public abstract Object GetValue(Object instance);
        /// <summary>
        /// Sets the value of the member for the specified instance.
        /// </summary>
        /// <param name="instance">The instance to set the value for.</param>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Object instance, Object value);
        /// <summary>
        /// Checks whether the value equals the default value of the member.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>True if the value equals the default value of the member; otherwise false.</returns>
        public Boolean IsDefaultValue(Object value) => Equals(value, _DefaultValue);
        /// <summary>
        /// Retrieves the current object represented as string.
        /// </summary>
        /// <returns>The current object represented as string.</returns>
        public override String ToString() => $"Name={Name} DeclaringType={DeclaringType.Type} ValueType={ValueType.Type} CanWrite={CanWrite}";
        #endregion
        #region #### PRIVATE ############################################################
        #endregion
    }
}