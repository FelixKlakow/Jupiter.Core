using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter
{
    
    /// <summary>
    /// Represents a method which will handle a event.
    /// </summary>
    /// <typeparam name="TSender">The type of the sender.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void GenericEventHandler<TSender, TEventArgs>(TSender sender, TEventArgs e)
        where TSender : class
        where TEventArgs : EventArgs;
}
