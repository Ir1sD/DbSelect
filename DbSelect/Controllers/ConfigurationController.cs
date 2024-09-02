using DbSelect.Enums;
using DbSelect.Helpers;
using DbSelect.Models;
using DbSelect.Models.StaticModels;
using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DbSelect.Controllers
{
    public static class ConfigurationController
    {
        /// <summary>
        /// Список строк на вывод в файл при создании конфигурации
        /// </summary>
        private static List<string> list = new List<string>();

        /// <summary>
        /// Значение быстрого создания
        /// </summary>
        public static bool Force = false;

        #region Create

        /// <summary>
        /// Выбор файла для конфигурации
        /// </summary>
        public static void SelectFileConfCreate()
        {
            if (string.IsNullOrEmpty(User.ConfPath2))
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    User.ConfPath2 = dialog.FileName;

                    FileInfo f = new FileInfo(dialog.FileName);
                    Cm.StartPage.SaveConfLabel.Text = $"Файл: {f.Name}";
                }
            }
            else if (MessageHelper.QueMessage("Сбросить текущий файл?"))
            {
                Cm.StartPage.SaveConfLabel.Text = $"Файл: пусто";
                User.ConfPath2 = string.Empty;
            }
        }

        /// <summary>
        /// Создание конфигурации в файл
        /// </summary>
        public static void CreateConf()
        {
            list.Clear();

            Cm.StartPage.List.Items.Clear();
            Cm.StartPage.Text.Text = string.Empty;

            File.WriteAllLines(User.ConfPath2, new string[] { "" });

            foreach (var table in User.Tables)
            {
                Write("table");
                Write(table.Id.ToString());
                Write(table.Name);
                Write(table.Count.ToString());

                foreach (var col in table.Columns)
                {
                    Write("col");
                    Write(col.Id.ToString());
                    Write(col.Name);
                    Write(col.Type.Id.ToString());
                    Write(col.IsNullable.ToString());
                    Write(col.IsPrimaryKey.ToString());
                    Write(col.Filling.Id.ToString());
                    WriteFillingSettings(col);

                    foreach (var c in col.Settings.ConditionList)
                    {
                        Write(c.Text);
                    }
                    Write("$");
                }
            }

            foreach (var file in User.Files)
            {
                Write("file");
                Write(file.Id.ToString());
                Write(file.Name);

                switch (file.Type)
                {
                    case Enums.FileTypeEnum.String:
                        Write("1");
                        break;
                    case Enums.FileTypeEnum.Image:
                        Write("2");
                        break;
                    case Enums.FileTypeEnum.ImageList:
                        Write("3");
                        break;

                }
            }
            Write("end");

            File.WriteAllLines(User.ConfPath2, list.ToArray());
            list.Clear();
            var fileinfo = new FileInfo(User.ConfPath2);
            MessageHelper.InfoMessage($"Файл конфигурации был успешно загружен в файл {fileinfo.Name}");
        }

        /// <summary>
        /// Создание записей в конфигурации о настройках автозаполнения полей
        /// </summary>
        /// <param name="col">Поле</param>
        private static bool WriteFillingSettings(Column col)
        {
            switch (col.Filling.Id)
            {
                case 1:
                    Write(col.Settings.IntConst);
                    return true;
                case 2:
                    Write(col.Settings.IntRandFrom);
                    Write(col.Settings.IntRandTo);
                    Write(col.Settings.IntRandSort);
                    return true;
                case 3:
                    Write(col.Settings.IntIdConstTable);
                    Write(col.Settings.IntIdConstColumn);
                    return true;
                case 4:
                    Write(col.Settings.IntIdRandTable);
                    Write(col.Settings.IntIdRandColumn);
                    return true;
                case 5:
                    Write(col.Settings.StringConst);
                    return true;
                case 6:
                    Write(col.Settings.StringFile.ToString());
                    Write(col.Settings.StringFileSort);
                    return true;
                case 7:
                    Write(col.Settings.FloatConst);
                    return true;
                case 8:
                    Write(col.Settings.FloatRandFrom);
                    Write(col.Settings.FloatRandTo);
                    Write(col.Settings.FloatRandSort);
                    return true;
                case 9:
                    Write(col.Settings.DateConst);
                    return true;
                case 10:
                    Write(col.Settings.DateRandFrom);
                    Write(col.Settings.DateRandTo);
                    Write(col.Settings.DateRandSort);
                    return true;
                case 11:
                    Write(col.Settings.ImgConst.ToString());
                    return true;
                case 12:
                    Write(col.Settings.ImgRand.ToString());
                    return true;
                case 13:
                    Write(col.Settings.BoolConst.Value.ToString());
                    return true;
            }

            return false;
        }
        #endregion

        #region Write

        /// <summary>
        /// Вывод сообщения в лист на странице запросов
        /// </summary>
        /// <param name="mes">Текст</param>
        public static void Write(string mes)
        {
            SystemHelper.WriteInfoStartPage(mes);
            list.Add(mes);
        }

        /// <summary>
        /// Вывод сообщения в лист на странице запросов
        /// </summary>
        /// <param name="mes">Текст</param>
        public static void Write(int? mes)
        {
            Write(mes.Value.ToString());
        }

        /// <summary>
        /// Вывод сообщения в лист на странице запросов
        /// </summary>
        /// <param name="mes">Текст</param>
        public static void Write(int mes)
        {
            Write(mes.ToString());
        }

        /// <summary>
        /// Вывод сообщения в лист на странице запросов
        /// </summary>
        /// <param name="mes">Текст</param>
        public static void Write(double? mes)
        {
            Write(mes.Value.ToString());
        }

        /// <summary>
        /// Вывод сообщения в лист на странице запросов
        /// </summary>
        /// <param name="mes">Текст</param>
        public static void Write(DateTime? mes)
        {
            Write(mes.Value.ToString("dd.MM.yyyy"));
        }

        /// <summary>
        /// Вывод сообщения в лист на странице запросов
        /// </summary>
        /// <param name="mes">Текст</param>
        public static void Write(SortEnum mes)
        {
            if(mes == SortEnum.Increasing)
            {
                Write("1");
            }
            else if(mes == SortEnum.Descending)
            {
                Write("2");
            }
            else
            {
                Write("0");
            }
        }

        /// <summary>
        /// Вывод ошибки в лист на странице запросов
        /// </summary>
        /// <param name="mes">Текст</param>
        /// <param name="i">Номер строки</param>
        public static void WriteErrorSelectConf(string mes , int i)
        {
            Write($"Строка: {i} Ошибка: {mes}");
        }

        #endregion

        #region Select

        /// <summary>
        /// Выбор файла для конфигурации
        /// </summary>
        public static void SelectConfLoad()
        {
            if(string.IsNullOrEmpty(User.ConfPath1))
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    User.ConfPath1 = dialog.FileName;

                    FileInfo f = new FileInfo(dialog.FileName);
                    Cm.StartPage.LoadConfLabel.Text = $"Файл: {f.Name}";
                }
            }
            else if(MessageHelper.QueMessage("Сбросить текущий файл?"))
            {
                Cm.StartPage.LoadConfLabel.Text = $"Файл: пусто";
                User.ConfPath1 = string.Empty;
            }

        }

        /// <summary>
        /// Загрузка конфигурации
        /// </summary>
        public static bool SelectConf()
        {
            User.Tables.Clear();
            User.Files.Clear();
            list.Clear();
            Cm.StartPage.List.Items.Clear();
            Cm.StartPage.Text.Text = string.Empty;

            var conditions = new List<ConditionViewModel>();

            bool IsError = false;
            var file = File.ReadAllLines(User.ConfPath1);
            int i = 0;

            try
            {
                Write("Таблицы:");

                while (file[i].Equals("table"))
                {
                    Table table;
                    i++;

                    if (ValueHelper.IsInt(file[i]))
                    {
                        table = new Table(Convert.ToInt32(file[i]));
                        Write($"Id таблицы - {file[i]}");
                    }
                    else
                    {
                        WriteErrorSelectConf("Непрочитан id таблицы", i);
                        IsError = true;
                        break;
                    }

                    i++;

                    if (file[i].Length > 0)
                    {
                        table.Name = file[i];
                        Write($"Имя таблицы - {file[i]}");
                    }
                    else
                    {
                        WriteErrorSelectConf("Непрочитано имя таблицы", i);
                        IsError = true;
                        break;
                    }

                    i++;

                    if (ValueHelper.IsInt(file[i]))
                    {
                        table.Count = Convert.ToInt32(file[i]);
                        Write($"Количество строк - {file[i]}");
                    }
                    else
                    {
                        WriteErrorSelectConf("Непрочитано количество строк в таблице", i);
                        IsError = true;
                        break;
                    }

                    i++;

                    Write("Поля:");

                    while (file[i].Equals("col"))
                    {
                        Column col;
                        i++;

                        if (ValueHelper.IsInt(file[i]))
                        {
                            col = new Column(Convert.ToInt32(file[i]));
                            Write($"Id поля - {file[i]}");
                        }
                        else
                        {
                            WriteErrorSelectConf("Непрочитан id поля", i);
                            IsError = true;
                            break;
                        }

                        i++;

                        if (file[i].Length > 0)
                        {
                            col.Name = file[i];
                            Write($"Имя поля - {file[i]}");
                        }
                        else
                        {
                            WriteErrorSelectConf("Непрочитано имя поля", i);
                            IsError = true;
                            break;
                        }

                        i++;

                        if (ValueHelper.IsInt(file[i]))
                        {
                            var type = StaticModels.Library.types.FirstOrDefault(c => c.Id == Convert.ToInt32(file[i]));

                            if (type != null)
                            {
                                col.Type = type;
                                Write($"Тип поля - {type.Name}");
                            }
                            else
                            {
                                WriteErrorSelectConf("Непрочитан Тип поля", i);
                                IsError = true;
                                break;
                            }

                        }
                        else
                        {
                            WriteErrorSelectConf("Непрочитан Тип поля", i);
                            IsError = true;
                            break;
                        }

                        i++;

                        if (ValueHelper.IsBool(file[i]))
                        {
                            col.IsNullable = ValueHelper.InBool(file[i]);
                            Write($"Значение null - {file[i]}");
                        }
                        else
                        {
                            WriteErrorSelectConf("Непрочитан индефикатор значения на null", i);
                            IsError = true;
                            break;
                        }

                        i++;

                        if (ValueHelper.IsBool(file[i]))
                        {
                            col.IsPrimaryKey = ValueHelper.InBool(file[i]);
                            Write($"Значение ключа - {file[i]}");
                        }
                        else
                        {
                            WriteErrorSelectConf("Непрочитан индефикатор ключа", i);
                            IsError = true;
                            break;
                        }

                        i++;

                        if (ValueHelper.IsInt(file[i]))
                        {
                            var type = StaticModels.Library.typeFilling.FirstOrDefault(c => c.Id == Convert.ToInt32(file[i]));

                            if (type != null)
                            {
                                col.Filling = type;
                                Write($"Тип автозаполнения - {type.Name}");
                            }
                            else
                            {
                                WriteErrorSelectConf("Непрочитан Тип автозаполнения", i);
                                IsError = true;
                                break;
                            }

                        }
                        else
                        {
                            WriteErrorSelectConf("Непрочитан Тип автозаполнение", i);
                            IsError = true;
                            break;
                        }

                        i++;

                        int v = SelectFillingSettings(col, file, i);

                        if (v > -1)
                        {
                            Write($"Настройки автозаполнения пересененны успешно");
                        }
                        else
                        {
                            WriteErrorSelectConf("Непрочитаны настройки автозаполнения", i + v);
                            IsError = true;
                            break;
                        }

                        i += v;

                        while (!file[i].Equals("$"))
                        {
                            var cot = new ConditionViewModel();
                            cot.Text = file[i];
                            cot.TableId = table.Id;
                            cot.ColumnId = col.Id;
                            conditions.Add(cot);

                            i++;
                        }

                        i++;

                        if (!IsError)
                            table.Columns.Add(col);
                    }

                    if (IsError)
                        break;

                    User.Tables.Add(table);
                }

                if (!IsError)
                {
                    Write("Файлы: ");
                    while (file[i].Equals("file"))
                    {
                        Files fil;
                        i++;

                        if (ValueHelper.IsInt(file[i]))
                        {
                            fil = new Files(Convert.ToInt32(file[i]));
                            Write($"Id файла - {file[i]}");
                        }
                        else
                        {
                            WriteErrorSelectConf("Непрочитан id файла", i);
                            IsError = true;
                            break;
                        }

                        i++;

                        if (file[i].Length > 0)
                        {
                            fil.Name = file[i];
                            Write($"Имя файла - {file[i]}");
                        }
                        else
                        {
                            WriteErrorSelectConf("Непрочитано имя файла", i);
                            IsError = true;
                            break;
                        }

                        i++;

                        if (ValueHelper.IsInt(file[i]))
                        {
                            switch (Convert.ToInt32(file[i]))
                            {
                                case 1:
                                    fil.Type = Enums.FileTypeEnum.String;
                                    break;
                                case 2:
                                    fil.Type = Enums.FileTypeEnum.Image;
                                    break;
                                case 3:
                                    fil.Type = Enums.FileTypeEnum.ImageList;
                                    break;
                            }
                            Write($"Тип файла прочитан успешно");
                        }
                        else
                        {
                            WriteErrorSelectConf("Непрочитан тип файла", i);
                            IsError = true;
                            break;
                        }

                        fil.FileName = string.Empty;
                        User.Files.Add(fil);
                        i++;
                    }

                }

                if (!IsError)
                {
                    // Условия
                    Write("Условия для таблиц");

                    foreach (var cond in conditions)
                    {
                        var table = TableHelper.GetTableById(cond.TableId);
                        var col = TableHelper.GetColumnsById(cond.ColumnId, table.Id);

                        int error;
                        var condition = TableController.GetCondition(cond.Text, col, table, out error);

                        if (condition != null)
                        {
                            col.Settings.ConditionList.Add(condition);
                            Write($"Таблица {table.Name} Поле {col.Name} Условие {cond.Text} загруженно успешно");
                        }
                        else
                        {
                            Write($"Таблица {table.Name} Поле {col.Name} Условие {cond.Text} завершенно с ошибкой");
                            IsError = true;
                            break;
                        }
                    }
                }

                if (!IsError && !Force)
                {
                    var fileinfo = new FileInfo(User.ConfPath1);
                    MessageHelper.InfoMessage($"Файл конфигурации был успешно загружен из файла {fileinfo.Name}");
                }
            }
            catch (Exception e)
            {
                Write("Ошибка чтения файла конфигурации. Попытка загрузить файл закончилось ошибкой");
                ErrorHelper.CreateError("Ошибка чтения файла конфигурации", "Исключение", e.Message);
            }

            list.Clear();

            return !IsError;
        }

        /// <summary>
        /// Заполнение настроек автозаполнения из конфигурации
        /// </summary>
        /// <param name="col">Колонка из таблицы</param>
        /// <param name="file">Список строк из файла конфигурации</param>
        /// <param name="i">Текущее месторасположение в файле</param>
        /// <returns>На сколько строк продвинулось</returns>
        private static int SelectFillingSettings(Column col , string[] file , int i)
        {
            col.Settings = new FillingSettings();

            switch (col.Filling.Id)
            {
                case 0:

                    return 0;

                case 1:

                    if (ValueHelper.IsInt(file[i]))
                    {
                        col.Settings.IntConst = Convert.ToInt32(file[i]);
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }

                case 2:
                    
                    if (ValueHelper.IsInt(file[i]))
                    {
                        col.Settings.IntRandFrom = Convert.ToInt32(file[i]);
                    }
                    else
                    {
                        return -1;
                    }

                    i++;

                    if (ValueHelper.IsInt(file[i]))
                    {
                        col.Settings.IntRandTo = Convert.ToInt32(file[i]);
                    }
                    else
                    {
                        return -1;
                    }

                    i++;

                    if (ValueHelper.IsInt(file[i]))
                    {
                        col.Settings.IntRandSort = GetSortEnum(Convert.ToInt32(file[i]));
                        return 3;
                    }
                    else
                    {
                        return -1;
                    }

                case 3:
                    
                    if (ValueHelper.IsInt(file[i]))
                    {
                        col.Settings.IntIdConstTable = Convert.ToInt32(file[i]);
                    }
                    else
                    {
                        return -1;
                    }

                    i++;

                    if (ValueHelper.IsInt(file[i]))
                    {
                        col.Settings.IntIdConstColumn = Convert.ToInt32(file[i]);
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }

                case 4:
                   
                    if (ValueHelper.IsInt(file[i]))
                    {
                        col.Settings.IntIdRandTable = Convert.ToInt32(file[i]);
                    }
                    else
                    {
                        return -1;
                    }

                    i++;

                    if (ValueHelper.IsInt(file[i]))
                    {
                        col.Settings.IntIdRandColumn = Convert.ToInt32(file[i]);
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }

                case 5:

                    if (file[i].Length > 0)
                    {
                        col.Settings.StringConst = file[i];
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }

                case 6:

                    if (ValueHelper.IsInt(file[i]))
                    {
                        col.Settings.StringFile = Convert.ToInt32(file[i]);
                    }
                    else
                    {
                        return -1;
                    }

                    i++;

					if (ValueHelper.IsInt(file[i]))
					{
						col.Settings.StringFileSort = GetSortEnum(Convert.ToInt32(file[i]));
						return 2;
					}
					else
					{
						return -1;
					}

				case 7:

                    if (ValueHelper.IsFloat(file[i]))
                    {
                        col.Settings.FloatConst = Convert.ToDouble(file[i]);
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }

                case 8:
                    
                    if (ValueHelper.IsFloat(file[i]))
                    {
                        col.Settings.FloatRandFrom = Convert.ToDouble(file[i]);
                    }
                    else
                    {
                        return -1;
                    }

                    i++;

                    if (ValueHelper.IsFloat(file[i]))
                    {
                        col.Settings.FloatRandTo = Convert.ToDouble(file[i]);
                    }
                    else
                    {
                        return 1;
                    }

                    i++;

					if (ValueHelper.IsInt(file[i]))
					{
						col.Settings.FloatRandSort = GetSortEnum(Convert.ToInt32(file[i]));
						return 3;
					}
					else
					{
						return -1;
					}

				case 9:
                    
                    if (ValueHelper.IsDate(file[i]))
                    {
                        col.Settings.DateConst = Convert.ToDateTime(file[i]);
                        return 1;
                    }
                    else
                    {
                        return -1   ;
                    }

                case 10:

                    if (ValueHelper.IsDate(file[i]))
                    {
                        col.Settings.DateRandFrom = Convert.ToDateTime(file[i]);
                    }
                    else
                    {
                        return -1;
                    }

                    i++;

                    if (ValueHelper.IsDate(file[i]))
                    {
                        col.Settings.DateRandTo = Convert.ToDateTime(file[i]);
                    }
                    else
                    {
                        return 1;
                    }

                    i++;

					if (ValueHelper.IsInt(file[i]))
					{
						col.Settings.DateRandSort = GetSortEnum(Convert.ToInt32(file[i]));
						return 3;
					}
					else
					{
						return -1;
					}

				case 11:
                   
                    if (ValueHelper.IsInt(file[i]))
                    {
                        col.Settings.ImgConst = Convert.ToInt32(file[i]);
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }

                case 12:
                   
                    if (ValueHelper.IsInt(file[i]))
                    {
                        col.Settings.ImgRand = Convert.ToInt32(file[i]);
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }

                case 13:
                    
                    if (ValueHelper.IsBool(file[i]))
                    {
                        col.Settings.BoolConst = ValueHelper.InBool(file[i]);
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
            }

            return -1;
        }

        /// <summary>
        /// Быстрая загрузка файла конфигурации для Админа
        /// </summary>
        public static void FastConfSelect()
        {
			User.ConfPath1 = "..\\..\\Source\\Conf\\c1.txt";
			Force = true;
            int j = User.TimeSleep;
            User.TimeSleep = 0;
			if (SelectConf())
				SystemController.UpdateInfoTree();

			Force = false;
			User.ConfPath1 = string.Empty;
            User.TimeSleep = j;
		}

        /// <summary>
        /// Преобразование из int в соотвествующий enum сортировки таблицы
        /// </summary>
        /// <param name="i">Значение</param>
        /// <returns>Enum значение сортировки</returns>
        private static SortEnum GetSortEnum (int i)
        {
            switch (i)
            {
                case 2:
                    return SortEnum.Increasing;
                case 3:
                    return SortEnum.Descending;
            }

            return SortEnum.Defoult;
        }
        #endregion

    }
}
