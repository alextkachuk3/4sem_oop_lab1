using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class User
    {
        private User() { }
        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
        public int id { get; private set; }
        public string login { get; set; }
        public string password { get; set; }
        public string notes_id { get; private set; }
    }
}
