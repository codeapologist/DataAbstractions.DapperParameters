using Dapper;
using System;
using System.Linq.Expressions;

namespace DataAbstractions.DapperParameters
{

    public interface IParameterBuilder<T>
    {
        IParameterBuilder<T> Add(string key, object value);
        IParameterBuilder<T> TryAdd(string key, object value);
        IParameterBuilder<T> Remove(Expression<Func<T, object>> propertyExpression);
        IParameterBuilder<T> Replace(Expression<Func<T, object>> propertyExpression, object value);
        DynamicParameters Create();
    }

    public interface IParameterFactory
    {
        IParameterBuilder<T> Parameterize<T>(T obj);
    }



    public static class ParameterBuilderExtensions
    {
        public static IParameterBuilder<T> Parameterize<T>(this T obj)
        {
            return new ParameterBuilder<T>(obj);
        }

        public static DynamicParameters CreateParameters<T>(this T obj)
        {
            return new ParameterBuilder<T>(obj).Create();
        }

    }
}
