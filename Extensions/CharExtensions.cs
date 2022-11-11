// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CharExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2022-11-06 09:14 </creationDate>
// <summary>
//      Defines the CharExtensions type to provides a implementation of extensions method char data types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Func;

using System.Linq;
using JetBrains.Annotations;

/// <summary>
///     The CharExtensions class contain extension functions for char objects.
/// </summary>
public static class CharExtensions
{
    /// <summary>
    ///     Checks Char object's value to array of Char values.
    /// </summary>
    /// <param name="value"> Value of char to check. </param>
    /// <param name="charValues"> Array of char values sequence which we searching specified value.  </param>
    /// <returns> True if the source sequence contains an element that has the specified value; otherwise, false. </returns>
    [PublicAPI]
    public static bool In(this char value, params char[] charValues)
    {
        return charValues.Contains(value);
    }
}