using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// User database class
    /// </summary>
    public class User
    {
        private User() { }
        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
            notes_id = string.Empty;
        }
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string notes_id { get; private set; }

        public void AddNote(int note_id)
        {
            this.notes_id += note_id + " ";
        }

        public void DeleteNote(int note_id)
        {
            notes_id = notes_id.Replace(note_id.ToString(), "");
        }
    }
}
