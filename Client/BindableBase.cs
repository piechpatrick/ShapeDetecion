using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client
{       
        /// <summary>
        /// Base implementation of the <see cref="System.ComponentModel.INotifyPropertyChanged"/>.
        /// </summary>
        public abstract class BindableBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// Notify observers that property value has changed.
            /// </summary>
            /// <param name="propertyName">Name of the property used to notify observers.</param>
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                var handler = PropertyChanged;

                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }

            /// <summary>
            /// Checks if property already equals new value. Sets the property and notifies observers if needed.
            /// </summary>
            /// <typeparam name="T">Type of the property.</typeparam>
            /// <param name="location">Referrence to a property.</param>
            /// <param name="value">New value for the property.</param>
            /// <param name="propertyName">Name of the property used to notify observers.</param>
            /// <returns>True if value was changed, false if the existing value equals new value.</returns>
            protected bool SetProperty<T>(ref T location, T value, [CallerMemberName] string propertyName = null)
            {
                var comparer = EqualityComparer<T>.Default;

                if (comparer.Equals(location, value))
                    return false;

                location = value;
                OnPropertyChanged(propertyName);

                return true;
            }
        }
    }
