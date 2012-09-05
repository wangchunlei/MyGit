﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domas.DAP.ADF.Notifier.Infrastructure
{
    public class SafeCollection<T> : ICollection<T>
    {
        private readonly ConcurrentDictionary<T, bool> _inner = new ConcurrentDictionary<T, bool>();
        public IEnumerator<T> GetEnumerator()
        {
            return _inner.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _inner.TryAdd(item, true);
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public bool Contains(T item)
        {
            return _inner.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _inner.Keys.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            bool value;
            return _inner.TryRemove(item, out value);
        }

        public int Count
        {
            get { return _inner.Count; }
        }
        public bool IsReadOnly { get { return false; }}
    }
}
