# C# Gists for Solving Ceratin Issues

### ModelBinderBuilder

The ModelBinderBuilder class allows for removing "magic string" arrays from the UpdateModel method of a controller.  Instead, the class type is used along with lambda functions to designate which model properties are to be bound.