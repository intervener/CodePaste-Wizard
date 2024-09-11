// FileOperations.cs
using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CodePasteWizard
{
    public static class FileOperations
    {
        public static string DetectFilename(string code)
        {
            string pattern = @"^//\s*(.+\.(cs|ts|js|html|css|svg))";
            Match match = Regex.Match(code, pattern, RegexOptions.Multiline);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            return string.Empty;
        }

        public static string PromptForFilename()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "All Files (*.*)|*.*";
                saveFileDialog.Title = "Save Code File";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return saveFileDialog.FileName;
                }
            }

            return string.Empty;
        }
    }
}