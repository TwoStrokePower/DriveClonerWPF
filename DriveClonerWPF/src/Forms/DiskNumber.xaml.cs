using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Path = System.IO.Path;

namespace DriveClonerWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class DiskNumber : Window
    {
        private Params _params = new Params();
        private MainFormCopy mainForm;
        private InsertDrive insertDrive;
        private SettingsForm settings = null;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();


        public DiskNumber()
        {
            InitializeComponent();
            _params.InitializeParams();
        }

        private string ParceDiskName(string diskName)
        {
            string result = diskName;
            foreach (char invalidChar in Path.InvalidPathChars)
                result = result.Replace(invalidChar, '_');
            result = result.Replace(Path.DirectorySeparatorChar, '_');
            result = result.Replace(Path.AltDirectorySeparatorChar, '_');
            result = result.Replace(Path.VolumeSeparatorChar, '_');
            return result;
        }

        private void SetCurrFormToParam()
        {
            _params.DiskName = ParceDiskName(textBoxDiskName.Text);
            _params.DiskNameBy = ParceDiskName(textBoxDiskNameByB.Text);

            _params.Post = new Post();
            _params.Post.Number = textBoxPostNumber.Text;
            _params.Post.DateStamp = dateTimePickerPostStamp.SelectedDate.Value;
            _params.Post.InputNumber = textBoInputPostNumber.Text;
            _params.Post.InputDateStamp = dateTimePickerPostInsideStamp.SelectedDate.Value;
        }
        

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
        protected static extern int mciSendString
        (string mciCommand,
            StringBuilder returnValue,
            int returnLenght,
            IntPtr callback);


        private void OpenDrive()
        {
            StringBuilder resultString = new StringBuilder();
            int result = mciSendString("set cdaudio door open", resultString, 0, IntPtr.Zero);
            return;
        }

        private void OpenStepCopy()
        {
            mainForm = new MainFormCopy(_params);
            this.Hide();
            mainForm.ShowDialog();
        }

        private void WaitDiskLoad()
        {
            bool isEnd = false;
            while (!isEnd)
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(_params.DiskDrive);
                    List<FileInfo> fi1 = di.EnumerateFiles("*.*", SearchOption.AllDirectories).ToList();
                    List<DirectoryInfo> di1 = di.EnumerateDirectories("*.*", SearchOption.AllDirectories).ToList();
                    if (fi1.Count != 0 || di1.Count != 0)
                    {
                        isEnd = true;
                    }
                }
                catch
                { }
            }
            //StringBuilder resultString = new StringBuilder();
            //int res = mciSendString("get cdaudio door closed", resultString, 0, IntPtr.Zero);
            //return;
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            insertDrive = new InsertDrive(ref backgroundWorker);
            insertDrive.ShowDialog();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            insertDrive.Close();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SetCurrFormToParam();
            OpenDrive();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
            WaitDiskLoad();
            backgroundWorker.CancelAsync();
            OpenStepCopy();
            OpenDrive();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            settings = new SettingsForm();
            settings.ShowDialog();
            Properties.Settings.Default.Reload();
        }
    }
}
