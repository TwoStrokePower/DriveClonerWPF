using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CopyerCKO.src.Types;

namespace CopyerCKO.src
{
    public class Params
    {
        private string diskDrive;
        private string destDir;

        private string diskName;
        private string diskNameBy;

        private PoType.TypeOfCko poType;
        private DiskType.TypeOfDisk diskType;
        private string ckoNumAndVer = "";

        private Post post = new Post();

        public string DiskDrive { get => diskDrive; set => diskDrive = value; }
        public string DestDir { get => destDir; set => destDir = value; }
        public string DiskName { get => diskName; set => diskName = value; }
        public string DiskNameBy { get => diskNameBy; set => diskNameBy = value; }
        public PoType.TypeOfCko PoType { get => poType; set => poType = value; }
        public DiskType.TypeOfDisk DiskType { get => diskType; set => diskType = value; }
        public string CkoNumAndVer { get => ckoNumAndVer; set => ckoNumAndVer = value; }
        public Post Post { get => post; set => post = value; }

        public void InitializeParams()
        {
            diskDrive = Properties.Settings.Default.DiskDrive;
            destDir = Properties.Settings.Default.DestDir;
        }
    }
}
