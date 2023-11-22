using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CopyerCKO.src
{
    public class MyProgressBarObject
    {
        uint currPositionOfAllFiles = 0;
        uint currPositionInFile = 0;
        uint maxAllFiles = 0;
        uint maxCurrFile = 0;
        string fileName = "";
        string message = "";

        public uint CurrPositionOfAllFiles { get => currPositionOfAllFiles; set => currPositionOfAllFiles = value; }
        public uint CurrPositionInFile { get => currPositionInFile; set => currPositionInFile = value; }
        public uint MaxAllFiles { get => maxAllFiles; set => maxAllFiles = value; }
        public uint MaxCurrFile { get => maxCurrFile; set => maxCurrFile = value; }
        public string FileName { get => fileName; set => fileName = value; }
        public string Message { get => message; set => message = value; }
    }
}
