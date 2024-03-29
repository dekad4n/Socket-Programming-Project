﻿namespace client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.username_area = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.button_disconnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_createPost = new System.Windows.Forms.TextBox();
            this.button_create_post = new System.Windows.Forms.Button();
            this.button_get_posts = new System.Windows.Forms.Button();
            this.button_myPostsbutton_myPosts = new System.Windows.Forms.Button();
            this.button_friendsPosts = new System.Windows.Forms.Button();
            this.deletePost_area = new System.Windows.Forms.TextBox();
            this.button_deletePost = new System.Windows.Forms.Button();
            this.button_addFriend = new System.Windows.Forms.Button();
            this.addFriend_area = new System.Windows.Forms.TextBox();
            this.friends_area = new System.Windows.Forms.ListBox();
            this.button_removeFriend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(115, 77);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(275, 31);
            this.textBox_ip.TabIndex = 0;
            // 
            // username_area
            // 
            this.username_area.Location = new System.Drawing.Point(157, 151);
            this.username_area.Name = "username_area";
            this.username_area.Size = new System.Drawing.Size(275, 31);
            this.username_area.TabIndex = 2;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(483, 77);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(275, 31);
            this.textBox_port.TabIndex = 4;
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(483, 224);
            this.logs.Name = "logs";
            this.logs.ReadOnly = true;
            this.logs.Size = new System.Drawing.Size(348, 476);
            this.logs.TabIndex = 7;
            this.logs.Text = "";
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(522, 138);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(199, 56);
            this.button_connect.TabIndex = 9;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // button_disconnect
            // 
            this.button_disconnect.Enabled = false;
            this.button_disconnect.Location = new System.Drawing.Point(632, 717);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(199, 51);
            this.button_disconnect.TabIndex = 10;
            this.button_disconnect.Text = "Disconnect";
            this.button_disconnect.UseVisualStyleBackColor = true;
            this.button_disconnect.Click += new System.EventHandler(this.button_disconnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(407, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 25);
            this.label2.TabIndex = 12;
            this.label2.Text = "Port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 25);
            this.label3.TabIndex = 13;
            this.label3.Text = "Username:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 325);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 25);
            this.label4.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label5.Location = new System.Drawing.Point(6, 424);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 24);
            this.label5.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label6.Location = new System.Drawing.Point(5, 525);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 24);
            this.label6.TabIndex = 16;
            // 
            // textBox_createPost
            // 
            this.textBox_createPost.Enabled = false;
            this.textBox_createPost.Location = new System.Drawing.Point(54, 494);
            this.textBox_createPost.Name = "textBox_createPost";
            this.textBox_createPost.Size = new System.Drawing.Size(256, 31);
            this.textBox_createPost.TabIndex = 17;
            // 
            // button_create_post
            // 
            this.button_create_post.Enabled = false;
            this.button_create_post.Location = new System.Drawing.Point(326, 487);
            this.button_create_post.Name = "button_create_post";
            this.button_create_post.Size = new System.Drawing.Size(151, 45);
            this.button_create_post.TabIndex = 18;
            this.button_create_post.Text = "Create Post";
            this.button_create_post.UseVisualStyleBackColor = true;
            this.button_create_post.Click += new System.EventHandler(this.button_create_post_Click);
            // 
            // button_get_posts
            // 
            this.button_get_posts.Enabled = false;
            this.button_get_posts.Location = new System.Drawing.Point(40, 700);
            this.button_get_posts.Name = "button_get_posts";
            this.button_get_posts.Size = new System.Drawing.Size(199, 56);
            this.button_get_posts.TabIndex = 19;
            this.button_get_posts.Text = "All Posts";
            this.button_get_posts.UseVisualStyleBackColor = true;
            this.button_get_posts.Click += new System.EventHandler(this.button_get_posts_Click);
            // 
            // button_myPostsbutton_myPosts
            // 
            this.button_myPostsbutton_myPosts.Enabled = false;
            this.button_myPostsbutton_myPosts.Location = new System.Drawing.Point(278, 700);
            this.button_myPostsbutton_myPosts.Name = "button_myPostsbutton_myPosts";
            this.button_myPostsbutton_myPosts.Size = new System.Drawing.Size(199, 56);
            this.button_myPostsbutton_myPosts.TabIndex = 20;
            this.button_myPostsbutton_myPosts.Text = "My Posts";
            this.button_myPostsbutton_myPosts.UseVisualStyleBackColor = true;
            this.button_myPostsbutton_myPosts.Click += new System.EventHandler(this.button_myPostsbutton_myPosts_Click);
            // 
            // button_friendsPosts
            // 
            this.button_friendsPosts.Enabled = false;
            this.button_friendsPosts.Location = new System.Drawing.Point(157, 623);
            this.button_friendsPosts.Name = "button_friendsPosts";
            this.button_friendsPosts.Size = new System.Drawing.Size(199, 56);
            this.button_friendsPosts.TabIndex = 21;
            this.button_friendsPosts.Text = "Friends\' Posts";
            this.button_friendsPosts.UseVisualStyleBackColor = true;
            this.button_friendsPosts.Click += new System.EventHandler(this.button_friendsPosts_Click);
            // 
            // deletePost_area
            // 
            this.deletePost_area.Enabled = false;
            this.deletePost_area.Location = new System.Drawing.Point(54, 557);
            this.deletePost_area.Name = "deletePost_area";
            this.deletePost_area.Size = new System.Drawing.Size(256, 31);
            this.deletePost_area.TabIndex = 22;
            // 
            // button_deletePost
            // 
            this.button_deletePost.Enabled = false;
            this.button_deletePost.Location = new System.Drawing.Point(326, 550);
            this.button_deletePost.Name = "button_deletePost";
            this.button_deletePost.Size = new System.Drawing.Size(151, 45);
            this.button_deletePost.TabIndex = 23;
            this.button_deletePost.Text = "Delete Post";
            this.button_deletePost.UseVisualStyleBackColor = true;
            this.button_deletePost.Click += new System.EventHandler(this.button_deletePost_Click);
            // 
            // button_addFriend
            // 
            this.button_addFriend.Enabled = false;
            this.button_addFriend.Location = new System.Drawing.Point(326, 413);
            this.button_addFriend.Name = "button_addFriend";
            this.button_addFriend.Size = new System.Drawing.Size(151, 45);
            this.button_addFriend.TabIndex = 24;
            this.button_addFriend.Text = "Add Friend";
            this.button_addFriend.UseVisualStyleBackColor = true;
            this.button_addFriend.Click += new System.EventHandler(this.button_addFriend_Click);
            // 
            // addFriend_area
            // 
            this.addFriend_area.Enabled = false;
            this.addFriend_area.Location = new System.Drawing.Point(54, 420);
            this.addFriend_area.Name = "addFriend_area";
            this.addFriend_area.Size = new System.Drawing.Size(256, 31);
            this.addFriend_area.TabIndex = 25;
            // 
            // friends_area
            // 
            this.friends_area.Enabled = false;
            this.friends_area.FormattingEnabled = true;
            this.friends_area.ItemHeight = 25;
            this.friends_area.Location = new System.Drawing.Point(54, 207);
            this.friends_area.Name = "friends_area";
            this.friends_area.Size = new System.Drawing.Size(256, 179);
            this.friends_area.TabIndex = 26;
            // 
            // button_removeFriend
            // 
            this.button_removeFriend.Enabled = false;
            this.button_removeFriend.Location = new System.Drawing.Point(326, 267);
            this.button_removeFriend.Name = "button_removeFriend";
            this.button_removeFriend.Size = new System.Drawing.Size(151, 83);
            this.button_removeFriend.TabIndex = 27;
            this.button_removeFriend.Text = "Remove Friend";
            this.button_removeFriend.UseVisualStyleBackColor = true;
            this.button_removeFriend.Click += new System.EventHandler(this.button_removeFriend_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 780);
            this.Controls.Add(this.button_removeFriend);
            this.Controls.Add(this.friends_area);
            this.Controls.Add(this.addFriend_area);
            this.Controls.Add(this.button_addFriend);
            this.Controls.Add(this.button_deletePost);
            this.Controls.Add(this.deletePost_area);
            this.Controls.Add(this.button_friendsPosts);
            this.Controls.Add(this.button_myPostsbutton_myPosts);
            this.Controls.Add(this.button_get_posts);
            this.Controls.Add(this.button_create_post);
            this.Controls.Add(this.textBox_createPost);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_disconnect);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.username_area);
            this.Controls.Add(this.textBox_ip);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox username_area;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Button button_disconnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_createPost;
        private System.Windows.Forms.Button button_create_post;
        private System.Windows.Forms.Button button_get_posts;
        private System.Windows.Forms.Button button_myPostsbutton_myPosts;
        private System.Windows.Forms.Button button_friendsPosts;
        private System.Windows.Forms.TextBox deletePost_area;
        private System.Windows.Forms.Button button_deletePost;
        private System.Windows.Forms.Button button_addFriend;
        private System.Windows.Forms.TextBox addFriend_area;
        private System.Windows.Forms.ListBox friends_area;
        private System.Windows.Forms.Button button_removeFriend;
    }
}

