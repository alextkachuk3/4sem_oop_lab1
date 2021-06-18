
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Server.TCP
{
    class ClientObject
    {
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
        /// Client object constructor
        /// </summary>
        /// <param name="tcpClient"></param>
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        /// <summary>
        /// TCP client variable
        /// </summary>
        private TcpClient client;

        /// <summary>
        /// Function for receiving command from app
        /// </summary>
        public void GetCommand()
        {
            try
            {
                COMMAND command = (COMMAND)int.Parse(Receive());
            switch (command)
            {
                case COMMAND.LOGIN:
                    {
                        Login();
                        break;
                    }
                case COMMAND.REGISTER:
                    {
                        Regiser();
                        break;
                    }
                case COMMAND.RECEIVE_NOTES_INFO:
                    {
                        SendNotesInfo();
                        break;
                    }
                case COMMAND.RECEIVE_NOTE:
                    {
                        SendNote();
                        break;
                    }
                case COMMAND.DELETE_NOTE:
                    {
                        DeleteNote();
                        break;
                    }
                case COMMAND.ADD_NOTE:
                    {
                        AddNote();
                        break;
                    }
                case COMMAND.UPDATE_NOTE:
                    {
                        UpdateNote();
                        break;
                    }
                default:
                    {
                        throw new Exception("Wrong command!");
                    }
            }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Run function if user send login request
        /// </summary>
        void Login()
        {
            string login = Receive();
            string password = Receive();
            User user = AppContext.getDataBase().Users.ToList().Find(x => x.login == login && x.password == password);
            if(user == null)
            {
                Send(COMMAND.LOGIN_FAIL);
            }
            else
            {
                Send(COMMAND.LOGIN_SUCCESS);
                Send(user.id.ToString());
            }
        }

        /// <summary>
        /// Run this function if user send registration request
        /// </summary>
        void Regiser()
        {
            string login = Receive();
            string password = Receive();
            User user = AppContext.getDataBase().Users.ToList().Find(x => x.login == login);
            if(user != null)
            {
                Send(COMMAND.REGISTER_FAIL);
            }
            else
            {
                Send(COMMAND.REGISTER_SUCCESS);
                User new_user = new User(login, password);
                AppContext.getDataBase().Users.Add(new_user);
                AppContext.getDataBase().SaveChanges();
                Send(new_user.id.ToString());
            }
        }

        /// <summary>
        /// Get local note and put to server
        /// </summary>
        void AddNote()
        {
            int user_id = int.Parse(Receive());
            string text = ReceiveBigText();
            string date = Receive();
            Note note = new Note(text, date);
            note.last_mod_time = date;
            AppContext.getDataBase().Notes.Add(note);
            AppContext.getDataBase().SaveChanges();
            User user = AppContext.getDataBase().Users.Find(user_id);
            user.AddNote(note.id);
            
            AppContext.getDataBase().SaveChanges();
            Send(note.id.ToString());
        }

        /// <summary>
        /// Delete note from server
        /// </summary>
        void DeleteNote()
        {
            int user_id = int.Parse(Receive());
            int note_id = int.Parse(Receive());
            User user = AppContext.getDataBase().Users.Find(user_id);
            user.DeleteNote(note_id);

            AppContext.getDataBase().SaveChanges();
        }

        /// <summary>
        /// Update text note
        /// </summary>
        void UpdateNote()
        {
            int note_id = int.Parse(Receive());
            string text = ReceiveBigText();
            string last_mod_time = Receive();
            Note note = AppContext.getDataBase().Notes.Find(note_id);
            note.text = text;
            note.last_mod_time = last_mod_time;
            AppContext.getDataBase().SaveChanges();
        }

        void SendNotesInfo()
        {
            int id = int.Parse(Receive());
            var user = AppContext.getDataBase().Users.Find(id);
            if(user == null)
            {
                throw new Exception("User ID not exist");
            }
            else
            {
                List<Note> notes = new List<Note>();
                List<string> notes_id_list = user.notes_id.Split(' ').ToList();
                {
                    foreach (var note_id in notes_id_list)
                    {
                        if (note_id != "")
                        {
                            Note note = AppContext.getDataBase().Notes.Find(int.Parse(note_id));
                            if (note != null)
                                notes.Add(note);
                        }
                    }
                }

                Send(notes.Count.ToString());

                foreach (var note in notes)
                {
                    Send(note.id.ToString());
                    Send(note.last_mod_time);
                }
            }
        }

        void SendNote()
        {
            int id = int.Parse(Receive());
            var note = AppContext.getDataBase().Notes.Find(id);
            if(note == null)
            {
                throw new Exception("Note ID not exist");
            }
            SendBigText(note.text);
        }

        private void Send(string data)
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

        private void SendBigText(string text)
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

        private string ReceiveBigText()
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

        private void Send(COMMAND command)
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

        private string Receive()
        {
            NetworkStream stream = null;

            stream = client.GetStream();
            byte[] data_array = new byte[256];
            stream.Read(data_array, 0, 256);
            return Encoding.UTF8.GetString(data_array).Replace("\0", "");
        }
    }
}