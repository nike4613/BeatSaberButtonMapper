using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeatSaberMod.Misc
{
    public static class EnumExtensions
    {
        public static string ToNiceName(this Enum enu) =>
            Utilities.AddSpacesToSentence(enu.ToString(), true);
    }
}
