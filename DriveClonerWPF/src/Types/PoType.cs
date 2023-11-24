using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriveClonerWPF.src.Types
{
    public class PoType
    {
        public enum TypeOfCko
        {
            type1,
            type2,
            type3,
        }

        public static string ToString(TypeOfCko poType)
        {
            if (poType == TypeOfCko.type1)
                return "Тип1";
            else if (poType == TypeOfCko.type2)
                return "Тип2";
            else if (poType == TypeOfCko.type3)
                return "Тип3";
            else
                return "";
        }
    }
}
