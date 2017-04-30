using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace BinderBuilder
{
    /// <summary>
    /// Provides a way to cleaner way to build up an array of strings from a type so that the 
    /// property names that are desired can be specified without using "magic string" arrays
    /// in the code.  Instead, lambda functions are used to specify property names.
    /// 
    ///     //Controller static field
    ///     private static ModelBinderBuilder<Movie> movieBinder;
    ///     
    ///     //Controller static constructor
    ///     movieBinder = ModelBinderBuilder<Movie>.Get()
    ///      .Add(x => x.MovieId)
    ///      .Add(x => x.MovieName);
    ///     
    ///     //Controller action
    ///     UpdateModel(movie, movieBinder.Fields);
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelBinderBuilder<T> where T : new()
    {
        //Using both a List and an array.  List is for more easily adding/removing
        //and array is for providing pre-built data that can be passed to UpdateModel 
        //method of controllers.  The array is rebuilt with each modification.
        private List<string> modelFields;
        private string[] fields;

        /// <summary>
        /// Factory method to return a ModelBinderBuilder for fluent programming.
        /// </summary>
        /// <returns></returns>
        public static ModelBinderBuilder<T> Get()
        {
            return new ModelBinderBuilder<T>();
        }


        public string[] Fields
        {
            get
            {
                return fields;
            }
        }

        public ModelBinderBuilder()
        {
            modelFields = new List<string>();
        }

        /// <summary>
        /// Build the data into an array.
        /// </summary>
        /// <returns></returns>
        private void Build()
        {
            fields = modelFields.ToArray();
        }

        /// <summary>
        /// Provide fluent method to add all the properties of a type to the list.
        /// </summary>
        /// <returns>this</returns>
        public ModelBinderBuilder<T> All()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                modelFields.Add(prop.Name);
            }

            Build();

            return this;
        }

        /// <summary>
        /// Provide fluent Add function so that Add can be chained to build the binding list.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        // 
        public ModelBinderBuilder<T> Add(Expression<Func<T, object>> property)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(property);

            if (!modelFields.Contains(propertyInfo.Name))
            {
                modelFields.Add(propertyInfo.Name);
            }

            Build();

            return this;
        }

        /// <summary>
        /// Provide fluent Remove function so that Remove can be chained to build the binding list.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public ModelBinderBuilder<T> Remove(Expression<Func<T, object>> property)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(property);

            if (modelFields.Contains(propertyInfo.Name))
            {
                modelFields.Remove(propertyInfo.Name);
            }

            Build();

            return this;
        }

        /// <summary>
        /// Gets the string name of a property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public string GetProperty(Expression<Func<T, object>> property)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(property);

            return propertyInfo.Name;
        }

        /// <summary>
        /// Gets property info for a lambda function and makes sure it is from the T type.
        ///     Input should be of form 'x => x.PropertyName'.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private PropertyInfo GetPropertyInfo(Expression<Func<T, object>> property)
        {
            PropertyInfo propertyInfo = null;

            if (property.Body is MemberExpression)
            {
                propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;
            }
            else
            {
                propertyInfo = (((UnaryExpression)property.Body).Operand as MemberExpression).Member as PropertyInfo;
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: 'x => x.Property'");
            }

            // the type that the passed in property is from must be of the type of this instance.
            if (!(propertyInfo.DeclaringType == typeof(T)))
            {
                throw new ArgumentException($"Type {typeof(T)} required, but received {propertyInfo.DeclaringType}.");
            }

            return propertyInfo;
        }

        /// <summary>
        /// The delimited list can be built with the ToString
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        // 
        public string ToString(string delimiter)
        {
            return String.Join(delimiter, modelFields);
        }

        public override string ToString()
        {
            return ToString(",");
        }
    }
}
