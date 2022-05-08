using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// ATEM (All The Extension Methods) is a C# class library containing a number of extension methods that
/// I personally find really useful and find myself writing into nearly every project I make, so I decided
/// to release them for people to hopefully make good use of.
/// 
/// Do note that there are some... interesting methods in here, but they may be of use.
/// </summary>
namespace ATEM
{
    /// <summary>
    /// A class containing all of the methods in ATEM
    /// </summary>
    public static partial class ATEMMethods
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
        /// <param name="startIndex">The zero-based starting element position of a subarray in this instance</param>
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
            }
            else if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "length is less than zero");
            }
            else if (startIndex + length > array.Length)
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
        /// <param name="startIndex">The zero-based starting element position of a subarray in this instance</param>
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
        public static T GetFromLast<T>(this T[] array, int index)
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
        public static T SetFromLast<T>(this T[] array, int index, T value)
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
        public static T GetFromLast<T>(this List<T> list, int index)
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
        public static T SetFromLast<T>(this List<T> list, int index, T value)
        {
            list[list.Count - index - 1] = value;
            return value;
        }
        #endregion
        #endregion

        #region Swap
        /// <summary>
        /// Swaps the position of two elements in the array
        /// </summary>
        /// <param name="index1">The index position of the first element in the array to swap</param>
        /// <param name="index2">The index position of the second element in the array to swap</param>
        /// <exception cref="IndexOutOfRangeException">One of the indexes was larger than the length of the array.</exception>
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            if (index1 >= array.Length || index2 >= array.Length)
                throw new IndexOutOfRangeException("One of the indexes is greater than or equal to the length of the array");
            else if (index1 != index2)
            {
                T twoplaceholder = array[index2];
                array[index2] = array[index1];
                array[index1] = twoplaceholder;
            }
        }

        #region Overloads
        /// <summary>
        /// Swaps the position of two elements in the list
        /// </summary>
        /// <param name="index1">The index position of the first element in the list to swap</param>
        /// <param name="index2">The index position of the second element in the list to swap</param>
        /// <exception cref="IndexOutOfRangeException">One of the indexes was larger than the count of the list</exception>
        public static void Swap<T>(this List<T> list, int index1, int index2)
        {
            if (index1 >= list.Count || index2 >= list.Count)
                throw new IndexOutOfRangeException("One of the indexes is greather than or equal to the count of the list");
            else if (index1 != index2)
            {
                T twoplaceholder = list[index2];
                list[index2] = list[index1];
                list[index1] = twoplaceholder;
            }
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

        #region Parse string as math formula
        // NOTE: This method is not fit for most real world cases due to how slow it is, and is only here for a proof of concept
        /// <summary>
        /// Parses a string as a mathematical formula and returns the result<br />
        /// Supported operations (executed in this order):<br />
        /// Variable substitution<br />
        /// Nested expressions ()<br />
        /// Exponents ^<br />
        /// Multiplication and division * and /<br />
        /// Addition and subtraction + and -<br />
        /// </summary>
        /// <param name="formula">The expression to execute</param>
        /// <param name="variables">Variables that might/will be in the formula</param>
        /// <param name="variable_regex">A regex for finding variables in the formula</param>
        /// <returns>The result of the formula, with the variables substituted in</returns>
        /// <exception cref="MissingVariableException">Raised when one or more variables in formula don't have a corresponding key in variables</exception>
        public static double MathParse(this string formula, IDictionary<string, double> variables, Regex variable_regex)
        {
            #region Replace variables
            List<string> missing_variables = new List<string>();
            while (true)
            {
                Match match = variable_regex.Match(formula);
                if (!match.Success)
                    break;
                string variable = match.Groups[1].Value;
                if (variables != null && variables.ContainsKey(variable))
                    formula = $"{formula.Substring(0, match.Index)}{variables[variable]}{formula.Substring(match.Index + match.Value.Length)}";
                else
                    missing_variables.Add(variable);
            }
            if (missing_variables.Count > 0)
            {
                string missing_variables_message = missing_variables[0];
                for (int i = 1; i < missing_variables.Count; i++)
                    missing_variables_message += $", {missing_variables[i]}";
                throw new MissingVariableException($"Variables were present in the formula that weren't found in the variables dictionary\nMissing variables: {missing_variables_message}");
            }
            #endregion

            // Strip whitespace
            formula = formula.Replace(" ", "");

            #region Execute brackets
            Regex bracket_regex = new Regex(@"(\(.+?\))");
            while (true)
            {
                Match match = bracket_regex.Match(formula);
                if (!match.Success)
                    break;
                string subformula = match.Groups[1].Value;
                subformula = subformula.Substring(1, subformula.Length - 2);
                formula = $"{formula.Substring(0, match.Index)}{ParseStringAsMath(subformula)}{formula.Substring(match.Index + match.Value.Length)}";
            }
            #endregion

            #region Exponents
            Regex exponent_regex = new Regex(@"(\d+(?:\.\d+)?)\^(\d+(?:\.\d+)?)");
            while (true)
            {
                Match match = exponent_regex.Match(formula);
                if (!match.Success)
                    break;
                double num = Convert.ToDouble(match.Groups[1].Value);
                double exponent = Convert.ToDouble(match.Groups[2].Value);
                formula = $"{formula.Substring(0, match.Index)}{Math.Pow(num, exponent)}{formula.Substring(match.Index + match.Value.Length)}";
            }
            #endregion

            #region Multiplication/Division
            Regex multi_div_regex = new Regex(@"(\d+(?:\.\d+)?)([*\/])(\d+(?:\.\d+)?)");
            while (true)
            {
                Match match = multi_div_regex.Match(formula);
                if (!match.Success)
                    break;
                double num_1 = Convert.ToDouble(match.Groups[1].Value);
                double num_2 = Convert.ToDouble(match.Groups[3].Value);
                double result = 0;
                switch (match.Groups[2].Value[0])
                {
                    case '*':
                        result = num_1 * num_2;
                        break;
                    case '/':
                        result = num_1 / num_2;
                        break;
                }
                formula = $"{formula.Substring(0, match.Index)}{result}{formula.Substring(match.Index + match.Value.Length)}";
            }
            #endregion

            #region Addition/Subtraction
            Regex add_sub_regex = new Regex(@"(\d+(?:\.\d+)?)([+-])(\d+(?:\.\d+)?)");
            while (true)
            {
                Match match = add_sub_regex.Match(formula);
                if (!match.Success)
                    break;
                double num_1 = Convert.ToDouble(match.Groups[1].Value);
                double num_2 = Convert.ToDouble(match.Groups[3].Value);
                double result = 0;
                switch (match.Groups[2].Value[0])
                {
                    case '+':
                        result = num_1 + num_2;
                        break;
                    case '-':
                        result = num_1 - num_2;
                        break;
                }
                formula = $"{formula.Substring(0, match.Index)}{result}{formula.Substring(match.Index + match.Value.Length)}";
            }
            #endregion

            return Convert.ToDouble(formula);
        }

        #region Overloads
        /// <summary>
        /// Parses a string as a mathematical formula and returns the result<br />
        /// Supported operations (executed in this order):<br />
        /// Variable substitution<br />
        /// Nested expressions ()<br />
        /// Exponents ^<br />
        /// Multiplication and division * and /<br />
        /// Addition and subtraction + and -<br />
        /// </summary>
        /// <param name="formula">The expression to execute</param>
        /// <param name="variables">Variables that might/will be in the formula</param>
        /// <returns>The result of the formula, with the variables substituted in</returns>
        /// <exception cref="MissingVariableException">Raised when one or more variables in formula don't have a corresponding key in variables</exception>
        public static double MathParse(this string formula, IDictionary<string, double> variables)
        {
            return formula.MathParse(variables, new Regex(@"([a-zA-Z_][a-zA-Z_\d]*)"));
        }

        /// <summary>
        /// Parses a string as a mathematical formula and returns the result<br />
        /// Supported operations (executed in this order):<br />
        /// Variable substitution<br />
        /// Nested expressions ()<br />
        /// Exponents ^<br />
        /// Multiplication and division * and /<br />
        /// Addition and subtraction + and -<br />
        /// </summary>
        /// <param name="formula">The expression to execute</param>
        /// <returns>The result of the formula, with the variables substituted in</returns>
        /// <exception cref="MissingVariableException">Raised when one or more variables is in the formula</exception>
        public static double MathParse(this string formula)
        {
            return formula.MathParse(null);
        }
        #endregion

        #region Missing variable exception
    }

    /// <summary>
    /// Represents an error in ATEMMethods.ParseStringAsMath when one or more variables are int the formula string, but don't have a corresponding entry in the variables IDictionary
    /// </summary>
    [Serializable]
    public class MissingVariableException : Exception
    {
        public MissingVariableException() { }
        public MissingVariableException(string message) : base(message) { }
        public MissingVariableException(string message, Exception inner) : base(message, inner) { }
        protected MissingVariableException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public static partial class ATEMMethods
    {
        #endregion
        #endregion
        #endregion

        #region Int methods
        #region Invert
        /// <summary>
        /// returns the number of equal and opposite distance of this from axis
        /// </summary>
        /// <param name="axis">The number to invert around</param>
        /// <returns></returns>
        public static int Invert(this int integer, int axis)
        {
            return (integer - axis) * -1 + axis;
        }
        #endregion
        #endregion

        #region Dictionary serialisation
        #region Class definition
    }

    /// <summary>
    /// A class that stores all the information in a Dictionary in a much less usable/optimised, but serialisable format.
    /// </summary>
    public partial class SerializableDictionary<TKey, TValue>
    {
        /// <summary>
        /// An array storing all the keys in the Dictionary
        /// </summary>
        public List<TKey> Keys { get; set; }

        /// <summary>
        /// An array storing all the values in the Dictionary
        /// </summary>
        public List<TValue> Values { get; set; }

        #region [] operator
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key. If the specified key is not found,
        /// a get operation throws a System.Collections.Generic.KeyNotFoundException, and
        /// a set operation creates a new element with the specified key (I know SerializableDictionary doesn't use a collection, but shut up).</returns>
        /// <exception cref="KeyNotFoundException">The Keys array does not contain key</exception>
        public TValue this[TKey key]
        {
            get
            {
                if (Keys.Contains(key))
                    for (int i = 0; i < Keys.Count; i++)
                    {
                        if (Keys[i].Equals(key))
                            return Values[i];
                    }
                throw new KeyNotFoundException();
            }

            set
            {
                if (Keys.Contains(key))
                {
                    for (int i = 0; i < Keys.Count; i++)
                        if (Keys[i].Equals(key))
                            Values[i] = value;
                }
                else
                    Add(key, value);
            }
        }
        #endregion

        /// <summary>
        /// Gets the number of key/value pairs contained in the ATEM.SerializableDictionary
        /// </summary>
        public int Count
        {
            get
            {
                return Keys.Count;
            }
        }

        /// <summary>
        /// Constructs an instance of the SerializableDictionary class
        /// </summary>
        public SerializableDictionary()
        {
            Keys = new List<TKey>();
            Values = new List<TValue>();
        }

        /// <summary>
        /// Converts to a Dictionary
        /// </summary>
        /// <returns>The resutling Dictionary</returns>
        public Dictionary<TKey, TValue> ToDictioary()
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (int i = 0; i < Keys.Count; i++)
            {
                result.Add(Keys[i], Values[i]);
            }
            return result;
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <exception cref="ArgumentNullException">key is null.</exception>
        /// <exception cref="ArgumentException">An element with the same key already exists in the ATEM.SerializableDictionary</exception>
        public void Add(TKey key, TValue value)
        {
            if (key.Equals(null))
                throw new ArgumentNullException("key is null");
            else if (Keys.Contains(key))
                throw new ArgumentException(key.ToString() + "is already a key");
            else
            {
                Keys.Add(key);
                Values.Add(value);
            }
        }

        /// <summary>
        /// Removes all keys and values from the ATEM.SerializableDictionary.
        /// </summary>
        public void Clear()
        {
            Keys.Clear();
            Values.Clear();
        }

        /// <summary>
        /// Removes the value with the specified key from the ATEM.SerializableDictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This
        /// method return false if the key is not found in Keys.</returns>
        /// <exception cref="ArgumentNullException">key is null.</exception>
        public bool Remove(TKey key)
        {
            if (key.Equals(null))
                throw new ArgumentNullException();
            else
            {
                if (Keys.Contains(key))
                {
                    Values.RemoveAt(Keys.IndexOf(key));
                    return Keys.Remove(key);
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public static partial class ATEMMethods
    {
        #endregion

        #region Dictionary.ToSerializableDictionary
        /// <summary>
        /// Converts to a SerializableDictionary object
        /// </summary>
        /// <returns>The resulting SerializableDictionary</returns>
        public static SerializableDictionary<TKey, TValue> ToSerializableDictionary<TKey, TValue>(this Dictionary<TKey, TValue> idictionary)
        {
            SerializableDictionary<TKey, TValue> result = new SerializableDictionary<TKey, TValue>();
            List<TKey> keys = new List<TKey>();
            List<TValue> values = new List<TValue>();

            foreach (TKey key in idictionary.Keys)
            {
                keys.Add(key);
                values.Add(idictionary[key]);
            }

            result.Keys = keys;
            result.Values = values;
            return result;
        }
        #endregion
        #endregion

        #region ATEM methods
        // These methods are not extension methods for another class, but are instead, methods that I, again, find useful and write again and again.
        // There's no commonality between any of these methods, but I'll do my best to keep them all organised.

        #region CopyDirectory
        /// <summary>
        /// Specifies what applicable Overload:ATEMMethods.CopyDirectory overloads should do in the event of destFolderName already existing
        /// </summary>
        [Flags]
        public enum FolderExistsResponse
        {
            /// <summary>
            /// An instance of the IOException class is thrown
            /// </summary>
            Reject,
            /// <summary>
            /// The two directories' contents will be merged into destFolderName.
            /// If both directories have a file of the same name, the one in sourceFolderName will be discarded.
            /// </summary>
            MergeKeep,
            /// <summary>
            /// The two directories' contents will be merged into destFolderName.
            /// If both directories have a file of the same name, the one in sourceFolderName will overwrite the one already in destFolderName.
            /// </summary>
            MergeOverwrite
        };

        /// <summary>
        /// Copies an existing directory and its contents to a new directory.
        /// All exceptions this can throw are exceptions thrown by System.IO.File.Copy and System.IO.Directory.CreateDirectory
        /// </summary>
        /// <param name="sourceFolderName">The directory to copy</param>
        /// <param name="destFolderName">The name of the destination directory. This cannot be an existing directory.</param>
        /// <param name="folderExistsResponse">The method by which destFolderName already existing is handled</param>
        /// <param name="copySubdirectories">If true, sourceFolderName's subdirectories and their subdirectories are also copied.</param>
        public static void CopyDirectory(string sourceFolderName, string destFolderName, FolderExistsResponse folderExistsResponse, bool copySubdirectories)
        {
            if (Directory.Exists(destFolderName))
            {
                switch (folderExistsResponse)
                {
                    case FolderExistsResponse.Reject:
                        throw new IOException('"' + destFolderName + "\" already exists");
                    default:
                        break;
                }
            }
            else
            {
                Directory.CreateDirectory(destFolderName);
            }

            foreach (string file in Directory.GetFiles(sourceFolderName))
            {
                string filename = file.Split('\\').GetFromLast(0);
                string new_path = destFolderName + '\\' + filename;
                if (File.Exists(new_path))
                {
                    if (folderExistsResponse == FolderExistsResponse.MergeKeep)
                        continue;
                    else
                        File.Delete(new_path);
                }
                File.Copy(file, new_path);
            }

            if (copySubdirectories)
            {
                foreach (string subdir in Directory.GetDirectories(sourceFolderName))
                {
                    string dirname = subdir.Split('\\').GetFromLast(0);
                    CopyDirectory(sourceFolderName + '\\' + dirname, destFolderName + '\\' + dirname, folderExistsResponse, true);
                }
            }
        }

        #region Overloads
        /// <summary>
        /// Copies an existing directory and its contents to a new directory.
        /// All exceptions this can throw are exceptions thrown by System.IO.File.Copy and System.IO.Directory.CreateDirectory
        /// </summary>
        /// <param name="sourceFolderName">The directory to copy</param>
        /// <param name="destFolderName">The name of the destination directory. This cannot be an existing directory.</param>
        /// <param name="folderExistsResponse">The method by which destFolderName already existing is handled</param>
        public static void CopyDirectory(string sourceFolderName, string destFolderName, FolderExistsResponse folderExistsResponse)
        {
            CopyDirectory(sourceFolderName, destFolderName, folderExistsResponse, false);
        }

        /// <summary>
        /// Copies an existing directory and its contents to a new directory.
        /// All exceptions this can throw are exceptions thrown by System.IO.File.Copy and System.IO.Directory.CreateDirectory
        /// </summary>
        /// <param name="sourceFolderName">The directory to copy</param>
        /// <param name="destFolderName">The name of the destination directory. This cannot be an existing directory.</param>
        public static void CopyDirectory(string sourceFolderName, string destFolderName)
        {
            CopyDirectory(sourceFolderName, destFolderName, FolderExistsResponse.Reject, false);
        }

        /// <summary>
        /// Copies an existing directory and its contents to a new directory.
        /// All exceptions this can throw are exceptions thrown by System.IO.File.Copy and System.IO.Directory.CreateDirectory
        /// </summary>
        /// <param name="sourceFolderName">The directory to copy</param>
        /// <param name="destFolderName">The name of the destination directory. This cannot be an existing directory.</param>
        /// <param name="copySubdirectories">If true, sourceFolderName's subdirectories and their subdirectories are also copied.</param>
        public static void CopyDirectory(string sourceFolderName, string destFolderName, bool copySubdirectories)
        {
            CopyDirectory(sourceFolderName, destFolderName, FolderExistsResponse.Reject, copySubdirectories);
        }
        #endregion
        #endregion
        #endregion
    }
}
