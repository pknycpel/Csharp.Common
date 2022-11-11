// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2020-05-11 07:20 </creationDate>
// <summary>
//      Defines the ListExtensions type to implements extension method for the IList{T} interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

/// <summary>
///     The ListExtensions implements extension method for the the <see cref="IList{T}" /> interface.
/// </summary>
[PublicAPI]
public static class ListExtensions
{
    /// <summary>
    ///     Adds the elements of the specified collection to the end of the <see cref="IList{T}" />.
    /// </summary>
    /// <param name="list"> Source list that implements <see cref="IList{T}" /> interface. </param>
    /// <param name="items">
    ///     The collection whose elements should be added to the <see cref="IList{T}" />.
    ///     The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.
    /// </param>
    /// <typeparam name="T"> The type of elements in the list. </typeparam>
    public static void AddRange<T>(this IList<T> list, IEnumerable<T>? items)
    {
        if (items is not null)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }

    /// <summary>
    ///     Determines whether a collection is null or has no elements without having to enumerate the entire collection to get
    ///     a count. Uses LINQ.
    /// </summary>
    /// <param name="list"> Source list that implements <see cref="IList{T}" /> interface. </param>
    /// <returns> <c>true</c> if this list is null or empty; otherwise, <c>false</c>. </returns>
    /// <typeparam name="T"> The type of elements in the list. </typeparam>
    public static bool IsNullOrEmpty<T>(this IList<T>? list)
    {
        return list == null || !list.Any();
    }

    /// <summary>
    ///     This extension method replaces an item in a collection that implements the <see cref="IList{T}" /> interface.
    /// </summary>
    /// <param name="list"> Source list that implements <see cref="IList{T}" /> interface. </param>
    /// <param name="position"> The position of the replaced item. </param>
    /// <param name="item"> The item we are going to put in its place. </param>
    /// <returns> <c>true</c> in case of a replace, <c>false</c> if failed. </returns>
    /// <typeparam name="T"> The type of elements in the list. </typeparam>
    public static bool Replace<T>(this IList<T> list, int position, T item)
    {
        if (position <= list.Count - 1)
        {
            list.RemoveAt(position);
            list.Insert(position, item);
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Sorts the elements in the entire <see cref="IList{T}" /> using the default comparer.
    /// </summary>
    /// <param name="list"> Source list that implements <see cref="IList{T}" /> interface. </param>
    /// <typeparam name="T"> The type of elements in the list. </typeparam>
    public static void Sort<T>(this IList<T> list)
    {
        if (list is List<T> secondList)
        {
            secondList.Sort();
        }
        else
        {
            var copy = new List<T>(list);
            copy.Sort();
            Copy(copy, 0, list, 0, list.Count);
        }
    }

    /// <summary>
    ///     Sorts the elements in the entire <see cref="IList{T}" /> using the specified <see cref="Comparison{T}" />.
    /// </summary>
    /// <param name="list"> Source list that implements <see cref="IList{T}" /> interface. </param>
    /// <param name="comparison"> The <see cref="Comparison{T}" /> to use when comparing elements. </param>
    /// <typeparam name="T"> The type of elements in the list. </typeparam>
    public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
    {
        if (list is List<T> secondList)
        {
            secondList.Sort(comparison);
        }
        else
        {
            var copy = new List<T>(list);
            copy.Sort(comparison);
            Copy(copy, 0, list, 0, list.Count);
        }
    }

    /// <summary>
    ///     Sorts the elements in the entire <see cref="IList{T}" /> using the specified comparer.
    /// </summary>
    /// <param name="list"> Source list that implements <see cref="IList{T}" /> interface. </param>
    /// <param name="comparer">
    ///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or null to use
    ///     the default comparer.
    /// </param>
    /// <typeparam name="T"> The type of elements in the list. </typeparam>
    public static void Sort<T>(this IList<T> list, IComparer<T> comparer)
    {
        if (list is List<T> secondList)
        {
            secondList.Sort(comparer);
        }
        else
        {
            var copy = new List<T>(list);
            copy.Sort(comparer);
            Copy(copy, 0, list, 0, list.Count);
        }
    }

    /// <summary>
    ///     Sorts the elements in a range of elements in <see cref="IList{T}" /> using the specified comparer.
    /// </summary>
    /// <param name="list"> Source list that implements <see cref="IList{T}" /> interface. </param>
    /// <param name="index"> The zero-based starting index of the range to sort. </param>
    /// <param name="count"> The length of the range to sort. The count. </param>
    /// <param name="comparer">
    ///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or null to use
    ///     the default comparer.
    /// </param>
    /// <typeparam name="T"> The type of elements in the list. </typeparam>
    public static void Sort<T>(this IList<T> list, int index, int count, IComparer<T> comparer)
    {
        if (list is List<T> secondList)
        {
            secondList.Sort(index, count, comparer);
        }
        else
        {
            var range = new List<T>(count);
            for (var i = 0; i < count; i++)
            {
                range.Add(list[index + i]);
            }

            range.Sort(comparer);
            Copy(range, 0, list, index, count);
        }
    }

    /// <summary>
    ///     Convert <see cref="IList{T}" /> to array of <see cref="object" />.
    /// </summary>
    /// <param name="items"> The items to convert. </param>
    /// <typeparam name="T"> The type of elements in value. </typeparam>
    /// <returns> The new array of <see cref="object" />. </returns>
    public static object[] ToObjectsArray<T>(this IList<T>? items)
    {
        if (items is not null)
        {
            var count = items.Count;

            var objArray = new object[count];

            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var element = items[i];
                    if (element is not null)
                    {
                        objArray[i] = element;
                    }
                }
            }

            return objArray;
        }

        throw new ArgumentNullException(nameof(items), @"List to convert can't be null !!!");
    }

    /// <summary>
    ///     Copies a range of elements from an <see cref="IList{T}" /> starting at the specified source index
    ///     and pastes them to another <see cref="IList{T}" /> starting at the specified destination index.
    ///     The length and the indexes are specified as 32-bit integers.
    /// </summary>
    /// <param name="sourceList"> The <see cref="IList{T}" /> that contains the data to copy. </param>
    /// <param name="sourceIndex">
    ///     A 32-bit integer that represents the index in the <paramref name="sourceList" /> at which
    ///     copying begins.
    /// </param>
    /// <param name="destinationList"> The <see cref="IList{T}" /> that receives the data. </param>
    /// <param name="destinationIndex">
    ///     A 32-bit integer that represents the index in the <paramref name="destinationList" />
    ///     at which storing begins.
    /// </param>
    /// <param name="count"> A 32-bit integer that represents the number of elements to copy. </param>
    /// <typeparam name="T"> The type of elements in the list. </typeparam>
    private static void Copy<T>(IList<T> sourceList, int sourceIndex, IList<T> destinationList, int destinationIndex, int count)
    {
        for (var i = 0; i < count; i++)
        {
            destinationList[destinationIndex + i] = sourceList[sourceIndex + i];
        }
    }
}