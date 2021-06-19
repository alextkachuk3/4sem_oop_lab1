﻿using _4sem_oop_lab1.TCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _4sem_oop_lab1
{
    /// <summary>
    /// Interaction logic for  MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DataGrid NotesGrid;

        public MainWindow()
        {
            InitializeComponent();

            NotesGrid = NotesList;

            NotesList.CanUserAddRows = false;

            if (AppContext.getDataBase().Users.ToList().Count == 1)
            {
                AccountManageIcon.Source = new BitmapImage(new Uri("logout.png", UriKind.Relative));
            }

            NotesList.ItemsSource = AppContext.getDataBase().Notes.ToList();

            try
            {
                Client.SyncNotes();

                NotesList.ItemsSource = AppContext.getDataBase().Notes.ToList();
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }

            Task inner = Task.Factory.StartNew(() =>
            {
                Sync();
            });
           
        }

        /// <summary>
        /// Function which run for sync ever 15 second 
        /// </summary>
        private void Sync()
        {
            while (true)
            {
                Client.SyncNotes();

                this.Dispatcher.Invoke(() =>
                {
                    NotesList.ItemsSource = AppContext.getDataBase().Notes.ToList();
                });

                int seconds = 15 * 1000;

                Thread.Sleep(seconds);
            }
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string my_text = "";

            Note note = new Note(my_text);
            
            //AppContext.getDataBase().Notes.Add(note);

            //AppContext.getDataBase().SaveChanges();

            //NotesList.ItemsSource = AppContext.getDataBase().Notes.ToList();

            NoteEditor noteEditor = new NoteEditor(note);

            noteEditor.Show();

        }
        
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppContext.getDataBase().Users.ToList().Count == 0)
            {
                Login loginWindow = new Login();


                loginWindow.Owner = this;

                loginWindow.Show();

                this.Visibility = Visibility.Hidden;

                AccountManageIcon.Source = new BitmapImage(new Uri("logout.png", UriKind.Relative));

            }
            else
            {
                MessageBoxResult boxResult = MessageBox.Show("Are you sure want to signout?", "Logout",
                MessageBoxButton.YesNo);
                if (boxResult == MessageBoxResult.Yes)
                {
                    foreach (User user in AppContext.getDataBase().Users)
                    {
                        AppContext.getDataBase().Users.Remove(user);
                    }
                    AppContext.getDataBase().SaveChanges();
                    AccountManageIcon.Source = new BitmapImage(new Uri("login.png", UriKind.Relative));
                    foreach (Note note in AppContext.getDataBase().Notes)
                    {
                        if (note.server_id != -1)
                            AppContext.getDataBase().Notes.Remove(note);
                    }
                    AppContext.getDataBase().SaveChanges();
                    NotesList.ItemsSource = AppContext.getDataBase().Notes.ToList();
                }
            }
            Client.SyncNotes();

            NotesList.ItemsSource = AppContext.getDataBase().Notes.ToList();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Note note = (Note)NotesList.SelectedItem;
            
                AppContext.getDataBase().Notes.Remove(note);
                AppContext.getDataBase().SaveChanges();
                NotesList.ItemsSource = AppContext.getDataBase().Notes.ToList();
            
        }

        private void NotesList_AutoGeneratedColumns(object sender, EventArgs e)
        {
            NotesList.Columns.FirstOrDefault(x => x.Header.ToString() == "local_id").Visibility = Visibility.Hidden;
            NotesList.Columns.FirstOrDefault(x => x.Header.ToString() == "server_id").Visibility = Visibility.Hidden;
            NotesList.Columns.FirstOrDefault(x => x.Header.ToString() == "text").Visibility = Visibility.Hidden;
            NotesList.Columns.FirstOrDefault(x => x.Header.ToString() == "last_mod_time").Visibility = Visibility.Hidden;
            NotesList.Columns.FirstOrDefault(x => x.Header.ToString() == "short_text").Width = 317;
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
