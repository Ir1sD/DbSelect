using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbSelect.Enums;

namespace DbSelect.Models.SystemModels
{

	/// <summary>
	/// Ошибка при проверке таблиц перед сохранением/загрузкой в БД
	/// </summary>
	public class CreateTableError
    {
		/// <summary>
		/// Идентификатор ошибки
		/// </summary>
		public int Id { get; private set; }

		/// <summary>
		/// Текст ошибки
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Таблица ошибки
		/// </summary>
		public int TableId { get; set; }

		/// <summary>
		/// Колонка ошибки
		/// </summary>
		public int ColumnId { get; set; }

		/// <summary>
		/// Файл ошибки
		/// </summary>
		public int FileId { get; set; }

		/// <summary>
		/// Тип ошибки
		/// </summary>
		public CreateTableErrorTypeEnum Type {  get; set; }

		/// <summary>
		/// Ошибка - true, Предупреждение - false
		/// </summary>
		public bool IsError { get; set; }

		/// <summary>
		/// Максимальный Идентификатор ошибки
		/// </summary>
		private static int MaxId = 1;

        public CreateTableError() 
        { 
            Id = MaxId++;
        }
    }
}
