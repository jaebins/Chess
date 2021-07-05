using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chess
{
    class ConnectServer
    {
        Dictionary<Socket, string> clientList = new Dictionary<Socket, string>();
        string address;
        int userCount = 0;

        public ConnectServer(string address, bool isMulti, bool isServerMake)
        {
            this.address = address;
            Console.WriteLine(address);

            if (isServerMake)
            {
                Thread t1 = new Thread(ServerOpen);
                t1.IsBackground = true;
                t1.Start();
            }

            Game game = new Game(address, isMulti);
            game.Show();
        }

        private void ServerOpen()
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(IPAddress.Any, 8000);
            server.Bind(point);
            server.Listen(10);

            try
            {
                while (true)
                {
                    Socket client = server.Accept();
                    clientList.Add(client, client.RemoteEndPoint.ToString());
                    if (userCount == 0)
                    {
                        SendMessage("PlayerNumber_0");
                    }
                    else if (userCount == 1)
                    {
                        SendMessage("PlayerNumber_1");
                    }
                    else if (userCount > 1)
                    {
                        SendMessage("PlayerNumber_2");
                    }
                    Handle handle = new Handle();
                    handle.MessageReceiveEvent += new Handle.MessageReceive(MessageReceive);
                    handle.DisconnectUserEvent += new Handle.DisconnectUser(DisconnectUser);
                    handle.ServerStart(client, clientList);
                    userCount++;
                }
            } catch(SocketException se)
            {
                Console.WriteLine(se.Message);
                server.Close();
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
                server.Close();
            }
        }

        private void MessageReceive(string msg)
        {
            SendMessage(msg);
        }

        private void SendMessage(string msg)
        {
            foreach (var listInClient in clientList)
            {
                Socket client = listInClient.Key;
                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                client.Send(buffer);
            }
        }

        private void DisconnectUser(Socket client)
        {
            if (clientList.ContainsKey(client))
            {
                clientList.Remove(client);
                userCount--;
            }
        }
    }
}
