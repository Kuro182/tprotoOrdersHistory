using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototype
{
    public class TempPasswordGenerator
    {
        public enum AuthorizationLevel
        {
            User,
            Admin,
        }

        private enum CharType
        {
            UpperCaseLetter,
            LowerCaseLetter,
            Digit,
            SpecialSymbol,
        }

        private readonly Random _random = new Random();

        public string Generate(AuthorizationLevel authorizationLevel)
        {
            if (authorizationLevel == AuthorizationLevel.Admin)
            {
                return Generate(10, CharType.UpperCaseLetter, CharType.LowerCaseLetter, CharType.Digit, CharType.SpecialSymbol);
            }
            return Generate(7, CharType.UpperCaseLetter, CharType.LowerCaseLetter, CharType.Digit);
        }

        private string Generate(int length, params CharType[] charTypes)
        {
            List<char> chars = charTypes.Select(GenerateChar).ToList();

            while (chars.Count < length)
            {
                var index = _random.Next(0, charTypes.Count());
                var charType = charTypes[index];
                var @char = GenerateChar(charType);
                chars.Add(@char);
            }

            chars = chars.OrderBy(_ => _random.NextDouble()).ToList();
            return new string(chars.ToArray());
        }

        private char GenerateChar(CharType charType)
        {
            switch (charType)
            {
                case CharType.UpperCaseLetter:
                    return GenerateUpeerCaseLetter();
                case CharType.LowerCaseLetter:
                    return GenerateLowerCaseLetter();
                case CharType.Digit:
                    return GenerateDigit();
                case CharType.SpecialSymbol:
                    return GenerateSpecialSymbol();
                default:
                    throw new Exception(/*typeof(CharType), charType*/);
            }
        }

        private char GenerateUpeerCaseLetter()
        {
            return GenerateChar('A', 'Z');
        }

        private char GenerateLowerCaseLetter()
        {
            return GenerateChar('a', 'z');
        }

        private char GenerateDigit()
        {
            return GenerateChar('0', '9');
        }

        private char GenerateChar(char from, char to)
        {
            return (char)_random.Next(from, to + 1);
        }

        private char GenerateSpecialSymbol()
        {
            const string chars = @"/*\-+@#$%!#$%^";
            var index = _random.Next(chars.Length);
            return chars[index];
        }
    }
}
