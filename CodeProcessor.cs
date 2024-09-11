using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CodePasteWizard
{
    public static class CodeProcessor
    {
        private const int ComparisonLength = 100; // Characters to compare for overlap
        private const int MinimumOverlapLength = 10; // Minimum characters to consider as overlap

        public static string ProcessSnippets(List<string> snippets)
        {
            if (snippets == null || snippets.Count == 0)
                return string.Empty;

            return MergeCodeExcerpts(snippets.ToArray());
        }

        private static string MergeCodeExcerpts(string[] excerpts)
        {
            if (excerpts.Length == 0) return string.Empty;

            string mergedCode = excerpts[0];

            for (int i = 1; i < excerpts.Length; i++)
            {
                mergedCode = MergeWithOverlap(mergedCode, excerpts[i]);
            }

            return mergedCode;
        }

        private static string MergeWithOverlap(string code1, string code2)
        {
            string strippedEnd = StripWhitespace(code1.Substring(Math.Max(0, code1.Length - ComparisonLength)));
            string strippedStart = StripWhitespace(code2.Substring(0, Math.Min(ComparisonLength, code2.Length)));

            int overlapLength = FindOverlapLength(strippedEnd, strippedStart);

            if (overlapLength < MinimumOverlapLength)
            {
                return code1 + Environment.NewLine + code2;
            }

            int actualOverlapStart = code1.Length - FindActualOverlapStart(code1, code2, overlapLength);

            return code1.Substring(0, actualOverlapStart) + code2;
        }

        private static string StripWhitespace(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }

        private static int FindOverlapLength(string end, string start)
        {
            for (int i = Math.Min(end.Length, start.Length); i >= MinimumOverlapLength; i--)
            {
                if (end.EndsWith(start.Substring(0, i), StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return 0;
        }

        private static int FindActualOverlapStart(string code1, string code2, int strippedOverlapLength)
        {
            int count = 0;
            int overlapStart = code1.Length;

            while (count < strippedOverlapLength && overlapStart > 0)
            {
                overlapStart--;
                if (!char.IsWhiteSpace(code1[overlapStart]))
                {
                    count++;
                }
            }

            // Fine-tune the overlap start to avoid partial word/line splits
            while (overlapStart > 0 && !char.IsWhiteSpace(code1[overlapStart - 1]))
            {
                overlapStart--;
            }

            return code1.Length - overlapStart;
        }
    }
}