using System.Net.Sockets;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace _4sem_oop_lab1.TCP
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
        ADD_NOTE
    }

    static class Client
    {
        const string ip = "192.168.0.85";
        const int port = 25565;
        static Client()
        {
            //client = new TcpClient();
        }

        public static bool Login(string login, string password)
        {
            Open();
            SendToApp(COMMAND.LOGIN);
            SendToApp(login);
            SendToApp(password);
            COMMAND command = (COMMAND)int.Parse(ReceiveFromApp());
            if(command == COMMAND.LOGIN_SUCCESS)
            {
                int id = int.Parse(Client.ReceiveFromApp());

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

        private static void Open()
        {
            client = new TcpClient(ip, port);
        }

        private static void Close()
        {
            client.Close();
        }

        public static bool Register(string login, string password)
        {
            Open();
            SendToApp(COMMAND.REGISTER);
            SendToApp(login);
            SendToApp(password);
            COMMAND command = (COMMAND)int.Parse(ReceiveFromApp());
            if (command == COMMAND.REGISTER_SUCCESS)
            {
                int id = int.Parse(Client.ReceiveFromApp());

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

        public static List<int> GetNotesID()
        {
            List<int> notes_id = new List<int>();


            return notes_id;
        }

        private static TcpClient client;

        /*public void GetCommand()
        {
            try
            {
                COMMAND command = (COMMAND)int.Parse(ReceiveFromApp());
                switch (command)
                {
                    case COMMAND.LOGIN:
                        {
                            
                            break;
                        }
                    case COMMAND.REGISTER:
                        {

                            break;
                        }
                    case COMMAND.RECEIVE_NOTES_ID:
                        {

                            break;
                        }
                    case COMMAND.DELETE_NOTE:
                        {

                            break;
                        }
                    case COMMAND.ADD_NOTE:
                        {

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
        }*/


        public static void SendToApp(string data)
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

        private static void SendToAppBigText(string text)
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();                
                SendToApp(text.Length.ToString());
                byte[] data_array = Encoding.UTF8.GetBytes(text);
                stream.Write(data_array, 0, data_array.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static string ReceiveBigText()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                int text_lenght = int.Parse(ReceiveFromApp());
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

        private static void SendToApp(COMMAND command)
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

        public static string ReceiveFromApp()
        {
            NetworkStream stream = null;

            stream = client.GetStream();
            byte[] data_array = new byte[256];
            stream.Read(data_array, 0, 256);
            return Encoding.UTF8.GetString(data_array).Replace("\0", "");
        }
    }
}
