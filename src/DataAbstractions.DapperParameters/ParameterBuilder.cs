using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAbstractions.DapperParameters
{
    public class ParameterBuilder<T> : IParameterBuilder<T>
    {
        protected readonly T _obj;
        protected Dictionary<string, object> _parameterDictionary = new Dictionary<string, object>();


        public ParameterBuilder(T obj)
        {
            _obj = obj;
            AddObjectProperties(_obj);
        }

        public void AddObjectProperties(object obj)
        {
            var dictionary = obj.ToDictionary();

            dictionary.ToList().ForEach(x => _parameterDictionary.Add(x.Key.ToLowerInvariant(), x.Value));
        }

        public IParameterBuilder<T> Add(string key, object value)
        {
            if (_parameterDictionary.ContainsKey(key.ToLowerInvariant()))
            {
                throw new InvalidOperationException($"Cannot add parameter. Key already exists: {key}");
            }

            _parameterDictionary.Add(key.ToLowerInvariant(), value);

            return this;
        }

        public IParameterBuilder<T> TryAdd(string key, object value)
        {
            if (!_parameterDictionary.ContainsKey(key.ToLowerInvariant()))
            {
                _parameterDictionary.Add(key.ToLowerInvariant(), value);
            }

            return this;
        }

        public IParameterBuilder<T> Remove(Expression<Func<T, object>> propertyExpression)
        {
            var key = GetKeyFromExpression(propertyExpression);
            RemoveInternal(key);

            return this;
        }

        protected void RemoveInternal(string key)
        {
            if (_parameterDictionary.ContainsKey(key.ToLowerInvariant()))
            {
                _parameterDictionary.Remove(key.ToLowerInvariant());
            }
        }

        public IParameterBuilder<T> Replace(Expression<Func<T, object>> propertyExpression, object value)
        {
            var key = GetKeyFromExpression(propertyExpression);
            ReplaceInternal(key, value);

            return this;
        }

        protected void ReplaceInternal(string key, object value)
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

        public DynamicParameters Create()
        {
            return new DynamicParameters(_parameterDictionary);
        }

    }
}
