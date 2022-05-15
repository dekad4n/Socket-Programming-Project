using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {
        // Connection elements
        bool terminating = false; // did i close the app
        bool connected = false; // am i connected still?
        bool didIDisc = false; // did i click disconnect

        // request types
        string loginheader = "Log-in\n";// login request
        string getPostHeader = "GetPosts\n"; // get post request
        string createPostHeader = "CreatePost\n"; // create post request

        string currentUsername = "";
        // my socket
        Socket clientSocket;
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;

            int portNum;
            if (Int32.TryParse(textBox_port.Text, out portNum))
            {

                try
                {
                    rejectedLogin = false;
                    // check states will be here (port, username, ip)
                    if (username_area.Text.Length == 0)
                    {
                        logs.AppendText("Please enter your username!\n");
                        return;
                    }
                    if(textBox_ip.Text.Length == 0 )
                    {
                        logs.AppendText("Please enter an IP address!!\n");
                        return;

                    }
                    clientSocket.Connect(IP, portNum);

                    // starts states
                    didIDisc = false;
                    rejectedLogin = false;
                    connected = true;

                    // frontend
                   
                    Thread receiveThread = new Thread(Receive);

                    loginHandle();


                    receiveThread.Start();

                }
                catch
                {
                    logs.AppendText(username_area.Text + " server is not responding, we cannot check if username is valid.\n");
                }
            }
            else
            {
                logs.AppendText("Check the port number!\n");
            }
        }
        bool rejectedLogin = false;

        private void Receive()
        {
            while (connected)
            {
                try
                {
                    // receive msg
                    Byte[] buffer = new Byte[128];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer); // change crr msg to string
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0")); // discard nulls
                    string[] msg_splitted = incomingMessage.Split('\n'); // split msg to get type and payload

                    // if i send login
                    if (msg_splitted[0] == "AcceptLogin") 
                    {

                        currentUsername = msg_splitted[1].Split(' ')[0];

                        button_connect.Enabled = false;
                        username_area.Enabled = false;
                        textBox_ip.Enabled = false;
                        textBox_port.Enabled = false;
                        button_disconnect.Enabled = true;

                        button_create_post.Enabled = true;
                        textBox_createPost.Enabled = true;

                        button_get_posts.Enabled = true;
                        logs.AppendText(msg_splitted[1] + "\n");
                    }
                    else if (msg_splitted[0] == "RejectLogin")
                    {
                        // handle login state
                        username_area.Enabled = true;
                        button_connect.Enabled = true;
                        textBox_ip.Enabled = true;
                        textBox_port.Enabled = true;
                        button_disconnect.Enabled = false;

                        button_create_post.Enabled = false;
                        textBox_createPost.Enabled = false;

                        button_get_posts.Enabled = false;

                        rejectedLogin = true;
                        username_area.Clear();
                        // print logout server msg
                        currentUsername = "";
                        try
                        {
                            clientSocket.Close();

                        }
                        catch
                        {
                        }
                        connected = false;
                        logs.AppendText(msg_splitted[1] + "\n");

                    }
                    else if (msg_splitted[0] == "CreatePost")
                    {
                        logs.AppendText(msg_splitted[1] + "\n" + msg_splitted[2] + "\n");
                        textBox_createPost.Clear();
                    }
                    else if (msg_splitted[0] == "GetPosts")
                    {
                        logs.AppendText("username: " + msg_splitted[1] + "\n" + "Post ID: " + msg_splitted[2] + "\n" + "Date: " + msg_splitted[3] + "\nPost:" + msg_splitted[4] + "\n\n");
                    }

                    // Print servers msg if not logout
                }
                catch
                {
                    if (!terminating && !didIDisc && !rejectedLogin)
                    {
                        logs.AppendText("The server has disconnected\n");

                        // connection state front
                        button_connect.Enabled = true;
                        button_disconnect.Enabled = false;
                        button_get_posts.Enabled = false;
                        button_create_post.Enabled = false;
                        textBox_createPost.Enabled = false;
                        textBox_ip.Enabled = true;
                        textBox_port.Enabled = true;
                        username_area.Enabled = true;
                        
                    }
                    connected = false;
                    clientSocket.Close();

                }

            }
        }

        

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            if (!terminating)
            {
                try
                {
                    connected = false;
                    clientSocket.Close();
                    logs.AppendText("You are disconnected from the server\n");


                    didIDisc = true;

                    // Handle frontend
                    // connection buttons
                    button_connect.Enabled = true;
                    button_disconnect.Enabled = false;
                    // connection text boxes
                    textBox_ip.Enabled = true;
                    textBox_port.Enabled = true;
                    // login text boxes
                    username_area.Enabled = true;
                    // login buttons
                    button_disconnect.Enabled = false;

                    button_create_post.Enabled = false;
                    button_get_posts.Enabled = false;
                    textBox_createPost.Enabled = false;
                }
                catch
                {
                    logs.AppendText("Something happened when disconnecting...\n");
                }

            }


        }

        private void loginHandle()
        {
           

            string message = loginheader + username_area.Text; // header  +  payload

            if (message != "" && message.Length <= 128)
            {
                try
                {
                    Byte[] buffer = Encoding.Default.GetBytes(message);
                    clientSocket.Send(buffer); // send login request
                }
                catch
                {
                    logs.AppendText("Something while logging in...\n");
                }
            }
        }

        private void button_create_post_Click(object sender, EventArgs e)
        {
            string post = textBox_createPost.Text;
            if (post.Length != 0)
            {
                if (post.Contains("&"))
                {
                    logs.AppendText("Posts cannot include ampersand\n");
                    return;
                }
                string time = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");

                try
                {
                    Byte[] buffer = Encoding.Default.GetBytes(createPostHeader + post + "\n" + time);
                    clientSocket.Send(buffer); // send login request
                }
                catch
                {
                    logs.AppendText("Something happened while posting...\n");
                }
            }
            else
            {
                logs.AppendText("Posts cannot be empty!\n");
            }
        }

        private void button_get_posts_Click(object sender, EventArgs e)
        {
            try
            {
                string message = getPostHeader;
                Byte[] buffer = Encoding.Default.GetBytes(message);
                clientSocket.Send(buffer);
                logs.AppendText("Posts\n");
            }
            catch
            {
                logs.AppendText("Something happened while getting posts...\n");
            }
        }
    }
}