namespace Func;

using System.Linq;

/// <summary>
///     The CharExtensions class contain extension functions for char objects.
/// </summary>
public static class CharExtensions
{
    /// <summary>
    ///     Checks Char object's value to array of Char values.
    /// </summary>
    /// <param name="value"> Char value. </param>
    /// <param name="charValues"> Array of char values sequence which we searching specified value.  </param>
    /// <returns> True if the source sequence contains an element that has the specified value; otherwise, false. </returns>
    public static bool In(this char value, params char[] charValues)
    {
        return charValues.Contains(value);
    }
}