using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;

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

    class Client
    {
        Client(string ip, int port)
        {
            client = new TcpClient(ip, port);
        }

        public bool Login(string login, string password)
        {

            return true;
        }

        public bool Register(string login, string password)
        {

            return false;
        }

        public List<int> GetNotesID()
        {
            List<int> notes_id = new List<int>;


            return notes_id;
        }

        private TcpClient client;
    }
}
