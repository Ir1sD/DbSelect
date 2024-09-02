using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Models
{
	/// <summary>
	/// Класс таблицы для автозаполнения
	/// </summary>
	public class ViewTable
	{
		/// <summary>
		/// Таблица
		/// </summary>
		public Table Table { get; private set; }

		/// <summary>
		/// Список полей в объектах ViewColumn
		/// </summary>
		public List<ViewColumn> Columns { get; set; }

		/// <summary>
		/// Список нужных данных значений по полям
		/// </summary>
		public List<ViewColumnData> ColumnsData { get; set; }

		public ViewTable(Table t)
		{
			Table = t;
			Columns = new List<ViewColumn>();
			ColumnsData = new List<ViewColumnData>();
		}
	}
}
