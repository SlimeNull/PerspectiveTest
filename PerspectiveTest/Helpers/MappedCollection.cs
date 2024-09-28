using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerspectiveTest.Helpers
{
    public class MappedCollection<TFrom, T> : IList<T>, INotifyCollectionChanged
    {
        private readonly ObservableCollection<TFrom> _source;
        private readonly Func<TFrom, T> _mapper;
        private readonly List<T> _storage;

        public MappedCollection(ObservableCollection<TFrom> source, Func<TFrom, T> mapper)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(mapper);

            _source = source;
            _mapper = mapper;
            _storage = new();

            foreach (var item in source)
            {
                _storage.Add(_mapper.Invoke(item));
            }

            _source.CollectionChanged += SourceCollectionChanged;
        }

        ~MappedCollection()
        {
            _source.CollectionChanged -= SourceCollectionChanged;
        }

        private void SourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    var newItems = new List<T>();

                    int index = e.NewStartingIndex;
                    foreach (TFrom originNewItem in e.NewItems!)
                    {
                        var newItem = _mapper.Invoke(originNewItem);
                        newItems.Add(newItem);
                        _storage.Insert(index, newItem);
                        index++;
                    }

                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, e.NewStartingIndex));
                    break;
                }

                case NotifyCollectionChangedAction.Remove:
                {
                    var removedItems = new List<T>();

                    int index = e.OldStartingIndex;
                    for (int i = 0; i < e.OldItems!.Count; i++)
                    {
                        var oldItem = _storage[index];
                        _storage.RemoveAt(index);
                        removedItems.Add(oldItem);
                    }

                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems, e.OldStartingIndex));
                    break;
                }

                case NotifyCollectionChangedAction.Replace:
                {
                    var newItems = new List<T>();
                    var oldItems = new List<T>();

                    var startIndex = e.NewStartingIndex;
                    for (int i = 0; i < e.NewItems!.Count; i++)
                    {
                        var oldItem = _storage[startIndex + i];
                        var newItem = _mapper.Invoke((TFrom)e.NewItems[i]!);
                        oldItems.Add(oldItem);
                        newItems.Add(newItem);
                        _storage[startIndex] = newItem;
                    }

                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems, e.NewStartingIndex));
                    break;
                }

                case NotifyCollectionChangedAction.Reset:
                {
                    _storage.Clear();
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    break;
                }
            }
        }

        public int Count => ((ICollection<T>)_storage).Count;

        public bool IsReadOnly => ((ICollection<T>)_storage).IsReadOnly;


        public T this[int index] { get => ((IList<T>)_storage)[index]; }
        T IList<T>.this[int index] { get => ((IList<T>)_storage)[index]; set => throw new InvalidOperationException(); }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public bool Contains(T item) => ((ICollection<T>)_storage).Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)_storage).CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_storage).GetEnumerator();
        public int IndexOf(T item) => ((IList<T>)_storage).IndexOf(item);
        void ICollection<T>.Add(T item) => throw new InvalidOperationException();
        void ICollection<T>.Clear() => throw new InvalidOperationException();
        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException();
        void IList<T>.Insert(int index, T item) => throw new InvalidOperationException();
        void IList<T>.RemoveAt(int index) => throw new InvalidOperationException();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_storage).GetEnumerator();
    }
}
