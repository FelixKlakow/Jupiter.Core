using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Provides information about a single property in the dependency property system.
    /// </summary>
    public abstract partial class DependencyProperty : IEquatable<DependencyProperty>
    {
        #region #### VARIABLES ##########################################################
        readonly Int32 _HashCode;
        #endregion
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the name of the <see cref="DependencyProperty"/>.
        /// </summary>
        /// <remarks>The name of the <see cref="DependencyProperty"/> is unique on the same hierarchy level for non-attachement properties.</remarks>
        public String Name { get; }
        /// <summary>
        /// Retrieves the default <see cref="BindingMode"/> which is used by a <see cref="Binding"/> to determine how a property should be bound if unspecified.
        /// </summary>
        public BindingMode DefaultBindingMode { get; }
        /// <summary>
        /// Retrieves if the property is a attachement to a existing type.
        /// </summary>
        public Boolean IsAttachement { get; }
        /// <summary>
        /// Retrieves if the property is readonly and can only be written to when having a <see cref="DependencyPropertyKey"/>.
        /// </summary>
        public Boolean IsReadonly { get { return Key != null; } }
        /// <summary>
        /// Retrieves the type in which the <see cref="DependencyProperty"/> is declared in.
        /// </summary>
        public TypeInfo DeclaringType { get; }
        /// <summary>
        /// Retrieves the type of the property.
        /// </summary>
        public TypeInfo PropertyType { get; }
        /// <summary>
        /// Retrieves the type which can own this <see cref="DependencyProperty"/>. The type is always equal to the declaring type for non attachement properties.
        /// </summary>
        public TypeInfo OwnerType { get; }
        /// <summary>
        /// Specifies if the <see cref="DependencyProperty"/> is using a value factory for its default value.
        /// </summary>
        internal Boolean IsUsingValueFactory { get; }
        /// <summary>
        /// Retrieves the key of the current property.
        /// </summary>
        internal abstract DependencyPropertyKey Key { get; }
        /// <summary>
        /// Retrieves the <see cref="DependencyType"/> in which the <see cref="DependencyProperty"/> is registered.
        /// </summary>
        internal DependencyType DependencyType { get; }
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
        /// <param name="isReadonly">Specifies if the property is readonly and a key is required to write the property.</param>
        /// <param name="isUsingValueFactory">Specifies if the property is using a value factory for the initial value.</param>
        /// <param name="isFastProperty">Specifies if the property is accessed in a special behaviour.</param>
        internal DependencyProperty(TypeInfo declaringType, TypeInfo ownerType, TypeInfo propertyType, String name, BindingMode defaultBindingMode, Boolean isAttachement, Boolean isUsingValueFactory)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name) + "cannot be null, empty or whitespace");
            // TODO : defaultBindingMode -> Enum check 


            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
            OwnerType = ownerType ?? throw new ArgumentNullException(nameof(ownerType));
            Name = name;
            DefaultBindingMode = defaultBindingMode;
            IsAttachement = isAttachement;
            IsUsingValueFactory = isUsingValueFactory;

            // Get the DependencyType for the current property and register the property
            DependencyType = DependencyType.GetDependencyType(declaringType, !isAttachement);
            DependencyType.Register(this);

            unchecked
            {
                // Precalculate the hash code if the depencency property
                _HashCode = 17;
                _HashCode = _HashCode * 23 + DeclaringType.GetHashCode();
                _HashCode = _HashCode * 23 + OwnerType.GetHashCode();
                _HashCode = _HashCode * 23 + PropertyType.GetHashCode();
                _HashCode = _HashCode * 23 + Name.GetHashCode();
            }
        }
        #endregion
        #region #### PUBLIC METHODS #####################################################
        /// <summary>
        /// Retrieves the default value of the current <see cref="DependencyProperty"/>.
        /// </summary>
        /// <param name="owner">The onwer of the dependency property.</param>
        /// <returns>Retrieves the default value of the property.</returns>
        internal Object GetPropertyDefault(IDependencyObject owner) => GetDefault(owner);
        /// <summary>
        /// Retrieves the default value of the current <see cref="DependencyProperty"/>.
        /// </summary>
        /// <param name="owner">The onwer of the dependency property.</param>
        /// <returns>Retrieves the default value of the property.</returns>
        protected abstract Object GetDefault(IDependencyObject owner);
        /// <summary>
        /// Create a <see cref="DependencyObjectContainer.PropertyValueStorage"/> for the current property.
        /// </summary>
        /// <param name="representedObject">The object which the property should represent.</param>
        /// <returns>The <see cref="DependencyObjectContainer.PropertyValueStorage"/> for the current property.</returns>
        internal abstract DependencyObjectContainer.PropertyValueStorage CreateValue(IDependencyObject representedObject);
        /// <summary>
        /// Tests if it's possible to access the <see cref="DependencyProperty"/> with the specified key.
        /// </summary>
        /// <param name="key">The <see cref="DependencyPropertyKey"/> to test.</param>
        /// <returns>True when the key grants write access to the property; otherwise false.</returns>
        public Boolean HasAccess(DependencyPropertyKey key) => Key == key;
        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override Boolean Equals(Object obj) => Equals(obj as DependencyProperty);
        /// <summary>
        /// Provides a hash code for the <see cref="DependencyProperty"/>.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode() => _HashCode;
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public Boolean Equals(DependencyProperty other) => 
            other != null &&
            PropertyType == other.PropertyType &&
            DeclaringType == other.DeclaringType &&
            Name == other.Name &&
            OwnerType == other.OwnerType && 
            IsAttachement == other.IsAttachement;
        /// <summary>
        /// Retrieves the current object represented as string.
        /// </summary>
        /// <returns>The current object represented as string.</returns>
        public override String ToString() => $"Name={Name} OwnerType={OwnerType} PropertyType={PropertyType}";
        #endregion
        #region #### PRIVATE METHODS ####################################################
        #endregion
        #region #### NESTED TYPES #######################################################
        #endregion
    }
}
