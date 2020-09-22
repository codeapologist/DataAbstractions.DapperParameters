using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using static Dapper.SqlMapper;

namespace DataAbstractions.DapperParameters
{
    public interface IParameterFactory
    {
        IParameterBuilder<T> Parameterize<T>(T obj);
    }


    public class ParameterFactory : IParameterFactory
    {
        public IParameterBuilder<T> Parameterize<T>(T obj)
        {
            return new ParameterBuilder<T>(obj);
        }

        public ICustomQueryParameter CreateTableValuedParameter<T>(IEnumerable<T> objects, string tableTypeName, IDbConnection connection)
        {
            List<ColumnSequence> sequencedColumns = GetSequencedColumns(tableTypeName, connection);

            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            var sequencedPropertyInfos = (from column in sequencedColumns
                                          join propertyInfo in propertyInfos on column.Name.ToLowerInvariant() equals propertyInfo.Name.ToLowerInvariant()
                                          select new { Property = propertyInfo, column.SequenceNumber }).OrderBy(x => x.SequenceNumber).ToList();

            if (!sequencedPropertyInfos.Any())
            {
                throw new InvalidOperationException($"No column and property names matched with table type: {tableTypeName}");
            }

            var dataTable = new DataTable(typeof(T).Name);

            foreach (var info in sequencedPropertyInfos)
            {
                dataTable.Columns.Add(info.Property.Name, Nullable.GetUnderlyingType(info.Property.PropertyType) ?? info.Property.PropertyType);
            }

            foreach (var obj in objects)
            {
                var values = new object[sequencedPropertyInfos.Count];

                for (var i = 0; i < sequencedPropertyInfos.Count; i++)
                {
                    values[i] = sequencedPropertyInfos[i].Property.GetValue(obj, null);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable.AsTableValuedParameter(tableTypeName);
        }

        private static List<ColumnSequence> GetSequencedColumns(string tableTypeName, IDbConnection connection)
        {
            var sequencedColumns = connection.Query<ColumnSequence>(Sql.GetTableType, new { TableTypeName = tableTypeName }, commandType: CommandType.StoredProcedure).ToList();

            if (!sequencedColumns.Any())
            {
                throw new InvalidOperationException($"Table type doesn't exist or has no columns defined: {tableTypeName}");
            }

            return sequencedColumns;
        }

        private class ColumnSequence
        {
            public string Name { get; set; }
            public int SequenceNumber { get; set; }
        }
    }


}
