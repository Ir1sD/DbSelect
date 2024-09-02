using DbSelect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Models.SystemModels
{

    /// <summary>
    /// Тип поля
    /// </summary>
    public class TypeOf
    {
        /// <summary>
        /// Индетификатор типа поля
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Названия типа поля
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// К какому типу применяется тип поля
        /// </summary>
        public TypeEnum Type { get; set; }

        /// <summary>
        /// Максимальная длинна строки
        /// </summary>
        public int MaxSizeString { get; set; }
    }
}
