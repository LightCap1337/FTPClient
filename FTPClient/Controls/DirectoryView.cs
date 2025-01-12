﻿using FTPClient.Common;
using FTPClient.Dialog;
using System;
using System.Linq;
using System.Windows.Forms;

namespace FTPClient.Controls
{
    public partial class DirectoryView : UserControl
    {
        private Uri directoryRoot;
        private bool isRootView = false;
        public event Action closed;

        LoadingOverlay lo;
        public DirectoryView()
        {
            InitializeComponent();
            directoryRoot = FTPHelper.Instance.RootUri;
            RefreshDirectory();
        }

        public DirectoryView(Uri root)
        {
            InitializeComponent();
            directoryRoot = root;
            RefreshDirectory();
        }

        private async void RefreshDirectory()
        {
            LoadStart();
            isRootView = directoryRoot.Equals(FTPHelper.Instance.RootUri);
            label1.Text = directoryRoot.ToString();
            back_btn.Enabled = !isRootView;
            prevDirectory_btn.Enabled = !isRootView;

            DirectoryItemHolder.Controls.Clear();
            var directoryDetails = await FTPHelper.Instance.GetDirectoryDetails(directoryRoot);
            foreach (string item in directoryDetails)
            {
                var info = item.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var name = info.Last();

                if (name != "." && name != "..")
                {
                    DirectoryItem d = new DirectoryItem(directoryRoot, item);
                    AddDirectoryItem(d);
                }
            }
            LoadEnd();
        }

        public void AddDirectoryItem(DirectoryItem d)
        {
            DirectoryItemHolder.Controls.Add(d);
            d.openDirRequest += OpenDir;
            d.DownloadingStarted += () => LoadStart(true);
            d.DownloadingProgress += (progressValue) => lo.SetProgress(progressValue);
            d.DownloadingEnded += LoadEnd;
        }

        public void OpenDir(Uri u)
        {
            DirectoryView dw = new DirectoryView(u);
            dw.Size = Size;
            dw.Dock = Dock;
            Parent.Controls.Add(dw);
            dw.Show();
            dw.closed += () =>
            {
                Show();
            };

            Hide();
        }

        private void back_btn_Click(object sender, EventArgs e)
        {
            closed?.Invoke();
            Dispose();
        }

        private async void uploadFile_btn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadStart(true);
                    var filePath = openFileDialog.FileName;

                    var progressReporter = new Progress<int>();
                    progressReporter.ProgressChanged += (obj, progress) => { lo.SetProgress(progress); };
                    if (!await FTPHelper.Instance.UploadFileToFtp(directoryRoot, filePath, progressReporter))
                        MessageBox.Show("Něco se pokazilo, asi nemáte práva");

                    RefreshDirectory();
                    LoadEnd();
                }
            }
        }

        private void LoadStart(bool progressBar = false)
        {
            if (lo != null) lo.Dispose();

            lo = new LoadingOverlay(progressBar);
            Controls.Add(lo);
            lo.BringToFront();
            panel1.Enabled = false;
            prevDirectory_btn.Enabled = false;
        }

        private void LoadEnd()
        {
            if (lo != null)
            lo.Dispose();
            panel1.Enabled = true;
            prevDirectory_btn.Enabled = true;
        }

        private void dirCreate_btn_Click(object sender, EventArgs e)
        {

            SingleInputDialog sid = new SingleInputDialog("Vytvořit adresář", "Název nového adresáře", "Vytvořit");
            if (sid.ShowDialog() == DialogResult.OK)
            {
                if (FTPHelper.Instance.CreateDirectory(directoryRoot, sid.OutputText))
                    RefreshDirectory();
                else
                    MessageBox.Show("Pravděpodobně nemáte práva pro tento adresář.");
            }
        }

        private void prevDirectory_btn_Click(object sender, EventArgs e)
        {
            DirectoryView dw = new DirectoryView(Helper.RemoveLastSegment(directoryRoot));
            dw.Size = Size;
            dw.Dock = Dock;

            Parent.Controls.Add(dw);

            dw.Show();

            dw.closed += () =>
            {
                Show();
            };

            Hide();
        }
    }
}
