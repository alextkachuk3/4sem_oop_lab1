using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Note
    {
        private Note() { }

        public Note(string text, string time)
        {
            this.text = text;
            this.last_mod_time = last_mod_time;            
        }
        public int id { get; private set; }
        public string text { get; set; }
        public string last_mod_time { get; private set; }
    }
}
