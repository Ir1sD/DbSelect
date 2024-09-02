using DbSelect.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Models.SystemModels
{
    /// <summary>
    /// Файл
    /// </summary>
    public class Files
    {
        /// <summary>
        /// Индетификатор файла
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Тип файла
        /// </summary>
        public FileTypeEnum Type {  get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Путь до файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Пути до множества файлов
        /// </summary>
        public List<string> FileNames { get; set; }

		/// <summary>
		/// Максимальный Идентификатор файла
		/// </summary>
		private static int MaxId = 1;

        public Files()
        {
            Id = MaxId++;
            FileNames = new List<string>();
            FileName = string.Empty;
        }

        public Files(int id)
        {
            Id = id;
            FileNames = new List<string>();
            FileName = string.Empty;
            MaxId++;
        }
    }
}
