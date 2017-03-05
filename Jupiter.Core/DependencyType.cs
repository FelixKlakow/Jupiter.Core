using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// Provides information about a type which implements <see cref="IDependencyObject"/>.
    /// </summary>
    public sealed class DependencyType
    {
        #region #### VARIABLES ##########################################################
        static readonly ConcurrentDictionary<TypeInfo, DependencyType> TypeLookup = new ConcurrentDictionary<TypeInfo, DependencyType>();
        static readonly Object SyncRoot = new Object();
        // Local variables
        readonly Object _SyncRoot = new Object();
        ImmutableDictionary<String, DependencyProperty> _Lookup;
        List<DependencyProperty> _PropertyList = new List<DependencyProperty>();
        ImmutableArray<DependencyProperty> _Properties;
        #endregion
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the type which is represented by the <see cref="DependencyType"/>.
        /// </summary>
        public TypeInfo Type { get; }
        /// <summary>
        /// Retrieves the parent <see cref="DependencyType"/> if any exists.
        /// </summary>
        public DependencyType Parent { get; }
        /// <summary>
        /// Retrieves all dependency properties of the type.
        /// </summary>
        public ImmutableArray<DependencyProperty> Properties
        {
            get
            {
                Lock();
                return _Properties;
            }
        }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Creates a new instance of the <see cref="DependencyType"/> class.
        /// </summary>
        /// <param name="type">The type for which information should be provided.</param>
        private DependencyType(TypeInfo type)
        {
            Type = type;
            // Get parent type
            if (type.BaseType != null) Parent = GetDependencyType(type.BaseType.GetTypeInfo());
            // Register the dependency type
            TypeLookup.TryAdd(type, this);
            // Run the class constructor for the current reflected class
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.AsType().TypeHandle);
        }
        #endregion
        #region #### PUBLIC / INTERNAL METHODS ##########################################
        /// <summary>
        /// Gets the <see cref="DependencyType"/> which describes a <see cref="IDependencyObject"/>.
        /// </summary>
        /// <param name="type">The type to get the <see cref="DependencyType"/> for.</param>
        /// <returns>The <see cref="DependencyType"/> which describes the type.</returns>
        public static DependencyType GetDependencyType(TypeInfo type) => GetDependencyType(type, true);
        /// <summary>
        /// Gets the <see cref="DependencyType"/> which describes a type.
        /// </summary>
        /// <param name="type">The type to get the <see cref="DependencyType"/> for.</param>
        /// <param name="requiresDependencyType"></param>
        /// <returns>The <see cref="DependencyType"/> which describes the type.</returns>
        internal static DependencyType GetDependencyType(TypeInfo type, Boolean requiresDependencyType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!TypeLookup.TryGetValue(type, out DependencyType result))
            {
                lock (SyncRoot)
                {
                    if (!requiresDependencyType || type.ImplementedInterfaces.Contains(typeof(IDependencyObject)))
                        result = new DependencyType(type);
                }
            }
            return result;
        }
        /// <summary>
        /// Tests whether the specified type is a subclass of the current type.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <returns>True if the type is a subclass of the current class; otherwise false.</returns>
        internal Boolean IsSubclassOf(DependencyType type) => this == type || (Parent?.IsSubclassOf(type) == true);
        /// <summary>
        /// Registers a property in the current <see cref="DependencyType"/>.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> which should be added.</param>
        internal void Register(DependencyProperty property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (property.DeclaringType != Type) throw new ArgumentException("Property type is not matching type of " + nameof(DependencyType));

            lock (_SyncRoot)
            {
                if (_PropertyList == null) throw new InvalidOperationException("DependencyType is locked");
                _PropertyList.Add(property);
            }
        }
        #endregion
        #region #### PRIVATE METHODS ####################################################
        /// <summary>
        /// Locks the current <see cref="DependencyType"/>.
        /// </summary>
        void Lock()
        {
            if (_PropertyList != null)
            {
                lock (_SyncRoot)
                {
                    if (_PropertyList != null)
                    {
                        // Create a immutable array which cannot be changed
                        _Properties = ImmutableArray.CreateRange(_PropertyList);
                        _PropertyList.Clear();
                        _PropertyList = null;

                        // Add all properties to the lookup
                        _Lookup = _Properties.ToImmutableDictionary(p => p.Name);
                    }
                }
            }
        }
        #endregion
    }
}
