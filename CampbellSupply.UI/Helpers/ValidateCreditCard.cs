using System;

using System.Text.RegularExpressions;

namespace CampbellSupply.Helpers
{
    public class ValidateCreditCard
    {
        public enum CreditCardTypeType
        {
            Visa,
            MasterCard,
            Discover,
            Amex
        }
        private const string cardRegex = "^(?:(?<Visa>4\\d{3})|(?<MasterCard>5[1-5]\\d{2})|(?<Discover>6011)|(?<DinersClub>(?:3[68]\\d{2})|(?:30[0-5]\\d))|(?<Amex>3[47]\\d{2}))([ -]?)(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})|(?:\\d{4}\\1\\d{4}\\1\\d{4})))$";
        public static bool PassesLuhnTest(string cardNumber)
        {
            //Clean the card number- remove dashes and spaces
            cardNumber = cardNumber.Replace("-", "").Replace(" ", "");

            //Convert card number into digits array
            int[] digits = new int[cardNumber.Length];
            int len = 0;
            while (len < cardNumber.Length)
            {
                digits[len] = Int32.Parse(cardNumber.Substring(len, 1));
                len += 1;
            }

            //Luhn Algorithm
            //Adapted from code availabe on Wikipedia at
            //http://en.wikipedia.org/wiki/Luhn_algorithm
            int sum = 0;
            bool alt = false;
            int i = digits.Length - 1;
            while (i >= 0)
            {
                int curDigit = digits[i];
                if (alt)
                {
                    curDigit *= 2;
                    if (curDigit > 9)
                    {
                        curDigit -= 9;
                    }
                }
                sum += curDigit;
                alt = !alt;
                i -= 1;
            }

            //If Mod 10 equals 0, the number is good and this will return true
            return sum % 10 == 0;
        }
        public static CreditCardTypeType? GetCardTypeFromNumber(string cardNum)
        {
            //Create new instance of Regex comparer with our
            //credit card regex patter
            Regex cardTest = new Regex(cardRegex);

            //Compare the supplied card number with the regex
            //pattern and get reference regex named groups
            GroupCollection gc = cardTest.Match(cardNum).Groups;

            //Compare each card type to the named groups to 
            //determine which card type the number matches
            if (gc[CreditCardTypeType.Amex.ToString()].Success)
            {
                return CreditCardTypeType.Amex;
            }
            else if (gc[CreditCardTypeType.MasterCard.ToString()].Success)
            {
                return CreditCardTypeType.MasterCard;
            }
            else if (gc[CreditCardTypeType.Visa.ToString()].Success)
            {
                return CreditCardTypeType.Visa;
            }
            else if (gc[CreditCardTypeType.Discover.ToString()].Success)
            {
                return CreditCardTypeType.Discover;
            }
            else
            {
                //Card type is not supported by our system, return null
                //(You can modify this code to support more (or less)
                // card types as it pertains to your application)
                return null;
            }
        }
        public static bool IsValidNumber(string cardNum, CreditCardTypeType? cardType)
        {
            //Create new instance of Regex comparer with our 
            //credit card regex pattern
            Regex cardTest = new Regex(cardRegex);

            //Make sure the supplied number matches the supplied
            //card type
            if (cardTest.Match(cardNum).Groups[cardType.ToString()].Success)
            {
                //If the card type matches the number, then run it
                //through Luhn's test to make sure the number appears correct
                if (PassesLuhnTest(cardNum))
                {
                    return true;
                }
                else
                {
                    return false;
                    //The card fails Luhn's test
                }
            }
            else
            {
                return false;
                //The card number does not match the card type
            }
        }
        public static bool IsValidNumber(string cardNum)
        {
            Regex cardTest = new Regex(cardRegex);

            //Determine the card type based on the number
            CreditCardTypeType? cardType = GetCardTypeFromNumber(cardNum);

            //Call the base version of IsValidNumber and pass the 
            //number and card type
            if (IsValidNumber(cardNum, cardType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string GetCardTestNumber(CreditCardTypeType cardType)
        {
            //Return bogus CC number that passes Luhn and format tests
            switch (cardType)
            {
                case CreditCardTypeType.Amex:
                    return "3782 822463 10005";
                case CreditCardTypeType.Discover:
                    return "6011 1111 1111 1117";
                case CreditCardTypeType.MasterCard:
                    return "5105 1051 0510 5100";
                case CreditCardTypeType.Visa:
                    return "4111 1111 1111 1111";
                default:
                    return null;
            }
        }
    }
}
