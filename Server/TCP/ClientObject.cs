
using System;
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
            RECEIVE_NOTES_ID,
            DELETE_NOTE,
            ADD_NOTE,
            UPDATE_NOTE
        }

        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        private TcpClient client;

        public void GetCommand()
        {
            //try
            //{
                COMMAND command = (COMMAND)int.Parse(Receive());
                switch(command)
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
                    case COMMAND.RECEIVE_NOTES_ID:
                        {
                            SendNotes();
                            break;
                        }
                    case COMMAND.DELETE_NOTE:
                        {
                            
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
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }

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

        void AddNote()
        {
            int user_id = int.Parse(Receive());
            string text = ReceiveBigText();
            string date = Receive();
            Note note = new Note(text, date);
            AppContext.getDataBase().Notes.Add(note);
            User user = AppContext.getDataBase().Users.Find(user_id);
            user.AddNote(note.id);
            AppContext.getDataBase().SaveChanges();
            Send(note.id.ToString());
        }

        void UpdateNote()
        {

        }

        void SendNotes()
        {
            int id = int.Parse(Receive());
            var user = AppContext.getDataBase().Users.Find(id);
            if(user == null)
            {
                throw new Exception("User ID not exist");
            }
            else
            {
                Send(user.notes_id);
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
                var text_byte_array = Encoding.UTF8.GetBytes(text);
                int block_count = ((text_byte_array.Length - 1) / 256) + 1;
                Send(block_count.ToString());
                for (int i = 0; i < block_count; i++)
                {
                    byte[] byte_array = new byte[256];
                    for (int j = 0; j < 256 && i * 256 + j < text_byte_array.Length; j++)
                    {
                        byte_array[j] = text_byte_array[i * 256 + j];
                    }
                    stream.Write(byte_array, 0, 256);
                    byte[] accepted = new byte[1];
                    stream.Read(accepted, 0, 1);
                }
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