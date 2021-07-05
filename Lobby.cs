using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Lobby : Form
    {

        public Lobby()
        {
            InitializeComponent();
        }

        private void but_SinglePlay_Click(object sender, EventArgs e)
        {
            new ConnectServer("", false, false);
        }

        private void but_MultiPlay_Click(object sender, EventArgs e)
        {
            new ConnectServer(Get_MyIP(), true, true);
            MessageBox.Show(Get_MyIP());
        }

        private void but_JoinServer_Click(object sender, EventArgs e)
        {
            new ConnectServer(InputAddress.Text, true, false);
        }

        public string Get_MyIP()
        {
            IPHostEntry host = Dns.GetHostByName(Dns.GetHostName());
            string myip = host.AddressList[0].ToString();
            return myip;
        }
    }
}
