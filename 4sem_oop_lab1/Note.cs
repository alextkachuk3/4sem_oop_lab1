using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4sem_oop_lab1
{
    public class Note
    {
        public Note() { }

        public Note(string text)
        {
            this.text = text;
            UpdateShortText();
        }

        [Key]
        public int local_id { get; private set; }
        public int server_id { get; set; }
        public string text { get; private set; }
        public string short_text { get; set; }

        private void UpdateShortText()
        {
            short_text = string.Empty;
            for(int i = 0, j = 0 ; i < 350 && i < text.Length; i++, j++)
            {
                if(j==50)
                {
                    short_text += '\n';
                    j = 0;
                }
                short_text += text[i];
            }
            for(int i = 0, j = 0; i < 350 - text.Length; i++, j++)
            {
                if (j == 50)
                {
                    short_text += '\n';
                    j = 0;
                }
                short_text += " ";
            }
        }

        public void SetText(string text)
        {
            this.text = text;
            UpdateShortText();
        }
    }
}
