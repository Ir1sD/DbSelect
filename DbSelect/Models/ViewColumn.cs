using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Models
{
	/// <summary>
	/// Класс поля для автозаполнения
	/// </summary>
	public class ViewColumn
	{
		/// <summary>
		/// Поле
		/// </summary>
		public Column Column {  get; private set; }

		/// <summary>
		/// Список возможных значений (если требуется)
		/// </summary>
		public List<string> Values {  get; set; }

		/// <summary>
		/// Значение поля для автозаполнения
		/// </summary>
		public string Value { get; set; }

		public ViewColumn(Column column) 
		{
			Column = column;
			Value = string.Empty;
			Values = new List<string>();
		}
	}
}
