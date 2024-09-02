using DbSelect.Helpers;
using DbSelect.Models;
using DbSelect.Models.StaticModels;
using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DbSelect.Controllers
{
	public static class SqlController
	{
		private static List<ViewTable> viewTables = new List<ViewTable>();


		/// <summary>
		/// Получение и проверка строки подключения
		/// </summary>
		/// <returns>true - Проверка пройдена</returns>
        public static bool GetConnectionString()
		{
			string text = Cm.StartPage.ConnectionText.Text;

			if(string.IsNullOrEmpty(text))
			{
				User.StringConnection = string.Empty;
				MessageHelper.InfoMessage("Строка подключения сброшена");
				Cm.StartPage.ConnectionText.Enabled = true;
				return false;
			}
			
			if(!Data.Check(text))
			{
				ErrorHelper.CreateError("Ошибка при подключении", $"При подключении по строке подключения {text} произошла ошибка");
                return false;
			}

            Cm.StartPage.ConnectionText.Enabled = false;
            User.StringConnection = text;

			return true;
		}

        /// <summary>
        /// Вывод сообщения в лист на странице запросов
        /// </summary>
        /// <param name="mes">Текст</param>
        public static void Write(string mes)
		{
			SystemHelper.WriteInfoStartPage(mes);
		}

		#region Создание таблиц

		/// <summary>
		/// Старт созданию таблиц
		/// </summary>
		/// <returns>Успешное выполнение - true, при ошибке - false</returns>
		public static bool CreateTables()
		{
			var list = new List<string>();

			Cm.StartPage.List.Items.Clear();
			Cm.StartPage.Text.Text = string.Empty;

			Write("Проверка подключения");

			if (Data.Check(User.StringConnection) || Cm.StartPage.QueryText.Checked)
			{
				Write("Подключение проверено");
			}
			else
			{
				Write("Ошибка при подключении");
				return false;
			}

			Data data = new Data();

			if(!Cm.StartPage.QueryText.Checked)
			{
                data.Set();
                data.Open();
            }

			foreach (var tab in User.Tables)
			{
				string query = CreateTableString(tab);
				Write($"Создание таблицы {tab.Name}");

				if(Cm.StartPage.QueryText.Checked)
				{
					list.Add(query);
				}
				else if (!SqlHelper.CommandStart(query, data))
                {
                    if (!Cm.StartPage.QueryText.Checked)
                        data.Close();
                    return false;
                }
                else
                {
                    Write($"Таблица {tab.Name} создана успешно");
                }
            }

			if(Cm.StartPage.QueryText.Checked)
			{
				Cm.StartPage.Text.Lines = list.ToArray();
			}

			Write("Таблицы созданы");
            if (!Cm.StartPage.QueryText.Checked)
                data.Close();
			return true;
		}

		/// <summary>
		/// Создание запроса sql для создания таблицы
		/// </summary>
		/// <param name="t">Таблица, которую создаем</param>
		/// <returns>Строка запроса</returns>
		private static string CreateTableString(Table t)
		{
			string query = $"CREATE TABLE {t.Name} (";

			foreach (var col in t.Columns)
			{
				query += CreateColumnString(col);
			}

			query = query.Remove(query.Length - 2, 2);

			query += ")";
			return query;
		}

		/// <summary>
		/// Создания фрагмента запроса для создания поля в таблице
		/// </summary>
		/// <param name="col">Поле, которое создаем</param>
		/// <returns>Строка фрагмента запроса для создания поля</returns>
		private static string CreateColumnString(Column col)
		{
			string query = $"{col.Name} {col.Type.Name}";

			if (col.IsPrimaryKey)
			{
				query += " IDENTITY(1,1) PRIMARY KEY";
			}
			else if(!col.IsNullable)
			{
				query += " NOT NULL";
			}

			query += ", ";

			return query;
		}

        #endregion

        #region Автозаполнение

        /// <summary>
        /// Старт автозаполнения таблиц
        /// </summary>
        /// <returns>Успешное выполнение - true, при ошибке - false</returns>
        public static bool FillingTable()
		{
			var list = new List<string>();
			Data data = new Data();

            if (!Cm.StartPage.QueryText.Checked)
			{
                data.Set();
                data.Open();
            }
                
			Write("Старт автозаполнения");

			foreach (var table in User.Tables)
			{
				
				Write($"Таблица {table.Name}.");
				Write($"Поля:");
				var t = ConvertToViewTable(table);

				foreach (var col in t.Columns)
				{
					Write(col.Column.Name);
					PullValuesViewColumn(col, table.Count);
				}

				for (int i = 0; i < table.Count; i++)
				{
					foreach (var col in t.Columns)
					{

						if(col.Column.IsPrimaryKey)
						{
							if(string.IsNullOrEmpty(col.Value))
							{
								col.Value = "1";
							}
							else
							{
								col.Value = (Convert.ToInt64(col.Value) + 1).ToString();
							}
						}
						else
						{
                            SetConditionsValueColumn(col, t);

                            if (string.IsNullOrEmpty(col.Value))
                            {
                                SetValueColumn(col);
                            }

                        }

                        if (TableHelper.CheckKeyColumn(col.Column, table))
                        {
                            SetViewTableDataColumn(t, col);
                        }

                    }

					string query = SqlHelper.CreateQuery(t);

					if(Cm.StartPage.QueryText.Checked)
					{
						list.Add(query);

                        foreach (var col in t.Columns.Where(c => c.Column.IsPrimaryKey == false))
                        {
                            col.Value = string.Empty;
                        }
                    }
					else if (SqlHelper.CommandStart(query, data))
					{
						foreach (var col in t.Columns.Where(c => c.Column.IsPrimaryKey == false))
						{
							col.Value = string.Empty;
						}
					}
					else
					{
                        if (!Cm.StartPage.QueryText.Checked)
                            data.Close();
						return false;
					}
				}

				if(Cm.StartPage.QueryText.Checked)
				{
					Cm.StartPage.Text.Lines = list.ToArray();
				}

				Write($"INSERT INTO {table.Name} ({table.Count})");
				t.Columns.Clear();
				viewTables.Add(t);
			}

			viewTables.Clear();

            if (!Cm.StartPage.QueryText.Checked)
                data.Close();

			Write("Данные в таблицы заполнены");
			MessageHelper.InfoMessage("Данные в таблицы заполнены успешно");
			return true;
		}

		/// <summary>
		/// Конвертация таблицы в ViewTable
		/// </summary>
		/// <param name="table">Таблица для конвертации</param>
		/// <returns>Объект ViewTable таблицы</returns>
		private static ViewTable ConvertToViewTable(Table table)
		{
			var viewTable = new ViewTable(table);

			foreach (var column in table.Columns)
			{
				var viewColumn = new ViewColumn(column);
				viewTable.Columns.Add(viewColumn);
			}

			return viewTable;
		}

		/// <summary>
		/// Заполнение списков возмыжные значений для поля (если необходимо)
		/// </summary>
		/// <param name="col">Поле в объекте ViewColumn</param>
		/// <param name="count">Количество значений</param>
		private static void PullValuesViewColumn (ViewColumn col , int count)
		{
			var listValues = new List<string>();

			// Случайное целое число
			if(col.Column.Filling.Id == 2)
			{
				var listValue = new List<int>();

				for (int i = 0; i < count; i++)
				{
					listValue.Add(ValueHelper.Rand(col.Column.Settings.IntRandFrom.Value, col.Column.Settings.IntRandTo.Value));
				}

				switch (col.Column.Settings.IntRandSort)
				{
					case Enums.SortEnum.Increasing:
						listValue = listValue.OrderBy(c => c).ToList();
						break;
					case Enums.SortEnum.Descending:
						listValue = listValue.OrderByDescending(c => c).ToList();
						break;
					default:
						break;
				}

				listValues = listValue.Select(c => c.ToString()).ToList();
			}
			
			// Ключ - уникальный
			else if(col.Column.Filling.Id == 3)
			{
				var listValue = viewTables
					.First(c => c.Table.Id == col.Column.Settings.IntIdConstTable.Value).ColumnsData
					.First(c => c.Id == col.Column.Settings.IntIdConstColumn.Value)
					.Value;

				var random = new Random();

				listValue = listValue.Distinct().OrderBy(c => random.Next()).ToList();
				listValues.AddRange(listValue);
			}

			// Ключ - рандомный
			else if(col.Column.Filling.Id == 4)
			{
				var listValue = viewTables
					.First(c => c.Table.Id == col.Column.Settings.IntIdRandTable.Value).ColumnsData
					.First(c => c.Id == col.Column.Settings.IntIdRandColumn.Value)
					.Value;

				var random = new Random();

				listValue = listValue.OrderBy(c => random.Next()).ToList();
				listValues.AddRange(listValue);
			}

			// Случайная строка из файла
			else if(col.Column.Filling.Id == 6)
			{
				var listValue = new List<string>();
				var list = File.ReadAllLines(FileHelper.GetFileById(col.Column.Settings.StringFile).FileName);
				for (int i = 0; i < count; i++)
				{
					listValue.Add(list[ValueHelper.Rand(0 , list.Count())]);
				}

				switch (col.Column.Settings.StringFileSort)
				{
					case Enums.SortEnum.Increasing:
						listValue = listValue.OrderBy(c => c).ToList();
						break;
					case Enums.SortEnum.Descending:
						listValue = listValue.OrderByDescending(c => c).ToList();
						break;
					default:
						break;
				}

				listValues = listValue.Select(c => c.ToString()).ToList();
			}

			// Случайное число с запятой
			else if (col.Column.Filling.Id == 8)
			{
				var listValue = new List<double>();
				for (int i = 0; i < count; i++)
				{
					listValue.Add(ValueHelper.Rand(col.Column.Settings.FloatRandFrom.Value, col.Column.Settings.FloatRandTo.Value));
				}

				switch (col.Column.Settings.FloatRandSort)
				{
					case Enums.SortEnum.Increasing:
						listValue = listValue.OrderBy(c => c).ToList();
						break;
					case Enums.SortEnum.Descending:
						listValue = listValue.OrderByDescending(c => c).ToList();
						break;
					default:
						break;
				}

				listValues = listValue.Select(c => ValueHelper.NormalizateDoubleString(c)).ToList();
			}

			// Случайная дата
			else if (col.Column.Filling.Id == 10)
			{
				var listValue = new List<DateTime>();
				for (int i = 0; i < count; i++)
				{
					listValue.Add(ValueHelper.Rand(col.Column.Settings.DateRandFrom.Value, col.Column.Settings.DateRandTo.Value));
				}

				switch (col.Column.Settings.DateRandSort)
				{
					case Enums.SortEnum.Increasing:
						listValue = listValue.OrderBy(c => c).ToList();
						break;
					case Enums.SortEnum.Descending:
						listValue = listValue.OrderByDescending(c => c).ToList();
						break;
					default:
						break;
				}

				listValues = listValue.Select(c => c.ToString("yyyy-MM-dd hh:mm")).ToList();
			}

			if(listValues.Any())
			{
				col.Values.AddRange(listValues);
			}
		}

        /// <summary>
        /// Присвоение значений полю
        /// </summary>
        /// <param name="col">Поле в объекте ViewColumn</param>
        private static void SetValueColumn (ViewColumn col)
		{
			switch (col.Column.Filling.Id)
			{
				case 1:
					col.Value = col.Column.Settings.IntConst.Value.ToString();
					break;
				case 2: case 4: case 6: case 8: case 10:
					col.Value = col.Values[ValueHelper.Rand(0 , col.Values.Count)];
					break;
                case 3:
                    col.Value = col.Values.First();
                    col.Values.RemoveAt(0);
                    break;
                case 5:
					col.Value = col.Column.Settings.StringConst;
					break;
				case 7:
					col.Value = col.Column.Settings.FloatConst.Value.ToString();
					break;
				case 9:
					col.Value = col.Column.Settings.DateConst.Value.ToString();
					break;
				case 11:
					col.Value = FileHelper.GetValueByFile(col.Column.Settings.ImgConst);
					break;
				case 12:
					col.Value = FileHelper.GetValueByFile(col.Column.Settings.ImgRand);
					break;
				case 13:
					col.Value = col.Column.Settings.BoolConst.Value.ToString();
					break;
				case 14:
					col.Value = ValueHelper.Rand().ToString();
					break;

			}
		}

        /// <summary>
        /// Загрузка в память значений колонки, если они будут необходимы дальше
        /// </summary>
        /// <param name="table">Таблица в объекте Viewtable</param>
        /// <param name="column">Поле в объекте ViewColumn</param>
        private static void SetViewTableDataColumn(ViewTable table , ViewColumn column)
		{
			var viewData = table.ColumnsData.FirstOrDefault(v => v.Id == column.Column.Id);
		    
			if(viewData != null)
			{
				viewData.Value.Add(column.Value);
			}
			else
			{
				viewData = new ViewColumnData(column.Column.Id);
				viewData.Value.Add(column.Value);
				table.ColumnsData.Add(viewData);
			}
		}
        #endregion

        #region Условия

        /// <summary>
        /// Устоновка значений полю по средствам условий в нем
        /// </summary>
        /// <param name="col">Поле в объекте ViewColumn</param>
        /// <param name="table">Таблица в объекте ViewTable</param>
        private static void SetConditionsValueColumn(ViewColumn col, ViewTable table)
		{
			var listCondition = col.Column.Settings.ConditionList;
            string value = string.Empty;

            for (int i = 0; i < listCondition.Count; i++)
			{
				var condition = listCondition[i];

				do
				{
					var value1 = GetConditionValue(table, col, condition);

					if(!string.IsNullOrEmpty(value) && !value1.Equals(value))
					{
						value = string.Empty;
					}
					else
					{
						value = value1;
					}
					
					if (condition.IdConditionNext != 0)
					{
						condition = col.Column.Settings.ConditionList.First(c => c.Id == condition.IdConditionNext);
					}
					else
					{
						condition = null;
					}

				} while (condition != null);

                if (!string.IsNullOrEmpty(value))
                {
                    break;
                }
            }

            col.Value = value;
        }

        /// <summary>
        /// Получение значения по средствам условия колонки
        /// </summary>
        /// <param name="table">Таблица в объекте ViewTable</param>
        /// <param name="col">Поле в объекте ViewColumn</param>
        /// <param name="condition">Условие</param>
        /// <returns>Значение из условия</returns>
        private static string GetConditionValue(ViewTable table , ViewColumn col, Condition condition)
		{
			// Значение, которое в колонке сравнения
			string ValueCondition = table.Columns.First(c => c.Column.Id == condition.ColumnIdIn).Value;

			// Значение с которым сравниваем
			string ValueOut = string.Empty;

			if(condition.ColumnIdOut != 0)
			{
				ValueOut = table.Columns.First(c => c.Column.Id == condition.ColumnIdOut).Value;
			}
			else
			{
				ValueOut = condition.ValueOut;
			}

			string Value = string.Empty;

			if(condition.ColumnIdValue != 0)
			{
				Value = table.Columns.First(c => c.Column.Id == condition.ColumnIdValue).Value;
			}
			else if(condition.IdFile != 0)
			{
				Value = FileHelper.GetValueByFile(condition.IdFile);
			}
			else if(!string.IsNullOrEmpty(condition.Value))
			{
				Value = condition.Value;
			}

			if(CheckConditionValue(ValueCondition , ValueOut , condition.Sign , table.Columns.First(c => c.Column.Id == condition.ColumnIdIn).Column.Type))
			{
				return Value;
			}
			else
			{
				return string.Empty;
			}
			
		}

		/// <summary>
		/// Проверка, соотвествует ли условие условия
		/// </summary>
		/// <param name="valueCondition">Значение, необходимое для сравнения</param>
		/// <param name="valueOut">Значение, которое сравниваем</param>
		/// <param name="sign">Арифметический знак</param>
		/// <param name="typeColumnIn">Тип поля</param>
		/// <returns>true - Значение соотвествует false - значение не соотвествует</returns>
		private static bool CheckConditionValue(string valueCondition , string valueOut , string sign , TypeOf typeColumnIn)
		{
			switch (typeColumnIn.Type)
			{
				case Models.Enums.TypeEnum.Number:

					if(sign.Equals(">"))
					{
						return Convert.ToInt32(valueCondition) > Convert.ToInt32(valueOut);
					}
					else if(sign.Equals("<"))
					{
						return Convert.ToInt32(valueCondition) < Convert.ToInt32(valueOut);
					}
					else if(sign.Equals("=="))
					{
						return Convert.ToInt32(valueCondition) == Convert.ToInt32(valueOut);
					}
					break;

				case Models.Enums.TypeEnum.String:

					if (sign.Equals("=="))
					{
						return valueCondition.Equals(valueOut);
					}
					break;

				case Models.Enums.TypeEnum.Float:

					if (sign.Equals(">"))
					{
						return Convert.ToDouble(valueCondition) > Convert.ToDouble(valueOut);
					}
					else if (sign.Equals("<"))
					{
						return Convert.ToDouble(valueCondition) < Convert.ToDouble(valueOut);
					}
					else if (sign.Equals("=="))
					{
						return Convert.ToDouble(valueCondition) == Convert.ToDouble(valueOut);
					}
					break;

				case Models.Enums.TypeEnum.Date:

					if (sign.Equals(">"))
					{
						return Convert.ToDateTime(valueCondition) > Convert.ToDateTime(valueOut);
					}
					else if (sign.Equals("<"))
					{
						return Convert.ToDateTime(valueCondition) < Convert.ToDateTime(valueOut);
					}
					else if (sign.Equals("=="))
					{
						return Convert.ToDateTime(valueCondition) == Convert.ToDateTime(valueOut);
					}
					break;

				case Models.Enums.TypeEnum.Bool:

					if (sign.Equals("=="))
					{
						return valueCondition.Equals(valueOut);
					}
					break;
				
			}

			return false;
		}
		#endregion


	}
}
