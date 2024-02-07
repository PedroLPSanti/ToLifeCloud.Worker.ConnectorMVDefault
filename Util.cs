using System.Globalization;
using System.Text;

namespace ToLifeCloud.Worker.ConnectorMVDefault
{
    public static class Util
    {
        public static string GetSuspicion(int idSuspicion)
        {
            string suspicion = "";
            switch (idSuspicion)
            {
                case 1:
                    suspicion = "Chikungunya";
                    break;
                case 2:
                    suspicion = "Dengue";
                    break;
                case 3:
                    suspicion = "Sepse";
                    break;
                case 4:
                    suspicion = "Zika Vírus";
                    break;
                case 5:
                    suspicion = "COVID-19";
                    break;
                case 6:
                    suspicion = "Não possui";
                    break;
            }
            return suspicion;
        }

        public static string RemoveDiacriticsAndCase(this string text)
        {
            text = text.Trim().ToLower();
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
