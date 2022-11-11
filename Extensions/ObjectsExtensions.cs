// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectsExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2020-05-11 10:12 </creationDate>
// <summary>
//      Defines the ObjectsExtensions type to contain extension functions for objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Extensions;

using System;
using JetBrains.Annotations;

/// <summary>
///     The ObjectsExtensions class contain extension functions for objects.
/// </summary>
[PublicAPI]
public static class ObjectsExtensions
{
    /// <summary>
    ///     Throw's a <see cref="ArgumentNullException" /> when value is null.
    /// </summary>
    /// <param name="value"> The value to test. </param>
    /// <param name="paramName"> The name of the parameter that caused the exception. </param>
    /// <exception cref="ArgumentNullException"> Throw when value is null. </exception>
    public static void ThrowIfNull([NoEnumeration] this object? value, string? paramName = default)
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName, $"The object can't be null. (Parameter name: '{paramName ?? string.Empty}').");
        }
    }

    /// <summary>
    ///     Safe convert to boolean values.
    /// </summary>
    /// <param name="originalValue"> The value to convert. </param>
    /// <returns> Boolean obj of object, if object is not boolean type return false. </returns>
    public static bool ToBoolean(this object originalValue)
    {
        if (originalValue is bool value)
        {
            return value;
        }

        return false;
    }
}