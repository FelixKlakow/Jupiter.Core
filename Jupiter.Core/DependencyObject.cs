using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace Jupiter
{
    public abstract class DependencyObject : DependencyObjectContainer, IDependencyObject, INotifyPropertyChanged
    {
        #region #### VARIABLES ##########################################################
        PropertyChangedEventHandler _PropertyChanged;
        #endregion
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Occurs when a property of the object has been changed.
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PropertyChangedEventHandler eventHandler = _PropertyChanged;
                PropertyChangedEventHandler currentEventHandler;
                do
                {
                    currentEventHandler = eventHandler;
                    PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(currentEventHandler, value);
                    eventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref _PropertyChanged, value2, currentEventHandler);
                }
                while (eventHandler != currentEventHandler);
            }
            remove
            {
                PropertyChangedEventHandler eventHandler = _PropertyChanged;
                PropertyChangedEventHandler eventHandler2;
                do
                {
                    eventHandler2 = eventHandler;
                    PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(eventHandler2, value);
                    eventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref _PropertyChanged, value2, eventHandler2);
                }
                while (eventHandler != eventHandler2);
            }
        }
        #endregion
        #region #### CTOR ###############################################################
        public DependencyObject() => PropertyChanged += DependencyObject_PropertyChanged;
        #endregion
        #region #### PUBLIC METHODS #####################################################
        #endregion
        #region #### PRIVATE METHODS ####################################################
        /// <summary>
        /// Handles when the underlying <see cref="IDependencyObject"/> property has been changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        void DependencyObject_PropertyChanged(IDependencyObject sender, PropertyChangedEventArgs e) => _PropertyChanged?.Invoke(sender, new System.ComponentModel.PropertyChangedEventArgs(e.Property.Name));
        #endregion

    }
}
