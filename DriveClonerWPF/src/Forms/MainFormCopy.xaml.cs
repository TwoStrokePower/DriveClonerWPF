using CopyerCKO.src;
using CopyerCKO.src.Types;
using SecuredStorage.ISO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для MainFormCopy.xaml
    /// </summary>
    public partial class MainFormCopy : Window
    {
        private Params param;
        private FileOperations fileOperations;
        public MainFormCopy()
        {
            InitializeComponent();
            this.param = _param;
        }

        private bool IsDistrDisk()
        {
            return File.Exists(Path.Combine(param.DiskDrive, "install.bat"));
        }

        private bool IsShluzDisk()
        {
            DirectoryInfo di = new DirectoryInfo(param.DiskDrive);
            List<DirectoryInfo> di2 = di.EnumerateDirectories("CKO_*.*", SearchOption.TopDirectoryOnly).ToList();
            if (di2.Count != 0)
                return false;
            List<DirectoryInfo> di3 = di.EnumerateDirectories("NT_*.*", SearchOption.TopDirectoryOnly).ToList();
            if (di3.Count != 0)
                return true;
            else
                return false;
        }

        private bool IsCkoDirectoryExist()
        {
            if (IsShluzDisk())
                return true;
            else
            {
                DirectoryInfo di = new DirectoryInfo(param.DiskDrive);
                List<DirectoryInfo> di2 = di.EnumerateDirectories("CKO_*.*", SearchOption.TopDirectoryOnly).ToList();
                if (di2.Count != 0)
                    return true;
                else
                    return false;
            }
        }

        private bool IsUpoDirectoryExist()
        {
            DirectoryInfo di = new DirectoryInfo(param.DiskDrive);
            List<DirectoryInfo> di2 = di.EnumerateDirectories("UPO_*.*", SearchOption.TopDirectoryOnly).ToList();
            if (di2.Count != 0)
                return true;
            else
                return false;
        }
        private void InitializeCkoType()
        {
            if (IsCkoDirectoryExist())
            {
                param.PoType = PoType.TypeOfCko.type1;
                domainUpDownTypeCko.SelectedIndex = (int)PoType.TypeOfCko.type1;
                domainUpDownTypeCko.SelectedItem = "СКО";
            }
            else if (IsUpoDirectoryExist())
            {
                param.PoType = PoType.TypeOfCko.type2;
                domainUpDownTypeCko.SelectedIndex = (int)PoType.TypeOfCko.type2;
                domainUpDownTypeCko.SelectedItem = "УПО";
            }
            else
            {
                param.PoType = PoType.TypeOfCko.type3;
                domainUpDownTypeCko.SelectedIndex = (int)PoType.TypeOfCko.type3;
                domainUpDownTypeCko.SelectedItem = "УМ";
            }

        }

        private void InitializeDiskType()
        {
            if (IsDistrDisk())
            {
                param.DiskType = DiskType.TypeOfDisk.driveType2;
                domainUpDownDiskType.SelectedIndex = (int)DiskType.TypeOfDisk.driveType2;
                domainUpDownDiskType.SelectedItem = "Дистрибутивный";
            }
            else if (IsShluzDisk())
            {
                param.DiskType = DiskType.TypeOfDisk.driveType3;
                domainUpDownDiskType.SelectedIndex = (int)DiskType.TypeOfDisk.driveType3;
                domainUpDownDiskType.SelectedItem = "Шлюзовой";
            }
            else
            {
                param.DiskType = DiskType.TypeOfDisk.driveType1;
                domainUpDownDiskType.SelectedIndex = (int)DiskType.TypeOfDisk.driveType1;
                domainUpDownDiskType.SelectedItem = "Загрузочный";
            }
        }

        private string GetCkoNumAndVer()
        {
            DirectoryInfo di = new DirectoryInfo(param.DiskDrive);
            List<DirectoryInfo> directories = di.EnumerateDirectories("*_*.*", SearchOption.TopDirectoryOnly).ToList();
            if (directories.Count == 0)
                return "";
            string[] parcedStrings = directories.First().Name.Split('_');
            if (parcedStrings.Length < 2)
                return "";
            return parcedStrings[1];
        }
        private void InitializeCkoNumAndVer()
        {
            param.CkoNumAndVer = GetCkoNumAndVer();
            textBoxCkoNumAndVer.Text = param.CkoNumAndVer;
        }

        private void InitializeProgressBar(ref ProgressBar pgAll, int max)
        {
            pgAll.Minimum = 0;
            pgAll.Value = 0;
            pgAll.Step = 1;
            pgAll.Maximum = max;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeCkoType();
            InitializeDiskType();
            InitializeCkoNumAndVer();
            InitializeProgressBar(ref progressBarAll, 0);
            InitializeProgressBar(ref progressBarCurrentFile, 0);
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //BackgroundWorkerCopy_DoWork(sender, null);
            backgroundWorkerCopy.RunWorkerAsync();
        }
        private void SavePostInfoToFile(Post post, string path, string driveName)
        {
            string result = string.Format("Диск {0} получен с письмом № {1} от {2} (вх. № {3} от {4})\n", driveName, post.Number, post.DateStamp.ToString("dd.MM.yyyy"), post.InputNumber, post.InputDateStamp.ToString("dd.MM.yyyy"));
            File.AppendAllText(Path.Combine(path, "post.txt"), result);
        }

        private void DomainUpDownTypeCko_SelectedItemChanged(object sender, EventArgs e)
        {
            param.PoType = (PoType.TypeOfCko)domainUpDownTypeCko.SelectedIndex;
        }

        private void DomainUpDownDiskType_SelectedItemChanged(object sender, EventArgs e)
        {
            param.DiskType = (DiskType.TypeOfDisk)domainUpDownDiskType.SelectedIndex;
        }

        private void TextBoxCkoNumAndVer_TextChanged(object sender, EventArgs e)
        {
            param.CkoNumAndVer = textBoxCkoNumAndVer.Text;
        }



        private void BackgroundWorkerCopy_DoWork(object sender, DoWorkEventArgs e)
        {
            fileOperations = new FileOperations(param, ref backgroundWorkerCopy);
            fileOperations.CopyDisk();
            string ckoName = Path.Combine(param.DestDir, string.Format("@{0}_{1}", PoType.ToString(param.PoType), param.CkoNumAndVer));
            string resultDestPath = string.Format("{0} ({1}, {2})", DiskType.ToString(param.DiskType), param.DiskName, param.DiskNameBy);
            resultDestPath = Path.Combine(ckoName, resultDestPath);
            ISOFile.WrtieDirAndFilesToISO(resultDestPath, Path.ChangeExtension(resultDestPath, ".iso"), ref backgroundWorkerCopy);

            SavePostInfoToFile(param.Post, ckoName, resultDestPath);
        }

        private void BackgroundWorkerCopy_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MyProgressBarObject currProgressBarPosition = (MyProgressBarObject)e.UserState;
            progressBarAll.Maximum = (int)currProgressBarPosition.MaxAllFiles;
            progressBarCurrentFile.Maximum = (int)currProgressBarPosition.MaxCurrFile;
            progressBarAll.Value = (int)currProgressBarPosition.CurrPositionOfAllFiles;
            progressBarCurrentFile.Value = (int)currProgressBarPosition.CurrPositionInFile;
            label5.Text = currProgressBarPosition.Message + currProgressBarPosition.FileName;
            progressBarAll.Update();
            progressBarCurrentFile.Update();
            label5.Update();
        }

        private void BackgroundWorkerCopy_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(this, e.Error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }
    }
}
