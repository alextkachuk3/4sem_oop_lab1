﻿using _4sem_oop_lab1.TCP;
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
        public static DataGrid NotesGrid;

        public MainWindow()
        {
            InitializeComponent();

            NotesGrid = NotesList;

            //foreach(var note in appContext.Notes)
            //{
            //    NotesList.Items.Add(note.short_text);
            //}

            NotesList.ItemsSource = AppContext.getDataBase().Notes.ToList();

            if(AppContext.getDataBase().Users.ToList().Count == 1)
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
            //note.server_id;
            AppContext.getDataBase().Notes.Add(note);

            AppContext.getDataBase().SaveChanges();

            NotesList.ItemsSource = AppContext.getDataBase().Notes.ToList();

            NoteEditor noteEditor = new NoteEditor(note);

            //noteEditor.Owner = this;

            noteEditor.Show();

            //noteEditor.

        }
        
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            if(AppContext.getDataBase().Users.ToList().Count == 0)
            {
                Login loginWindow = new Login();

                loginWindow.Owner = this;

                loginWindow.Show();

                this.Visibility = Visibility.Hidden;
            }
            else 
            {
                MessageBoxResult boxResult = MessageBox.Show("Are you sure want to signout?", "Logout",
                MessageBoxButton.YesNo);
                if(boxResult == MessageBoxResult.Yes)
                {
                    foreach (User user in AppContext.getDataBase().Users)
                    {
                        AppContext.getDataBase().Users.Remove(user);
                    }
                    AppContext.getDataBase().SaveChanges();
                    AccountManageIcon.Source = new BitmapImage(new Uri("login.png", UriKind.Relative));
                }
            }
            
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void NotesList_AutoGeneratedColumns(object sender, EventArgs e)
        {
            NotesList.Columns.FirstOrDefault(x => x.Header.ToString() == "local_id").Visibility = Visibility.Hidden;
            NotesList.Columns.FirstOrDefault(x => x.Header.ToString() == "server_id").Visibility = Visibility.Hidden;
            NotesList.Columns.FirstOrDefault(x => x.Header.ToString() == "text").Visibility = Visibility.Hidden;
            NotesList.Columns.FirstOrDefault(x => x.Header.ToString() == "last_mod_time").Visibility = Visibility.Hidden;
            NotesList.Columns.FirstOrDefault(x => x.Header.ToString() == "short_text").Width = 317;
            NotesList.Columns.FirstOrDefault(x => x.Header.ToString() == "short_text").Header = string.Empty;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            foreach(Note note in NotesList.SelectedItems)
            {
                NoteEditor noteEditor = new NoteEditor(note);

                noteEditor.Owner = this;

                noteEditor.Show();
            }
        }
    }
}
