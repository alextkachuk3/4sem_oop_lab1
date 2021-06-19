using System.Net.Sockets;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace _4sem_oop_lab1.TCP
{
    /// <summary>
    /// Commands for communication between server and app
    /// </summary>
    enum COMMAND
    {
        LOGIN,
        LOGIN_SUCCESS,
        LOGIN_FAIL,
        REGISTER,
        REGISTER_SUCCESS,
        REGISTER_FAIL,
        RECEIVE_NOTES_INFO,
        RECEIVE_NOTE,
        DELETE_NOTE,
        ADD_NOTE,
        UPDATE_NOTE
    }

    /// <summary>
    /// Class for working app with server
    /// </summary>
    static class Client
    {
        const string ip = "192.168.0.85";
        const int port = 25565;
        static Client()
        {
            //client = new TcpClient();
        }

        /// <summary>
        /// Connect to remote server
        /// </summary>
        private static void Open()
        {
            client = new TcpClient(ip, port);
        }

        /// <summary>
        /// Disconnect from remote server
        /// </summary>
        private static void Close()
        {
            client.Close();
        }

        /// <summary>
        /// User login user
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <returns>True if login and password is correct, else return false</returns>
        public static bool Login(string login, string password)
        {
            Open();
            Send(COMMAND.LOGIN);
            Send(login);
            Send(password);
            COMMAND command = (COMMAND)int.Parse(Receive());
            if(command == COMMAND.LOGIN_SUCCESS)
            {
                int id = int.Parse(Client.Receive());

                foreach (User user in AppContext.getDataBase().Users)
                {
                    AppContext.getDataBase().Users.Remove(user);
                }

                AppContext.getDataBase().SaveChanges();

                User login_user = new User(login, password);

                login_user.is_logined = 1;

                login_user.server_id = id;

                AppContext.getDataBase().Users.Add(login_user);

                AppContext.getDataBase().SaveChanges();

                Close();

                return true;
            }
            Close();

            return false;
        }

        /// <summary>
        /// Delete note from server
        /// </summary>
        public static void Delete(int note_id)
        {
            Note note = AppContext.getDataBase().Notes.ToList().Find(x => x.server_id == note_id);
            User user = AppContext.getDataBase().Users.FirstOrDefault();
            if(user != default && note != null)
            {
                Open();
                Send(COMMAND.DELETE_NOTE);
                Send(user.server_id.ToString());
                Send(note.server_id.ToString());
                Close();
            }
        }

        /// <summary>
        /// User register command
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="password">user password</param>
        /// <returns>Return true if register success, else return false</returns>
        public static bool Register(string login, string password)
        {
            Open();
            Send(COMMAND.REGISTER);
            Send(login);
            Send(password);
            COMMAND command = (COMMAND)int.Parse(Receive());
            if (command == COMMAND.REGISTER_SUCCESS)
            {
                int id = int.Parse(Receive());

                foreach (User user in AppContext.getDataBase().Users)
                {
                    AppContext.getDataBase().Users.Remove(user);
                }

                AppContext.getDataBase().SaveChanges();

                User login_user = new User(login, password);

                login_user.is_logined = 1;

                login_user.server_id = id;

                AppContext.getDataBase().Users.Add(login_user);

                AppContext.getDataBase().SaveChanges();

                Close();

                return true;
            }
            Close();
            return false;
        }

        /// <summary>
        /// Add local note to server
        /// </summary>
        /// <param name="user">Local user</param>
        /// <param name="note">Local note</param>
        /// <returns></returns>
        public static int AddNoteToServer(User user, Note note)
        {
            Open();
            Send(COMMAND.ADD_NOTE);
            Send(user.server_id.ToString());
            SendBigText(note.text);
            Send(note.last_mod_time);
            int server_id = int.Parse(Receive());
            note.server_id = server_id;
            Close();
            return server_id;
        }

        /// <summary>
        /// Update note text on server
        /// </summary>
        /// <param name="note">Note text which will send to server</param>
        public static void UpdateNoteOnServer(Note note)
        {
            Open();
            Send(COMMAND.UPDATE_NOTE);
            Send(note.server_id.ToString());
            SendBigText(note.text);
            Send(note.last_mod_time);
            Close();
        }

        /// <summary>
        /// Receive note text from server
        /// </summary>
        /// <param name="id">Note server id</param>
        /// <returns>Note text</returns>
        public static string GetNoteText(int id)
        {
            Open();
            Send(COMMAND.RECEIVE_NOTE);
            Send(id.ToString());
            string text = ReceiveBigText();
            Close();
            return text;
        }

        /// <summary>
        /// TCP client varrieble
        /// </summary>
        private static TcpClient client;

        /// <summary>
        /// Send string with max lenght 256
        /// </summary>
        /// <param name="data"></param>
        public static void Send(string data)
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data_array = new byte[256];
                Encoding.UTF8.GetBytes(data).CopyTo(data_array, 0);
                stream.Write(data_array, 0, 256);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Send text with any length
        /// </summary>
        /// <param name="text"></param>
        private static void SendBigText(string text)
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();                
                Send(text.Length.ToString());
                byte[] data_array = Encoding.UTF8.GetBytes(text);
                stream.Write(data_array, 0, data_array.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Receive text with any length
        /// </summary>
        /// <returns>Text</returns>
        private static string ReceiveBigText()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                int text_lenght = int.Parse(Receive());
                byte[] data_array = new byte[text_lenght];
                stream.Read(data_array, 0, data_array.Length);
                return Encoding.UTF8.GetString(data_array);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        /// <summary>
        /// Send command to server
        /// </summary>
        /// <param name="command"></param>
        private static void Send(COMMAND command)
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data_array = new byte[256];
                Encoding.UTF8.GetBytes(((int)command).ToString()).CopyTo(data_array, 0);
                stream.Write(data_array, 0, 256);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Receive
        /// </summary>
        /// <returns></returns>
        private static string Receive()
        {
            NetworkStream stream = null;

            stream = client.GetStream();
            byte[] data_array = new byte[256];
            stream.Read(data_array, 0, 256);
            return Encoding.UTF8.GetString(data_array).Replace("\0", "");
        }


        /// <summary>
        /// Function sync notes
        /// </summary>
        /// <returns>True if user logined, else return false</returns>
        public static bool SyncNotes()
        {
            if (AppContext.getDataBase().Users.Count() != 0)
            {
                Open();

                User user = AppContext.getDataBase().Users.ToList().First();

                Send(COMMAND.RECEIVE_NOTES_INFO);

                Send(user.server_id.ToString());

                List<int> notes_id = new List<int>();

                List<string> last_mod_data = new List<string>();

                int notes_count = int.Parse(Receive());

                for (int i = 0; i < notes_count; i++)
                {
                    notes_id.Add(int.Parse(Receive()));
                    last_mod_data.Add(Receive());
                }

                Close();

                for (int i = 0; i < notes_count; i++)
                {
                    var note = AppContext.getDataBase().Notes.ToList().Find(x => x.server_id == notes_id[i]);
                    if (note == null)
                    {
                        Open();
                        Send(COMMAND.RECEIVE_NOTE);
                        Send(notes_id[i].ToString());
                        string text = ReceiveBigText();
                        Note received_note = new Note(text);
                        received_note.server_id = notes_id[i];
                        received_note.last_mod_time = last_mod_data[i];
                        Close();
                        AppContext.getDataBase().Notes.Add(received_note);
                        AppContext.getDataBase().SaveChanges();
                    }
                    else
                    {

                        DateTime local_t = DateTime.Parse(last_mod_data[i]);
                        DateTime server_t = DateTime.Parse(note.last_mod_time);
                        if (local_t < server_t)
                        {
                            Open();
                            Send(COMMAND.RECEIVE_NOTE);
                            note.SetText(GetNoteText(notes_id[i]));

                            Close();
                        }
                        else if (local_t > server_t)
                        {
                            UpdateNoteOnServer(note);
                        }
                    }
                }

                foreach(Note local_note in AppContext.getDataBase().Notes)
                {
                    if(local_note.server_id == -1)
                    {
                        AddNoteToServer(user, local_note);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
