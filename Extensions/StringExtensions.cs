// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2020-05-11 07:20 </creationDate>
// <summary>
//      Defines the StringExtensions type to contain extension functions for string objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Extensions;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

/// <summary>
///     The StringExtensions class contain extension functions for string objects.
/// </summary>
[PublicAPI]
public static class StringExtensions
{
    /// <summary>
    ///     Returns a value indicating whether the specified <see cref="string" /> object occurs within the
    ///     <paramref name="value" /> string.
    ///     A parameter specifies the type of search to use for the specified string.
    /// </summary>
    /// <param name="value"> The string to search in. </param>
    /// <param name="toSeek"> The string to seek. </param>
    /// <exception cref="ArgumentNullException">
    ///     When <paramref name="value" /> is <c>null</c> or <paramref name="toSeek" /> is
    ///     <c>null</c>>.
    /// </exception>
    /// <returns>
    ///     <c>True</c> if the <paramref name="value" /> parameter occurs within the <paramref name="value" /> parameter,
    ///     or if <paramref name="value" /> is the empty string (<c>""</c>); otherwise, <c>false</c>.
    /// </returns>
    public static bool ContainsIgnoreCase(this string? value, string? toSeek)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (toSeek is null)
        {
            throw new ArgumentNullException(nameof(toSeek));
        }

        return value.Contains(toSeek, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     Replaces the format item in a specified System.String with the value equivalent
    ///     of the value of a specified System.Object instance.
    /// </summary>
    /// <param name="value"> A composite format string. </param>
    /// <param name="arg0"> An System.Object to format. </param>
    /// <returns> A copy of format in which the first format item has been replaced by the System.String equivalent of arg0. </returns>
    public static string Format(this string value, object arg0)
    {
        return string.Format(CultureInfo.DefaultThreadCurrentCulture, value, arg0);
    }

    /// <summary>
    ///     Replaces the format item in a specified System.String with the value equivalent
    ///     of the value of a specified System.Object instance.
    /// </summary>
    /// <param name="value"> A composite format string. </param>
    /// <param name="args"> An System.Object array containing zero or more objects to format. </param>
    /// <returns>
    ///     A copy of format in which the format items have been replaced by the System.String equivalent of the
    ///     corresponding instances of System.Object in args.
    /// </returns>
    public static string Format(this string value, params object[] args)
    {
        return string.Format(CultureInfo.DefaultThreadCurrentCulture, value, args);
    }

    /// <summary>
    ///     Checks string object's value to array of string values.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <param name="stringValues"> Array of string values to compare.  </param>
    /// <returns> Return true if any string value matches.  </returns>
    public static bool In(this string value, params string[] stringValues)
    {
        return stringValues.Any(otherValue => string.CompareOrdinal(value, otherValue) == 0);
    }

    /// <summary>
    ///     Determine if a given string holds a value that can be converted into a <see cref="DateTime" /> object.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <returns> <c>true</c> if this string value is date; otherwise, <c>false</c>. </returns>
    public static bool IsDate(this string? value)
    {
        return !string.IsNullOrEmpty(value) && DateTime.TryParse(value, out _);
    }

    /// <summary>
    ///     Indicates whether the specified string is null or an empty string ("").
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <returns> <c>true</c> if the value parameter is null or an empty string (""); otherwise, <c>false</c>. </returns>
    public static bool IsEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    ///     Indicates whether the specified string is not null and filled by value.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <returns>
    ///     <c>true</c> if the string parameter is not null and filed by value; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsFilled(this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    ///     Replaces NULL value string, with the specified replacement value.
    /// </summary>
    /// <param name="value"> The value. </param>
    /// <param name="alt"> The alternative string. </param>
    /// <returns> Returns alternative string if value string is null, if alternative text is null returns empty string (""). </returns>
    public static string IsNullThen(this string? value, string alt)
    {
        return value ?? alt;
    }

    /// <summary>
    ///     Checks if a string value is numeric according to you system culture.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <returns> <c>true</c> if this string value is numeric; otherwise, <c>false</c>. </returns>
    public static bool IsNumeric(this string? value)
    {
        return long.TryParse(value, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out _);
    }

    /// <summary>
    ///     Returns characters from right of specified length.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <param name="length"> Max number of characters to return. </param>
    /// <returns> Returns string from right. </returns>
    public static string Right(this string? value, int length)
    {
        if (value is not null && value.Length > length)
        {
            return value[^length..];
        }

        throw new ArgumentException($"The value can't be null or empty. (Parameter name: '{nameof(value)}').", nameof(value));
    }

    /// <summary>
    ///     Returns characters from left of specified length.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <param name="length"> Max number of characters to return. </param>`
    /// <returns> Returns string from left. </returns>
    public static string Left(this string? value, int length)
    {
        if (value is not null && value.Length > length)
        {
            return value[..length];
        }

        throw new ArgumentException($"The value can't be null or empty. (Parameter name: '{nameof(value)}').", nameof(value));
    }

    /// <summary>
    ///     Returns the first string with the non-empty non-null value.
    /// </summary>
    /// <example>
    ///     string someResult = someString1.Or(someString2).Or("foo");.
    /// </example>
    /// <param name="value"> String value. </param>
    /// <param name="alternative"> The alternative value. </param>
    /// <returns> Returns the first string with a non-empty non-null value. </returns>
    public static string Or(this string? value, string alternative)
    {
        return string.IsNullOrEmpty(value) ? alternative : value;
    }

    /// <summary>
    ///     Remove whitespaces from string.
    /// </summary>
    /// <param name="value"> The value string. </param>
    /// <returns> The string without whitespace. </returns>
    public static string RemoveWhitespace(this string value)
    {
        return new string(value.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
    }

    /// <summary>
    ///     Returns an enumerable collection of the specified type containing the substrings in this instance that are
    ///     delimited by elements of a specified Char array.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <param name="separator">
    ///     An array of Unicode characters that delimit the substrings in this instance, an empty array
    ///     containing no delimiters, or null.
    /// </param>
    /// <typeparam name="T"> The type of the element to return in the collection, this type must implement IConvertible. </typeparam>
    /// <returns>
    ///     An enumerable collection whose elements contain the substrings in this instance that are delimited by one or
    ///     more characters in separator.
    /// </returns>
    public static IEnumerable<T> SplitTo<T>(this string value, params char[] separator)
        where T : IConvertible
    {
        return value.Split(separator, StringSplitOptions.None).Select(s => (T)Convert.ChangeType(s, typeof(T)));
    }

    /// <summary>
    ///     Strip a string of the specified character.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <param name="character"> Character to remove from the string. </param>
    /// <example>
    ///     string value = "abcde";
    ///     value = value.Strip('b');  //value becomes 'acde';.
    /// </example>
    /// <returns>
    ///     A string that is equivalent to the current string except that all instances of oldValue are replaced with newValue.
    ///     If oldValue is not found in the current instance, the method returns the current instance unchanged.
    /// </returns>
    public static string Strip(this string value, char character)
    {
        return value.Replace(character.ToString(), string.Empty);
    }

    /// <summary>
    ///     Strip a string of the specified characters.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <param name="chars"> List of characters to remove from the string. </param>
    /// <example>
    ///     string value = "abcde";
    ///     value = value.Strip('a', 'd');  //value becomes 'bce';.
    /// </example>
    /// <returns>
    ///     A string that is equivalent to the current string except that all instances of oldValue are replaced with newValue.
    ///     If oldValue is not found in the current instance, the method returns the current instance unchanged.
    /// </returns>
    public static string Strip(this string value, params char[] chars)
    {
        return chars.Aggregate(value, (current, c) => current.Replace(c.ToString(), string.Empty));
    }

    /// <summary>
    ///     Strip a string of the specified substring.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <param name="subString"> Substring to remove. </param>
    /// <example>
    ///     string value = "abcde";
    ///     value = value.Strip("bcd");  //value becomes 'ae';.
    /// </example>
    /// <returns>
    ///     A string that is equivalent to the current string except that all instances of oldValue are replaced with newValue.
    ///     If oldValue is not found in the current instance, the method returns the current instance unchanged.
    /// </returns>
    public static string Strip(this string value, string subString)
    {
        return value.Replace(subString, string.Empty);
    }

    /// <summary>
    ///     Throw's a given exception when string is null or empty.
    /// </summary>
    /// <param name="value"> The string value. </param>
    /// <param name="paramName"> The name of the parameter that caused the exception. </param>
    /// <exception cref="ArgumentNullException"> Throw when string is null or empty. </exception>
    public static void ThrowIfNullOrEmpty(this string? value, string? paramName = default)
    {
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentNullException(paramName, $"This string can't be null. (Parameter name: '{paramName ?? string.Empty}').");
        }
    }

    /// <summary>
    ///     Converts a file on a given path to a byte array.
    /// </summary>
    /// <param name="fileName"> The path with file name. </param>
    /// <returns> The <see cref="byte" /> array. </returns>
    /// <exception cref="FileNotFoundException"> Throw when file at given name not found. </exception>
    public static byte[] ToBytes(this string fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new FileNotFoundException(fileName);
        }

        return File.ReadAllBytes(fileName);
    }

    /// <summary>
    ///     Converts a string into a <see cref="DateTime" /> value, if invalid returns null <see cref="DateTime" />.
    /// </summary>
    /// <param name="value"> A string containing a date and time to convert. </param>
    /// <returns>
    ///     A <see cref="DateTime" /> value that is equivalent to string representation of a date and time in value,
    ///     or null <see cref="DateTime" /> if string representation is not a date.
    /// </returns>
    public static DateTime? ToDateTime(this string value)
    {
        var isDateTime = DateTime.TryParse(value, out var result);
        return isDateTime ? result : default(DateTime?);
    }

    /// <summary>
    ///     Converts string to enum object.
    /// </summary>
    /// <typeparam name="T"> Type of enum. </typeparam>
    /// <param name="value"> String value to convert. </param>
    /// <returns> Returns enum object. </returns>
    public static T ToEnum<T>(this string value)
        where T : struct
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

#pragma warning disable CA1806

    /// <summary>
    ///     Converts a string into a <c>decimal</c> number, if invalid returns 0.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <returns> A decimal number that is equivalent to the number in value, or 0 (zero) if value is null. </returns>
    public static decimal ToDecimal(this string value)
    {
        decimal.TryParse(value, out var result);
        return result;
    }

    /// <summary>
    ///     Converts a string into a <c>double</c> number, if invalid returns 0.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <returns>
    ///     A double-precision floating-point number that is equivalent to the number in value, or 0 (zero) if value is
    ///     null.
    /// </returns>
    public static double ToDouble(this string value)
    {
        double.TryParse(value, out var result);
        return result;
    }

    /// <summary>
    ///     Converts a string into a <c>int</c> number, if invalid returns 0.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <returns> A 32-bit signed integer that is equivalent to the number in value, or 0 (zero) if value is null. </returns>
    public static int ToInt(this string value)
    {
        int.TryParse(value, out var result);
        return result;
    }

    /// <summary>
    ///     Converts a string into a <c>long</c> value, if invalid returns 0.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <returns> A 64-bit signed integer that is equivalent to the number in value, or 0 (zero) if value is null. </returns>
    public static long ToLong(this string value)
    {
        long.TryParse(value, out var result);
        return result;
    }

#pragma warning restore CA1806

    /// <summary>
    ///     Splits a string into a NameValueCollection, where each "nameValue" is separated by
    ///     the "outerSeparator". The parameter "nameValueSeparator" sets the split between Name and Value.
    ///     Example:
    ///     String value = "param1=value1;param2=value2";
    ///     NameValueCollection nvOut = value.ToNameValueCollection(';', '=');
    ///     The result is a NameValueCollection where:
    ///     key[0] is "param1" and value[0] is "value1"
    ///     key[1] is "param2" and value[1] is "value2".
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <param name="outerSeparator"> Separator for each "NameValue". </param>
    /// <param name="nameValueSeparator"> Separator for Name/Value splitting. </param>
    /// <returns> The <see cref="NameValueCollection" /> witch represents associated string keys and string values. </returns>
    public static NameValueCollection ToNameValueCollection(this string value, char outerSeparator, char nameValueSeparator)
    {
        var nameValueCollection = new NameValueCollection();
        value = value.TrimEnd(outerSeparator);

        if (value.IsFilled())
        {
            var arrStrings = value.TrimEnd(outerSeparator).Split(outerSeparator);

            foreach (var s in arrStrings)
            {
                var posSep = s.IndexOf(nameValueSeparator);
                var name = s[..posSep];
                var subValue = s[(posSep + 1)..];
                nameValueCollection.Add(name, subValue);
            }
        }

        return nameValueCollection;
    }

    /// <summary>
    ///     Truncates the string to a specified length and replace the truncated to a {...}.
    /// </summary>
    /// <param name="value"> String value. </param>
    /// <param name="maxLength"> Total length of characters to maintain before the truncate happens. </param>
    /// <returns> Truncated string. </returns>
    [PublicAPI]
    public static string Truncate(this string value, int maxLength)
    {
        const string suffix = "...";
        var truncatedString = value;

        if (maxLength <= 0)
        {
            return truncatedString;
        }

        var strLength = maxLength - suffix.Length;

        if (strLength <= 0)
        {
            return truncatedString;
        }

        if (value.Length <= maxLength)
        {
            return truncatedString;
        }

        truncatedString = value[..strLength];
        truncatedString = truncatedString.TrimEnd();
        truncatedString += suffix;
        return truncatedString;
    }
}