// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2020-05-14 12:45 </creationDate>
// <summary>
//      Defines the EnumExtensions type to implements extension method for the <see cref="Enum" /> objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Func;

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

/// <summary>
///     The EnumExtensions class implements extension method for the <see cref="Enum" /> objects.
/// </summary>
[PublicAPI]
public static class EnumExtensions
{
    /// <summary>
    ///     Converts Enumeration type into a dictionary of names and values.
    /// </summary>
    /// <param name="value"> Enum type. </param>
    /// <returns> The <see cref="IDictionary{TKey,TValue}" /> of names and values from <paramref name="value" />. </returns>
    public static IDictionary<string, int> EnumToDictionary(this Type value)
    {
        value.ThrowIfNull(nameof(value));

        if (!value.IsEnum)
        {
            throw new InvalidCastException("This enumeration type for convert to dictionary, is not enum.");
        }

        var names = Enum.GetNames(value);
        var values = Enum.GetValues(value);

        return (from i in Enumerable.Range(0, names.Length)
            select new
            {
                Key = names[i],
                Value = Convert.ToInt32(values.GetValue(i)),
            }).ToDictionary(k => k.Key, k => k.Value);
    }
}