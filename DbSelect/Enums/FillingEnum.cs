using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Enums
{
    /// <summary>
    /// Тип автозаполнения
    /// </summary>
    public enum FillingEnum
    {
        Null = 1,
        IntConst = 2,
        IntRand = 3,
        IntConstId = 4,
        IntRandId = 5,
        StringConst = 6,
        StringFile = 7,
        FloatConst = 8,
        FloatRand = 9,
        DataConst = 10,
        DataRand = 11,
        ImgConst = 12,
        ImgFile = 13
        
    }
}
