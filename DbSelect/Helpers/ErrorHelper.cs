using DbSelect.Controllers;
using DbSelect.Enums;
using DbSelect.Models.StaticModels;
using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Helpers
{
    public static class ErrorHelper
    {
        /// <summary>
        /// Создания и вывод ошибки
        /// </summary>
        /// <param name = "text"> Текст ошибки </param>
        public static void CreateError(string text)
        {
            var er = new Exeptions(text);
            User.ExeptionsList.Add(er);
            MessageHelper.ErrorMessage(text);
        }

        /// <summary>
        /// Создания и вывод ошибки
        /// </summary>
        /// <param name = "text"> Текст ошибки </param>
        /// <param name = "dopinfo"> Системная информация об ошибке </param>
        public static void CreateError(string text , string dopinfo)
        {
            var er = new Exeptions(text , dopinfo);
            User.ExeptionsList.Add(er);
            MessageHelper.ErrorMessage(text);
        }

        /// <summary>
        /// Создания и вывод ошибки
        /// </summary>
        /// <param name = "text"> Текст ошибки </param>
        /// <param name = "dopinfo"> Системная информация об ошибке </param>
        /// <param name = "loginfo"> Лог ошибки в catch </param>
        public static void CreateError(string text, string dopinfo , string loginfo)
        {
            var er = new Exeptions(text, dopinfo , loginfo);
            User.ExeptionsList.Add(er);
            MessageHelper.ErrorMessage(text + ". " + "Лог ошибки см. в списке ошибок");
        }


		/// <summary>
		/// Создание и вывод предупреждения при проверке таблиц при загрузки или сохранении
		/// </summary>
		/// <param name = "text">Текст ошибки</param>
		public static void CreateTableWarning(string text)
        {
            var error = new CreateTableError()
            {
                Text = text,
                IsError = false,
                TableId = 0,
                ColumnId = 0,
                FileId = 0,
                Type = Enums.CreateTableErrorTypeEnum.None
            };

            User.CreateTableErrors.Add(error);
        }


		/// <summary>
		/// Создание и вывод ошибки при проверке таблиц при загрузки или сохранении
		/// </summary>
		/// <param name = "text">Текст ошибки</param>
		/// <param name = "tableId">Идентификатор таблицы ошибки</param>
		/// <param name = "columnId">Идентификатор колонки ошибки</param>
		/// <param name = "fileId">Идентификатор файла ошибки</param>
		/// <param name = "type">Тип ошибки</param>
		public static void CreateTableError(string text , int tableId , int columnId , int fileId , CreateTableErrorTypeEnum type)
        {
            var error = new CreateTableError()
            {
                Text = text,
                IsError = true,
                TableId = tableId,
                ColumnId = columnId,
                FileId = fileId,
                Type = type
            };

            User.CreateTableErrors.Add(error);
        }

        
    }
}
