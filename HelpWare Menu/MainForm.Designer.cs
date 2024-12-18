using System.Drawing;
using System.Windows.Forms;
using System;

namespace TriggerBotApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnToggle;
        private Button btnSetGrabZone;
        private TrackBar trackBarTapTime;
        private Label lblTapTime;
        private Label lblTapTimeValue;
        private Label lblGrabWidth;
        private Label lblGrabHeight;
        private TextBox txtGrabWidth;
        private TextBox txtGrabHeight;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnToggle = new System.Windows.Forms.Button();
            this.trackBarTapTime = new System.Windows.Forms.TrackBar();
            this.lblTapTime = new System.Windows.Forms.Label();
            this.lblTapTimeValue = new System.Windows.Forms.Label();
            this.lblGrabWidth = new System.Windows.Forms.Label();
            this.lblGrabHeight = new System.Windows.Forms.Label();
            this.txtGrabWidth = new System.Windows.Forms.TextBox();
            this.txtGrabHeight = new System.Windows.Forms.TextBox();
            this.btnSetGrabZone = new System.Windows.Forms.Button();
            this.HelpWare = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTapTime)).BeginInit();
            this.SuspendLayout();
            // 
            // btnToggle
            // 
            this.btnToggle.Location = new System.Drawing.Point(12, 9);
            this.btnToggle.Name = "btnToggle";
            this.btnToggle.Size = new System.Drawing.Size(73, 43);
            this.btnToggle.TabIndex = 0;
            this.btnToggle.Text = "Start HelpWare";
            this.btnToggle.UseVisualStyleBackColor = true;
            this.btnToggle.Click += new System.EventHandler(this.btnToggle_Click);
            // 
            // trackBarTapTime
            // 
            this.trackBarTapTime.BackColor = System.Drawing.Color.IndianRed;
            this.trackBarTapTime.Location = new System.Drawing.Point(224, 181);
            this.trackBarTapTime.Maximum = 200;
            this.trackBarTapTime.Minimum = 1;
            this.trackBarTapTime.Name = "trackBarTapTime";
            this.trackBarTapTime.Size = new System.Drawing.Size(241, 45);
            this.trackBarTapTime.TabIndex = 2;
            this.trackBarTapTime.TickFrequency = 20;
            this.trackBarTapTime.Value = 60;
            this.trackBarTapTime.Scroll += new System.EventHandler(this.trackBarTapTime_Scroll);
            // 
            // lblTapTime
            // 
            this.lblTapTime.AutoSize = true;
            this.lblTapTime.Location = new System.Drawing.Point(230, 211);
            this.lblTapTime.Name = "lblTapTime";
            this.lblTapTime.Size = new System.Drawing.Size(55, 13);
            this.lblTapTime.TabIndex = 3;
            this.lblTapTime.Text = "Tap Time:";
            // 
            // lblTapTimeValue
            // 
            this.lblTapTimeValue.AutoSize = true;
            this.lblTapTimeValue.Location = new System.Drawing.Point(424, 211);
            this.lblTapTimeValue.Name = "lblTapTimeValue";
            this.lblTapTimeValue.Size = new System.Drawing.Size(35, 13);
            this.lblTapTimeValue.TabIndex = 4;
            this.lblTapTimeValue.Text = "60 ms";
            // 
            // lblGrabWidth
            // 
            this.lblGrabWidth.AutoSize = true;
            this.lblGrabWidth.Location = new System.Drawing.Point(318, 12);
            this.lblGrabWidth.Name = "lblGrabWidth";
            this.lblGrabWidth.Size = new System.Drawing.Size(41, 13);
            this.lblGrabWidth.TabIndex = 5;
            this.lblGrabWidth.Text = "FOV X:";
            // 
            // lblGrabHeight
            // 
            this.lblGrabHeight.AutoSize = true;
            this.lblGrabHeight.Location = new System.Drawing.Point(318, 39);
            this.lblGrabHeight.Name = "lblGrabHeight";
            this.lblGrabHeight.Size = new System.Drawing.Size(41, 13);
            this.lblGrabHeight.TabIndex = 6;
            this.lblGrabHeight.Text = "FOV Y:";
            // 
            // txtGrabWidth
            // 
            this.txtGrabWidth.BackColor = System.Drawing.Color.White;
            this.txtGrabWidth.Location = new System.Drawing.Point(365, 9);
            this.txtGrabWidth.Name = "txtGrabWidth";
            this.txtGrabWidth.Size = new System.Drawing.Size(100, 20);
            this.txtGrabWidth.TabIndex = 7;
            this.txtGrabWidth.Text = "2-50";
            // 
            // txtGrabHeight
            // 
            this.txtGrabHeight.Location = new System.Drawing.Point(365, 35);
            this.txtGrabHeight.Name = "txtGrabHeight";
            this.txtGrabHeight.Size = new System.Drawing.Size(100, 20);
            this.txtGrabHeight.TabIndex = 8;
            this.txtGrabHeight.Text = "2-50";
            // 
            // btnSetGrabZone
            // 
            this.btnSetGrabZone.Location = new System.Drawing.Point(365, 61);
            this.btnSetGrabZone.Name = "btnSetGrabZone";
            this.btnSetGrabZone.Size = new System.Drawing.Size(100, 22);
            this.btnSetGrabZone.TabIndex = 9;
            this.btnSetGrabZone.Text = "SAVE FOV";
            this.btnSetGrabZone.UseVisualStyleBackColor = true;
            this.btnSetGrabZone.Click += new System.EventHandler(this.btnSetGrabZone_Click);
            // 
            // HelpWare
            // 
            this.HelpWare.Icon = ((System.Drawing.Icon)(resources.GetObject("HelpWare.Icon")));
            this.HelpWare.Text = "HelpWare";
            this.HelpWare.Visible = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "20";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(183, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "205";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(101, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Enemy Color:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.IndianRed;
            this.BackgroundImage = global::HelpWare_Menu.Properties.Resources.helpware2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(472, 233);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSetGrabZone);
            this.Controls.Add(this.txtGrabHeight);
            this.Controls.Add(this.txtGrabWidth);
            this.Controls.Add(this.lblGrabHeight);
            this.Controls.Add(this.lblGrabWidth);
            this.Controls.Add(this.lblTapTimeValue);
            this.Controls.Add(this.lblTapTime);
            this.Controls.Add(this.trackBarTapTime);
            this.Controls.Add(this.btnToggle);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "discord.gg/helpware";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTapTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private NotifyIcon HelpWare;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}

