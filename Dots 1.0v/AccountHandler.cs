using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Dots_1._0v
{
    public static class AccountHandler
    {
        public static Account? CurrentAccount = null;
        public static bool IsLoggedIn => CurrentAccount != null;

        public const string PassPath = @"\Users\passwords.json";
        public const string UsersPath = @"\Users\users_values.json";
        public static string ExeFullPath => Path.GetDirectoryName(Environment.ProcessPath);

        private static bool AddNewLogin(string login, string password)
        {
            string txt = File.ReadAllText(ExeFullPath + PassPath);

            Dictionary<string, string>? values = null;
            if (txt == string.Empty)
                values = new Dictionary<string, string>();
            else
                values = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(ExeFullPath + PassPath));

            if (values == null) return true;

            if (values.ContainsKey(login)) return false;

            values[login] = password;
            File.WriteAllText(ExeFullPath + PassPath, JsonSerializer.Serialize(values));
            return true;
        }

        //
        private static string CheckUniqueness(string login, string email)
        {
            string[] lines = File.ReadAllLines(ExeFullPath + UsersPath);

            foreach (string line in lines)
            {
                if (line.Trim().Length == 0) continue;
                Account? a = JsonSerializer.Deserialize<Account>(line);
                if (a == null) continue;
                if (a.Login == login) return "Such login already exists\r\n";
                if (a.Email == email) return "Such email already exists\r\n";
            }

            return "";
        }

        private static string CheckLogin(string login)
        {
            login = login.Trim();
            if (login == "")
                return "Login cannot be empty\r\n";
            return "";
        }
        private static string CheckEmail(string email)
        {
            email = email.Trim();
            try
            {
                string address = new MailAddress(email).Address;
                return "";
            }
            catch (Exception)
            {
                return "Wrong email format\r\n";
            }
        }
        private static string CheckPassword(string pass1, string pass2)
        {
            if (pass1 == "")
                return "Password cannot be empty\r\n";
            if (pass1.ToList().Any(c => char.IsWhiteSpace(c)))
                return "Password cannot contain whitespace\r\n";
            if (pass1 != pass2)
                return "Passwords aren't equal\r\n";
            return "";
        }

        public static bool TryToAddNewUser(string login, string email, string pass1, string pass2)
        {
            string message = "";
            message += CheckLogin(login);
            message += CheckEmail(email);
            message += CheckPassword(pass1, pass2);
            message += CheckUniqueness(login, email);
            if (message != "")
            {
                MessageBox.Show(message);
                return false;
            }
            AddNewLogin(login, pass1);
            Account user = new Account(login, email);
            AddNewUser(user);
            return true;
        }
        //
        public static bool TryAuthorization(string login, string pass)
        {
            string line = File.ReadAllText(ExeFullPath + PassPath);
            Dictionary<string, string> values = JsonSerializer.Deserialize<Dictionary<string, string>>(line);
            if (values == null || values.Count == 0)
            {
                return false;
            }

            if (values.ContainsKey(login) && values[login] == pass)    
            {
                LogInWith(login);
                return true; 
            }
        
            return false;
        }

        private static void LogInWith(string login)
        {
            string[] lines = File.ReadAllLines(ExeFullPath + UsersPath);

            foreach (string line in lines)
            {
                if (line.Trim().Length == 0) continue;
                Account? a = JsonSerializer.Deserialize<Account>(line);
                if (a == null) continue;
                if (a.Login == login)
                {
                    CurrentAccount = a;
                    return;
                }
            }
        }

        public static void AddNewUser(Account a)
        {
            StreamWriter sw = new(ExeFullPath + UsersPath, true);
            sw.Write("\r\n");
            sw.Write(JsonSerializer.Serialize(a));
            sw.Close();
        }

        public static void CheckFiles()
        {
            if (!Directory.Exists(ExeFullPath + @"\Users"))
                Directory.CreateDirectory(ExeFullPath + @"\Users");

            if (!File.Exists(ExeFullPath + PassPath))
                File.Create(ExeFullPath + PassPath);

            if (!File.Exists(ExeFullPath + UsersPath))
                File.Create(ExeFullPath + UsersPath);
        }

        public static void SaveCurrent()
        {
            if (!IsLoggedIn) return;
            string[] lines = File.ReadAllLines(ExeFullPath + UsersPath);
            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Trim().Length == 0) continue;
                Account? user = JsonSerializer.Deserialize<Account>(lines[i]);
                if (user == null) continue;
                if (user.Login == CurrentAccount.Login)
                    lines[i] = JsonSerializer.Serialize(CurrentAccount);
            }
            File.WriteAllLines(ExeFullPath + UsersPath, lines);
        }
    }
}
