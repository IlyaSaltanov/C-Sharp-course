using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] _bytes;
        private int? _hashCode;

        public ReadonlyBytes(params byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            _bytes = bytes;
        }

        public int Length => _bytes.Length;

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= _bytes.Length) throw new IndexOutOfRangeException();
                return _bytes[index];
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            for (int i = 0; i < _bytes.Length; i++)
            {
                yield return _bytes[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            var other = (ReadonlyBytes)obj;
            if (_bytes.Length != other._bytes.Length) return false;

            for (int i = 0; i < _bytes.Length; i++)
            {
                if (_bytes[i] != other._bytes[i]) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            if (_hashCode.HasValue) return _hashCode.Value;

            unchecked
            {
                int hash = -2128831035; // FNV-1a 32-bit offset basis
                const int p = 16777619; // FNV-1a 32-bit prime

                foreach (var b in _bytes)
                {
                    hash = (hash ^ b) * p;
                }
                
                _hashCode = hash;
                return hash;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < _bytes.Length; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(_bytes[i]);
            }
            sb.Append(']');
            return sb.ToString();
        }
    }
}
