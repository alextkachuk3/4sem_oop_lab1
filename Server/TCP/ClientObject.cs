
using System;
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
            ADD_NOTE
        }

        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
            appContext = new AppContext();
        }

        private TcpClient client;

        AppContext appContext;

        public void GetCommand()
        {
            try
            {
                COMMAND command = (COMMAND)int.Parse(ReceiveFromApp());
                switch(command)
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
        }

        private void SendToApp(string data)
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

        private void SendToAppBigText(string text)
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                var text_byte_array = Encoding.UTF8.GetBytes(text);
                int block_count = ((text_byte_array.Length - 1) / 256) + 1;
                SendToApp(block_count.ToString());
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

        private void SendToApp(COMMAND command)
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

        private string ReceiveFromApp()
        {
            NetworkStream stream = null;

            stream = client.GetStream();
            byte[] data_array = new byte[256];
            stream.Read(data_array, 0, 256);
            return Encoding.UTF8.GetString(data_array).Replace("\0", "");
        }
    }
}