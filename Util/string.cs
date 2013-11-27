using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class StringHelpers
    {
        public static string Args(this string str, object arg0)
        {
            return String.Format(str, arg0);
        }

        public static string Args(this string str, object arg0, object arg1)
        {
            return String.Format(str, arg0, arg1);
        }

        public static string Args(this string str, object arg0, object arg1, object arg2)
        {
            return String.Format(str, arg0, arg1, arg2);
        }

        public static string Args(this string str, params object[] args)
        {
            return String.Format(str, args);
        }

        /// Like linq take - takes the first x characters
        public static string Take(this string theString, int count, bool ellipsis = false)
        {
            int lengthToTake = Math.Min(count, theString.Length);
            var cutDownString = theString.Substring(0, lengthToTake);

            if (ellipsis && lengthToTake < theString.Length)
                cutDownString += "...";

            return cutDownString;
        }

        //like linq skip - skips the first x characters and returns the remaining string
        public static string Skip(this string theString, int count)
        {
            int startIndex = Math.Min(count, theString.Length);
            var cutDownString = theString.Substring(startIndex - 1);

            return cutDownString;
        }

        //reverses the string... pretty obvious really
        public static string Reverse(this string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new String(chars);
        }

        // "a string".IsNullOrEmpty() beats string.IsNullOrEmpty("a string")
        public static bool IsNullOrEmpty(this string theString)
        {
            return string.IsNullOrEmpty(theString);
        }

        //not so sure about this one -
        //"a string {0}".Format("blah") vs string.Format("a string {0}", "blah")
        public static string With(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        //ditches html tags - note it doesnt get rid of things like &nbsp;
        public static string StripHtml(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            return Regex.Replace(html, @"<[^>]*>", string.Empty);
        }

        public static bool Match(this string value, string pattern)
        {
            return Regex.IsMatch(value, pattern);
        }

        //splits string into array with chunks of given size. not really that useful..
        public static string[] SplitIntoChunks(this string toSplit, int chunkSize)
        {
            if (string.IsNullOrEmpty(toSplit))
                return new string[] { "" };

            int stringLength = toSplit.Length;

            int chunksRequired = (int)Math.Ceiling((decimal)stringLength / (decimal)chunkSize);
            var stringArray = new string[chunksRequired];

            int lengthRemaining = stringLength;

            for (int i = 0; i < chunksRequired; i++)
            {
                int lengthToUse = Math.Min(lengthRemaining, chunkSize);
                int startIndex = chunkSize * i;
                stringArray[i] = toSplit.Substring(startIndex, lengthToUse);

                lengthRemaining = lengthRemaining - lengthToUse;
            }

            return stringArray;
        }

        public static string Join(this IEnumerable<object> array, string seperator)
        {
            if (array == null)
                return "";

            return string.Join(seperator, array.ToArray());
        }

        public static string Join(this object[] array, string seperator)
        {
            if (array == null)
                return "";

            return string.Join(seperator, array);
        }



        static char joinChar = (char)0;
        static char[] joinSeperator = new char[] { joinChar};
        static string joinStr = new string((char)0, 1);

        public static string StandardJoin(this object[] array)
        {
            if (array == null)
                return "";


            return string.Join(joinStr, array);
        }


        public static string StandardJoin(this string[] array , string joinValue)
        {
            if (array == null)
                return "";

            string[] newArray = new string[array.Length + 1];
            array.CopyTo(newArray, 0);
            newArray[array.Length] = joinValue;

            return string.Join(joinStr, newArray);
        }

        public static string[] StandardSplit(this string input)
        {
            return input.Split(joinSeperator, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
