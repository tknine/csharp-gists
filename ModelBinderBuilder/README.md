# Model Binder Builder

The ModelBinderBuilder class allows for removing "magic string" arrays from the UpdateModel method of a MVC or WebApi controller.  Instead, the class type is used along with lambda functions to designate which model properties are to be bound. 

Providing a binding field list for UpdateModel is always a best practice to avoid having the client sending data that should not change.

Demonstrates usage of the following .Net features:
* Generics
* Reflection
* Lambda functions
* Expression trees
* Unit testing

## Usage

To not have the binder rebuilt with each action invocation, it would be best to have these binders be defined in the static constructor and then used in the actions.

```csharp
//Controller static field
private static ModelBinderBuilder<Movie> movieBinder;

//Inside controller static constructor
movieBinder = ModelBinderBuilder<Movie>.Get()
    .Add(x => x.Id)
    .Add(x => x.Name);

//Inside controller action - get fields as string[]
UpdateModel(movie, movieBinder.Fields);
```

This is assuming some Movie model:
```csharp
public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
}
```

## Instantiation

Can be instantiated by either use **new** or the factory method.  The factory method provides for fluent programming.

```csharp
//Factory method - fluent method.
var movieBinder = ModelBinderBuilder<Movie>.Get();

//New
var movieBinder = new ModelBinderBuilder<Movie>();
```

## Methods

#### All
Adds all the properties of the model.
```csharp
movieBinder = ModelBinderBuilder<Movie>.Get().All();
```

#### Add
Add the specified property tot he model.

```csharp
movieBinder = ModelBinderBuilder<Movie>.Get()
    .Add(x => x.Id);
```

#### Remove
Remove the specified property.
```csharp
movieBinder = ModelBinderBuilder<Movie>.Get()
    .All()
    .Remove(x => x.Year);
```

 #### GetProperty
 Return the string name of the property.  This method is not fluent.
 ```csharp
 var yearFieldName = ModelBinderBuilder<Movie>.Get()
    .GetProperty(x => x.Year).
 
