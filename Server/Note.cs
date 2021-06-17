using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Note
    {
        public string id { get; private set; }
        public string text { get; set; }
        public string last_mod_time { get; private set; }
    }
}
