using System;
using System.Linq.Expressions;

namespace SoftwarePassion.Common.Core.Extensions
{
    /// <summary>
    /// Useful extensions to object.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Access to a property name on any class type with compile time checked name.
        /// </summary>
        /// <typeparam name="TClass">The type of the object instance.</typeparam>
        /// <typeparam name="TProperty">The type of the property to access.</typeparam>
        /// <param name="me">The object instance on which to access a property.</param>
        /// <param name="action">The action.</param>
        /// <returns>The name of the property.</returns>
        /// <remarks>This allows you to write code like:
        /// <code>
        /// var instance = new MyClass();
        /// var propertyName = instance.PropName(inst => inst.Property);
        /// </code>
        /// </remarks>
        public static string PropName<TClass, TProperty>(
            this TClass me, 
            Expression<Func<TClass, TProperty>> action) 
            where TClass : class
        {
            var expression = (MemberExpression)action.Body;
            string name = expression.Member.Name;

            return name;
        }
    }
}