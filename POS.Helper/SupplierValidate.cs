using System;
using System.Net.Mail;

namespace POS.Helper
{
    public static class ValidateEmail
    {
        public static bool IsValid(string emailaddress)
        {
            if (string.IsNullOrWhiteSpace(emailaddress))
                return false;
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
