using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jupiter.Core.Reflection
{
    /// <summary>
    /// Provides information about a member of a class.
    /// </summary>
    public abstract class SharedMemberInfo
    {
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the name of the member.
        /// </summary>
        public String Name { get; }
        /// <summary>
        /// Retrieves the <see cref="SharedTypeInfo"/> of the declaring type.
        /// </summary>
        public SharedTypeInfo DeclaringType { get; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes a new instance of the <see cref="SharedMemberInfo"/>.
        /// </summary>
        /// <param name="declaringType">The <see cref="SharedTypeInfo"/> of the declaring type.</param>
        /// <param name="member">The member which is represented.</param>
        internal SharedMemberInfo(SharedTypeInfo declaringType, MemberInfo member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            if (declaringType.Type != member.DeclaringType.GetTypeInfo()) throw new ArgumentException("declaringType must match member declaring type");

            Name = member.Name;
        }
        #endregion
        #region #### PUBLIC #############################################################
        #endregion
    }
}