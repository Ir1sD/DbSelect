using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Models.SystemModels
{

    /// <summary>
    /// Колонка в таблице
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Идентификатор колонки
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Название колонки
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Название колонки
        /// </summary>
        public TypeOf Type { get; set; }

        /// <summary>
        /// Тип колонки
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Возможность на null колонки
        /// </summary>
        public bool IsPrimaryKey { get; set; }

		/// <summary>
		/// Приориет заполнения колонки
		/// </summary>
		public int Priority { get; set; }

        /// <summary>
        /// Наличие ключа колонки
        /// </summary>
        public TypeFilling Filling { get; set; }

        /// <summary>
        /// Настройки Автозаполнения колонки
        /// </summary>
        public FillingSettings Settings { get; set; }

		/// <summary>
		/// Максимальный Идентификатор поля
		/// </summary>
		private static int MaxId = 1;

        public Column()
        {
            Id = MaxId++;
            Priority = 0;
        }

        public Column(int id)
        {
            Id = id;
            Priority = 0;
            MaxId++;
        }
    }
}
