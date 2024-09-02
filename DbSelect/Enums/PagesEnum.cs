using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Enums
{
    /// <summary>
    /// Тип выбранной страницы
    /// </summary>
    public enum PagesEnum
    {
        None = 0,
        CreateTable = 1,
        UpdateTable = 2,
        Filling = 3,
        FileString = 4,
        FileImg = 5,
        FileImgList = 6
    }
}
