using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DriveClonerWPF
{
    /// <summary>
    /// Логика взаимодействия для SettingsForm.xaml
    /// </summary>
    public partial class SettingsForm : Window
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            InitializePathToCopy();
            InitializeDriveList();
        }

        private void InitializeDriveList()
        {
            comboBox1.Items.Clear();
            DriveInfo[] driveList = DriveInfo.GetDrives();
            foreach (DriveInfo di in driveList)
            {
                if (di.DriveType == DriveType.Removable)
                {
                    comboBox1.Items.Add(di.Name);
                    if (di.Name == Properties.Settings.Default.DiskDrive)
                    {
                        comboBox1.SelectedItem = di.Name;
                    }
                }
            }
        }

        private void InitializePathToCopy()
        {
            label3.Text = Properties.Settings.Default.DestDir;
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label3.Text = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.DestDir = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            this.Close();
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.DiskDrive = comboBox1.SelectedItem.ToString();
        }
    }
}
