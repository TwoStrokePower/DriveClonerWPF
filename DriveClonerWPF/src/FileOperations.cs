using CopyerCKO.src.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CopyerCKO.src
{
    public class FileOperations
    {
        private Params _params;
        private string resultDestPath;
        private BackgroundWorker backgroundWorker;

        public Params Params { get => _params; set => _params = value; }
        public FileOperations(Params _params, ref BackgroundWorker backgroundWorker)
        {
            this._params = _params;
            this.backgroundWorker = backgroundWorker;
        }

        private void CreateDestDirectory()
        {
            string ckoName = Path.Combine(_params.DestDir, string.Format("@{0}_{1}", PoType.ToString(_params.PoType), _params.CkoNumAndVer));
            Directory.CreateDirectory(ckoName);
            resultDestPath = string.Format("{0} ({1}, {2})", DiskType.ToString(_params.DiskType),  _params.DiskName, _params.DiskNameBy);
            resultDestPath = Path.Combine(ckoName, resultDestPath);
            Directory.CreateDirectory(resultDestPath);
        }
        

        private string FullPathWithoutFileName(string fileName)
        {
            int posLastSlash = fileName.LastIndexOf(Path.DirectorySeparatorChar);

            return fileName.Substring(0, posLastSlash);
        }

        private void SetEndFileAtribute(string sourceFile, string destFile)
        {
            FileInfo source = new FileInfo(sourceFile);
            FileInfo dest = new FileInfo(destFile);

            try
            {
                File.SetCreationTime(dest.FullName, source.CreationTime);
                File.SetLastAccessTime(dest.FullName, source.LastAccessTime);
                File.SetLastWriteTime(dest.FullName, source.LastWriteTime);
            }
            catch (Exception ex)
            {

            }

            dest.Attributes = source.Attributes;
        }

        private void CopyFile(ref MyProgressBarObject pgFile, FileInfo fi)
        {
            const int buffSize = 8196;

            string fullNameWithoutRoot = fi.FullName.Replace(_params.DiskDrive, "");
            FileStream fs = File.OpenRead(fi.FullName);

            string destFilePath = Path.Combine(resultDestPath, fullNameWithoutRoot);
            
            //Directory.CreateDirectory(FullPathWithoutFileName(destFilePath));
            FileStream fWs = File.Create(destFilePath);
            
            pgFile.MaxCurrFile = (uint)(fi.Length / buffSize) + 1;

            long readedBytes = 0;

            int crc16SourceFile = 0;
            int buffCounter = 0;
            while (readedBytes < fi.Length)
            {
                byte[] buff = new byte[buffSize];
                int readedBytes2 = fs.Read(buff, 0, buffSize);
                readedBytes += readedBytes2;
                crc16SourceFile = CRCAll.CRC.CRC16(buff, crc16SourceFile, readedBytes2);
                fWs.Write(buff, 0, readedBytes2);
                buffCounter++;
                pgFile.CurrPositionInFile = (uint)buffCounter;
                this.backgroundWorker.ReportProgress(0, pgFile);
            }
            fs.Close();
            fWs.Close();
            SetEndFileAtribute(fi.FullName, destFilePath);
            if (crc16SourceFile != CRCAll.CRC.CalcCRC16(destFilePath))
                throw new InvalidOperationException(string.Format("Не верно скопирован файл {0}", fi.FullName));
        }

        private string GetDirectoryPathWithoutRoot(string dirName)
        {
            return dirName.Substring(3);
        }

        private void CreateAllDirectories()
        {
            DirectoryInfo di = new DirectoryInfo(_params.DiskDrive);
            foreach (DirectoryInfo di2 in di.EnumerateDirectories("*.*", SearchOption.AllDirectories))
            {
                string result = Path.Combine(resultDestPath, GetDirectoryPathWithoutRoot(di2.FullName));
                Directory.CreateDirectory(result);
                Directory.SetCreationTime(result, di2.CreationTime);
                Directory.SetLastAccessTime(result, di2.LastAccessTime);
                Directory.SetLastWriteTime(result, di2.LastWriteTime);
            }
        }

        public void CopyDisk()
        {
            CreateDestDirectory();
            CreateAllDirectories();
            DirectoryInfo di = new DirectoryInfo(_params.DiskDrive);
            List<FileInfo> fi = di.EnumerateFiles("*.*", SearchOption.AllDirectories).ToList();
            MyProgressBarObject progressBarObject = new MyProgressBarObject();
            progressBarObject.MaxAllFiles = (uint)fi.Count;
            for (int i = 0; i < fi.Count; i++)
            {
                progressBarObject.Message = "Имя обрабатываемого файла: ";
                progressBarObject.FileName = fi[i].FullName;
                
                CopyFile(ref progressBarObject, fi[i]);
                progressBarObject.CurrPositionOfAllFiles = (uint)(i + 1);
                
                //pgAll.PerformStep();
            }
        }
    }
}
