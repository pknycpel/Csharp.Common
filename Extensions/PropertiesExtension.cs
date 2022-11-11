// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertiesExtension.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2020-05-11 07:20 </creationDate>
// <summary>
//      Defines the PropertiesExtension type to provide methods for properties management.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Extensions;

using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

/// <summary>
///     The PropertiesExtension to provide methods for properties management.
/// </summary>
[PublicAPI]
public static class PropertiesExtension
{
    /// <summary>
    ///     Get property value by name.
    /// </summary>
    /// <param name="instance"> The object instance. </param>
    /// <param name="propertyName"> The property name. </param>
    /// <typeparam name="TValue"> Type of property. </typeparam>
    /// <returns> The value of property as specified type. </returns>
    public static TValue? GetPropertyValue<TValue>(this object instance, string propertyName)
    {
        var type = instance.GetType();
        var field = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
            .FirstOrDefault(e => typeof(TValue).IsAssignableFrom(e.PropertyType) && e.Name == propertyName);
        if (field != null)
        {
            return (TValue?)field.GetValue(instance);
        }

        return default;
    }

    /// <summary>
    ///     Has property by name.
    /// </summary>
    /// <param name="type"> The type of object. </param>
    /// <param name="propertyName"> The property name. </param>
    /// <returns> True if type has that property, otherwise false. </returns>
    public static bool HasProperty(this Type type, string propertyName)
    {
        return type.GetProperties().Any(p => p.Name == propertyName);
    }

    /// <summary>
    ///     Set property value by name.
    /// </summary>
    /// <param name="instance"> The object instance. </param>
    /// <param name="propertyName"> The property name. </param>
    /// <param name="value"> The value to set. </param>
    /// <typeparam name="TValue"> Type of property. </typeparam>
    public static void SetPropertyValue<TValue>(this object instance, string propertyName, TValue value)
    {
        var type = instance.GetType();
        var field = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
            .FirstOrDefault(e => typeof(TValue).IsAssignableFrom(e.PropertyType) && e.Name == propertyName);
        field?.SetValue(instance, value);
    }

    /// <summary>
    ///     Creating from <see cref="PropertyInfo" /> string with type name and property name.
    /// </summary>
    /// <param name="props"> Enumerator with <see cref="PropertyInfo" /> objects. </param>
    /// <returns> String with type name and property name. </returns>
    public static string CacheKey(this IEnumerable<PropertyInfo> props)
    {
        return string.Join(",", props.Select(info =>
            {
                if (info.DeclaringType != null)
                {
                    return info.DeclaringType.FullName + "." + info.Name;
                }

                return string.Empty;
            })
            .ToArray());
    }
}