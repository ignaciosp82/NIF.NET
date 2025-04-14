using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NIF.NET
{
    public class NIFValidator
        : INIFValidator
    {
        public const string naturalPersonControlChars = "TRWAGMYFPDXBNJZSQVHLCKE";
        public const string legalPersonMustLetter = "PQRSWN";
        public const string legalPersonMustNumber = "ABEH";
        public const string legalPersonMustAny = "CDFGJUV";
        public const string legalPersonControlChars = "JABCDEFGHI";

        private static readonly Regex naturalPersonRegex = new Regex(@$"[0-9]{{8}}[{naturalPersonControlChars}]$");
        private static readonly Regex foreignerRegex = new Regex(@$"[XYZ][0-9]{{7}}[{naturalPersonControlChars}]$");
        private static readonly Regex legalPersonRegex = new Regex(@$"[{legalPersonMustLetter + legalPersonMustNumber + legalPersonMustAny}][0-9]{{7}}[0-9{legalPersonControlChars}]$");

        private static readonly Dictionary<char, char> foreignerFirstLetterMap = new Dictionary<char, char> { ['X'] = '0', ['Y'] = '1', ['Z'] = '2' };

        public bool IsValid(string nif)
            => IsValidNaturalPerson(nif) || IsValidLegalPerson(nif) || IsValidForeigner(nif);

        public bool IsValid(string nif, NIFType type)
            => type switch
            {
                NIFType.NaturalPerson => IsValidNaturalPerson(nif),
                NIFType.Foreigner => IsValidForeigner(nif),
                NIFType.LegalPerson => IsValidLegalPerson(nif),
                _ => false
            };

        private static bool IsValidNaturalPerson(string nif)
            => (nif = nif?.ToUpper()) != null
                && naturalPersonRegex.IsMatch(nif)
                && naturalPersonControlChars[int.Parse(nif[0..8]) % 23] == nif[^1];

        private static bool IsValidForeigner(string nif)
            => (nif = nif?.ToUpper()) != null
                && foreignerRegex.IsMatch(nif)
                && IsValidNaturalPerson($"{foreignerFirstLetterMap[nif[0]]}{nif[1..]}");

        private static bool IsValidLegalPerson(string nif)
        {
            nif = nif?.ToUpper();

            if (nif == null || !legalPersonRegex.IsMatch(nif))
            {
                return false;
            }

            var firstChar = nif[0];
            var randomNumber = nif[1..8];
            var controlChar = nif[^1];

            var controlNumber = (10 - (randomNumber.Select(CalculateCharValueLegalPerson).Sum() % 10)) % 10;

            return
                ((legalPersonMustLetter + legalPersonMustAny).Contains(firstChar) && legalPersonControlChars[controlNumber] == controlChar)
                || ((legalPersonMustNumber + legalPersonMustAny).Contains(firstChar) && CharToInt(controlChar) == controlNumber);
        }

        private static int CalculateCharValueLegalPerson(char c, int index)
        {
            var cValue = CharToInt(c);
            return index % 2 != 0 ? cValue : (cValue * 2).ToString().Sum(CharToInt);
        }

        private static int CharToInt(char c)
            => c - '0';
    }
}
