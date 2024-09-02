using DbSelect.Models.StaticModels;
using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DbSelect.Helpers
{
    public static class TableHelper
    {

        /// <summary>
        /// Получение поля по имени
        /// </summary>
        /// <returns> 
        /// Объект Поля
        /// </returns>
        /// <param name = "Name"> Название поля</param>
        /// <param name = "TableName"> Название таблицы </param>
        public static Column GetColumnByName(string Name , string TableName)
        {
            var table = User.Tables.FirstOrDefault(t => t.Name.ToLower().Equals(TableName.ToLower()));
            var col = table.Columns.FirstOrDefault(c => c.Name.ToLower().Equals(Name.ToLower()));
            return col;
        }

        /// <summary>
        /// Получение таблицы по имени
        /// </summary>
        /// <returns> 
        /// Объект Таблицы
        /// </returns>
        /// <param name = "Name"> Название таблицы</param>
        public static Table GetTableByName(string Name)
        {
            var table = User.Tables
                .FirstOrDefault(c => c.Name.ToLower().Equals(Name.ToLower()));

            return table;
        }

        /// <summary>
        /// Получение таблицы по Id
        /// </summary>
        /// <returns> 
        /// Объект Таблицы
        /// </returns>
        /// <param name = "id"> Id поля</param>
        public static Table GetTableById(int id)
        {
            var table = User.Tables.FirstOrDefault(t => t.Id == id);
            return table;
        }

        /// <summary>
        /// Получение поля по Id
        /// </summary>
        /// <returns> 
        /// Объект Поля
        /// </returns>
        /// <param name = "id"> Id поля</param>
        /// <param name = "tableId"> Id таблицы</param>
        public static Column GetColumnsById(int id , int tableId)
        {
            var table = User.Tables.FirstOrDefault(t => t.Id == tableId);
            var col = table.Columns.FirstOrDefault(c => c.Id == id);
            return col;
        }

		/// <summary>
		/// Изменение приоритета в таблицах
		/// </summary>
		/// <param name = "tables"> Список таблиц</param>
		/// <param name = "priority"> Приоритет</param>
		public static void ChangePriorityTable(List<Table> tables, int priority = 0)
        {
            foreach (var table in tables)
            {
                if(priority > 0)
                {
					table.Priority = priority + 1;
				}
               
                foreach (var col in table.Columns)
                {
					var tlist = new List<Table>();
					Table tab;

                    if(col.Settings.IntIdConstTable.HasValue)
                    {
                        tab = GetTableById(col.Settings.IntIdConstTable.Value);
                        tlist.Add(tab);
                    }
                    else if(col.Settings.IntIdRandTable.HasValue)
                    {
                        tab = GetTableById(col.Settings.IntIdRandTable.Value);
                        tlist.Add(tab);
                    }

                    ChangePriorityTable(tlist, table.Priority + 1);
                }
            }
        }

		/// <summary>
		/// Изменение приоритета в колонках
		/// </summary>
		/// <param name = "cols"> Список колонок</param>
		/// <param name = "priority"> Приоритет</param>
		public static void ChangePriorityColumn(List<Column> cols , int priority = 0)
		{
            foreach (var col in cols)
            {
                if(priority > 0) // Просмотреть и отредактировать приоритет колонкам
                {
                    col.Priority = priority + 1;
                }

                var collist = new List<Column>();
                Column c;

                if(col.Settings.IntIdConstColumn.HasValue)
                {
                    c = GetColumnsById(col.Settings.IntIdConstColumn.Value, col.Settings.IntIdConstTable.Value);
                    collist.Add(c);
                }

                if(col.Settings.IntIdRandColumn.HasValue)
                {
					c = GetColumnsById(col.Settings.IntIdRandColumn.Value, col.Settings.IntIdRandTable.Value);
					collist.Add(c);
				}

                foreach (var item in col.Settings.ConditionList)
                {
                    if(item.ColumnIdValue != 0)
                    {
                        c = GetColumnsById(item.ColumnIdValue, item.TableId);
                        collist.Add(c);
                    }

                    if(item.ColumnIdIn != 0)
                    {
						c = GetColumnsById(item.ColumnIdIn, item.TableId);
						collist.Add(c);
					}

                    if(item.ColumnIdOut != 0)
                    {
						c = GetColumnsById(item.ColumnIdOut, item.TableId);
						collist.Add(c);
					}
                }

                ChangePriorityColumn(collist, col.Priority + 1);


            }
		}

        /// <summary>
        /// Проверка, нужны ли значения поля в дальнейшем для автозаполнения
        /// </summary>
        /// <param name="column">Поле</param>
        /// <param name="table">Таблица</param>
        /// <returns>true - Да false - Нет</returns>
        public static bool CheckKeyColumn(Column column , Table table)
        {
            foreach (var t in User.Tables.Where(c => c.Id != table.Id).ToList()) //
            {
                foreach (var col in t.Columns)
                {
                    if((col.Settings.IntIdConstTable.HasValue 
                        && col.Settings.IntIdConstTable.Value == table.Id 
                        && col.Settings.IntIdConstColumn.HasValue 
                        && col.Settings.IntIdConstColumn.Value == column.Id)
						|| (col.Settings.IntIdRandTable.HasValue 
                        && col.Settings.IntIdRandTable.Value == table.Id 
                        && col.Settings.IntIdRandColumn.HasValue 
                        && col.Settings.IntIdRandColumn.Value == column.Id))
                    {
                        return true;
                    }

                }
            }

            return false;
        }
	}
}
