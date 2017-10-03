using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jupiter.Core.Reflection
{
    /// <summary>
    /// Provides extended information about types.
    /// </summary>
    public sealed class SharedTypeInfo
    {
        #region #### CONSTANTS ##########################################################
        #endregion
        #region #### VARIABLES ##########################################################
        IReadOnlyList<SharedPropertyInfo> _DeclaredProperties;

        //ManualResetEvent _MembersReadyCompletion;
        ManualResetEvent _ObjectParseCompletion = new ManualResetEvent(false);
        //ManualResetEvent _AdditionalParseCompletion;
        #endregion
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the type which is reflected.
        /// </summary>
        public TypeInfo Type { get; }
        /// <summary>
        /// Specifies if the type is static.
        /// </summary>
        public Boolean IsStatic { get; }
        /// <summary>
        /// Retrieves the default value of the object.
        /// </summary>
        public Object DefaultValue { get; }
        /// <summary>
        /// Specifies if the type is a value type.
        /// </summary>
        public Boolean IsValueType => Type.IsValueType;
        /// <summary>
        /// Retrieves the base type of the current type.
        /// </summary>
        public SharedTypeInfo BaseType { get; }
        /// <summary>
        /// Retrieves a list with properties declared by the type.
        /// </summary>
        public IReadOnlyList<SharedPropertyInfo> DeclaredProperties
        {
            get
            {
                WaitUntilReady(_ObjectParseCompletion);
                return _DeclaredProperties;
            }
        }
        #endregion
        #region #### CTOR ###############################################################
        internal SharedTypeInfo(SharedReflectionManager reflectionManager, SharedReflectionManager.RegisterAction registerCallback, TypeInfo type)
        {
            if (reflectionManager == null) throw new ArgumentNullException(nameof(reflectionManager));
            if (registerCallback == null) throw new ArgumentNullException(nameof(registerCallback));

            // Basic initialization
            Type = type ?? throw new ArgumentNullException(nameof(type));
            IsStatic = type.IsSealed && type.IsAbstract;

            // Generate a sample object if possible
            if (!type.IsAbstract)
            {
                try
                {
                    // Search for a matching constructor
                    if (type.DeclaredConstructors.FirstOrDefault(p => !p.IsStatic && p.IsPublic && p.GetParameters().Length == 0) is ConstructorInfo constructor)
                    {
                        // Generate a default value
                        DefaultValue = constructor.Invoke(null);
                    }
                }
                // Catch all exceptions
                catch { }
            }


            // Registers the current type in the manager
            registerCallback(this);

            // Get the basetype of the current type
            if (Type.BaseType != null) BaseType = reflectionManager.GetInfo(Type.BaseType.GetTypeInfo());

            // Start a new task which initializes the type
            Task.Factory.StartNew(() => InitializeType(reflectionManager), TaskCreationOptions.LongRunning);
        }
        #endregion
        #region #### PUBLIC #############################################################
        #endregion
        #region #### PRIVATE ############################################################
        /// <summary>
        /// Initializes the current type.
        /// </summary>
        void InitializeType(SharedReflectionManager reflectionManager)
        {
            using (_ObjectParseCompletion)
            {
                List<SharedPropertyInfo> properties = new List<SharedPropertyInfo>();
                Dictionary<String, DependencyProperty> propertyLookup = DependencyType.GetDependencyType(Type, false).Properties.ToDictionary(p => p.Name);

                // Go through all declared properties and generate the property infos
                foreach (PropertyInfo item in Type.DeclaredProperties)
                {
                    propertyLookup.TryGetValue(item.Name, out DependencyProperty dependencyProperty);
                    properties.Add(new SharedPropertyInfo(reflectionManager, this, item, dependencyProperty, DefaultValue));
                }
                _DeclaredProperties = properties.ToImmutableArray();

                _ObjectParseCompletion.Set();
            }
            _ObjectParseCompletion = null;
        }
        /// <summary>
        /// Waits until the object is ready for usage.
        /// </summary>
        /// <param name="resetEvent">The <see cref="ManualResetEvent"/> to wait for.</param>
        static void WaitUntilReady(ManualResetEvent resetEvent)
        {
            try
            {
                resetEvent?.WaitOne();
            }
            catch (ObjectDisposedException)
            {
            }
        }
        #endregion
        #region #### NESTED TYPES #######################################################
        #endregion
    }
}
