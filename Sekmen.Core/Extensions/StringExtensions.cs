using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Sekmen.Core.Extensions
{
    public static class StringExtensions
    {
        private static readonly string[] CharsToRemove = { "ı", "ş", "ö", "ç", "ğ", "ü", "İ", "Ş", "Ö", "Ç", "Ğ", "Ü" };
        private static readonly string[] CharToReplace = { "i", "s", "o", "c", "g", "u", "I", "S", "O", "C", "G", "U" };

        /// <summary>
        /// thisString.Equals(otherString, StringComparison.InvariantCultureIgnoreCase)
        /// </summary>
        /// <param name="thisString"></param>
        /// <param name="otherString"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string thisString, string otherString)
        {
            return thisString.Equals(otherString, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Convert Excel Col Names to Model Names
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FixColNames(this string input)
        {
            //make string camel case and remove spaces
            input = input.Replace("/", " ").ToCamelCase().Replace(" ", "").Replace(".", "");
            //replace all characters with replacements
            return input.FixTurkishLetters();
        }

        /// <summary>
        /// Replaces turkish letters with latin ones
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FixTurkishLetters(this string input)
        {
            for (int i = 0; i < CharsToRemove.Length; i++)
            {
                input = input.Replace(CharsToRemove[i], CharToReplace[i]);
            }
            return input;
        }

        /// <summary>
        /// Removes HTML codes from string
        /// </summary>
        /// <param name="htmlCode">string with HTML</param>
        /// <returns>string</returns>
        public static string HtmlToText(this string htmlCode)
        {
            // Remove tab spaces
            htmlCode = htmlCode.Replace("\t", " ");

            // Remove multiple white spaces from HTML
            htmlCode = Regex.Replace(htmlCode, "\\s+", " ");

            // Remove HEAD tag
            htmlCode = Regex.Replace(htmlCode, "<head.*?</head>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Remove any JavaScript
            htmlCode = Regex.Replace(htmlCode, "<script.*?</script>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Remove any CSS
            htmlCode = Regex.Replace(htmlCode, "<style.*?</style>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Replace special characters like &, <, >, " etc.
            StringBuilder sbHtml = new StringBuilder(htmlCode);
            // Note: There are many more special characters, these are just
            // most common. You can add new characters in this arrays if needed
            string[] oldWords = { "&nbsp;", "&amp;", "&quot;", "&lt;", "&gt;", "&reg;", "&copy;", "&bull;", "&trade;" };
            string[] newWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢" };
            for (int i = 0; i < oldWords.Length; i++)
            {
                sbHtml.Replace(oldWords[i], newWords[i]);
            }

            // Check if there are line breaks (<br>) or paragraph (<p>)
            sbHtml.Replace("<br>", "\r\n<br>");
            sbHtml.Replace("<br ", "\r\n<br ");
            sbHtml.Replace("<p ", "\r\n<p ");

            Regex urlTagPattern = new Regex(@"<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)<\/a>", RegexOptions.IgnoreCase);
            MatchCollection urls = urlTagPattern.Matches(sbHtml.ToString());
            foreach (Match url in urls)
            {
                sbHtml.Replace(url.Groups[0].ToString(), url.Groups[2] + ": " + url.Groups[1] + " ");
            }

            // Finally, remove all other HTML tags and return plain text
            return Regex.Replace(sbHtml.ToString(), "<[^>]*>", "");
        }

        /// <summary>
        /// Wraps DateTime.TryParse() and all the other kinds of code you need to determine if a given string holds a value that can be converted into a DateTime object.
        /// </summary>
        public static bool IsDate(this string input, string format = "dd.MM.yyyy")
        {
            return !string.IsNullOrEmpty(input) && DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _);
        }

        /// <summary>
        /// Checks if string is an email
        /// </summary>
        /// <param name="email">string</param>
        /// <returns>bool</returns>
        public static bool IsEmail(this string email)
        {
            try
            {
                const string defaultEmailValidationRegEx = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                bool result = Regex.IsMatch(email, defaultEmailValidationRegEx);
                if (!result)
                {
                    throw new Exception("regex");
                }

                //check as string
                MailAddress address = new MailAddress(email);
                result = new EmailAddressAttribute().IsValid(email);
                if (!result)
                {
                    throw new Exception("attribute");
                }

                string domain = email.Split('@')[1];
                if (domain.Contains("kizilay") || domain.Contains("crema.com.tr"))
                {
                    return true;
                }
                //if (GlobalConstants.TemporaryEmailDomains != null && GlobalConstants.TemporaryEmailDomains.Contains(domain))
                //{
                //    throw new Exception("temp");
                //}

                ////check emails domain
                //bool dns = CheckDnsEntries(address.Host);

                //return
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 11 haneli bir rakamdır.
        /// 0'la başlayamaz.
        /// ilk 10 rakamın toplamının birler basamağı, son rakama eşittir
        ///
        /// Wikipedia:
        /// 1, 3, 5, 7 ve 9. rakamın toplamının 7 katı ile 2, 4, 6 ve 8. rakamın toplamının 9 katının toplamının birler basamağı 10. rakamı;
        /// 1, 3, 5, 7 ve 9. rakamın toplamının 8 katının birler basamağı 11. rakamı vermektedir.
        ///
        /// Program olarak düşünürsek, ilk rakam 0. index olduğuna göre:
        /// 0, 2, 4, 6, 8. rakamın(çift haneler 10 hariç) toplamının 7 katı ile 1, 3, 5, 7.(tek haneler 9 ve 11 hariç) toplamının 9 katının toplamının birler basamağı(mod10) sondan bir önceki rakama eşittir
        /// 0, 2, 4, 6, 8. rakamın toplamının 8 katının birler basamağı(mod10) son rakama eşittir
        ///
        /// https://gist.github.com/HasanAliKaraca/90a2011c3e649108c551a4b63376db63
        /// </summary>
        public static bool IsTcValid(this string tcNo)
        {
            bool isNumber = long.TryParse(tcNo, out long _);
            bool hasElevenChar = tcNo.Length == 11;
            bool isNotStartWithZero = tcNo.StartsWith("0") == false;

            // şartlar sağlanmıyorsa devam etme
            if (isNumber == false || hasElevenChar == false || isNotStartWithZero == false)
            {
                return false;
            }

            bool isValid = false;

            int first10NumberSum = 0;
            int evenNumbersSum = 0;
            int oddNumbersSum = 0;
            for (int i = 0; i < tcNo.Length; i++)
            {
                int n = (int)char.GetNumericValue(tcNo[i]); // değerini al

                bool isLastNo = i == 10;
                if (!isLastNo)
                {
                    first10NumberSum += n;
                }

                bool isEven = i % 2 == 0;
                if (isEven && i <= 8) // i = 0,2,4,6,8
                {
                    evenNumbersSum += n;
                }

                bool isOdd = i % 2 != 0;
                if (isOdd && i < 8) // i = 1,3,5,7
                {
                    oddNumbersSum += n;
                }
            }

            int numberBeforeLastNumber = (int)char.GetNumericValue(tcNo[9]);
            int lastNumber = (int)char.GetNumericValue(tcNo[10]);

            // ilk 10 rakamın toplamının birler basamağı, son rakama eşittir
            bool isFirst10NumberSumEqualLastNumber = ((first10NumberSum % 10) == lastNumber);

            // ( çift haneler * 7 + tek haneler * 9 ) mod 10 = sondan bir önceki
            bool isRuleTrue = ((evenNumbersSum * 7 + oddNumbersSum * 9) % 10) == numberBeforeLastNumber;

            // çift haneler toplamının 8 katının birler basamağı(mod10) son rakama eşittir
            bool isEvenRuleTrue = (evenNumbersSum * 8) % 10 == lastNumber;

            // extra
            bool rule = (evenNumbersSum * 7 - oddNumbersSum) % 10 == numberBeforeLastNumber;
            bool isLastNumberEven = lastNumber % 2 == 0;

            if (isFirst10NumberSumEqualLastNumber && isEvenRuleTrue && isRuleTrue && rule && isLastNumberEven)
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// IP validator of some kind
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsValidIp(this string ip)
        {
            if (!Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {
                return false;
            }

            string[] ips = ip.Split('.');
            if (ips.Length == 4 || ips.Length == 6)
            {
                return int.Parse(ips[0]) < 256 && int.Parse(ips[1]) < 256
                       & int.Parse(ips[2]) < 256 & int.Parse(ips[3]) < 256;
            }
            return false;
        }

        /// <summary>
        /// Phone validator of some kind
        /// +905578080312
        /// 00905578080312
        /// 0090 557 808 03 12
        /// +90 557 808 0312
        /// </summary>
        /// <param name="phone">string</param>
        /// <returns>bool</returns>
        public static bool IsValidPhone(this string phone)
        {
            bool result = Regex.IsMatch(phone, "^((\\+|0{1,2})?[ ]?(90|0)?[ ]?(\\d{3})[ ]?(\\d{3})[ ]?(\\d{2})[ ]?(\\d{2}))$");
            return result;
        }

        /// <summary>
        /// URL validator of some kind
        /// Usage: "https://www.danylkoweb.com/".IsValidUrl(); // returns: true
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsValidUrl(this string text)
        {
            Regex rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return rx.IsMatch(text);
        }

        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        public static string Left(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(0, length) : value;
        }

        /// <summary>
        /// Removes last n chars from a string
        /// </summary>
        public static string RemoveFromEnd(this string instr, int number = 1)
        {
            return number >= instr.Length ? "" : instr.Substring(0, instr.Length - number);
        }

        /// <summary>
        /// Removes first n chars from a string
        /// </summary>
        public static string RemoveFromStart(this string instr, int number = 1)
        {
            return number >= instr.Length ? "" : instr.Substring(number);
        }

        /// <summary>
        /// Returns characters from right of specified length
        /// </summary>
        public static string Right(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(value.Length - length) : value;
        }

        /// <summary>
        /// Remove HTML tags from string
        /// https://stackoverflow.com/a/30026144/3950877
        /// </summary>
        public static string RemoveHtmlTags(this string strHtml)
        {
            string strText = Regex.Replace(strHtml, "<(.|\n)*?>", string.Empty);
            strText = HttpUtility.HtmlDecode(strText);
            strText = Regex.Replace(strText, @"\s+", " ");
            return strText;
        }

        /// <summary>
        /// Converts string to Camel Case
        /// </summary>
        /// <param name="str"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string str, string culture = "en-US")
        {
            CultureInfo cultureInfo = new CultureInfo(culture, true);
            // If there are 0 or 1 characters, just return the string.
            if (str == null)
            {
                return null;
            }

            if (str.Length < 2)
            {
                return str.ToUpper(cultureInfo);
            }

            str = str.ToLower();

            // Split the string into words.
            string[] words = str.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            return words.Aggregate("", (current, t) => current + (current == "" ? "" : " ") + t.Substring(0, 1).ToUpper(cultureInfo) + t.Substring(1));
        }

        /// <summary>
        /// Converts string to Title Case
        /// Converts the specified string to title case (except for words that are entirely in uppercase, which are considered to be acronyms)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string input, string culture = "en-US")
        {
            TextInfo txtInfo = new CultureInfo(culture, false).TextInfo;
            input = txtInfo.ToTitleCase(input);
            return input;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/23572631/how-can-i-truncate-a-string-using-mvc-html-helpers
        /// truncate a long string for display
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <param name="endsWith"></param>
        /// <returns></returns>
        public static string Truncate(this string source, int length = 100, string endsWith = "...")
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length) + endsWith;
            }

            return source;
        }

        public static string TruncateAtWord(this string value, int length, string endsWith = "...")
        {
            if (value == null || value.Length < length || value.IndexOf(" ", length, StringComparison.InvariantCulture) == -1)
                return value;

            return value.Substring(0, value.IndexOf(" ", length, StringComparison.InvariantCulture)) + endsWith;
        }

        /// <summary>
        /// Counts words in a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int WordCount(this string str)
        {
            return str.Split(new[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
