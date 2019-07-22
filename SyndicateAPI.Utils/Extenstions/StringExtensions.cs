using System.Globalization;

namespace SyndicateAPI.Utils.Extenstions
{
    public static class StringExtensions
    {
        public static string DecodeEncodedNonAsciiCharacters(this string value)
        {
            return System.Text.RegularExpressions.Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }
    }
}
