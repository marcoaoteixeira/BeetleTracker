using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtension {

        #region Public Static Methods

        /// <summary>
        /// Returns <paramref name="fallback"/> if <see cref="string"/> is <c>null</c>, empty or
        /// white spaces.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="fallback">The fallback <see cref="string"/>.</param>
        /// <returns>
        /// The <paramref name="source"/> if not <c>null</c>, empty or white spaces, otherwise,
        /// <paramref name="fallback"/>.
        /// </returns>
        public static string OnBlank(this string source, string fallback) =>
            string.IsNullOrWhiteSpace(source) ? fallback : source;

        /// <summary>
        /// Remove diacritics from <paramref name="source"/><see cref="string"/>. Diacritics are
        /// signs, such as an accent or cedilla, which when written above or below a letter indicates
        /// a difference in pronunciation from the same letter when unmarked or differently marked.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <returns>A new <see cref="string"/> without diacritics.</returns>
        public static string RemoveDiacritics(this string source) {
            if (string.IsNullOrWhiteSpace(source)) { return source; }

            var normalized = source.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var @char in normalized) {
                if (CharUnicodeInfo.GetUnicodeCategory(@char) != UnicodeCategory.NonSpacingMark) {
                    stringBuilder.Append(@char);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Repeats the <paramref name="source"/> N times.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="times">Times to repeat.</param>
        /// <returns>
        /// A new <see cref="string"/> representing the <paramref name="source"/> repeated N times.
        /// </returns>
        public static string Repeat(this string source, int times) {
            if (string.IsNullOrWhiteSpace(source)) { return string.Empty; }
            if (times <= 0) { return source; }

            using (var writer = new StringWriter()) {
                for (var counter = 0; counter < times; counter++) {
                    writer.Write(source);
                }
                return writer.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Transforms the <see cref="string"/> instance into a stream.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <returns>An instance of <see cref="MemoryStream"/> representing the source <see cref="string"/>.</returns>
        public static Stream ToStream(this string source) => source != null
            ? new MemoryStream(Encoding.UTF8.GetBytes(source))
            : Stream.Null;

        /// <summary>
        /// Separates a phrase by camel case.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <returns>
        /// A camel case separated <see cref="string"/> representing the source <see cref="string"/>.
        /// </returns>
        public static string CamelFriendly(this string source) {
            if (string.IsNullOrWhiteSpace(source)) { return string.Empty; }

            var result = new StringBuilder(source);
            for (var idx = source.Length - 1; idx > 0; idx--) {
                var current = result[idx];
                if ('A' <= current && current <= 'Z') {
                    result.Insert(idx, ' ');
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Slice the source <see cref="string"/> by <paramref name="characterCount"/> and adds an
        /// ellipsis HTML symbol at the end.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="characterCount">The number of characters to slice.</param>
        /// <returns>A <see cref="string"/> representation of the sliced source.</returns>
        public static string Ellipsize(this string source, int characterCount) =>
            source.Ellipsize(characterCount, "&#160;&#8230;");

        /// <summary>
        /// Slice the source <see cref="string"/> by <paramref name="characterCount"/> and adds an
        /// ellipsis defined symbol at the end.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="characterCount">The number of characters to slice.</param>
        /// <param name="ellipsis">The ellipsis symbol.</param>
        /// <param name="wordBoundary">Use word boundary to slice.</param>
        /// <returns>A <see cref="string"/> representation of the sliced source.</returns>
        public static string Ellipsize(this string source, int characterCount, string ellipsis, bool wordBoundary = false) {
            if (string.IsNullOrWhiteSpace(source)) {
                return string.Empty;
            }

            if (characterCount < 0 || source.Length <= characterCount) {
                return source;
            }

            // search beginning of word
            var backup = characterCount;
            while (characterCount > 0 && source[characterCount - 1].IsLetter()) {
                characterCount--;
            }

            // search previous word
            while (characterCount > 0 && source[characterCount - 1].IsSpace()) {
                characterCount--;
            }

            // if it was the last word, recover it, unless boundary is requested
            if (characterCount == 0 && !wordBoundary) {
                characterCount = backup;
            }

            var trimmed = source.Substring(0, characterCount);
            return string.Concat(trimmed, ellipsis);
        }

        /// <summary>
        /// Transforms a hexa <see cref="string"/> value into a <see cref="byte"/> array.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <returns>An array of <see cref="byte"/>.</returns>
        public static byte[] FromHexToByteArray(this string source) => Enumerable.Range(0, source.Length)
            .Where(_ => (_ % 2) == 0)
            .Select(_ => Convert.ToByte(source.Substring(_, 2), 16))
            .ToArray();

        /// <summary>
        /// Replaces all occurrences from <paramref name="source"/> with the values presents in
        /// <paramref name="replacements"/>.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="replacements">The replacements.</param>
        /// <returns>The replaced <see cref="string"/>.</returns>
        public static string ReplaceAll(this string source, IDictionary<string, string> replacements) {
            var pattern = string.Format("{0}", string.Join("|", replacements.Keys));
            return Regex.Replace(source, pattern, match => replacements[match.Value]);
        }

        /// <summary>
        /// Converts the <paramref name="source"/> into a Base64 <see cref="string"/> representation.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <returns>The Base64 <see cref="string"/> representation.</returns>
        public static string ToBase64(this string source) => Convert.ToBase64String(Encoding.UTF8.GetBytes(source));

        /// <summary>
        /// Converts from a Base64 <see cref="string"/> representation.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <returns>The <see cref="string"/> representation.</returns>
        public static string FromBase64(this string source) => Encoding.UTF8.GetString(Convert.FromBase64String(source));

        /// <summary>
        /// Strips a <see cref="string"/> by the specified <see cref="char"/> from <paramref name="stripped"/>.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="stripped">Stripper values</param>
        /// <returns>A stripped version of the <paramref name="source"/> parameter.</returns>
        public static string Strip(this string source, params char[] stripped) {
            if (string.IsNullOrWhiteSpace(source) || stripped.IsNullOrEmpty()) {
                return source;
            }

            var result = new char[source.Length];
            var cursor = 0;
            for (var idx = 0; idx < source.Length; idx++) {
                var current = source[idx];
                if (Array.IndexOf(stripped, current) < 0) {
                    result[cursor++] = current;
                }
            }

            return new string(result, 0, cursor);
        }

        /// <summary>
        /// Strips a <see cref="string"/> by the specified function.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="predicate">The stripper function.</param>
        /// <returns>A stripped version of the <paramref name="source"/> parameter.</returns>
        public static string Strip(this string source, Func<char, bool> predicate) {
            var result = new char[source.Length];
            var cursor = 0;

            for (var idx = 0; idx < source.Length; idx++) {
                var current = source[idx];
                if (!predicate(current)) {
                    result[cursor++] = current;
                }
            }

            return new string(result, 0, cursor);
        }

        /// <summary>
        /// Checks if there is any presence of the specified <see cref="char"/> s in the source <see cref="string"/>.
        /// </summary>
        /// <param name="source">The source <see cref="string"/></param>
        /// <param name="chars">The <see cref="char"/> s to check.</param>
        /// <returns><c>true</c> if any of the <see cref="char"/> exists, otherwise, <c>false</c>.</returns>
        public static bool Any(this string source, params char[] chars) {
            if (string.IsNullOrEmpty(source) || chars == null || chars.Length == 0) {
                return false;
            }

            for (var idx = 0; idx < source.Length; idx++) {
                var current = source[idx];
                if (Array.IndexOf(chars, current) >= 0) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if there is all presences of the specified <see cref="char"/> s in the source <see cref="string"/>.
        /// </summary>
        /// <param name="source">The source <see cref="string"/></param>
        /// <param name="chars">The <see cref="char"/> s to check.</param>
        /// <returns><c>true</c> if all of the <see cref="char"/> exists, otherwise, <c>false</c>.</returns>
        public static bool All(this string source, params char[] chars) {
            if (string.IsNullOrEmpty(source)) { return true; }
            if (chars == null || chars.Length == 0) { return false; }

            for (var idx = 0; idx < source.Length; idx++) {
                var current = source[idx];
                if (Array.IndexOf(chars, current) < 0) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Changes the specified <see cref="char"/> s of <paramref name="from"/> with the specified
        /// <see cref="char"/> s of <paramref name="to"/>.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="from">"from" <see cref="char"/> array</param>
        /// <param name="to">"to" <see cref="char"/> array</param>
        /// <returns>The translated representation of <paramref name="source"/>.</returns>
        public static string Translate(this string source, char[] from, char[] to) {
            if (string.IsNullOrEmpty(source)) { return source; }

            Prevent.ParameterNull(from, nameof(from));
            Prevent.ParameterNull(to, nameof(to));

            if (from.Length != to.Length) {
                throw new ArgumentNullException("from", "Parameters must have the same length");
            }

            var map = new Dictionary<char, char>(from.Length);
            for (var idx = 0; idx < from.Length; idx++) {
                map[from[idx]] = to[idx];
            }

            var result = new char[source.Length];
            for (var idx = 0; idx < source.Length; idx++) {
                var current = source[idx];
                result[idx] = map.ContainsKey(current)
                    ? map[current]
                    : current;
            }
            return new string(result);
        }

        /// <summary>
        /// Generates a valid technical name.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <remarks>Uses a white list set of chars.</remarks>
        public static string ToSafeName(this string source) {
            if (string.IsNullOrWhiteSpace(source)) { return string.Empty; }

            var result = RemoveDiacritics(source)
                .Strip(character => !character.IsLetter() && !Char.IsDigit(character))
                .Trim();

            // don't allow non A-Z chars as first letter, as they are not allowed in prefixes
            while (result.Length > 0 && !result[0].IsLetter()) {
                result = result.Substring(1);
            }

            if (result.Length > 128) {
                result = result.Substring(0, 128);
            }

            return result;
        }

        /// <summary>
        /// Removes all HTML tags from <paramref name="source"/><see cref="string"/>.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="htmlDecode"><c>true</c> if should HTML decode.</param>
        /// <returns></returns>
        public static string RemoveHtmlTags(this string source, bool htmlDecode = false) {
            if (string.IsNullOrEmpty(source)) {
                return string.Empty;
            }

            var content = new char[source.Length];
            var cursor = 0;
            var inside = false;
            for (var idx = 0; idx < source.Length; idx++) {
                char current = source[idx];

                switch (current) {
                    case '<':
                        inside = true;
                        continue;
                    case '>':
                        inside = false;
                        continue;
                }

                if (!inside) { content[cursor++] = current; }
            }

            var result = new string(content, 0, cursor);
            return htmlDecode
                ? WebUtility.HtmlDecode(result)
                : result;
        }

        /// <summary>
        /// Checks if the <paramref name="source"/><see cref="string"/> contains the specified value.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="contains">The text that should look for.</param>
        /// <returns><c>true</c> if contains, otherwise, <c>false</c>.</returns>
        public static bool Contains(this string source, string contains) =>
            Contains(source, contains, StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// Checks if the <paramref name="source"/><see cref="string"/> contains the specified value.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="contains">The text that should look for.</param>
        /// <param name="stringComparison">Comparison style.</param>
        /// <returns><c>true</c> if contains, otherwise, <c>false</c>.</returns>
        public static bool Contains(this string source, string contains, StringComparison stringComparison) {
            Prevent.ParameterNull(contains, nameof(contains));

            if (source == null) { return false; }

            return (source.IndexOf(contains, stringComparison) > 0);
        }

        /// <summary>
        /// Checks if the <paramref name="source"/> matchs (Regexp) a specified pattern.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="regExp">The regexp.</param>
        /// <param name="regexOptions">The regexp options.</param>
        /// <returns><c>true</c> if matchs, otherwise, <c>false</c>.</returns>
        public static bool IsMatch(this string source, string regExp, RegexOptions regexOptions = RegexOptions.None) {
            Prevent.ParameterNull(regExp, nameof(regExp));

            if (source == null) { return false; }

            return Regex.IsMatch(source, regExp, regexOptions);
        }

        /// <summary>
        /// Replaces a specified value given a regexp.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="regExp">The regexp.</param>
        /// <param name="replacement">The replacement value..</param>
        /// <param name="regexOptions">The regexp options</param>
        /// <returns>A <see cref="string"/> representing the new value.</returns>
        public static string Replace(this string source, string regExp, string replacement, RegexOptions regexOptions = RegexOptions.None) {
            Prevent.ParameterNull(regExp, nameof(regExp));
            Prevent.ParameterNull(replacement, nameof(replacement));

            if (source == null) { return source; }

            return Regex.Replace(source, regExp, replacement, regexOptions);
        }

        /// <summary>
        /// Splits the string by camel case.
        /// </summary>
        /// <param name="source">The string value.</param>
        /// <returns>An array of strings.</returns>
        public static string[] SplitCamelCase(this string source) {
            if (source == null) { return null; }

            return Regex.Replace(
                Regex.Replace(
                    source,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            ).Split(' ');
        }

        /// <summary>
        /// Splits the <paramref name="source"/><see cref="string"/> by the specified regexp.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="regExp">The regexp.</param>
        /// <param name="regexOptions">The regexp options.</param>
        /// <returns>A array of <see cref="string"/>.</returns>
        public static string[] Split(this string source, string regExp, RegexOptions regexOptions = RegexOptions.None) {
            Prevent.ParameterNull(regExp, nameof(regExp));

            if (source == null) { return new string[0]; }

            return Regex.Split(source, regExp, regexOptions);
        }

        /// <summary>
        /// Returns a <see cref="byte"/> array representation of the <paramref name="source"/><see
        /// cref="string"/>. Default encoding UTF-8.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <returns>An array of <see cref="byte"/>.</returns>
        public static byte[] GetBytes(this string source) => GetBytes(source, Encoding.UTF8);

        /// <summary>
        /// Returns a <see cref="byte"/> array representation of the <paramref name="source"/><see cref="string"/>.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="encoding">The encoding type.</param>
        /// <returns>An array of <see cref="byte"/>.</returns>
        public static byte[] GetBytes(this string source, Encoding encoding) {
            Prevent.ParameterNull(encoding, nameof(encoding));

            if (source == null) { return new byte[0]; }

            return encoding.GetBytes(source);
        }

        /// <summary>
        /// Splits the <paramref name="source"/><see cref="string"/> by camel case.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <returns>An array of <see cref="string"/>.</returns>
        /// <remarks>Source: http://haacked.com/archive/2005/09/24/splitting-pascalcamel-cased-strings.aspx</remarks>
        public static string[] SplitUpperCase(this string source) {
            if (string.IsNullOrWhiteSpace(source)) { return new string[0]; }

            var words = new StringCollection();
            var wordStartIndex = 0;
            var letters = source.ToCharArray();
            var previousChar = char.MinValue;

            // Skip the first letter. we don't care what case it is.
            for (var idx = 1; idx < letters.Length; idx++) {
                if (char.IsUpper(letters[idx]) && !char.IsWhiteSpace(previousChar)) {
                    //Grab everything before the current character.
                    words.Add(new string(letters, wordStartIndex, idx - wordStartIndex));
                    wordStartIndex = idx;
                }

                previousChar = letters[idx];
            }

            //We need to have the last word.
            words.Add(new string(letters, wordStartIndex, letters.Length - wordStartIndex));

            var wordArray = new string[words.Count];

            words.CopyTo(wordArray, 0);

            return wordArray;
        }

        /// <summary>
        /// Retrieves the MD5 for the current string.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <returns>The MD5 representation for the <paramref name="source"/>.</returns>
        public static string GetMD5(this string source) {
            if (source == null) { return null; }

            using (var provider = MD5.Create()) {
                var buffer = Encoding.UTF8.GetBytes(source);
                var result = provider.ComputeHash(buffer);
                return BitConverter.ToString(result);
            }
        }

        /// <summary>
        /// Removes the prefix.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="prefix">The specified prefix</param>
        /// <returns></returns>
        public static string TrimPrefix(this string source, string prefix) {
            if (source == null) { return null; }
            if (prefix == null) { return source; }

            return source.StartsWith(prefix, StringComparison.Ordinal)
                ? source.Substring(prefix.Length)
                : source;
        }

        /// <summary>
        /// Removes the suffix.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="suffix">The specified suffix.</param>
        /// <returns></returns>
        public static string TrimSuffix(this string source, string suffix) {
            if (source == null) { return null; }
            if (suffix == null) { return source; }

            return source.EndsWith(suffix, StringComparison.Ordinal)
                ? source.Substring(0, source.Length - suffix.Length)
                : source;
        }

        #endregion Public Static Methods
    }
}