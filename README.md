# DataAbstractions.DapperParameters 

A library to easily build Dapper dynamic parameters using a Fluent API.

Start building parameters from any object with the .BuildParameters() extension method
```csharp
public class MyObject
{
    public int Id {get; set; }
    public string Name { get; set; }
    public DateTime ModifiedDate { get; set; }
}
```
Each property will turned into a dynamic parameter using the property name and value.
```csharp
var parameters = myObject.BuildParameters().Create();
```

Add, remove, and replace parameters using a fluent syntax.
```csharp
var parameters = myObject.BuildParameters()
                         .Add("NewId", myIdentifier)
                         .Remove(x => x.Name)
                         .Replace(x => x.ModifiedDate, DateTime.Now)
                         .Create();
```
