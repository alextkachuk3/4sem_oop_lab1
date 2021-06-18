using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4sem_oop_lab1
{
    /// <summary>
    /// Local database user class
    /// </summary>
    public class User
    {
        private User() { }
        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
        
        [Key]
        public int local_id { get; private set; }
        public int server_id { get; set; }
        public string login { get; private set; }
        public string password { get; set; }
        public int is_logined { get; set; }
    }
}
