using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _4sem_oop_lab1
{
    /// <summary>
    /// Логика взаимодействия для NoteEditor.xaml
    /// </summary>
    public partial class NoteEditor : Window
    {
        public NoteEditor(Note note)
        {
            this.note = note;
            InitializeComponent();

            NoteText.Text = this.note.text;
        }

        private Note note;

        private void Window_Closed(object sender, EventArgs e)
        {
            note.SetText(NoteText.Text);
            AppContext appContext = new AppContext();
            appContext.Notes.Add(note);
            appContext.SaveChanges();
        }
    }
}
