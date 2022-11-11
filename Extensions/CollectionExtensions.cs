// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2020-05-13 13:57 </creationDate>
// <summary>
//      Defines the CollectionExtensions type to implements extension method
//      for the <see cref="ICollection{T}" /> interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Extensions;

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

/// <summary>
///     The CollectionExtensions to implements extension method for the <see cref="ICollection{T}" /> interface.
/// </summary>
[PublicAPI]
public static class CollectionExtensions
{
    /// <summary>
    ///     Adds the elements of the specified collection to the end of the <see cref="ICollection{T}" />.
    /// </summary>
    /// <param name="list"> Source list that implements <see cref="ICollection{T}" /> interface. </param>
    /// <param name="items">
    ///     The collection whose elements should be added to the <see cref="IList{T}" />.
    ///     The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.
    /// </param>
    /// <typeparam name="T"> The type of elements in the list. </typeparam>
    public static void AddRange<T>(this ICollection<T>? list, IEnumerable<T>? items)
    {
        if (items is not null && list is not null)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }

    /// <summary>
    ///     Convert <see cref="ICollection{T}" /> to array of <see cref="object" />.
    /// </summary>
    /// <param name="items"> The items to convert. </param>
    /// <typeparam name="T"> The type of elements in list. </typeparam>
    /// <returns> The new array of <see cref="object" />. </returns>
    public static object?[]? ToObjectsArray<T>(this ICollection<T>? items)
    {
        if (items is not null)
        {
            var count = items.Count;

            if (count > 0)
            {
                var objArray = new object?[count];
                for (var i = 0; i < count; i++)
                {
                    objArray[i] = items.ElementAt(i);
                }

                return objArray;
            }
        }

        return null;
    }
}