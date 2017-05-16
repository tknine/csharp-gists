# C# Gists for Solving Certain Issues

### [ModelBinderBuilder](./ModelBinderBuilder)

The ModelBinderBuilder class allows for removing "magic string" arrays from the UpdateModel method of a MVC or WebApi controller.  Instead, the class type is used along with lambda functions to designate which model properties are to be bound.

Demonstrates usage of the following .Net features:
* Generics
* Reflection
* Lambda functions
* Expression trees
* Unit testing