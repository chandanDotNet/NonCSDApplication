using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS.Helper
{
    public static class StringHelper
    {
        public static string GetUnescapestring(this string str)
        {
            var genreForWhereClause = str.Trim().ToLowerInvariant();
            var name = Uri.UnescapeDataString(genreForWhereClause);
            var encodingName = WebUtility.UrlDecode(name);
            return Regex.Unescape(encodingName);
        }
    }
}
