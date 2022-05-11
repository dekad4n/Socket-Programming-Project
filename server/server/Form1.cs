using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Form1 : Form
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>();
        List<string> connectedUsers = new List<string>();
        bool terminating = false;
        bool listening = false;
        string loginheader = "AcceptLogin\n";
        string rejectLogin = "RejectLogin\n";
        string logoutheader = "AcceptLogout\n";
        string getPostHeader = "GetPosts\n";
        string createPostHeader = "CreatePost\n";
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void listen_button_Click(object sender, EventArgs e)
        {
            int serverPort;

            if (Int32.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(3); // hard coded clients, fix it

                listening = true;
                listen_button.Enabled = false;

                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");

            }
            else
            {
                logs.AppendText("Check the port number!\n");
            }
        }

        private void Accept()
        {
            while (listening)
            {
                try
                {
                    Socket newClient = serverSocket.Accept();
                    clientSockets.Add(newClient);

                    Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                    receiveThread.Start();
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }

                }
            }
        }
        private bool login_user(string username)
        {
            string[] lines = File.ReadAllLines(@"../../user-db.txt");
            foreach (string line in lines)
            {
                if(line == username)
                {
                    
                    return true;
                }
            }
            logs.AppendText("There is no account with username " + username + "\n");
            
            return false;
            
        }
        private int handleLogin(Socket thisClient, string incomingMessage)
        {
            string msg = loginheader;
            bool alreadyLoggedIn = false;

            try
            {
                foreach (string line in connectedUsers)
                {
                    if (line == incomingMessage)
                    {
                        alreadyLoggedIn = true;
                        
                        logs.AppendText("User tried with username " + incomingMessage + " but it is already logged in.\n");
                        break;
                    }
                   
                }
                if (!alreadyLoggedIn)
                {
                    bool is_there_such = login_user(incomingMessage);

                    msg = !is_there_such ? rejectLogin+"There is no account with username " + incomingMessage : loginheader +incomingMessage + " you successfully logged in!";

                    // if there is no connected user called X
                    if (is_there_such)
                    {
                        connectedUsers.Add(incomingMessage);
                        logs.AppendText(incomingMessage + " has logged in.\n");

                        // send login success
                        Byte[] sndr_bfr = Encoding.Default.GetBytes(msg);
                        thisClient.Send(sndr_bfr);
                        return connectedUsers.Count();
                    }
                    else
                    {
                        Byte[] sndr_bfr = Encoding.Default.GetBytes(msg);
                        thisClient.Send(sndr_bfr);
                        return 0;
                    }
                    
                }
                else
                {
                    msg = rejectLogin+incomingMessage + " already logged in!";
                    Byte[] sender_buffer = Encoding.Default.GetBytes(msg);
                    thisClient.Send(sender_buffer);
                    return 0;
                }

            }
            catch
            {
                logs.AppendText("There is a problem! Check the connection...\n");
                terminating = true;
                textBox_port.Enabled = true;
                listen_button.Enabled = true;
                serverSocket.Close();
            }
            return 0;
        }
        // give numbers to clients
        private void Receive(Socket thisClient) // updated
        {
            bool connected = true;
            int loginCount = 0;
            while (connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[128];
                    thisClient.Receive(buffer);
                    Console.WriteLine("buffer");
                    string incomingMessage = Encoding.Default.GetString(buffer);
                    Console.WriteLine(incomingMessage);

                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    string[] msg_splitted = incomingMessage.Split('\n');

                    if(msg_splitted[0] == "Log-in")
                    {
                       loginCount =  handleLogin(thisClient, msg_splitted[1]);
                        if(loginCount == 0)
                        {
                            connected = false;
                            thisClient.Close();
                            clientSockets.Remove(thisClient);
                            break;
                        }
                        
                    }
                    else if(msg_splitted[0] == "Log-out")
                    {
                        connectedUsers.RemoveAt(loginCount-1);

                        // if client does not leave
                        Byte[] sndr_bfr = Encoding.Default.GetBytes(logoutheader);
                        thisClient.Send(sndr_bfr);
                        logs.AppendText( msg_splitted[1]+" has logged out...\n");
                        loginCount = 0;
                    }
                    


                }
                catch
                {
                    if (!terminating)
                    {
                        if(loginCount != 0)
                        {
                            logs.AppendText(connectedUsers[loginCount - 1] + " has disconnected\n");
                            connectedUsers.RemoveAt(loginCount - 1);
                        }
                       
                    }
                    thisClient.Close();
                    clientSockets.Remove(thisClient);
                    connected = false;
                }
            }
        }

        
    }
}
