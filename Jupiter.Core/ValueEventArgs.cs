using System;

namespace Jupiter
{
    /// <summary>
    /// Represents event data which contains a single wrapped object.
    /// </summary>
    /// <typeparam name="T">The type of the object provided.</typeparam>
    public class ValueEventArgs<T> : EventArgs
    {
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Gets the wrapped event data.
        /// </summary>
        public T Value { get; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value">The event data provided.</param>
        public ValueEventArgs(T value)
        {
            Value = value;
        }
        #endregion
        #region #### PUBLIC #############################################################
        /// <inheritdoc/>
        public override String ToString() => $"Value={Value}";
        /// <summary>
        /// Implicit creates a new instance of the <see cref="ValueEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value">The value to create the instance for.</param>
        public static implicit operator ValueEventArgs<T>(T value) => new ValueEventArgs<T>(value);
        /// <summary>
        /// Implicit gets the <see cref="ValueEventArgs{T}.Value"/> from the <see cref="ValueEventArgs{T}"/>.
        /// </summary>
        /// <param name="eventArgs">The event args to get the value from.</param>
        public static implicit operator T(ValueEventArgs<T> eventArgs) => eventArgs.Value;
        #endregion
    }
}