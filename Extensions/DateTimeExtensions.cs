// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2020-05-11 07:20 </creationDate>
// <summary>
//      Defines the DateTimeExtensions type to implements extension method for the <see cref="DateTime"/> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Extensions;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;

/// <summary>
///     The DateTimeExtensions to implements extension method for the <see cref="DateTime" /> class.
/// </summary>
[PublicAPI]
public static class DateTimeExtensions
{
    /// <summary>
    ///     Get date range between two dates.
    /// </summary>
    /// <example>
    ///     Get next 80 days
    ///     var dateRange = DateTime.Now.GetDateRangeTo(DateTime.Now.AddDays(80)).
    /// </example>
    /// <param name="dateTime"> The start date. </param>
    /// <param name="endDate"> The end date. </param>
    /// <returns> The <see cref="IEnumerable{DateTime}" /> of date values between two dates. </returns>
    public static IEnumerable<DateTime> GetDateRangeTo(this DateTime dateTime, DateTime endDate)
    {
        var range = Enumerable.Range(0, new TimeSpan(endDate.Ticks - dateTime.Ticks).Days);
        return range.Select(p => dateTime.Date.AddDays(p));
    }

    /// <summary>
    ///     Get the first day of month as date.
    /// </summary>
    /// <param name="dateTime"> The date witch we need to generate last day. </param>
    /// <returns> The <see cref="DateTime" /> represents first day of month. </returns>
    public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }

    /// <summary>
    ///     Get the Iso8601 standard week of year.
    /// </summary>
    /// <param name="dateTime"> The source date. </param>
    /// <returns> The week number. </returns>
    public static int GetIso8601WeekOfYear(this DateTime dateTime)
    {
        var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dateTime);
        if (day is >= DayOfWeek.Monday and <= DayOfWeek.Wednesday)
        {
            dateTime = dateTime.AddDays(3);
        }

        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    /// <summary>
    ///     Get the week of year.
    /// </summary>
    /// <param name="dateTime"> The source date. </param>
    /// <returns> The week number, if date will be null return null. </returns>
    public static int? GetIso8601WeekOfYear(this DateTime? dateTime)
    {
        return dateTime?.GetIso8601WeekOfYear();
    }

    /// <summary>
    ///     Get the last day of month as date.
    /// </summary>
    /// <param name="dateTime"> The date witch we need to generate last day. </param>
    /// <returns> The <see cref="DateTime" /> represents last day of month. </returns>
    public static DateTime GetLastDayOfMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    }

    /// <summary>
    ///     The get start of week for always uses Monday-to-Sunday weeks.
    /// </summary>
    /// <param name="dateTime"> The dateTime date. </param>
    /// <returns> The <see cref="DateTime" />. </returns>
    public static DateTime GetStartOfWeek(this DateTime dateTime)
    {
        var dayOfWeek = ((int)dateTime.DayOfWeek + 6) % 7;
        return dateTime.Date.AddDays(-dayOfWeek);
    }

    /// <summary>
    ///     Return the week difference between two date values.
    /// </summary>
    /// <param name="dateTime"> The start date. </param>
    /// <param name="endDate"> The end date. </param>
    /// <returns> The week difference between two date. </returns>
    public static int GetWeeks(this DateTime dateTime, DateTime endDate)
    {
        dateTime = dateTime.GetStartOfWeek();
        endDate = endDate.GetStartOfWeek();
        var days = (int)(endDate - dateTime).TotalDays;
        return (days / 7) + 1;
    }
}