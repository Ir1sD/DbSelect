using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Helpers
{
    public static class MessageHelper
    {

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        /// <param name = "text"> Текст сообщения</param>
        public static void ErrorMessage(string text)
        {
            MessageBox.Show(text, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Информационное сообщение
        /// </summary>
        /// <param name = "text"> Текст сообщения</param>
        public static void InfoMessage(string text)
        {
            MessageBox.Show(text, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Сообщение с вопросом и ответами Да и Нет.
        /// </summary>
        /// <param name = "text"> Текст сообщения</param>
        public static bool QueMessage(string text)
        {
            return MessageBox.Show(text, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// Сообщение с вопросом и ответами Да, Нет , Отмена.
        /// </summary>
        /// <param name = "text"> Текст сообщения</param>
        public static DialogResult QueMessageCancel(string text)
        {
            return MessageBox.Show(text, "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Сообщение с предупреждением
        /// </summary>
        /// <param name = "text"> Текст сообщения</param>
        public static void WarningMessage(string text)
        {
            MessageBox.Show(text, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
