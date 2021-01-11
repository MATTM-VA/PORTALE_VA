using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace VALib.Helpers
{
    public static class CultureHelper
    {
        public const string _it = "it-IT";
        public const string _en = "en-GB";
        
        public static bool IsValidCulture(string lang)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(lang) &&
                (lang.Equals("it-IT", StringComparison.OrdinalIgnoreCase) || 
                lang.Equals("en-GB", StringComparison.OrdinalIgnoreCase)))
                result = true;

            return result;
        }

        public static string GetImplementedCulture(string lang)
        {
            string result = "";

            switch (lang.ToLower())
            {
                case "en-gb":
                    result = "en-GB";
                    break;
                case "it-it":
                    result = "it-IT";
                    break;
                default:
                    result = "it-IT";
                    break;
            }

            return result;
        }

        public static string GetDateFormat()
        {
            string result = "";

            CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;

            switch (culture.ToString().ToLower())
            {
                case "en-gb":
                    result = "dd/MM/yyyy";
                    break;
                case "it-it":
                    result = "dd/MM/yyyy";
                    break;
                default:
                    result = "dd/MM/yyyy";
                    break;
            }

            return result;
        }

        public static CultureInfo GetCurrentCultureInfo()
        {
            return System.Globalization.CultureInfo.CurrentCulture;
        }

        public static bool IsCurrentCulture(string lang)
        {
            bool result = false;

            if (IsValidCulture(lang))
            {
                CultureInfo cultureInfo = new CultureInfo(lang);

                result = (cultureInfo.Equals(GetCurrentCultureInfo()));
            }

            return result;
        }

        public static string GetCurrentCultureShortName()
        {
            return GetCurrentCultureInfo().TwoLetterISOLanguageName;
        }
    }
}