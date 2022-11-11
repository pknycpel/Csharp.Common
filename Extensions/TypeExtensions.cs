// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2021-10-20 20:21 </creationDate>
// <summary>
//      Defines the TypeExtensions type to contain extension functions for objects types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Extensions;

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

/// <summary>
///     The TypeExtensions class contain extension functions for objects types.
/// </summary>
[PublicAPI]
public static class TypeExtensions
{
    /// <summary>
    ///     Determine if type is simple type or complex.
    /// </summary>
    /// <param name="type"> The type to determine. </param>
    /// <returns> True if type is simple type, otherwise if is complex type, return False. </returns>
    public static bool IsSimpleType(this Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type);
        type = underlyingType ?? type;
        var simpleTypes = new List<Type>
        {
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(bool),
            typeof(string),
            typeof(char),
            typeof(Guid),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(byte[]),
        };
        return simpleTypes.Contains(type) || type.IsEnum;
    }
}