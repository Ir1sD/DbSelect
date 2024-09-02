using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Models
{
	/// <summary>
	/// Значения для нужных в дальейшем полей при автозаполнении
	/// </summary>
	public class ViewColumnData
	{
		/// <summary>
		/// Индетификатор поля
		/// </summary>
		public int Id {  get; private set; }

		/// <summary>
		/// Список значений поля
		/// </summary>
		public List<string> Value { get; set; }

		public ViewColumnData(int columnId)
		{
			Id = columnId;
			Value = new List<string>();
		}
	}
}
