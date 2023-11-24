namespace DriveClonerWPF.src.Types
{
    public class DiskType
    {
        public enum TypeOfDisk
        {
            driveType1,
            driveType2,
            driveType3,
            driveType4,
            driveType5
        }

        public static string ToString(TypeOfDisk diskType)
        {
            if (diskType == TypeOfDisk.driveType1)
                return "LOAD";
            else if (diskType == TypeOfDisk.driveType2)
                return "DIST";
            else if (diskType == TypeOfDisk.driveType3)
                return "SHLUZ";
            else if (diskType == TypeOfDisk.driveType4)
                return "SHB";
            else if (diskType == TypeOfDisk.driveType5)
                return "DEV";
            else
                return "";
        }
    }
}
