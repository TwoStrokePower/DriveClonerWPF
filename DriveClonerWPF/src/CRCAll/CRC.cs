using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CopyerCKO.src.CRCAll
{
    public static class CRC
    {
        /// <summary>
        /// Таблица для посчета значений CRC16
        /// </summary>
        private static readonly ushort[] CRC16TAB = new ushort[]
       {
     0x00000, 0x0C0C1, 0x0C181, 0x00140, 0x0C301, 0x003C0, 0x00280, 0x0C241,
     0x0C601, 0x006C0, 0x00780, 0x0C741, 0x00500, 0x0C5C1, 0x0C481, 0x00440,
     0x0CC01, 0x00CC0, 0x00D80, 0x0CD41, 0x00F00, 0x0CFC1, 0x0CE81, 0x00E40,
     0x00A00, 0x0CAC1, 0x0CB81, 0x00B40, 0x0C901, 0x009C0, 0x00880, 0x0C841,
     0x0D801, 0x018C0, 0x01980, 0x0D941, 0x01B00, 0x0DBC1, 0x0DA81, 0x01A40,
     0x01E00, 0x0DEC1, 0x0DF81, 0x01F40, 0x0DD01, 0x01DC0, 0x01C80, 0x0DC41,
     0x01400, 0x0D4C1, 0x0D581, 0x01540, 0x0D701, 0x017C0, 0x01680, 0x0D641,
     0x0D201, 0x012C0, 0x01380, 0x0D341, 0x01100, 0x0D1C1, 0x0D081, 0x01040,
     0x0F001, 0x030C0, 0x03180, 0x0F141, 0x03300, 0x0F3C1, 0x0F281, 0x03240,
     0x03600, 0x0F6C1, 0x0F781, 0x03740, 0x0F501, 0x035C0, 0x03480, 0x0F441,
     0x03C00, 0x0FCC1, 0x0FD81, 0x03D40, 0x0FF01, 0x03FC0, 0x03E80, 0x0FE41,
     0x0FA01, 0x03AC0, 0x03B80, 0x0FB41, 0x03900, 0x0F9C1, 0x0F881, 0x03840,
     0x02800, 0x0E8C1, 0x0E981, 0x02940, 0x0EB01, 0x02BC0, 0x02A80, 0x0EA41,
     0x0EE01, 0x02EC0, 0x02F80, 0x0EF41, 0x02D00, 0x0EDC1, 0x0EC81, 0x02C40,
     0x0E401, 0x024C0, 0x02580, 0x0E541, 0x02700, 0x0E7C1, 0x0E681, 0x02640,
     0x02200, 0x0E2C1, 0x0E381, 0x02340, 0x0E101, 0x021C0, 0x02080, 0x0E041,
     0x0A001, 0x060C0, 0x06180, 0x0A141, 0x06300, 0x0A3C1, 0x0A281, 0x06240,
     0x06600, 0x0A6C1, 0x0A781, 0x06740, 0x0A501, 0x065C0, 0x06480, 0x0A441,
     0x06C00, 0x0ACC1, 0x0AD81, 0x06D40, 0x0AF01, 0x06FC0, 0x06E80, 0x0AE41,
     0x0AA01, 0x06AC0, 0x06B80, 0x0AB41, 0x06900, 0x0A9C1, 0x0A881, 0x06840,
     0x07800, 0x0B8C1, 0x0B981, 0x07940, 0x0BB01, 0x07BC0, 0x07A80, 0x0BA41,
     0x0BE01, 0x07EC0, 0x07F80, 0x0BF41, 0x07D00, 0x0BDC1, 0x0BC81, 0x07C40,
     0x0B401, 0x074C0, 0x07580, 0x0B541, 0x07700, 0x0B7C1, 0x0B681, 0x07640,
     0x07200, 0x0B2C1, 0x0B381, 0x07340, 0x0B101, 0x071C0, 0x07080, 0x0B041,
     0x05000, 0x090C1, 0x09181, 0x05140, 0x09301, 0x053C0, 0x05280, 0x09241,
     0x09601, 0x056C0, 0x05780, 0x09741, 0x05500, 0x095C1, 0x09481, 0x05440,
     0x09C01, 0x05CC0, 0x05D80, 0x09D41, 0x05F00, 0x09FC1, 0x09E81, 0x05E40,
     0x05A00, 0x09AC1, 0x09B81, 0x05B40, 0x09901, 0x059C0, 0x05880, 0x09841,
     0x08801, 0x048C0, 0x04980, 0x08941, 0x04B00, 0x08BC1, 0x08A81, 0x04A40,
     0x04E00, 0x08EC1, 0x08F81, 0x04F40, 0x08D01, 0x04DC0, 0x04C80, 0x08C41,
     0x04400, 0x084C1, 0x08581, 0x04540, 0x08701, 0x047C0, 0x04680, 0x08641,
     0x08201, 0x042C0, 0x04380, 0x08341, 0x04100, 0x081C1, 0x08081, 0x04040
       };

        private static readonly uint[] CRC32TAB = new uint[]
        {
    0x00000000, 0x77073096, 0xee0e612c, 0x990951ba, 0x076dc419, 0x706af48f, 0xe963a535, 0x9e6495a3,
    0x0edb8832, 0x79dcb8a4, 0xe0d5e91e, 0x97d2d988, 0x09b64c2b, 0x7eb17cbd, 0xe7b82d07, 0x90bf1d91,
    0x1db71064, 0x6ab020f2, 0xf3b97148, 0x84be41de, 0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7,
    0x136c9856, 0x646ba8c0, 0xfd62f97a, 0x8a65c9ec, 0x14015c4f, 0x63066cd9, 0xfa0f3d63, 0x8d080df5,
    0x3b6e20c8, 0x4c69105e, 0xd56041e4, 0xa2677172, 0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b,
    0x35b5a8fa, 0x42b2986c, 0xdbbbc9d6, 0xacbcf940, 0x32d86ce3, 0x45df5c75, 0xdcd60dcf, 0xabd13d59,
    0x26d930ac, 0x51de003a, 0xc8d75180, 0xbfd06116, 0x21b4f4b5, 0x56b3c423, 0xcfba9599, 0xb8bda50f,
    0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924, 0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d,
    0x76dc4190, 0x01db7106, 0x98d220bc, 0xefd5102a, 0x71b18589, 0x06b6b51f, 0x9fbfe4a5, 0xe8b8d433,
    0x7807c9a2, 0x0f00f934, 0x9609a88e, 0xe10e9818, 0x7f6a0dbb, 0x086d3d2d, 0x91646c97, 0xe6635c01,
    0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e, 0x6c0695ed, 0x1b01a57b, 0x8208f4c1, 0xf50fc457,
    0x65b0d9c6, 0x12b7e950, 0x8bbeb8ea, 0xfcb9887c, 0x62dd1ddf, 0x15da2d49, 0x8cd37cf3, 0xfbd44c65,
    0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2, 0x4adfa541, 0x3dd895d7, 0xa4d1c46d, 0xd3d6f4fb,
    0x4369e96a, 0x346ed9fc, 0xad678846, 0xda60b8d0, 0x44042d73, 0x33031de5, 0xaa0a4c5f, 0xdd0d7cc9,
    0x5005713c, 0x270241aa, 0xbe0b1010, 0xc90c2086, 0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f,
    0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4, 0x59b33d17, 0x2eb40d81, 0xb7bd5c3b, 0xc0ba6cad,
    0xedb88320, 0x9abfb3b6, 0x03b6e20c, 0x74b1d29a, 0xead54739, 0x9dd277af, 0x04db2615, 0x73dc1683,
    0xe3630b12, 0x94643b84, 0x0d6d6a3e, 0x7a6a5aa8, 0xe40ecf0b, 0x9309ff9d, 0x0a00ae27, 0x7d079eb1,
    0xf00f9344, 0x8708a3d2, 0x1e01f268, 0x6906c2fe, 0xf762575d, 0x806567cb, 0x196c3671, 0x6e6b06e7,
    0xfed41b76, 0x89d32be0, 0x10da7a5a, 0x67dd4acc, 0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5,
    0xd6d6a3e8, 0xa1d1937e, 0x38d8c2c4, 0x4fdff252, 0xd1bb67f1, 0xa6bc5767, 0x3fb506dd, 0x48b2364b,
    0xd80d2bda, 0xaf0a1b4c, 0x36034af6, 0x41047a60, 0xdf60efc3, 0xa867df55, 0x316e8eef, 0x4669be79,
    0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236, 0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f,
    0xc5ba3bbe, 0xb2bd0b28, 0x2bb45a92, 0x5cb36a04, 0xc2d7ffa7, 0xb5d0cf31, 0x2cd99e8b, 0x5bdeae1d,
    0x9b64c2b0, 0xec63f226, 0x756aa39c, 0x026d930a, 0x9c0906a9, 0xeb0e363f, 0x72076785, 0x05005713,
    0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0x0cb61b38, 0x92d28e9b, 0xe5d5be0d, 0x7cdcefb7, 0x0bdbdf21,
    0x86d3d2d4, 0xf1d4e242, 0x68ddb3f8, 0x1fda836e, 0x81be16cd, 0xf6b9265b, 0x6fb077e1, 0x18b74777,
    0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c, 0x8f659eff, 0xf862ae69, 0x616bffd3, 0x166ccf45,
    0xa00ae278, 0xd70dd2ee, 0x4e048354, 0x3903b3c2, 0xa7672661, 0xd06016f7, 0x4969474d, 0x3e6e77db,
    0xaed16a4a, 0xd9d65adc, 0x40df0b66, 0x37d83bf0, 0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9,
    0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6, 0xbad03605, 0xcdd70693, 0x54de5729, 0x23d967bf,
    0xb3667a2e, 0xc4614ab8, 0x5d681b02, 0x2a6f2b94, 0xb40bbe37, 0xc30c8ea1, 0x5a05df1b, 0x2d02ef8d
        };



        /// <summary>
        /// Выполняет рассчет контрольной суммы CRC16
        /// </summary>
        /// <param name="bytes">Массив байт для подсчета</param>
        /// <param name="InitCRC">Начальное значение CRC</param>
        /// <returns></returns>
        public static int CRC16(byte[] bytes, int InitCRC, int sizeOfElem = 0)
        {
            if (sizeOfElem != 0)
            {
                for (int j = 0; j < sizeOfElem; j++)
                {
                    InitCRC = (InitCRC >> 8) ^ CRC16TAB[(InitCRC ^ bytes[j]) & 0xFF];
                }
            }
            else
            {
                for (int j = 0; j < bytes.Length; j++)
                {
                    InitCRC = (InitCRC >> 8) ^ CRC16TAB[(InitCRC ^ bytes[j]) & 0xFF];
                }
            }
            return InitCRC;
        }

        public static uint CRC32(byte[] bytes, uint InitCRC)
        {
            for (int j = 0; j < bytes.Length; j++)
            {
                InitCRC = (InitCRC >> 8) ^ CRC32TAB[(InitCRC ^ bytes[j]) & 0xFF];
            }
            return InitCRC;
        }

        /// <summary>
        /// Выполняет рассчет контрольной суммы файла CRC16
        /// </summary>
        /// <param name="source">Путь к файлу</param>
        /// <param name="WithoutEndBytes">Признак, что рассчет CRC необходимо выполнять без последних байт в файле, в которых записано его crc</param>
        /// <returns></returns>
        public static int CalcCRC16(string source, ref System.Windows.Forms.ProgressBar progressBar, bool WithoutEndBytes = false)
        {
            try
            {
                bool isError = false;
                if (!File.Exists(source))
                    throw new ArgumentException("Не удается найти файл");
                int result = 0x0000;
                using (FileStream FS = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.SequentialScan))
                using (BinaryReader BR = new BinaryReader(FS))
                {
                    long calcLegngth = FS.Length;

                    progressBar.Minimum = 0;
                    progressBar.Value = 0;
                    progressBar.Step = 1;

                    //отнимаем два байта, если в конце файла записано его CRC
                    if (WithoutEndBytes)
                        calcLegngth -= 2;
                    UInt16 bufsize = 8192;
                    if (calcLegngth <= bufsize)
                    {
                        progressBar.Maximum = 1;
                        progressBar.PerformStep();
                        result = CRC.CRC16(BR.ReadBytes((int)calcLegngth), 0);
                    }
                    else
                    {
                        long readcount = calcLegngth / bufsize;
                        try
                        {
                            progressBar.Maximum = (int)readcount;
                        }
                        catch
                        {
                            progressBar.Maximum = 1;
                            isError = true;
                        }
                        uint tail = (uint)calcLegngth % bufsize;
                        for (long l = 1; l <= readcount; l++)
                        {
                            if (!isError)
                                progressBar.PerformStep();
                            result = CRC.CRC16(BR.ReadBytes(bufsize), result);
                        }
                        //посчитаем остаток
                        result = CRC.CRC16(BR.ReadBytes((int)tail), result);
                    }
                    BR.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Ошибка при подсчете CRC16", ex);
            }
        }

        public static uint CalcCRC32(string source, ref System.Windows.Forms.ProgressBar progressBar, bool WithoutEndBytes = false)
        {
            try
            {
                bool isError = false;
                if (!File.Exists(source))
                    throw new ArgumentException("Не удается найти файл");
                uint result = 0x0000;
                using (FileStream FS = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.SequentialScan))
                using (BinaryReader BR = new BinaryReader(FS))
                {
                    long calcLegngth = FS.Length;

                    progressBar.Minimum = 0;
                    progressBar.Value = 0;
                    progressBar.Step = 1;

                    //отнимаем два байта, если в конце файла записано его CRC
                    if (WithoutEndBytes)
                        calcLegngth -= 4;
                    UInt16 bufsize = 8192;
                    if (calcLegngth <= bufsize)
                    {
                        progressBar.Maximum = 1;
                        progressBar.PerformStep();
                        result = CRC.CRC32(BR.ReadBytes((int)calcLegngth), 0);
                    }
                    else
                    {
                        long readcount = calcLegngth / bufsize;
                        try
                        {
                            progressBar.Maximum = (int)readcount;
                        }
                        catch
                        {
                            progressBar.Maximum = 1;
                            isError = true;
                        }
                        uint tail = (uint)calcLegngth % bufsize;
                        for (long l = 1; l <= readcount; l++)
                        {
                            if (!isError)
                                progressBar.PerformStep();
                            result = CRC.CRC32(BR.ReadBytes(bufsize), result);
                        }
                        //посчитаем остаток
                        result = CRC.CRC32(BR.ReadBytes((int)tail), result);
                    }
                    BR.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(source))
                    throw new ArgumentException(String.Format("Ошибка при подсчете контрольной суммы файла {0}", source), ex);
                else
                    throw new ArgumentException("Ошибка при подсчете контрольной суммы файла", ex);
            }
        }

        public static uint CalcCRC32(string source, bool WithoutEndBytes = false)
        {
            try
            {
                if (!File.Exists(source))
                    throw new ArgumentException("Не удается найти файл");
                uint result = 0x0000;
                using (FileStream FS = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.SequentialScan))
                using (BinaryReader BR = new BinaryReader(FS))
                {
                    long calcLegngth = FS.Length;

                    //отнимаем два байта, если в конце файла записано его CRC
                    if (WithoutEndBytes)
                        calcLegngth -= 4;
                    UInt16 bufsize = 8192;
                    if (calcLegngth <= bufsize)
                    {
                        result = CRC.CRC32(BR.ReadBytes((int)calcLegngth), 0);
                    }
                    else
                    {
                        long readcount = calcLegngth / bufsize;
                        uint tail = (uint)calcLegngth % bufsize;
                        for (long l = 1; l <= readcount; l++)
                        {
                            result = CRC.CRC32(BR.ReadBytes(bufsize), result);
                        }
                        //посчитаем остаток
                        result = CRC.CRC32(BR.ReadBytes((int)tail), result);
                    }
                    BR.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(source))
                    throw new ArgumentException(String.Format("Ошибка при подсчете контрольной суммы файла {0}", source), ex);
                else
                    throw new ArgumentException("Ошибка при подсчете контрольной суммы файла", ex);
            }
        }

        public static int CalcCRC16(string source, bool WithoutEndBytes = false)
        {
            try
            {
                bool isError = false;
                if (!File.Exists(source))
                    throw new ArgumentException("Не удается найти файл");
                int result = 0x0000;
                using (FileStream FS = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.SequentialScan))
                using (BinaryReader BR = new BinaryReader(FS))
                {
                    long calcLegngth = FS.Length;
       
                    //отнимаем два байта, если в конце файла записано его CRC
                    if (WithoutEndBytes)
                        calcLegngth -= 2;
                    UInt16 bufsize = 8192;
                    if (calcLegngth <= bufsize)
                    {
                        result = CRC.CRC16(BR.ReadBytes((int)calcLegngth), 0);
                    }
                    else
                    {
                        long readcount = calcLegngth / bufsize;
                        uint tail = (uint)calcLegngth % bufsize;
                        for (long l = 1; l <= readcount; l++)
                        {
                            result = CRC.CRC16(BR.ReadBytes(bufsize), result);
                        }
                        //посчитаем остаток
                        result = CRC.CRC16(BR.ReadBytes((int)tail), result);
                    }
                    BR.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Ошибка при подсчете CRC16", ex);
            }
        }
    }
}
