using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Models
{
	/// <summary>
	/// Условие при автозаполнении
	/// </summary>
	public class Condition
	{
		/// <summary>
		/// Идентификатор Условия
		/// </summary>
		public int Id {  get; private set; }

		/// <summary>
		/// Текст условия
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Колонка, от которой зависит результат в условии
		/// </summary>
		public int ColumnIdIn { get; set; }

		/// <summary>
		/// Колонка с которой сравниваем в условии
		/// </summary>
		public int ColumnIdOut { get; set; }

		/// <summary>
		/// Колонка откуда берем значение в условии
		/// </summary>
		public int ColumnIdValue { get; set; }

		/// <summary>
		/// Значение с которым сравниваем в условии
		/// </summary>
		public string ValueOut { get; set; }

		/// <summary>
		/// Знак в условии
		/// </summary>
		public string Sign { get; set; }

		/// <summary>
		/// Итоговое значение в условии
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Идентификатор следующего словия при else в условии
		/// </summary>
		public int IdConditionNext {  get; set; }

		/// <summary>
		/// Идентификатор файла итогового значения в условии
		/// </summary>
		public int IdFile { get; set; }

		/// <summary>
		/// Идентификатор таблицы, в которой находится условие
		/// </summary>
		public int TableId { get; set; }


		/// <summary>
		/// Максимальный Идентификатор условия
		/// </summary>
		private static int MaxId = 1;

		public Condition()
		{
			Id = MaxId++;
		}

	}

	public class ConditionViewModel
	{
		public int TableId { get; set; }
		public int ColumnId { get; set; }
		public string Text { get; set; }
	}
}
