﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DriveClonerWPF
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class InsertDrive : Window
    {
        private BackgroundWorker bgWorker = null;

        public InsertDrive(ref BackgroundWorker bgWorker)
        {
            InitializeComponent();
            this.bgWorker = bgWorker;
            label1.Text += " " + Properties.Settings.Default.DiskDrive;
        }

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
        protected static extern int mciSendString
        (string mciCommand,
            StringBuilder returnValue,
            int returnLenght,
            IntPtr callback);

        private void CloseDrive()
        {
            StringBuilder resultString = new StringBuilder();
            int result = mciSendString("set cdaudio door closed", resultString, 0, IntPtr.Zero);
            return;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            CloseDrive();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.bgWorker.CancellationPending)
                this.Close();
        }
        private void InsertDrive_Load(object sender, EventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Start();
        }
    }
}
