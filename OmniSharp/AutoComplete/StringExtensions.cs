﻿using System.Linq;
using OmniSharp.Configuration;

namespace OmniSharp.AutoComplete
{
    public static class StringExtensions
    {
        public static bool IsValidCompletionFor(this string completion, string partial)
        {
            return completion.IsValidCompletionStartsWithIgnoreCase(partial) || completion.IsSubsequenceMatch(partial);
        }

        public static bool IsValidCompletionStartsWithExactCase(this string completion, string partial)
        {
             return completion.StartsWith(partial);
        }

        public static bool IsValidCompletionStartsWithIgnoreCase(this string completion, string partial)
        {
             return completion.ToLower().StartsWith(partial.ToLower());
        }

        public static bool IsCamelCaseMatch(this string completion, string partial)

        {
            return new string(completion.Where(c => c >= 'A' && c <= 'Z').ToArray()).StartsWith(partial.ToUpper());
        }

        public static bool IsSubsequenceMatch(this string completion, string partial)
        {
            if (partial == string.Empty)
                return true;

            var firstLetter = partial.ToLower()[0];

            if (!(firstLetter >= 'a' && firstLetter <= 'z'))
                return false;

            completion = completion.ToUpper();
            partial = partial.ToUpper();

            int index = 0;
            foreach (char c in partial)
            {
                bool found = false;

                while (index < completion.Length)
                {
                    if (completion[index] == c)
                    {
                        found = true;
                        index++;
                        break;
                    }

                    index++;
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
