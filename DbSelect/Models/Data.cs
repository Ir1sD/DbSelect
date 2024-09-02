using DbSelect.Helpers;
using DbSelect.Models.StaticModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Models
{
	/// <summary>
	/// Объект подключения к бд
	/// </summary>
	public class Data
	{
		/// <summary>
		/// Объект подключения
		/// </summary>
		private SqlConnection Connection;

		/// <summary>
		/// Открыть подключение
		/// </summary>
		public void Open()
		{
			Connection.Open();
		}

		/// <summary>
		/// Закрыть подключение
		/// </summary>
		public void Close()
		{
			Connection.Close();
		}

		/// <summary>
		/// Получить подключение
		/// </summary>
		/// <returns>Объект подключения</returns>
		public SqlConnection Get()
		{
			return Connection;
		}

		/// <summary>
		/// Устоновить подключение
		/// </summary>
		public void Set()
		{
			Connection = new SqlConnection(User.StringConnection);
		}
		
		/// <summary>
		/// Проверка строки подключения
		/// </summary>
		/// <param name="con">Строка подключения</param>
		/// <returns>true - строка подключения верна</returns>
		public static bool Check(string con)
		{
			try
			{
				var cons = new SqlConnection(con);
				cons.Open();

				cons.Close();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
