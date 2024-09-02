using DbSelect.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Helpers
{
	public static class SqlHelper
	{
		/// <summary>
		/// Выполнение команды - запроса
		/// </summary>
		/// <param name="command">Строка запроса</param>
		/// <param name="data">Объект подключения</param>
		/// <returns></returns>
		public static bool CommandStart(string command , Data data)
		{
			var query = new SqlCommand(command, data.Get());

			try
			{
				query.ExecuteNonQuery();
				return true;
			}
			catch (Exception e)
			{
				ErrorHelper.CreateError("Ошибка при выполнении команды." , $"Команда {command}" , e.Message);
				return false;
			}
		}

		/// <summary>
		/// Создания запроса для автозаполнения таблицы
		/// </summary>
		/// <param name="t">Таблица в объекте ViewTable</param>
		/// <returns>Строка запроса</returns>
		public static string CreateQuery(ViewTable t)
		{

			string query = $"INSERT INTO {t.Table.Name} (";

			foreach (var col in t.Columns.Where(c => !c.Column.IsPrimaryKey))
			{
				query += $"{col.Column.Name}, ";
			}

			query = query.Remove(query.Length - 2, 2);
			query += ") VALUES ( ";

			foreach (var col in t.Columns.Where(c => !c.Column.IsPrimaryKey))
			{
				query += GetQueryColumn(col);
			}

			query = query.Remove(query.Length - 1, 1);
			query += ")";

			return query;
		}

		/// <summary>
		/// Получения фрагмента запроса для автозаполнения поля
		/// </summary>
		/// <param name="col">Поле в объекте ViewColumn</param>
		/// <returns>Фрагмент запроса</returns>
		private static string GetQueryColumn(ViewColumn col)
		{
			
			switch (col.Column.Type.Type)
			{
				case Models.Enums.TypeEnum.Number:
					return $" CAST({Convert.ToInt64(col.Value)} AS {col.Column.Type.Name}),";
				case Models.Enums.TypeEnum.String:
					return $" '{col.Value}',";
				case Models.Enums.TypeEnum.Float:
					return $" CAST({col.Value} AS {col.Column.Type.Name}),";
				case Models.Enums.TypeEnum.Date:
					return $" CAST({$"'{Convert.ToDateTime(col.Value).ToString("yyyy-MM-dd")}T{Convert.ToDateTime(col.Value).ToString("HH:mm:ss")}'"} AS {col.Column.Type.Name}),";
				case Models.Enums.TypeEnum.Bool:
					return $" CAST({Convert.ToBoolean(col.Value)} AS {col.Column.Type.Name}),";
				case Models.Enums.TypeEnum.Image:
					return $" '{col.Value}',";
			}

			return string.Empty;
		}

	}
}
