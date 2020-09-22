using Dapper;

namespace DataAbstractions.DapperParameters
{

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
