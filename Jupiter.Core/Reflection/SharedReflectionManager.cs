using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jupiter.Reflection
{
    /// <summary>
    /// Provides information about 
    /// </summary>
    public sealed class SharedReflectionManager
    {
        #region #### CONSTANTS ##########################################################
        #endregion
        #region #### DEPENDENCY DECLARATIONS ############################################
        #endregion
        #region #### VARIABLES ##########################################################
        readonly ConcurrentDictionary<TypeInfo, SharedTypeInfo> _RegisteredType = new ConcurrentDictionary<TypeInfo, SharedTypeInfo>();
        readonly RegisterAction _SharedAction;
        #endregion
        #region #### PROPERTIES #########################################################
        #endregion
        #region #### EVENTS #############################################################
        #endregion
        #region #### CTOR ###############################################################
        public SharedReflectionManager() => _SharedAction = new RegisterAction(Register);
        #endregion
        #region #### PUBLIC #############################################################
        /// <summary>
        /// Gets a <see cref="SharedTypeInfo"/> for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to get the <see cref="SharedTypeInfo"/> for.</typeparam>
        /// <returns>The <see cref="SharedTypeInfo"/> for the specified type.</returns>
        public SharedTypeInfo GetInfo<T>() => GetInfo(TypeOf<T>.TypeInfo);
        /// <summary>
        /// Gets a <see cref="SharedTypeInfo"/> for the specified <see cref="TypeInfo"/>.
        /// </summary>
        /// <param name="type">The <see cref="TypeInfo"/> to get the <see cref="SharedTypeInfo"/> for.</param>
        /// <returns>The <see cref="SharedTypeInfo"/> for the specified type.</returns>
        public SharedTypeInfo GetInfo(TypeInfo type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!_RegisteredType.TryGetValue(type, out SharedTypeInfo sharedType))
            {
                lock (_RegisteredType)
                {
                    if (!_RegisteredType.TryGetValue(type, out sharedType))
                    {
                        _RegisteredType.TryAdd(type, sharedType = new SharedTypeInfo(this, _SharedAction, type));
                    }
                }
            }
            return sharedType;
        }
        #endregion
        #region #### PRIVATE ############################################################
        /// <summary>
        /// Registers a <see cref="SharedTypeInfo"/> in the <see cref="SharedReflectionManager"/>.
        /// </summary>
        /// <param name="typeInfo">The <see cref="SharedTypeInfo"/> to register.</param>
        void Register(SharedTypeInfo typeInfo) => _RegisteredType[typeInfo.Type] = typeInfo;
        #endregion
        #region #### NESTED TYPES #######################################################
        /// <summary>
        /// Registers a <see cref="SharedTypeInfo"/> in the <see cref="SharedReflectionManager"/>.
        /// </summary>
        /// <param name="typeInfo">The <see cref="SharedTypeInfo"/> to register.</param>
        internal delegate void RegisterAction(SharedTypeInfo typeInfo);
        #endregion
    }
}