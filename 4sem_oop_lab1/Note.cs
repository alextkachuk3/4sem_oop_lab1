using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4sem_oop_lab1
{
    /// <summary>
    /// Local database note table
    /// </summary>
    public class Note
    {
        public Note() { }

        public Note(string text)
        {
            this.text = text;
            UpdateShortText();
            UpdateEditTime();
            server_id = -1;
        }

        [Key]
        public int local_id { get; private set; }
        public int server_id { get; set; }
        public string text { get; private set; }
        public string short_text { get; private set; }
        public string last_mod_time { get; set; }

        /// <summary>
        /// Update short version of text note
        /// </summary>
        private void UpdateShortText()
        {
            short_text = string.Empty;
            for (int i = 0, j = 0; i < 200 && i < text.Length; i++, j++)
            {
                if (j == 39)
                {
                    short_text += '\n';
                    j = 0;
                }
                short_text += text[i];
            }
        }

        /// <summary>
        /// Set note text
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            this.text = text;
            UpdateShortText();
            UpdateEditTime();
        }

        /// <summary>
        /// Update editing time
        /// </summary>
        public void UpdateEditTime()
        {
            this.last_mod_time = DateTime.UtcNow.ToString();
        }
    }
}
