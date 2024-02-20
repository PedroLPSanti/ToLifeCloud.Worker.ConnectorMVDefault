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


        public static string GetPainDescription(int idPain)
        {
            string pain = "";
            switch (idPain)
            {
                case 2:
                case 3:
                case 4:
                case 5:
                    pain = "paciente com dor a menos de 7 dias.";
                    break;
                case 8:
                case 7:
                case 6:
                case 9:
                    pain = "paciente com dor a mais de 7 dias.";
                    break;
            }
            return pain;
        }

        public static float GetPain(int idPain)
        {
            float pain = 0;
            switch (idPain)
            {
                case 1:
                    pain = 0;
                    break;
                case 2:
                case 6:
                    pain = 1;
                    break;
                case 3:
                case 7:
                    pain = 2;
                    break;
                case 4:
                case 8:
                    pain = 3;
                    break;
                case 5:
                case 9:
                    pain = 4;
                    break;
                case 10:
                    pain = 5;
                    break;
                case 11:
                    pain = 6;
                    break;
                case 12:
                    pain = 7;
                    break;
                case 13:
                    pain = 8;
                    break;
                case 14:
                    pain = 9;
                    break;
                case 15:
                    pain = 10;
                    break;

            }
            return pain;
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
