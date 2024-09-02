using DbSelect.Enums;
using DbSelect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Models.SystemModels
{
    /// <summary>
    /// Тип Автозаполнения
    /// </summary>
    public class TypeFilling
    {
        /// <summary>
        /// Индетификатор автозаполнения
        /// </summary>
        public int Id {  get; set; }

        /// <summary>
        /// Название автозхаполнения
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип поля, к которому применяется Автозаполнение
        /// </summary>
        public TypeEnum Type { get; set; }


    }
}
