using _4sem_oop_lab1.TCP;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _4sem_oop_lab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppContext appContext;


        public MainWindow()
        {
            InitializeComponent();

            appContext = new AppContext();

            foreach(var note in appContext.Notes)
            {
                NotesList.Items.Add(note.short_text);
            }

            if(appContext.Users.ToList().Count == 1)
            {                
                AccountManageIcon.Source = new BitmapImage(new Uri("logout.png", UriKind.Relative));
            }
            else
            {
                
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string my_text = "";
            for (int i = 0; i < 50; i++)
            {
                my_text += "a";
            }
            for (int i = 0; i < 50; i++)
            {
                my_text += "b";
            }
            for (int i = 0; i < 50; i++)
            {
                my_text += "b";
            }
            for (int i = 0; i < 50; i++)
            {
                my_text += "b";
            }
            for (int i = 0; i < 50; i++)
            {
                my_text += "b";
            }
            for (int i = 0; i < 50; i++)
            {
                my_text += "b";
            }
            for (int i = 0; i < 50; i++)
            {
                my_text += "b";
            }
            Note note = new Note(my_text);
            note.server_id = 4333;
            //appContext.Notes.Add(note);

            //appContext.SaveChanges();

            NotesList.Items.Add(note.short_text);

            NoteEditor noteEditor = new NoteEditor(note);

            // noteEditor.Owner = this;

            noteEditor.Show();
        }
        
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            if(appContext.Users.ToList().Count == 0)
            {
                Login loginWindow = new Login();

                loginWindow.Owner = this;

                loginWindow.Show();
            }
            else 
            {
                MessageBoxResult boxResult = MessageBox.Show("Are you sure want to signout?", "Logout",
                MessageBoxButton.YesNo);
                if(boxResult == MessageBoxResult.Yes)
                {
                    foreach (User user in appContext.Users)
                    {
                        appContext.Users.Remove(user);
                    }
                    appContext.SaveChanges();
                    AccountManageIcon.Source = new BitmapImage(new Uri("login.png", UriKind.Relative));
                }
            }
            
        }
    }
}
