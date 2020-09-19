using Dapper;
using System;
using System.Linq.Expressions;

namespace DataAbstractions.DapperParameters
{
    public static class FluentParameterExtensions
    {
        public static IParameterBuilder<T> BuildParameters<T>(this T obj)
        {
            return new ParameterBuilder<T>(obj);
        }

        public static IParameterBuilder<T> Add<T>(this IParameterBuilder<T> builder, string key, object value)
        {
            var pb = CastToParameterBuilder(builder);
            pb.Add(key, value);

            return pb;
        }

        public static IParameterBuilder<T> Remove<T>(this IParameterBuilder<T> builder, Expression<Func<T, object>> propertyExpression)
        {
            var pb = CastToParameterBuilder(builder);
            var key = pb.GetKeyFromExpression(propertyExpression);
            pb.Remove(key);

            return pb;
        }

        public static IParameterBuilder<T> Replace<T>(this IParameterBuilder<T> builder, Expression<Func<T, object>> propertyExpression, object value)
        {
            var pb = CastToParameterBuilder(builder);
            var key = pb.GetKeyFromExpression(propertyExpression);
            pb.Replace(key, value);

            return pb;
        }

        public static DynamicParameters Create<T>(this IParameterBuilder<T> builder)
        {
            var pb = CastToParameterBuilder(builder);
            return pb.CreateParameters();
        }


        private static ParameterBuilder<T> CastToParameterBuilder<T>(IParameterBuilder<T> builder)
        {
            if (!(builder is ParameterBuilder<T> pb))
            {
                throw new ArgumentException($"Argument needs to be of type ParameterBuilder", nameof(builder));
            }

            return pb;
        }
    }
}
