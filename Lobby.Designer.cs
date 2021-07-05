
namespace Chess
{
    partial class Lobby
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
            this.label1 = new System.Windows.Forms.Label();
            this.but_SinglePlay = new System.Windows.Forms.Button();
            this.but_MultiPlay = new System.Windows.Forms.Button();
            this.InputAddress = new System.Windows.Forms.TextBox();
            this.but_JoinServer = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(263, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "체스";
            // 
            // but_SinglePlay
            // 
            this.but_SinglePlay.Location = new System.Drawing.Point(243, 89);
            this.but_SinglePlay.Name = "but_SinglePlay";
            this.but_SinglePlay.Size = new System.Drawing.Size(111, 43);
            this.but_SinglePlay.TabIndex = 1;
            this.but_SinglePlay.Text = "싱글 플레이";
            this.but_SinglePlay.UseVisualStyleBackColor = true;
            this.but_SinglePlay.Click += new System.EventHandler(this.but_SinglePlay_Click);
            // 
            // but_MultiPlay
            // 
            this.but_MultiPlay.Location = new System.Drawing.Point(243, 152);
            this.but_MultiPlay.Name = "but_MultiPlay";
            this.but_MultiPlay.Size = new System.Drawing.Size(111, 43);
            this.but_MultiPlay.TabIndex = 2;
            this.but_MultiPlay.Text = "멀티 플레이";
            this.but_MultiPlay.UseVisualStyleBackColor = true;
            this.but_MultiPlay.Click += new System.EventHandler(this.but_MultiPlay_Click);
            // 
            // InputAddress
            // 
            this.InputAddress.Font = new System.Drawing.Font("굴림", 12F);
            this.InputAddress.Location = new System.Drawing.Point(202, 214);
            this.InputAddress.Name = "InputAddress";
            this.InputAddress.Size = new System.Drawing.Size(185, 26);
            this.InputAddress.TabIndex = 3;
            // 
            // but_JoinServer
            // 
            this.but_JoinServer.Location = new System.Drawing.Point(257, 260);
            this.but_JoinServer.Name = "but_JoinServer";
            this.but_JoinServer.Size = new System.Drawing.Size(72, 25);
            this.but_JoinServer.TabIndex = 4;
            this.but_JoinServer.Text = "입장";
            this.but_JoinServer.UseVisualStyleBackColor = true;
            this.but_JoinServer.Click += new System.EventHandler(this.but_JoinServer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(408, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 30);
            this.label2.TabIndex = 5;
            this.label2.Text = "-장재빈";
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 309);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.but_JoinServer);
            this.Controls.Add(this.InputAddress);
            this.Controls.Add(this.but_MultiPlay);
            this.Controls.Add(this.but_SinglePlay);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Lobby";
            this.Text = "메인화면";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button but_SinglePlay;
        private System.Windows.Forms.Button but_MultiPlay;
        private System.Windows.Forms.TextBox InputAddress;
        private System.Windows.Forms.Button but_JoinServer;
        private System.Windows.Forms.Label label2;
    }
}