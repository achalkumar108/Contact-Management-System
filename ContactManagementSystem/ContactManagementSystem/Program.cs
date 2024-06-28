using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
namespace ContactManagementSystem
{
    class ContactManagementSystem
    {
        public static int ctr = 0;
        static string directoryPath;
        static string userFilePath;
        static string SearchLogFile;
        static string DeleteLogFile;
        static ContactManagementSystem()
        {
            string pathTo = Assembly.GetExecutingAssembly().Location;
            DirectoryInfo Dinfo = new DirectoryInfo(pathTo).Parent.Parent.Parent;
            directoryPath = Path.Combine(Dinfo.FullName, "Output");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            userFilePath = Path.Combine(directoryPath, "user.txt");
            SearchLogFile = Path.Combine(directoryPath, "Search.txt");
            DeleteLogFile = Path.Combine(directoryPath, "Delete.txt");
        }
        public class Contact
        {           
           public string ContactID { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public Contact(string ID, string name, string email, string phone)
            {
                ContactID = ID;
                Name = name;
                Email = email;
                PhoneNumber = phone;
            }
            public override string ToString()
            {
                return $"{ContactID},{Name},{Email},{PhoneNumber}";
            }
            public static Contact FromString(string userData)
            {
                string[] data = userData.Split(',');
                return new Contact(data[0], data[1], data[2], data[3]);
            }
            public static Contact CreateUser()
            {
                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                string contactID;
                string name;
                do
                {
                    ctr++;
                    contactID = $"{currentDate}_{ctr}";
                } while (contact.users.Any(u => u.ContactID == contactID));
                Console.WriteLine("Enter Name:");
                name = validation.ValidateFullName(Console.ReadLine().Trim());              
                Console.WriteLine("Enter Email:");
                string email = validation.ValidateEmail(Console.ReadLine().Trim());
                Console.WriteLine("Enter Phone Number:");
                string phone = validation.ValidatePhoneNumber(Console.ReadLine().Trim());
                return new Contact(contactID, name, email, phone);
            }
        }
        public static class contact
        {
            public static readonly List<Contact> users = new List<Contact>();
            public static void LoadUsersFromFile()
            {
                try
                {
                    if (File.Exists(userFilePath))
                    {
                        string[] lines = File.ReadAllLines(userFilePath);
                        foreach (var line in lines)
                        {
                            Contact user = Contact.FromString(line);
                            users.Add(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load users from file: {ex.Message}");
                }
            }
            public static void SaveUsersToFile()
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(userFilePath, false))
                    {
                        foreach (var user in users)
                        {
                            writer.WriteLine(user.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save users to file: {ex.Message}");
                }
            }
            public static void AddUser(Contact user)
            {
                users.Add(user);
                SaveUsersToFile();
            }
            public static Contact FindUser(string name, string email)
            {
                return users.FirstOrDefault(u => u.Name == name && u.Email == email);
            }
            public static Contact FindUser(Func<Contact, bool> predicate)
            {
                return users.FirstOrDefault(predicate);
            }
            public static void RemoveUser(Contact user)
            {
                users.Remove(user);
                SaveUsersToFile();
            }
        }
        private static void ShowMenu()
        {
            bool var = true;
            while (var)
            {
                Console.WriteLine("Press 1: Add New Details");
                Console.WriteLine("Press 2: View All Details");
                Console.WriteLine("Press 3: Search by Name or Email");
                Console.WriteLine("Press 4: Delete By Name or Email");
                Console.WriteLine("Press 5: Exit");
                Console.WriteLine("\nChoose Your option:");
                int option;
                if (int.TryParse(Console.ReadLine(), out option))
                {
                    switch (option)
                    {
                        case 1:
                            AddDetails();
                            break;
                        case 2:
                            AllDetails();
                            break;
                        case 3:
                            Search();
                            break;
                        case 4:
                            Delete();
                            break;
                        case 5:
                            Console.WriteLine("\nThanks for visiting our portal.\n");
                            var = false;
                            break;
                        default:
                            Console.WriteLine("\nInvalid option. Please try again.\n");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please enter a number.\n");
                }
            }
        }
        private static void AllDetails()
        {
            foreach (var user in contact.users)
            {
                Console.WriteLine($"Contact ID : {user.ContactID}");
                Console.WriteLine($"Name : {user.Name}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Phone No: {user.PhoneNumber}\n");
            }
        }
        private static void AddDetails()
        {
            Contact newUser = Contact.CreateUser();
            contact.AddUser(newUser);
            Console.WriteLine("\nUser details added successfully!\n");
        }
        private static void Search()
        {
            Console.WriteLine("Enter Name or Email to search:");
            string searchTerm = Console.ReadLine().Trim();
            var foundUser = contact.FindUser(u => u.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) || u.Email.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));
            if (foundUser != null)
            {
                Console.WriteLine("\nUser found:\n");
                Console.WriteLine($"Contact ID : {foundUser.ContactID}");
                Console.WriteLine($"Name : {foundUser.Name}");
                Console.WriteLine($"Email: {foundUser.Email}");
                Console.WriteLine($"Phone No: {foundUser.PhoneNumber}\n");
            }
            else
            {
                DateTime dateTime = DateTime.Now;
                using (StreamWriter writer = File.AppendText(SearchLogFile))
                {
                    writer.WriteLine($"{searchTerm},{dateTime}");
                    Console.WriteLine("\nContact not Found!\n");
                }               
            }
        }
        private static void Delete()
        {
            Console.WriteLine("Enter Name or Email to delete:");
            string searchTerm = Console.ReadLine().Trim();
            var userToDelete = contact.FindUser(u => u.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) || u.Email.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));
            if (userToDelete != null)
            {
                contact.RemoveUser(userToDelete);
                Console.WriteLine("\nUser deleted successfully.\n");
            }
            else
            {
                DateTime dateTime = DateTime.Now;
                using (StreamWriter writer = File.AppendText(DeleteLogFile))
                {
                    writer.WriteLine($"{searchTerm},{dateTime}");
                    Console.WriteLine("\nContact not found. Deletion failed!\n");
                }               
            }
        }
        public static void Main()
        {
            contact.LoadUsersFromFile();
            ShowMenu();
            contact.SaveUsersToFile();
        }
    }
}
