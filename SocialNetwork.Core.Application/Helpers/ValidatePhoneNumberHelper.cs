using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Helpers
{
    public static class ValidatePhoneNumberHelper
    {
        public static bool ValidateRDPhoneNumber(string phoneNumber)
        {
            Regex regex = new Regex(@"^\+?\d.\s?\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}?", RegexOptions.IgnoreCase);
            return regex.IsMatch(phoneNumber);
        }

    }
}
