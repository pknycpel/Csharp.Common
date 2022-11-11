// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2020-05-11 07:20 </creationDate>
// <summary>
//      Defines the EnumerableExtensions type to implements extension method for the IEnumerable{T} interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Extensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Extensions;
using JetBrains.Annotations;

/// <summary>
///     The EnumerableExtensions to implements extension method for the <see cref="IEnumerable{T}" /> interface.
/// </summary>
[PublicAPI]
public static class EnumerableExtensions
{
    /// <summary>
    ///     Caches the an IEnumerable items.
    /// </summary>
    /// <param name="enumerable"> Enumerable collection. </param>
    /// <typeparam name="T"> The type of the elements of source. </typeparam>
    /// <returns> The <see cref="IEnumerable{T}" /> of cached elements. </returns>
    public static IEnumerable<T> Cached<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable is ICollection<T> collection)
        {
            return CachedIterator(enumerable, new List<T>(collection.Count));
        }

        return CachedIterator(enumerable, new LinkedList<T>());
    }

    /// <summary>
    ///     Enumerate each element in the enumeration and execute specified action.
    /// </summary>
    /// <typeparam name="T"> Type of enumeration. </typeparam>
    /// <param name="enumerable"> Enumerable collection. </param>
    /// <param name="action"> Action to perform. </param>
    /// <example>
    ///     string[] names = new string[] { "C#", "Java" };
    ///     names.ForEach(i => Console.WriteLine(i));.
    /// </example>
    /// <returns> The enumerable collection with performed action. </returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
            yield return item;
        }
    }

    /// <summary>
    ///     Enumerate each element in the enumeration and execute specified action.
    /// </summary>
    /// <typeparam name="T"> Type of enumeration. </typeparam>
    /// <param name="enumerable"> Enumerable collection. </param>
    /// <param name="action"> Action to perform. </param>
    /// <returns> The enumerable collection with performed action at specified type. </returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable enumerable, Action<T> action)
    {
        return enumerable.Cast<T>().ForEach(action);
    }

    /// <summary>
    ///     Enumerate each element in the enumeration and execute specified action.
    /// </summary>
    /// <param name="enumerable"> Enumerable collection. </param>
    /// <param name="action"> Action to perform. </param>
    /// <typeparam name="T"> Type of enumeration. </typeparam>
    /// <typeparam name="TResult"> Type of returned enumeration after action. </typeparam>
    /// <returns> The enumerable collection with performed action. </returns>
    public static IEnumerable<TResult> ForEach<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> action)
    {
        return enumerable.Select(action).Where(obj => obj != null).ToList();
    }

    /// <summary>
    ///     Returns the index of the first occurrence in a sequence by using the default equality comparer.
    /// </summary>
    /// <typeparam name="T"> The type of the elements of source. </typeparam>
    /// <param name="enumerable"> A sequence in which to locate a value. </param>
    /// <param name="value"> The object to locate in the sequence. </param>
    /// <returns> The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1. </returns>
    public static int IndexOf<T>(this IEnumerable<T> enumerable, T value)
        where T : IEquatable<T>
    {
        return enumerable.IndexOf(value, EqualityComparer<T>.Default);
    }

    /// <summary>
    ///     Returns the index of the first occurrence in a sequence by using a specified <see cref="IEqualityComparer" />.
    /// </summary>
    /// <typeparam name="T"> The type of the elements of source. </typeparam>
    /// <param name="enumerable"> A sequence in which to locate a value. </param>
    /// <param name="value"> The object to locate in the sequence. </param>
    /// <param name="comparer"> An equality comparer to compare values. </param>
    /// <returns> The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1. </returns>
    public static int IndexOf<T>(this IEnumerable<T> enumerable, T value, IEqualityComparer<T> comparer)
    {
        var index = 0;
        foreach (var item in enumerable)
        {
            if (comparer.Equals(item, value))
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    /// <summary>
    ///     Determines whether a collection is not null or has elements without having to enumerate the entire collection to
    ///     get
    ///     a count. Uses LINQ.
    /// </summary>
    /// <typeparam name="T"> The type of the elements of source. </typeparam>
    /// <param name="enumerable"> Source enumerable collection. </param>
    /// <returns> <c>true</c> if this list is not null and not empty; otherwise, <c>false</c>. </returns>
    public static bool IsNotNullAndEmpty<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable is not null && enumerable.Any();
    }

    /// <summary>
    ///     Determines whether a collection is null or has no elements without having to enumerate the entire collection to get
    ///     a count. Uses LINQ.
    /// </summary>
    /// <typeparam name="T"> The type of the elements of source. </typeparam>
    /// <param name="enumerable"> Source enumerable collection. </param>
    /// <returns> <c>true</c> if this list is null or empty; otherwise, <c>false</c>. </returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable is null || !enumerable.Any();
    }

    /// <summary>
    ///     Convert a <see cref="IEnumerable{T}" /> to a <see cref="Collection{T}" />.
    /// </summary>
    /// <param name="enumerable"> Enumerable collection. </param>
    /// <typeparam name="T"> Type of enumeration. </typeparam>
    /// <example>
    ///     var someArray = new int[] {1,2,3,4,5,6,7,8}
    ///     var numbers = (from item in someArray
    ///     where item > 4
    ///     select item).ToCollection();.
    /// </example>
    /// <returns> The <see cref="Collection{T}" /> that contains elements from the input sequence. </returns>
    public static Collection<T> ToCollection<T>(this IEnumerable<T> enumerable)
    {
        var collection = new Collection<T>();
        foreach (var i in enumerable)
        {
            collection.Add(i);
        }

        return collection;
    }

    /// <summary>
    ///     Converts an IEnumerable to a HashSet.
    /// </summary>
    /// <typeparam name="T"> Type of enumeration. </typeparam>
    /// <param name="enumerable"> Enumerable collection. </param>
    /// <returns> The <see cref="HashSet{T}" /> that contains elements from the input sequence. </returns>
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
    {
        var hashSet = new HashSet<T>();
        foreach (var item in enumerable)
        {
            hashSet.Add(item);
        }

        return hashSet;
    }

    /// <summary>
    ///     Converts bytes collection to hexadecimal representation.
    /// </summary>
    /// <param name="enumerable"> Bytes to convert. </param>
    /// <returns> Hexadecimal representation string. </returns>
    public static string ToHexString(this IEnumerable<byte> enumerable)
    {
        return string.Join(string.Empty, enumerable.Select(b => ("0" + b.ToString("X")).PadRight(2)));
    }

    /// <summary>
    ///     Convert <see cref="IEnumerable{T}" /> to array of <see cref="object" />.
    /// </summary>
    /// <param name="items"> The items to convert. </param>
    /// <typeparam name="T"> The type of elements in value. </typeparam>
    /// <returns> The new array of <see cref="object" />. </returns>
    public static object?[] ToObjectsArray<T>(this IEnumerable<T>? items)
    {
        return Array.ConvertAll(items.ThrowIfNull(nameof(items)).ToArray(), obj => (object?)obj);
    }

    /// <summary>
    ///     Convert a <see cref="IEnumerable{T}" /> to a <see cref="ObservableCollection{T}" />.
    /// </summary>
    /// <param name="enumerable"> Enumerable collection. </param>
    /// <typeparam name="T"> Type of enumeration. </typeparam>
    /// <example>
    ///     var list = new ObservableCollection{Employee}()
    ///     list = list.OrderBy(emp => emp.Salary).ToObservableCollection();.
    /// </example>
    /// <returns> The <see cref="ObservableCollection{T}" /> that contains elements from the input sequence. </returns>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
    {
        var collection = new ObservableCollection<T>();
        foreach (var item in enumerable)
        {
            collection.Add(item);
        }

        return collection;
    }

    /// <summary>
    ///     Create read only collection of any enumeration.
    /// </summary>
    /// <typeparam name="T"> Type of enumeration. </typeparam>
    /// <param name="enumerable"> Enumerable collection. </param>
    /// <returns> <see cref="ReadOnlyCollection{T}" /> of the enumerable collection. </returns>
    public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> enumerable)
    {
        return new List<T>(enumerable).AsReadOnly();
    }

    /// <summary>
    ///     Concatenates a specified separator String between each element of a specified enumeration, yielding a single
    ///     concatenated string.
    /// </summary>
    /// <example>
    ///     var i = new int[] { 5, 12, 44, -4 };
    ///     Console.WriteLine(i.ToString(":"));.
    /// </example>
    /// <typeparam name="T"> The type of the elements of source. </typeparam>
    /// <param name="enumerable"> Enumerable collection. </param>
    /// <param name="separator"> The specified separator. </param>
    /// <returns> The string consisting of the elements of value interspersed with the separator string. </returns>
    public static string ToString<T>(this IEnumerable<T> enumerable, string separator)
    {
        var stringBuilder = new StringBuilder();
        foreach (var obj in enumerable)
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(separator);
            }

            stringBuilder.Append(obj);
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    ///     The cached iterator.
    /// </summary>
    /// <param name="enumerable"> Enumerable collection. </param>
    /// <param name="cacheCollection"> The cache collection. </param>
    /// <typeparam name="T"> The type of the elements of source. </typeparam>
    /// <returns> The <see cref="IEnumerable{T}" /> of cached elements. </returns>
    private static IEnumerable<T> CachedIterator<T>(IEnumerable<T> enumerable, ICollection<T> cacheCollection)
    {
        foreach (var item in cacheCollection)
        {
            yield return item;
        }

        foreach (var item in enumerable)
        {
            cacheCollection.Add(item);
            yield return item;
        }
    }
}