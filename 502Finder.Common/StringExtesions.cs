using System.Globalization;
using _502Finder.Models.Enums;

namespace _502Finder.Common
{
    public static class StringExtesions
    {
        public static StrainType ToStrainType(this string source)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            string sativa = StrainType.Sativa.ToString();
            string indica = StrainType.Indica.ToString();
            string hybrid = StrainType.Hybrid.ToString();
            string concentrate = StrainType.Concentrate.ToString();

            switch (source.ToLowerInvariant())
            {
                case "sativa":
                    return StrainType.Sativa;

                case "indica":
                    return StrainType.Indica;

                case "hybrid":
                    return StrainType.Hybrid;

                case "concentrate":
                    return StrainType.Concentrate;

                default:
                    return StrainType.Unknown;
            }
        }

        public static string ToStrainTitle(this string source)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            string sativa = StrainType.Sativa.ToString();
            string indica = StrainType.Indica.ToString();
            string hybrid = StrainType.Hybrid.ToString();
            string concentrate = StrainType.Concentrate.ToString();

            switch (source.ToLowerInvariant())
            {
                case "sativa":
                    return textInfo.ToTitleCase(sativa);

                case "indica":
                    return textInfo.ToTitleCase(indica);

                case "hybrid":
                    return textInfo.ToTitleCase(hybrid);

                case "concentrate":
                    return textInfo.ToTitleCase(concentrate);

                default:
                    return "Unknown";
            }
        }

        public static DispensaryType ToDispensaryType(this string source)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            string medical = DispensaryType.Medical.ToString();
            string recreational = DispensaryType.Recreational.ToString();
            string recreationalAndMedical = DispensaryType.RecreationalAndMedical.ToString();

            switch (source.ToLowerInvariant())
            {
                case "medicinal":
                    return DispensaryType.Medical;

                case "recreational":
                    return DispensaryType.Recreational;

                case "recreationalAndMedical":
                    return DispensaryType.RecreationalAndMedical;

                default:
                    return DispensaryType.Unknown;
            }
        }

        public static string ToDispensaryTitle(this string source)
        {
            string name = (new CultureInfo("en-US", false).TextInfo).ToTitleCase(source);

            // TODO: This need to be altered to support weird dispensary names
            // For instance: ABC Greener Tomorrow becomes Abc Greener Tomorrow.
            // I'm sure if the business takes the time to uppercase it, we should to.

            return name;
        }
    }
}
