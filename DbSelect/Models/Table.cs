using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Models.SystemModels
{

    /// <summary>
    /// Таблица
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Индетификатор таблицы
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Название таблицы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список полей таблицы
        /// </summary>
        public List<Column> Columns { get; set; }

        /// <summary>
        /// Приоритет для загрузки/сохранения
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Количество строк при автозаполнении
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Максимальный идентификатор для таблицы
        /// </summary>
        private static int MaxId = 1;

        public Table()
        {
            Id = MaxId++;
            Priority = 0;
            Columns = new List<Column>();
        }

        public Table(int id)
        {
            Id = id;
            Priority = 0;
            Columns = new List<Column>();
            MaxId++;
        }

    }
}
