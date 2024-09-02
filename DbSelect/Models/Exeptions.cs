using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Models.SystemModels
{

    /// <summary>
    /// Ошибка
    /// </summary>
    public class Exeptions
    {
        /// <summary>
        /// Идентификатор ошибки
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///   Лог текст ошибки
        /// </summary>
        public string LogText { get; set; }

        /// <summary>
        ///  Дополнительный текст ошибки
        /// </summary>
        public string DopInfo { get; set; }

        /// <summary>
        /// Дата и время создания ошибки
        /// </summary>
        public DateTime DateTime {  get; private set; }

		/// <summary>
		/// Максимальный Идентификатор ошибки
		/// </summary>
		private static int MaxId = 1;

        public Exeptions(string text)
        {
            Text = text;

            DateTime = DateTime.Now;
            Id = MaxId++;
        }

        public Exeptions(string text , string dopInfo)
        {
            Text = text;
            DopInfo = dopInfo;

            DateTime = DateTime.Now;
            Id = MaxId++;
        }

        public Exeptions(string text , string dopInfo , string logText)
        {
            Text = text;
            LogText = logText;
            DopInfo = dopInfo;

            DateTime = DateTime.Now;
            Id = MaxId++;
        }
    }
}
