using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// Sorted dictionary to imitate behavior for Windows Phone
    /// </summary>
    /// <typeparam name="T">Key type</typeparam>
    /// <typeparam name="K">Value type</typeparam>
    internal class SortedDictionary<T, K> : IDictionary<T, K>
    {
        private readonly Dictionary<T, K> _dictionary;

        /// <summary>
        /// Initializes a new instance of the SortedDictionary class.
        /// </summary>
        public SortedDictionary()
        {
            _dictionary = new Dictionary<T, K>();
        }

        /// <summary>
        /// Initializes a new instance of the SortedDictionary class with an <see cref="IDictionary<T, K>"/>.
        /// </summary>
        /// <param name="initialDictionary">Initial <see cref="IDictionary<T, K>"/> to be used</param>
        public SortedDictionary(IDictionary<T, K> initialDictionary)
            : this()
        {
            initialDictionary
                .OrderBy(pair => pair.Key)
                .ToList()
                .ForEach(pair => _dictionary.Add(pair.Key, pair.Value));
        }

        private Dictionary<T, K> GetSortedDictionary()
        {
            var tempDictionary = new Dictionary<T, K>();
            ((List<T>)Keys).ForEach(key => tempDictionary.Add(key, _dictionary[key]));

            return tempDictionary;
        }

        #region IDictionary<T,K> Members

        public void Add(T key, K value)
        {
            _dictionary.Add(key, value);
        }

        public bool ContainsKey(T key)
        {
            return _dictionary.ContainsKey(key);
        }

        public ICollection<T> Keys
        {
            get { return _dictionary.Keys.OrderBy(k => k).ToList(); }
        }

        public bool Remove(T key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(T key, out K value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public ICollection<K> Values
        {
            get
            {
                return GetSortedDictionary().Values;
            }
        }

        public K this[T key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                _dictionary[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<T,K>> Members

        public void Add(KeyValuePair<T, K> item)
        {
            ((ICollection<KeyValuePair<T, K>>)_dictionary).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<T, K>>)_dictionary).Clear();
        }

        public bool Contains(KeyValuePair<T, K> item)
        {
            return ((ICollection<KeyValuePair<T, K>>)_dictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<T, K>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<T, K>>)GetSortedDictionary()).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return ((ICollection<KeyValuePair<T, K>>)_dictionary).Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<T, K>>)_dictionary).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<T, K> item)
        {
            return ((ICollection<KeyValuePair<T, K>>)_dictionary).Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<T,K>> Members

        public IEnumerator<KeyValuePair<T, K>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<T, K>>)GetSortedDictionary()).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)GetSortedDictionary()).GetEnumerator();
        }

        #endregion
    }
}
