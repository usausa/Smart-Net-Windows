namespace Smart.Windows.Input
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows.Input;

    public abstract class ObserveCommandBase<T>
        where T : ObserveCommandBase<T>
    {
        private HashSet<INotifyPropertyChanged> observeObjects;

        private Dictionary<INotifyPropertyChanged, HashSet<string>> observeProperties;

        private HashSet<INotifyCollectionChanged> observeCollections;

        private EventHandler canExecuteChanged;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                canExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                canExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Ignore")]
        public void RaiseCanExecuteChanged()
        {
            canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public T Observe(INotifyPropertyChanged target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            observeObjects ??= new HashSet<INotifyPropertyChanged>();
            if (!observeObjects.Contains(target))
            {
                observeObjects.Add(target);
                target.PropertyChanged += HandleAllPropertyChanged;
            }

            return (T)this;
        }

        public T Observe(INotifyPropertyChanged target, string propertyName)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            observeProperties ??= new Dictionary<INotifyPropertyChanged, HashSet<string>>();
            if (!observeProperties.TryGetValue(target, out var properties))
            {
                properties = new HashSet<string>();
                observeProperties[target] = properties;
                target.PropertyChanged += HandlePropertyChanged;
            }

            properties.Add(propertyName);

            return (T)this;
        }

        public T ObserveCollection(INotifyCollectionChanged target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            observeCollections ??= new HashSet<INotifyCollectionChanged>();
            if (!observeCollections.Contains(target))
            {
                observeCollections.Add(target);
                target.CollectionChanged += HandleCollectionChanged;
            }

            return (T)this;
        }

        public T RemoveObserver(INotifyPropertyChanged target, string propertyName)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (observeProperties is not null)
            {
                if (observeProperties.TryGetValue(target, out var properties))
                {
                    properties.Remove(propertyName);

                    if (properties.Count == 0)
                    {
                        observeProperties.Remove(target);
                        target.PropertyChanged -= HandlePropertyChanged;
                    }
                }
            }

            return (T)this;
        }

        public T RemoveObserver(INotifyPropertyChanged target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (observeObjects is not null)
            {
                if (observeObjects.Remove(target))
                {
                    target.PropertyChanged -= HandleAllPropertyChanged;
                }
            }

            if (observeProperties is not null)
            {
                if (observeProperties.Remove(target))
                {
                    target.PropertyChanged -= HandlePropertyChanged;
                }
            }

            return (T)this;
        }

        public T RemoveCollectionObserver(INotifyCollectionChanged target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (observeCollections is not null)
            {
                if (observeCollections.Contains(target))
                {
                    target.CollectionChanged -= HandleCollectionChanged;
                    observeCollections.Remove(target);
                }
            }

            return (T)this;
        }

        public T RemoveObserver()
        {
            if (observeObjects is not null)
            {
                foreach (var target in observeObjects)
                {
                    target.PropertyChanged -= HandleAllPropertyChanged;
                }

                observeObjects.Clear();
            }

            if (observeProperties is not null)
            {
                foreach (var target in observeProperties.Keys)
                {
                    target.PropertyChanged -= HandlePropertyChanged;
                }

                observeProperties.Clear();
            }

            if (observeCollections is not null)
            {
                foreach (var target in observeCollections)
                {
                    target.CollectionChanged -= HandleCollectionChanged;
                }

                observeCollections.Clear();
            }

            return (T)this;
        }

        private void HandleAllPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseCanExecuteChanged();
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var properties = observeProperties[(INotifyPropertyChanged)sender];
            if (properties.Contains(e.PropertyName))
            {
                RaiseCanExecuteChanged();
            }
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaiseCanExecuteChanged();
        }
    }
}
