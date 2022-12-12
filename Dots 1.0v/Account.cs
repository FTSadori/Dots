using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace Dots_1._0v
{
    public class Account
    {
        public Account(string login, string email)
        {
            Login = login;
            Email = email;
            AccountCreated = DateTime.Now;
        }

        public string Login { get; set; } = "";
        public string Email { get; set; } = "";

        public int FirstsWins { get; set; } = 0;
        public int SecondsWins { get; set; } = 0;
        public int FirstsGiveUps { get; set; } = 0;
        public int SecondsGiveUps { get; set; } = 0;
        public int Draws { get; set; } = 0;

        public int ClickedDotsNumber { get; set; } = 0;

        public int GamesNumber { get; set; } = 0;

        public DateTime AccountCreated { get; set; }
    }
}
