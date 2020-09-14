using System;
using System.Linq.Expressions;

namespace DataAbstractions.DapperParameters
{
    public static class FluentParameterExtensions
    {
        public static ParameterBuilder BuildParameters(this object obj)
        {
            return new ParameterBuilder(obj);
        }

        public static ParameterBuilder Add<T, P>(this ParameterBuilder builder, Expression<Func<T, P>> propertyExpression, object value)
        {
            var key = builder.GetKeyFromExpression(propertyExpression);
            builder.Add(key, value);
            return builder;
        }

        public static ParameterBuilder Remove<T, P>(this ParameterBuilder builder, Expression<Func<T, P>> propertyExpression, object value)
        {
            var key = builder.GetKeyFromExpression(propertyExpression);
            builder.Remove(key);
            return builder;
        }

        public static ParameterBuilder Replace<T, P>(this ParameterBuilder builder, Expression<Func<T, P>> propertyExpression, object value)
        {
            var key = builder.GetKeyFromExpression(propertyExpression);
            builder.Replace(key, value);
            return builder;
        }
    }
}
