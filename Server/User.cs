using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class User
    {
        public int id { get; private set; }
        public string login { get; set; }
        public string password { get; set; }
        public string notes_id { get; private set; }
    }
}
