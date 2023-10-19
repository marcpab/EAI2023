using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using EAI.SAPNco.Utils.Model;

namespace EAI.SAPNco.Utils.ReadTable
{
    public class SapReadTableRow : IDictionary<string, string> //  IEnumerable<KeyValuePair<string, string>>
    {
        private int _rowNum;
        private TAB512 _data;
        private IEnumerable<RFC_DB_FLD> _fields;

        public SapReadTableRow(TAB512 data, int rowNum, IEnumerable<RFC_DB_FLD> fields)
        {
            _data = data;
            _rowNum = rowNum;
            _fields = fields;
        }

        public int RowNum { get => _rowNum; set => _rowNum = value; }

        public ICollection<string> Keys => new Collection<string>(_fields.Select(f => f.FIELDNAME).ToList());

        public ICollection<string> Values => new Collection<string>(_fields.Select(f => GetFieldValue(f.FIELDNAME)).ToList());

        public int Count => _fields.Count();

        public bool IsReadOnly => true;

        public string this[string key] { get => GetFieldValue(key); set => throw new NotImplementedException(); }

        public string GetFieldValue(string fieldName)
        {
            var field = _fields.Where(f => f.FIELDNAME == fieldName).First();

            var offset = (int)field.OFFSET;
            var length = (int)field.LENGTH;

            if (offset >= _data.WA.Length)
                return string.Empty;

            if (offset + length >= _data.WA.Length)
                length = _data.WA.Length - offset;

            return _data.WA.Substring(offset, length);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var field in _fields)
                yield return new KeyValuePair<string, string>(field.FIELDNAME, GetFieldValue(field.FIELDNAME));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var field in _fields)
                yield return new KeyValuePair<string, string>(field.FIELDNAME, GetFieldValue(field.FIELDNAME));
        }

        public void Add(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            return _fields.Select(f => f.FIELDNAME).Contains(key);
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out string value)
        {
            value = null;

            if (!ContainsKey(key))
                return false;

            value = GetFieldValue(key);

            return true;
        }

        public void Add(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }
    }
}
