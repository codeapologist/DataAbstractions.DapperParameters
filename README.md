# DataAbstractions.DapperParameters 

### A library for building Dapper dynamic parameters with a simple a fluent API.

## Extension Methods

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

## IParameterBuilder Interface

An **IParameterFactory** interface is available for projects leveraging dependency injection. This interface has the same API method signatures as the extension methods above.  

```csharp
IParameterFactory parameterFactory = new ParameterFactory();

var parameters = parameterFactory.Parameterize(myObject)
                         .Add("NewId", myIdentifier)
                         .Remove(x => x.Name)
                         .Replace(x => x.ModifiedDate, DateTime.Now)
                         .Create();

```

## Add Your Own Fluent Methods

You can define your own fluent api with extension methods.  The following example adds a LastModified parameter.

```csharp

    public static class MyParameterBuilderExtensions
    {
        public static IParameterBuilder<T> AddLastModified<T>(this IParameterBuilder<T> parameterBuilder)
        {
            //Cast to ParameterBuilder<T>
            var builder = (ParameterBuilder<T>)parameterBuilder; 

            //Add tells the builder to add this parameter key and value
            builder.Add("LastModified", DateTime.Now);

            //Return the builder
            return builder;
        }
    }
```

Use the new fluent method and create the dynamic parameter: 

```csharp
IParameterFactory parameterFactory = new ParameterFactory();

var parameters = parameterFactory.Parameterize(myObject)
                         .AddLastModified()
                         .Create();

```

TODO:
- Create fluent API for customizing and creating Table Valued Parameters