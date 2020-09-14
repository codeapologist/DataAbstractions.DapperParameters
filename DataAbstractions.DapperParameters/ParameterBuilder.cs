using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAbstractions.DapperParameters
{
    public class ParameterBuilder
    {
        private readonly object _objContext;
        private Dictionary<string, object> _parameterDictionary = new Dictionary<string, object>();

        public ParameterBuilder(object objContext)
        {
            _objContext = objContext;
            AddObjectProperties(_objContext);
        }

        public void AddObjectProperties(object obj)
        {
            var dictionary = obj.ToDictionary();

            dictionary.ToList().ForEach(x => _parameterDictionary.Add(x.Key, x.Value));
        }

        public void Add(string key, object value)
        {
            if (_parameterDictionary.ContainsKey(key.ToLowerInvariant()))
            {
                throw new InvalidOperationException($"Cannot add parameter. Key already exists: {key}");
            }

            _parameterDictionary.Add(key.ToLowerInvariant(), value);
        }

        public void Remove(string key)
        {
            if (_parameterDictionary.ContainsKey(key.ToLowerInvariant()))
            {
                _parameterDictionary.Remove(key.ToLowerInvariant());
            }
        }

        public void Replace(string key, object value)
        {
            var normalizedKey = key.ToLowerInvariant();

            if (_parameterDictionary.ContainsKey(normalizedKey))
            {
                _parameterDictionary[normalizedKey] = value;
            }

            throw new InvalidOperationException($"Cannot replace parameter. Key does not exist: {key}");
        }

        public string GetKeyFromExpression<T, P>(Expression<Func<T, P>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            return expression.Member.Name;
        }

        public DynamicParameters Create()
        {
            return new DynamicParameters(_parameterDictionary);
        }

    }
}
