// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2020-05-13 10:19 </creationDate>
// <summary>
//      Defines the GenericExtensions type to contain generic extension functions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Func;

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using JetBrains.Annotations;

/// <summary>
///     The GenericExtensions class contain generic extension functions.
/// </summary>
[PublicAPI]
public static class GenericExtensions
{
    /// <summary>
    ///     The c# version of "Between" clause of sql query.
    /// </summary>
    /// <typeparam name="T"> The type of object. </typeparam>
    /// <param name="value"> The value to check. </param>
    /// <param name="from"> Lower range value. </param>
    /// <param name="to"> Upper range value. </param>
    /// <returns>
    ///     <b>true</b> if <paramref name="value" /> is between <paramref name="from" /> and <paramref name="to" />,
    ///     otherwise <b>false</b>.
    /// </returns>
    public static bool Between<T>(this T value, T from, T to)
        where T : IComparable<T>
    {
        return value.CompareTo(from) >= 0 && value.CompareTo(to) <= 0;
    }

    /// <summary>
    ///     Checks object's value to array of object values.
    /// </summary>
    /// <typeparam name="T"> The type of object. </typeparam>
    /// <param name="value"> Value to check. </param>
    /// <param name="array"> Array of object values to compare. </param>
    /// <returns> Return true if any object value matches. </returns>
    public static bool In<T>(this T value, params T[] array)
    {
        return array.Contains(value);
    }

    /// <summary>
    ///     Checks object's value to array of object values.
    /// </summary>
    /// <typeparam name="T"> The type of object. </typeparam>
    /// <param name="value"> Value to check. </param>
    /// <param name="list"> List of object values to compare. </param>
    /// <returns> Return true if any object value matches. </returns>
    public static bool In<T>(this T value, IEnumerable<T> list)
    {
        return list.Contains(value);
    }

    /// <summary>
    ///     Unified advanced generic check for NOT: DbNull.Value, INullable.IsNull, null reference.
    /// </summary>
    /// <typeparam name="T"> The type of object. </typeparam>
    /// <param name="value"> The object to check.  </param>
    /// <returns> Returns <b>true</b> if object value is not null or not DBNull, otherwise <b>false</b>. </returns>
    public static bool IsNotNull<T>(this T value)
    {
        return !IsNull(value);
    }

    /// <summary>
    ///     Unified advanced generic check for : Nullable{T}.HasValue reference.
    /// </summary>
    /// <typeparam name="T"> Base type of object. </typeparam>
    /// <param name="value"> The object to check.  </param>
    /// <returns> Returns <b>true</b> if object value is not null or not DBNull, otherwise <b>false</b>. </returns>
    public static bool IsNotNull<T>(this T? value)
        where T : struct
    {
        return value.HasValue;
    }

    /// <summary>
    ///     Unified advanced generic check for: DbNull.Value, INullable.IsNull, null reference.
    /// </summary>
    /// <typeparam name="T"> The type of object. </typeparam>
    /// <param name="value"> The object to check.  </param>
    /// <returns> Returns <b>true</b> if object value is null or DBNull, otherwise <b>false</b>. </returns>
    public static bool IsNull<T>(this T value)
    {
        if (value is INullable { IsNull: true })
        {
            return true;
        }

        var type = typeof(T);
        if (type.IsValueType)
        {
            if (Nullable.GetUnderlyingType(type) is not null && value?.GetHashCode() == 0)
            {
                return true;
            }
        }
        else
        {
            if (value == null)
            {
                return true;
            }

            if (Convert.IsDBNull(value))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Unified advanced generic check for: !Nullable{T}.HasValue reference.
    /// </summary>
    /// <typeparam name="T"> Base type of object. </typeparam>
    /// <param name="value"> The object to check.  </param>
    /// <returns> Returns <b>true</b> if object value is null or DBNull, otherwise <b>false</b>. </returns>
    public static bool IsNull<T>(this T? value)
        where T : struct
    {
        return !value.HasValue;
    }

    /// <summary>
    ///     Return true if the type is a System.Nullable wrapper of a value type.
    /// </summary>
    /// <param name="type"> The type to check. </param>
    /// <returns> <b>True</b> if the type is a System.Nullable wrapper, otherwise <b>False</b>. </returns>
    public static bool IsNullable(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    /// <summary>
    ///     Throw's a <see cref="ArgumentNullException" /> when value is null.
    /// </summary>
    /// <typeparam name="T"> The type of elements in value. </typeparam>
    /// <param name="value"> The value to test. </param>
    /// <param name="paramName"> The name of the parameter that caused the exception. </param>
    /// <returns> The unchanged value. </returns>
    /// <exception cref="ArgumentNullException"> Throw when value is null. </exception>
    public static T ThrowIfNull<T>(this T? value, string? paramName = default)
    {
        return value ?? throw new ArgumentNullException(paramName, $"The value can't be null. (Parameter name: '{paramName ?? string.Empty}').");
    }
}