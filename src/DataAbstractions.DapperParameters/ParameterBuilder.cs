using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAbstractions.DapperParameters
{

    public interface IParameterBuilder<T>
    {
        IParameterBuilder<T> Add(string key, object value);
        IParameterBuilder<T> TryAdd(string key, object value);
        IParameterBuilder<T> Remove(Expression<Func<T, object>> propertyExpression);
        IParameterBuilder<T> Update(Expression<Func<T, object>> propertyExpression, object value);
        IParameterBuilder<T> Rename(Expression<Func<T, object>> propertyExpression, string name);
        DynamicParameters Create();
    }

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

        public IParameterBuilder<T> Update(Expression<Func<T, object>> propertyExpression, object value)
        {
            var key = GetKeyFromExpression(propertyExpression);
            UpdateInternal(key, value);

            return this;
        }

        protected void UpdateInternal(string key, object value)
        {
            var normalizedKey = key.ToLowerInvariant();

            if (!_parameterDictionary.ContainsKey(normalizedKey))
            {
                throw new InvalidOperationException($"Cannot update parameter. Key does not exist: {key}");
            }

            _parameterDictionary[normalizedKey] = value;
            return;
        }

        public IParameterBuilder<T> Rename(Expression<Func<T, object>> propertyExpression, string name)
        {
            var key = GetKeyFromExpression(propertyExpression);
            RenameInternal(key, name);

            return this;
        }

        protected void RenameInternal(string key, string name)
        {
            var normalizedKey = key.ToLowerInvariant();

            if (!_parameterDictionary.ContainsKey(normalizedKey))
            {
                throw new InvalidOperationException($"Rename error. Key does not exist: {key}");
            }

            var value = _parameterDictionary[normalizedKey];
            _parameterDictionary.Add(name.ToLowerInvariant(), value);
            _parameterDictionary.Remove(normalizedKey);

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
