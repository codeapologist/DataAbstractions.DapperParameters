namespace DataAbstractions.DapperParameters
{
    public static class Sql
    {
        public const string GetTableType = @"SELECT Col.[name], Col.column_id AS [Order] FROM sys.table_types TableTypes INNER JOIN sys.columns Col ON Col.object_id = TableTypes.type_table_object_id WHERE TableTypes.[name] = @TableTypeName ORDER BY Col.column_id;";
    }

}
