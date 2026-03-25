using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Program
{
    public static class App
    {
        public static readonly Dictionary<string, string> Inventory = new();

        public enum appOption
        {
            ListAll = 1,
            AddOrChange,
            GetPassword,
            Delete,
            FileInfo,
            Exit
        }

        static string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Passwords.txt");

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            ReadPasswords();
            while (true)
            {
                Console.WriteLine("Please, Select an option:- ");
                Console.WriteLine("1. list all the passwords");
                Console.WriteLine("2. Add / change Passwords");
                Console.WriteLine("3. Get Password");
                Console.WriteLine("4. Delete Password");
                Console.WriteLine("5. File Info");
                Console.WriteLine("6. Exit the Program");

                if (Enum.TryParse<appOption>(Console.ReadLine(), out appOption SelectedOption))
                {
                    switch (SelectedOption)
                    {
                        case appOption.ListAll:
                            ListAllPasswords();
                            break;
                        case appOption.AddOrChange:
                            AddOrChangePasswords();
                            break;
                        case appOption.GetPassword:
                            GetPasswords();
                            break;
                        case appOption.Delete:
                            DeletePassword();
                            break;
                        case appOption.FileInfo:
                            fileInfo();
                            break;
                        case appOption.Exit:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("THX For Using the Terminal Manager :)");
                            Console.ForegroundColor = ConsoleColor.White;
                            return;
                        default:
                            Console.WriteLine("Invalid option");
                            Console.WriteLine("----------------");
                            break;
                    }
                }
            }
        }

        static void ListAllPasswords()
        {
            foreach (var item in Inventory)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($" {item.Key} = {item.Value} ");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void AddOrChangePasswords()
        {
            Console.Write("Please, Write UR Website/App Name: ");
            var WebsiteName = Console.ReadLine();
            Console.Write("Please, Write UR Password: ");
            var Password = Console.ReadLine();

            if (string.IsNullOrEmpty(WebsiteName) || string.IsNullOrEmpty(Password)) return;

            if (Inventory.ContainsKey(WebsiteName))
            {
                Inventory[WebsiteName] = Password;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{WebsiteName} Password Has Been Changed Successfully!");
            }
            else
            {
                Inventory.Add(WebsiteName, Password);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("New Password Has Been Added!");
            }
            Console.ForegroundColor = ConsoleColor.White;
            SavePasswords();
        }

        static void GetPasswords()
        {
            Console.Write("Please, Write the Website/App Name: ");
            var WebsiteName = Console.ReadLine();
            if (WebsiteName != null && Inventory.ContainsKey(WebsiteName))
                Console.WriteLine($"The Password is: {Inventory[WebsiteName]}");
            else
                Console.WriteLine("Password Not Found!");
        }

        static void DeletePassword()
        {
            Console.Write("Please, Write UR Website/App Name: ");
            var WebsiteName = Console.ReadLine();
            if (WebsiteName != null && Inventory.ContainsKey(WebsiteName))
            {
                Inventory.Remove(WebsiteName);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{WebsiteName} Password Has Been Deleted Successfully!");
                Console.ForegroundColor = ConsoleColor.White;
                SavePasswords();
            }
            else Console.WriteLine("Password Not Found!");
        }

        public static void fileInfo()
        {
            if (File.Exists(FilePath))
            {
                FileInfo info = new(FilePath);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"File: {info.FullName}");
                Console.WriteLine($"Created: {info.CreationTime}");
                Console.WriteLine($"Size: {info.Length / 1024.0:F2} KB");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else Console.WriteLine("File not found.");
        }

        private static void ReadPasswords()
        {
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var separatorIndex = line.IndexOf('=');
                    if (separatorIndex > 0)
                    {
                        var webSite = line.Substring(0, separatorIndex).Trim();
                        var encryptedPass = line.Substring(separatorIndex + 1).Trim();
                        try
                        {
                            Inventory[webSite] = EncryptionUtitlity.Decrypt(encryptedPass);
                        }
                        catch { }
                    }
                }
            }
        }

        private static void SavePasswords()
        {
            var sb = new StringBuilder();
            foreach (var pass in Inventory)
            {
                sb.AppendLine($"{pass.Key}={EncryptionUtitlity.Encrypt(pass.Value)}");
            }
            File.WriteAllText(FilePath, sb.ToString());
        }
    }
}