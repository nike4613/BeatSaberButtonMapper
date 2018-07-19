using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberKeyboardMapperPlugin.Utilities
{
    public static class Extensions
    {
        public static string ToNiceName(this Enum enu) =>
            AddSpacesToSentence(enu.ToString(), true);

        public static string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (text == null || text.Trim().Length == 0)
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) || char.IsNumber(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1]) && !char.IsNumber(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}
