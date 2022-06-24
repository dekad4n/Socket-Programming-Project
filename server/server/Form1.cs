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
    class Friends
    {
        public Friends(string username)
        {
            myUsername = username;
        }
        public List<string> getMyFriends()
        {
            string[] lines = File.ReadAllLines(@"../../friends.txt");
            List<string> friends = new List<string>();
            foreach (string line in lines)
            {

                string[] lineDivided = line.Split('&');
                if (lineDivided[0] == myUsername || lineDivided[1] == myUsername)
                {
                    friends.Add(lineDivided[0] == myUsername ? lineDivided[1] : lineDivided[0]);
                }

            }
            Console.WriteLine(friends.Count);
            return friends;
        }

        public string addFriend(string otherUsername)
        {
            bool isInDatabase = checkUserInDatabase(otherUsername);
            if(otherUsername == myUsername)
            {
                return otherUsername + " you cannot add yourself as friend...";
            }

            if (!isInDatabase)
            {
                return "User that you are trying to add is not in database!";
            }
            string[] lines = File.ReadAllLines(@"../../friends.txt");
            foreach (string line in lines)
            {
                string[] line_splitted = line.Split('&'); 
                if ((line_splitted[0] == otherUsername &&  line_splitted[1] == myUsername) ||(line_splitted[1] == otherUsername && line_splitted[0] == myUsername))
                {
                    return "You are already friends with " + otherUsername;
                }
            }
            using (StreamWriter file = new StreamWriter(@"../../friends.txt", append: true))
            {
                file.WriteLine(myUsername + "&"  + otherUsername);

            }
            return "You have successfully added " + otherUsername;
        }
        public string removeFriend(string otherUsername)
        {
            bool didIDelete = false;
            var tempFile = Path.GetTempFileName();
            var linesToKeep = File.ReadLines(@"../../friends.txt").Where(
                current =>
                {
                 
                    string[] currentLine = current.Split('&'); // split line with &
                    if ((otherUsername == currentLine[1] && myUsername == currentLine[0]) || (otherUsername == currentLine[0] && myUsername == currentLine[1]))
                    {
                        didIDelete = true;

                        return false;
                    }
                    return true; // do not delete
                }
                );

            File.WriteAllLines(tempFile, linesToKeep);

            File.Delete(@"../../friends.txt");
            File.Move(tempFile, @"../../friends.txt");
            if(didIDelete)
            {
                return otherUsername;
            }
            return "NOT OK";
        }
        private bool postByteSend(string msg, Socket thisClient)
        {
            try
            {
                msg = msg.PadRight(128, '\0');
                Byte[] sender_buffer = Encoding.Default.GetBytes(msg);
                int x = thisClient.Send(sender_buffer);

            }
            catch
            {
                return false;


            }
            return true;
        }
        private bool checkUserInDatabase(string otherUsername)
        {
            string[] lines = File.ReadAllLines(@"../../user-db.txt");
            foreach (string line in lines)
            {
                if (line == otherUsername)
                {
                    return true;
                }

            }
            return false;
        }

        public bool sendFriendsPosts(Socket thisClient)
        {
            List<string> friendsList = getMyFriends();

            try
            {
                string msg = ("CustomMessage\n" +"Your friends posts\n" ).PadRight(128, '\0');
                Byte[] sender_buffer = Encoding.Default.GetBytes(msg);
                int x = thisClient.Send(sender_buffer);

            }
            catch
            {
            }
            bool isFirstLine = true;
            bool allPostsSent = true;
            string[] lines = File.ReadAllLines(@"../../posts.txt");
            foreach (string line in lines)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;

                    continue;
                }

                string[] lineDivided = line.Split('&');
                if (friendsList.Contains(lineDivided[0]))
                {
                    try
                    {
                        bool temp = postByteSend("GetPosts\n" + lineDivided[0] + "\n" + lineDivided[1] + "\n" + lineDivided[2] + "\n" + lineDivided[3] + "\n\n", thisClient);
                        if(!temp)
                        {
                            allPostsSent = false;
                        }
                    }
                    catch
                    {
                        allPostsSent = false;
                    }
                }

            }

            return allPostsSent;

        }
        private string myUsername;
    }
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
        string deletePostHeader = "DeletePost\n";
        string addFriendHeader = "AddFriend\n";
        string removeFriendHeader = "RemoveFriend\n";
        string getFriendsHeader = "GetFriends\n";
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
                serverSocket.Listen(5);

                listening = true;
                listen_button.Enabled = false;

                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                textBox_port.Enabled = false;

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
                    string incomingMessage = Encoding.Default.GetString(buffer);

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
                        Friends myFriends = new Friends(connectedUsers[loginCount - 1]);
                        
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
                    else if (msg_splitted[0] == "CreatePost")
                    {
                        createPost(msg_splitted[1], msg_splitted[2], connectedUsers[loginCount - 1]);
                        sendNewPostMsg(createPostHeader + connectedUsers[loginCount - 1] + " you have created new post:\n" + msg_splitted[1], thisClient);

                    }
                    else if (msg_splitted[0] == "GetPosts")
                    {
                        sendPosts(connectedUsers[loginCount - 1], thisClient);
                    }
                    else if(msg_splitted[0] == "GetMyPosts")
                    {
                        sendMyPosts(msg_splitted[1], thisClient);
                    }
                    else if(msg_splitted[0] == "DeletePost")
                    {
                        deletePost(msg_splitted[1], thisClient, connectedUsers[loginCount - 1]);
                    }
                    else if(msg_splitted[0] == "AddFriend")
                    {
                        addFriend(connectedUsers[loginCount - 1], msg_splitted[1], thisClient);
                    }
                    else if(msg_splitted[0] == "GetFriends")
                    {
                        sendFriends(connectedUsers[loginCount-1], thisClient);
                    }
                    else if(msg_splitted[0] == "RemoveFriend")
                    {
                        removeFriend(connectedUsers[loginCount - 1],msg_splitted[1] ,thisClient);
                    }
                    else if(msg_splitted[0] == "GetFriendsPosts")
                    {
                        getFriendsPosts(connectedUsers[loginCount - 1], thisClient);
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
        private void getFriendsPosts(string username, Socket thisClient)
        {
            logs.AppendText(username + " has requested to send friends post!\n");
            Friends myFriends = new Friends(username);

            bool allPostsSent = myFriends.sendFriendsPosts(thisClient);

            if(allPostsSent)
            {
                logs.AppendText("All of friends' posts send to " + username + "\n");
            }
            else
            {
                logs.AppendText("Not all of friends' posts send to " + username + "\n");

            }


        }
        private void removeFriend(string username, string removedFriend, Socket thisClient)
        {
            logs.AppendText(username + " wants to remove friend with " + removedFriend + "\n");
            Friends myFriends = new Friends(username);

            string result = myFriends.removeFriend(removedFriend);
            string msg = removeFriendHeader;
            if(result == "NOT OK")
            {
                logs.AppendText(username + " and " + removedFriend + " has no relationship, nothing happened\n");
                try
                {
                    msg = msg + "No friendship relationship";
                    msg = msg.PadRight(128, '\0');
                    Byte[] sender_buffer = Encoding.Default.GetBytes(msg);
                    int x = thisClient.Send(sender_buffer);

                    
                }
                catch
                {
                    logs.AppendText("We couldn't react the user for unsuccessful remove friend!\n");
                }
            }
            else
            {
                logs.AppendText("Successfully removed relationship with " + result + "\n");
                try
                {
                    msg = msg + "Successfully removed relationship with " + result + "\n";
                    msg = msg + result;
                    msg = msg.PadRight(128, '\0');
                    Byte[] sender_buffer = Encoding.Default.GetBytes(msg);
                    int x = thisClient.Send(sender_buffer);

                    int idx = isConnected(removedFriend);
                    if (idx != -1)
                    {
                        msg = "RemovedYou\n" + username.PadRight(128, '\0');
                        Byte[] addedYou = Encoding.Default.GetBytes(msg);
                        clientSockets[idx].Send(addedYou);
                    }
                }
                catch
                {
                    logs.AppendText("We couldn't react the user for successful remove friend!\n");
                }
            }
        }
        private void sendFriends(string username, Socket thisClient)
        {
            Friends myFriends = new Friends(username);

            List<string> friendsList = myFriends.getMyFriends();

            foreach(string friend in friendsList)
            {
                try
                {
                    string msg = (getFriendsHeader + friend).PadRight(128, '\0');
                    Byte[] sender_buffer = Encoding.Default.GetBytes(msg);
                    int x = thisClient.Send(sender_buffer);

                }
                catch
                {
                    logs.AppendText("We couldn't react the user for sending " + friend + " !\n");

                }
            }
        }

        private int isConnected(string username)
        {
            for(int i = 0; i < connectedUsers.Count; i++)
            {
                if(username == connectedUsers[i])
                {
                    return i;
                }
            }
            return -1;
        }
        private void addFriend(string myUsername, string otherUsername, Socket thisClient)
        {
            logs.AppendText(myUsername + " wants to be friends with " + otherUsername + "\n");
            Friends myFriends = new Friends(myUsername);
            string res = myFriends.addFriend(otherUsername);
            string msg = addFriendHeader + res ;
            try
            {
                msg = msg.PadRight(128, '\0');
                Byte[] sender_buffer = Encoding.Default.GetBytes(msg);
                Console.WriteLine(msg);
                int x = thisClient.Send(sender_buffer);

                if(msg.Contains("success"))
                {
                    msg = getFriendsHeader + otherUsername.PadRight(128, '\0');
                    Byte[] friendSender = Encoding.Default.GetBytes(msg);
                    int y = thisClient.Send(friendSender);
                    logs.AppendText(myUsername + " successfully added " + otherUsername + "\n");

                    int idx = isConnected(otherUsername);
                    if (idx != -1)
                    {
                        msg = "AddedYou\n" +myUsername.PadRight(128, '\0');
                        Byte[] addedYou = Encoding.Default.GetBytes(msg);
                        clientSockets[idx].Send(addedYou);
                    }
                }
                else
                {
                    logs.AppendText(myUsername + " wanted to be friends with " + otherUsername + " but got this error: " + res + "\n");

                }

            }
            catch
            {
                logs.AppendText("We couldn't inform user about: " + msg + " !\n");


            }

        }
        private void deletePost(string postID, Socket thisClient, string username)
        {
            logs.AppendText(username + " wants to delete post with ID " + postID.ToString() + "\n");
            bool isThereSuchPost = false;
            bool isOwnedbyUser = false;
            bool isFirstLine = true;
            var tempFile = Path.GetTempFileName();
            var linesToKeep = File.ReadLines(@"../../posts.txt").Where(
                current =>
                {
                    if(isFirstLine)
                    {
                        isFirstLine = false;
                        return true; // skip the first line
                    }
                    string[] currentLine = current.Split('&'); // split line with &

                    if(postID == currentLine[1])
                    {
                        isThereSuchPost = true;
                        if(currentLine[0] == username)
                        {
                            isOwnedbyUser = true;
                            return false;
                        }
                        else
                        {
                            return true; // do not delete
                        }
                    }
                    return true; // do not delete
                }
                );

            File.WriteAllLines(tempFile, linesToKeep);

            File.Delete(@"../../posts.txt");
            File.Move(tempFile, @"../../posts.txt");

            if(isOwnedbyUser)
            {
                postByteSend(deletePostHeader + "Post with ID " + postID + " is deleted successfully.",thisClient);
                logs.AppendText(username + " deleted post with ID " + postID + " successfully.\n");
                return;
            }
            else if(!isThereSuchPost)
            {
                postByteSend(deletePostHeader + "There is no post with ID " + postID + ".", thisClient);
                logs.AppendText(username + " wanted to delete post with ID " + postID + " but there is no such post.\n");
                return;
            }
            else if(!isOwnedbyUser)
            {
                postByteSend(deletePostHeader + "The post with ID " + postID + " does not belong to you.", thisClient);
                logs.AppendText(username + " wanted to delete post with ID " + postID + " but the owner is different.\n");
                return;
            }
        }
        private void sendNewPostMsg(string msg, Socket thisClient)
        {
            try
            {
                Byte[] sender_buffer = Encoding.Default.GetBytes(msg);
                thisClient.Send(sender_buffer);
            }
            catch
            {
                logs.AppendText("We couldn't react the user, but we created the post!\n");


            }

        }
        private void postByteSend(string msg, Socket thisClient)
        {
            try
            {
                msg = msg.PadRight(128, '\0');
                Byte[] sender_buffer = Encoding.Default.GetBytes(msg);
                int x = thisClient.Send(sender_buffer);
                
            }
            catch
            {
                logs.AppendText("We couldn't react the user for post" + msg + " !\n");


            }

        }
        private void createPost(string postContent, string postDate, string username)
        {
            // get post ID
            string idCountString = File.ReadLines(@"../../posts.txt").First();
            int firstLineInt = Int16.Parse(idCountString);
            using (StreamWriter file = new StreamWriter(@"../../posts.txt", append: true))
            {
                file.WriteLine(username + "&" + (Int16.Parse(idCountString) + 1).ToString() + "&" + postDate + "&" + postContent);

            }
            string[] arrLine = File.ReadAllLines(@"../../posts.txt");
            arrLine[0] = (firstLineInt + 1).ToString();
            File.WriteAllLines(@"../../posts.txt", arrLine);

            logs.AppendText(username + " has created new post:\n" + postContent + "\n");
        }


        private void sendMyPosts(string username, Socket thisClient)
        {
            logs.AppendText(username + " has requested send its own posts.\n");
            string[] lines = File.ReadAllLines(@"../../posts.txt");
            bool isFirstLine = true;
            foreach (string line in lines)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;

                    continue;
                }

                string[] lineDivided = line.Split('&');
                if (lineDivided[0] == username)
                {
                    try
                    {
                        postByteSend(getPostHeader + lineDivided[0] + "\n" + lineDivided[1] + "\n" + lineDivided[2] + "\n" + lineDivided[3] + "\n\n", thisClient);
                        logs.AppendText("Post sent: \n" + "username: " + lineDivided[0] + "\n" + "post ID:" + lineDivided[1] + "\n" + "post date:" + lineDivided[2] + "\n" + "post: " + lineDivided[3] + "\n\n");
                    }
                    catch
                    {
                        Console.WriteLine("err");
                    }
                }

            }
        }
        private void sendPosts(string username, Socket thisClient)
        {
            logs.AppendText(username + " has requested send posts.\n");
            string[] lines = File.ReadAllLines(@"../../posts.txt");
            bool isFirstLine = true;
            foreach (string line in lines)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;

                    continue;
                }

                string[] lineDivided = line.Split('&');
                if (lineDivided[0] != username)
                {
                    try
                    {
                        postByteSend(getPostHeader + lineDivided[0] + "\n" + lineDivided[1] + "\n" + lineDivided[2] + "\n" + lineDivided[3] + "\n\n", thisClient);
                        logs.AppendText("Post sent: \n" + "username: " + lineDivided[0] + "\n" + "post ID:" +lineDivided[1] + "\n" + "post date:" + lineDivided[2] + "\n" + "post: " +lineDivided[3] + "\n\n");
                    }
                    catch
                    {
                        Console.WriteLine("err");
                    }
                }

            }
        }


    }
}
