using System;
using System.Text.RegularExpressions;
namespace ContactManagementSystem
{
    public static class validation
    { 
        private const string EmailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        private const string PhonePattern = @"^\(?([6-9]{1})\)?([0-9]{9})$";
        private const string FullNamePattern = @"^[A-Z][a-z]* [A-Z][a-z]*$";
        public static string ValidateEmail(string email)
        {
            bool valid = false;
            while (!valid)
            {
                if (Regex.IsMatch(email, EmailPattern))
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Invalid Email Address. Please enter a valid email address.");
                    email = Console.ReadLine();
                }
            }
            return email;
        }
        public static string ValidatePhoneNumber(string phoneNumber)
        {
            bool valid = false;
            while (!valid)
            {
                

                if (Regex.IsMatch(phoneNumber, PhonePattern))
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Invalid Phone Number. Please enter a valid 10-digit phone number starting with 6-9.");
                    phoneNumber = Console.ReadLine();
                }
            }
            return phoneNumber;
        }
        public static string ValidateFullName(string fullName)
        {
            bool valid = false;
            while (!valid)
            {
                if (Regex.IsMatch(fullName, FullNamePattern))
                { 
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Invalid Full Name. Please enter a valid Full name with First and Last names capitalized and one space between First and Last Name.");
                    fullName = Console.ReadLine();
                }
            }
            return fullName;
        }
    }
}
