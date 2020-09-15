using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAbstractions.DapperParameters
{
    public interface IParameterBuilder<T>
    {
        //Empty interface
    }


    public class ParameterBuilder<T> : IParameterBuilder<T>
    {
        private readonly T _objContext;
        private Dictionary<string, object> _parameterDictionary = new Dictionary<string, object>();

        public ParameterBuilder(T objContext)
        {
            _objContext = objContext;
            AddObjectProperties(_objContext);
        }

        public void AddObjectProperties(object obj)
        {
            var dictionary = obj.ToDictionary();

            dictionary.ToList().ForEach(x => _parameterDictionary.Add(x.Key.ToLowerInvariant(), x.Value));
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

            if (!_parameterDictionary.ContainsKey(normalizedKey))
            {
                throw new InvalidOperationException($"Cannot replace parameter. Key does not exist: {key}");
            }

            _parameterDictionary[normalizedKey] = value;
            return;
        }

        public string GetKeyFromExpression<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            return (propertyLambda.Body as MemberExpression ?? ((UnaryExpression)propertyLambda.Body).Operand as MemberExpression).Member.Name;
        }

        public DynamicParameters CreateParameters()
        {
            return new DynamicParameters(_parameterDictionary);
        }

    }
}
