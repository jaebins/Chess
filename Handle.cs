using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chess
{
    class Handle
    {
        Socket client;
        Dictionary<Socket, string> clientList = new Dictionary<Socket, string>();

        public delegate void MessageReceive(string msg);
        public event MessageReceive MessageReceiveEvent;

        public delegate void DisconnectUser(Socket client);
        public event DisconnectUser DisconnectUserEvent;

        public void ServerStart(Socket client, Dictionary<Socket, string> clientList)
        {
            this.client = client;
            this.clientList = clientList;

            Thread t1 = new Thread(Receive);
            t1.IsBackground = true;
            t1.Start();
        }

        public void Receive()
        {
            byte[] buffer = new byte[1024];
            int length = 0;
            string msg = string.Empty;

            try
            {
                while (true)
                {
                    length = client.Receive(buffer);
                    msg = Encoding.UTF8.GetString(buffer, 0, length);
                    MessageReceiveEvent(msg);
                }
            } catch(SocketException se)
            {
                Console.WriteLine(se.Message);
                DisconnectUserEvent(client);
                client.Close();
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
                DisconnectUserEvent(client);
                client.Close();
            }
        }
    }
}
