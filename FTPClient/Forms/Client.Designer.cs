﻿
namespace FTPClient.Forms
{
    partial class Client
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
            this.rootView = new FTPClient.Controls.DirectoryView();
            this.SuspendLayout();
            // 
            // rootView
            // 
            this.rootView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootView.Location = new System.Drawing.Point(0, 0);
            this.rootView.Name = "rootView";
            this.rootView.Size = new System.Drawing.Size(702, 475);
            this.rootView.TabIndex = 0;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 475);
            this.Controls.Add(this.rootView);
            this.Name = "Client";
            this.Text = "Client";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.DirectoryView rootView;
    }
}