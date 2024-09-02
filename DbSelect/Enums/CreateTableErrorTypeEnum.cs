using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Enums
{
    /// <summary>
    /// Тип ошибки при проверке таблиц перед сохранением конфигурации или загрузки в БД
    /// </summary>
    public enum CreateTableErrorTypeEnum
    {
        None = 0,
        TableName = 1,
        TableCount = 2,
        TableKey = 3,
        ColName = 4,
        ColFilling = 5,
        ColIsNull = 6,
        FileName = 7,
        FileNull = 8

    }
}
