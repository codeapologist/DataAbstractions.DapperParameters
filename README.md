# DataAbstractions.DapperParameters 

### A library for building Dapper dynamic parameters with a simple a fluent API.

CreateParameters() extension method will turn each property into a dynamic parameter using the property name and value.
```csharp
public class MyObject
{
    public int Id {get; set; }
    public string Name { get; set; }
    public DateTime ModifiedDate { get; set; }
}
```

```csharp
var parameters = myObject.CreateParameters();
```

To customize parameters, use Parameterize() to add, remove, and replace with a fluent syntax.
```csharp
var parameters = myObject.Parameterize()
                         .Add("NewId", myIdentifier)
                         .Remove(x => x.Name)
                         .Replace(x => x.ModifiedDate, DateTime.Now)
                         .Create();

connection.Execute("myStoredProcedure", parameters, commandType: CommandType.StoredProcedure);                         
```

TODO:
- Document IParameterFactory interface
- Create fluent API for customizing and creating Table Valued Parameters