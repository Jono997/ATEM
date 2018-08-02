using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// ATEM (All The Extension Methods) is a C# class library containing a number of extension methods that
/// I personally find really useful and find myself writing into nearly every project I make, so I decided
/// to release them for people to hopefully make good use of.
/// </summary>
namespace ATEM
{
    public static partial class ATEM
    {
        #region Array methods
        // Some of these extension methods may also be usable on List objects
        #region Stitch
        /// <summary>
        /// Combines all the elements of an array into a string.
        /// </summary>
        /// <param name="joining_string">The string to put in between each element of the array</param>
        /// <returns>The elements of the array combined into a string</returns>
        public static string Stitch<T>(this T[] array, string joining_string)
        {
            switch (array.Length)
            {
                case 0:
                    return "";
                case 1:
                    return array[0].ToString();
                default:
                    {
                        string final = array[0].ToString();
                        for (int i = 1; i < array.Length; i++)
                        {
                            final += joining_string + array[i].ToString();
                        }
                        return final;
                    }
            }
        }

        #region Overloads
        /// <summary>
        /// Combines all the elements of an array into a string.
        /// </summary>
        /// <param name="joining_char">The character to put in between each element of the array</param>
        /// <returns>The elements of the array combined into a string</returns>
        public static string Stitch<T>(this T[] array, char joining_char)
        {
            return array.Stitch("" + joining_char);
        }

        /// <summary>
        /// Combines all the elements of an array into a string.
        /// </summary>
        /// <returns>The elements of the array combined into a string</returns>
        public static string Stitch<T>(this T[] array)
        {
            return array.Stitch("");
        }

        /// <summary>
        /// Combines all the elements of a list into a string.
        /// </summary>
        /// <param name="joining_string">The string to put in between each element of the array</param>
        /// <returns>The elements of the array combined into a string</returns>
        public static string Stitch<T>(this List<T> list, string joining_string)
        {
            return list.ToArray().Stitch(joining_string);
        }

        /// <summary>
        /// Combines all the elements of a list into a string.
        /// </summary>
        /// <param name="joining_char">The character to put in between each element of the array</param>
        /// <returns>The elements of the array combined into a string</returns>
        public static string Stitch<T>(this List<T> list, char joining_char)
        {
            return list.ToArray().Stitch("" + joining_char);
        }

        /// <summary>
        /// Combines all the elements of a list into a string.
        /// </summary>
        /// <returns>The elements of the array combined into a string</returns>
        public static string Stitch<T>(this List<T> list)
        {
            return list.ToArray().Stitch("");
        }
        #endregion
        #endregion

        #region SubArray
        // Method summary shamelessly ripped from substring summary
        // Also exists for the List class as the SubList extension method
        /// <summary>
        /// Retrieves a subarray from this instance. This subarray starts at a specified
        /// element position and has a specified length
        /// </summary>
        /// <param name="index">The zero-based starting element position of a subarray in this instance</param>
        /// <param name="length">The number of elements in the subarray</param>
        /// <returns>An array that is equivalent to the subarray of length length that begins at
        /// startIndex in this instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">startIndex plus length indicates a position not within this instance. -or- startIndex
        /// or length is less than zero.</exception>
        public static T[] SubArray<T>(this T[] array, int startIndex, int length)
        {
            #region Check exceptions
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex", "startIndex is less than zero");
            } else if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "length is less than zero");
            } else if (startIndex + length > array.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex plus length indicates a position not within this instance");
            }
            #endregion
            else
            {
                T[] final = new T[length];
                for (int i = 0; i < length; i++)
                {
                    final[i] = array[i + startIndex];
                }
                return final;
            }
        }

        #region Overloads
        /// <summary>
        /// Retrieves a subarray from this instance. This subarray starts at a specified
        /// element position and continues to the end of the array
        /// </summary>
        /// <param name="index">The zero-based starting element position of a subarray in this instance</param>
        /// <returns>An array that is equivalent to the subarray that begins at startIndex in this
        /// instance, or new object[] { } if startIndex is equal to the length of this
        /// instance</returns>
        /// <exception cref="ArgumentOutOfRangeException">startIndex plus length indicates a position not within this instance. -or- startIndex
        /// is less than zero.</exception>
        public static T[] SubArray<T>(this T[] array, int startIndex)
        {
            return array.SubArray(startIndex, array.Length - startIndex);
        }

        /// <summary>
        /// Retrieves a sublist from this instance. This sublist starts at a specified
        /// element position and has a specified length
        /// </summary>
        /// <param name="index">The zero-based starting element position of a sublist in this instance</param>
        /// <param name="length">The number of elements in the sublist</param>
        /// <returns>An list that is equivalent to the sublist of length length that begins at
        /// startIndex in this instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">startIndex plus length indicates a position not within this instance. -or- startIndex
        /// or length is less than zero.</exception>
        public static List<T> SubList<T>(this List<T> list, int startIndex, int length)
        {
            return list.ToArray().SubArray(startIndex, length).ToList();
        }

        /// <summary>
        /// Retrieves a sublist from this instance. This sublist starts at a specified
        /// element position and continues to the end of the list
        /// </summary>
        /// <param name="index">The zero-based starting element position of a sublist in this instance</param>
        /// <returns>An list that is equivalent to the sublist that begins at startIndex in this
        /// instance, or new object[] { } if startIndex is equal to the length of this
        /// instance</returns>
        /// <exception cref="ArgumentOutOfRangeException">startIndex plus length indicates a position not within this instance. -or- startIndex
        /// is less than zero.</exception>
        public static List<T> SubList<T>(this List<T> list, int startIndex)
        {
            return list.ToArray().SubArray(startIndex, list.Count - startIndex).ToList();
        }
        #endregion
        #endregion

        #region Negative indexing
        // If there's a way to do this via the [] operator instead, please let me know and I'll implement it ASAP
        /// <summary>
        /// Returns an element from an array, counting from the end of the array rather than the start
        /// Imagine T[i] if T[i] called the last element of the array
        /// </summary>
        /// <param name="index">The position in the array you want to get from, counting from the last element in the array. The last element is 0.</param>
        /// <returns>The requested item in the array</returns>
        public static T GetFromLast<T> (this T[] array, int index)
        {
            return array[array.Length - index - 1];
        }

        /// <summary>
        /// Sets an element to an array, counting from the end of the array rather than the start
        /// Imagine T[i] = val if T[i] called the last element of the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index">The position in the array you want to get from, counting from the last element in the array. The last element is 0.</param>
        /// <param name="value">The value to set the index position to.</param>
        /// <returns>value</returns>
        public static T SetFromLast<T> (this T[] array, int index, T value)
        {
            array[array.Length - index - 1] = value;
            return value;
        }

        #region Overloads
        // If there's a way to do this via the [] operator instead, please let me know and I'll implement it ASAP
        /// <summary>
        /// Returns an element from an array, counting from the end of the array rather than the start
        /// Imagine T[i] if T[i] called the last element of the array
        /// </summary>
        /// <param name="index">The position in the array you want to get from, counting from the last element in the array. The last element is 0.</param>
        /// <returns>The requested item in the array</returns>
        public static T GetFromLast<T> (this List<T> list, int index)
        {
            return list[list.Count - index - 1];
        }

        /// <summary>
        /// Sets an element to an array, counting from the end of the array rather than the start
        /// Imagine T[i] = val if T[i] called the last element of the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index">The position in the array you want to get from, counting from the last element in the array. The last element is 0.</param>
        /// <param name="value">The value to set the index position to.</param>
        /// <returns>value</returns>
        public static T SetFromLast<T> (this List<T> list, int index, T value)
        {
            list[list.Count - index - 1] = value;
            return value;
        }
        #endregion
        #endregion
        #endregion

        #region String methods
        #region Split overloads
        /// <summary>
        /// Returns a string array that contains the substrings in this instance that are
        /// delimited by elements of a specified Unicode character array.
        /// </summary>
        /// <param name="separator">An array of Unicode strings that delimit the substrings in this instance,
        /// an empty array that contains no delimiters, or null.</param>
        /// <returns>An array whose elements contain the substrings in this instance that are delimited
        /// by one or more strings in separator. For more information, see the Remarks
        /// section.</returns>
        public static string[] Split(this string str, params string[] separator)
        {
            return str.Split(separator, StringSplitOptions.None);
        }

        /// <summary>
        /// Returns a string array that contains the substrings in this string that are delimited
        /// by elements of a specified string array. Parameters specify the maximum number
        /// of substrings to return and whether to return empty array elements.
        /// </summary>
        /// <param name="separator">A string array that delimits the substrings in this string, an empty array that
        /// contains no delimiters, or null.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited
        /// by one or more strings in separator. For more information, see the Remarks section.</returns>
        /// <exception cref="ArgumentOutOfRangeException">count is negative.</exception>
        /// <exception cref="ArgumentException">options is not one of the System.StringSplitOptions values.</exception>
        public static string[] Split(this string str, string[] separator, int count)
        {
            return str.Split(separator, count, StringSplitOptions.None);
        }
        #endregion
        #endregion

        #region Int methods
        #region Invert
        public static int Invert(this int integer, int axis)
        {
            return (integer - axis) * -1 + axis;
        }
        #endregion
        #endregion
    }
}
