using DbSelect.Controllers;
using DbSelect.Models.StaticModels;
using DbSelect.Models.SystemModels;
using DbSelect.StaticModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Helpers
{
    public static class SystemHelper
    {

        /// <summary>
        /// Получение занчения, выбранного в меню
        /// </summary>
        /// <returns> 
        /// Объект TreeNode
        /// </returns>
        public static TreeNode GetSelectedItem()
        {
            foreach (TreeNode item1 in Cm.System.Menu.Nodes)
            {
                if (item1.IsSelected)
                    return item1;

                foreach (TreeNode item2 in item1.Nodes)
                {
                    if (item2.IsSelected)
                        return item2;

                    foreach (TreeNode item3 in item2.Nodes)
                    {
                        if (item3.IsSelected)
                            return item3;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Получение Типа по названию
        /// </summary>
        /// <returns> 
        /// Объект Типа
        /// </returns>
        /// <param name = "Name"> Название типа</param>
        public static TypeOf GetType(string Name)
        {
            var t = StaticModels.Library.types
                .FirstOrDefault(c => c.Name.Equals(Name));

            return t;
        }

        /// <summary>
        /// Получение Типа Автозаполнения по Названию
        /// </summary>
        /// <returns> 
        /// Объект TypeFilling
        /// </returns>
        /// <param name = "Name"> Название типа</param>
        public static TypeFilling GetTypeFilling(string Name)
        {
            var t = StaticModels.Library.typeFilling
                .FirstOrDefault(c => c.Name.Equals(Name));

            return t;
        }

        /// <summary>
        /// Получение типов заполнения по типу поля.
        /// </summary>
        /// <returns> 
        /// Список TypeFilling
        /// </returns>
        /// <param name = "nameType"> Название типа</param>
        public static List<TypeFilling> GetListFilling(string nameType)
        {
            var type = GetType(nameType);
            return GetListFilling(type);
        }

        /// <summary>
        /// Получение типов заполнения по типу поля.
        /// </summary>
        /// <returns> 
        /// Список TypeFilling
        /// </returns>
        /// <param name = "type"> Тип поля</param>
        public static List<TypeFilling> GetListFilling(TypeOf type)
        {
            var listFilling = StaticModels.Library.typeFilling
                .Where(c => c.Type == Models.Enums.TypeEnum.All
                || c.Type == type.Type)
                .OrderBy(c => c.Id)
                .ToList();
            
            var list = new List<TypeFilling>();
            list.AddRange(listFilling);

            return list;
                
        }

        /// <summary>
        /// Выбор в ComboBox нужного элемента по имени
        /// </summary>
        /// <param name = "c"> КомбоБокс</param>
        /// <param name = "name"> Искомое значение</param>
        public static void ComboBoxSelect(ComboBox c , string name)
        {
            for(int i = 0; i < c.Items.Count; i++)
            {
                if (c.Items[i].ToString().Equals(name))
                {
                    c.SelectedIndex = i;
                    break;
                }
            }
        }

		/// <summary>
		/// Нахождение индекса в listbox
		/// </summary>
		/// <returns> 
		/// Индекс элемента
		/// </returns>
		/// <param name = "c"> ЛистБокс</param>
		/// <param name = "name"> Искомое значение</param>
		public static int ListBoxSearch(ListBox c , string name)
        {
            var r = -1;

            for(int i = 0; i < c.Items.Count; i++)
            {
                if (c.Items[i].Equals(name))
                {
                    r = i;
                    break;
                }
            }

            return r;
        }

        /// <summary>
        /// Загрузка иконок 
        /// </summary>
        public static void PullIconsList()
        {
            StaticModels.Library.Icons.Images.Add(Image.FromFile("..\\..\\Source\\Icons\\Tables.png"));
            StaticModels.Library.Icons.Images.Add(Image.FromFile("..\\..\\Source\\Icons\\Table.png"));
            StaticModels.Library.Icons.Images.Add(Image.FromFile("..\\..\\Source\\Icons\\Files.png"));
            StaticModels.Library.Icons.Images.Add(Image.FromFile("..\\..\\Source\\Icons\\text.png"));
            StaticModels.Library.Icons.Images.Add(Image.FromFile("..\\..\\Source\\Icons\\image.png"));
            StaticModels.Library.Icons.Images.Add(Image.FromFile("..\\..\\Source\\Icons\\Group.png"));

            Cm.System.Menu.ImageList = StaticModels.Library.Icons;
        }

        /// <summary>
        /// Преобразование строки из англ/рус символов в рус/англ
        /// </summary>
        /// <param name="t">ТекстБокс для преобразования</param>
        public static void ChangeTextBoxText(TextBox t)
        {
            char[] eng = "QWERTYUIOP{}ASDFGHJKL:\"ZXCVBNM<>qwertyuiop[]asdfghjkl;'zxcvbnm,.".ToCharArray();
            char[] ru = "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮйцукенгшщзхъфывапролджэячсмитьбю".ToCharArray();

            string result = string.Empty;

            for(int i = 0; i < t.Text.Length; i++)
            {
				if (t.Text[i] == ' ')
					result += ' ';

				for (int j = 0; j < eng.Length; j++)
                {
                    if (t.Text[i] == eng[j])
                    {
                        result += ru[j];
                        break;
                    }
                }
            }

            if(result.Length == 0  || result.Length == result.Count(c => c == ' '))
            {
                result = string.Empty;

				for (int i = 0; i < t.Text.Length; i++)
				{
					if (t.Text[i] == ' ')
						result += ' ';

					for (int j = 0; j < ru.Length; j++)
					{
						if (t.Text[i] == ru[j])
						{
							result += eng[j];
							break;
						}
					}

				}
			}
            t.Text = result;
			t.SelectionStart = t.Text.Length;
		}

        /// <summary>
        /// Вывод сообщения в лист на странице запросов
        /// </summary>
        /// <param name="message">Текст</param>
        public static void WriteInfoStartPage(string message)
        {
            Application.DoEvents();
            Thread.Sleep(User.TimeSleep);
            Cm.StartPage.List.Items.Add(message);
            Cm.StartPage.List.SelectedIndex = Cm.StartPage.List.Items.Count - 1;

        }

    }
}
