using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DriveClonerWPF.src;
using DiscUtils;

namespace SecuredStorage.ISO
{
    class ISOFile
    {
        public static void WrtieDirAndFilesToISO(string fromPath, string isoFile, ref System.ComponentModel.BackgroundWorker backgroundWorkerCopy)
        {
            DirectoryInfo di = new DirectoryInfo(fromPath);
            string fromPathP = Path.GetFullPath(fromPath) + Path.DirectorySeparatorChar;
            MyProgressBarObject progressBarObject = new MyProgressBarObject();

            progressBarObject.Message = "Создание образа:";
            progressBarObject.MaxAllFiles = (uint) di.EnumerateFiles("*.*", SearchOption.AllDirectories).Count();
            progressBarObject.MaxCurrFile = 0;

            uint fileCounter = 0;

            DiscUtils.Iso9660.CDBuilder cdBuilder = new DiscUtils.Iso9660.CDBuilder();
            foreach (FileInfo fi in di.EnumerateFiles("*.*", SearchOption.AllDirectories))
            {
                fileCounter++;
                progressBarObject.FileName = fi.Name;
                progressBarObject.CurrPositionOfAllFiles = fileCounter;
                backgroundWorkerCopy.ReportProgress(0, progressBarObject);

                if (fi.DirectoryName != fromPath)
                {
                    string path = fi.DirectoryName.Replace(fromPathP, "");
                    cdBuilder.AddDirectory(path);
                }

                cdBuilder.AddFile(fi.FullName.Replace(fromPathP, ""), fi.FullName);
            }

            cdBuilder.Build(isoFile);
            
        }
        public static void ReadIsoFilesToDrive(string isoFileName, string sDestinationRootPath)
        {
            Stream streamIsoFile = null;
            try
            {
                streamIsoFile = new FileStream(isoFileName, FileMode.Open);
                DiscUtils.FileSystemInfo[] fsia = FileSystemManager.DetectDefaultFileSystems(streamIsoFile);
                if (fsia.Length < 1)
                {
                    MessageBox.Show("No valid disc file system detected.");
                }
                else
                {
                    DiscFileSystem dfs = fsia[0].Open(streamIsoFile);
                    ReadIsoFolder(dfs, @"", sDestinationRootPath);
                    return;
                }
            }
            finally
            {
                if (streamIsoFile != null)
                {
                    streamIsoFile.Close();
                }
            }
        }

        private static void ReadIsoFolder(DiscFileSystem cdReader, string sIsoPath, string sDestinationRootPath)
        {
            try
            {
                string[] saFiles = cdReader.GetFiles(sIsoPath);
                foreach (string sFile in saFiles)
                {
                    DiscFileInfo dfiIso = cdReader.GetFileInfo(sFile);
                    string sDestinationPath = Path.Combine(sDestinationRootPath, dfiIso.DirectoryName.Substring(0, dfiIso.DirectoryName.Length - 1));
                    if (!Directory.Exists(sDestinationPath))
                    {
                        Directory.CreateDirectory(sDestinationPath);
                    }
                    string sDestinationFile = Path.Combine(sDestinationPath, dfiIso.Name);
                    SparseStream streamIsoFile = cdReader.OpenFile(sFile, FileMode.Open);
                    FileStream fsDest = new FileStream(sDestinationFile, FileMode.Create);
                    byte[] baData = new byte[0x4000];
                    while (true)
                    {
                        int nReadCount = streamIsoFile.Read(baData, 0, baData.Length);
                        if (nReadCount < 1)
                        {
                            break;
                        }
                        else
                        {
                            fsDest.Write(baData, 0, nReadCount);
                        }
                    }
                    streamIsoFile.Close();
                    fsDest.Close();
                }
                string[] saDirectories = cdReader.GetDirectories(sIsoPath);
                foreach (string sDirectory in saDirectories)
                {
                    ReadIsoFolder(cdReader, sDirectory, sDestinationRootPath);
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        
    }
}
