using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DbSelect.Helpers;
using DbSelect.Models;
using DbSelect.Models.StaticModels;
using DbSelect.Models.SystemModels;
using DbSelect.StaticModels;
using System.Linq;
using System.Xml.Linq;
using System.Drawing;
using System.Security.Cryptography;
using DbSelect.Enums;
using System.Diagnostics.Eventing.Reader;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Text;
using System.IO;

namespace DbSelect.Controllers
{
    public static class TableController
    {
        #region Создание таблиц

        /// <summary>
        /// Заполнение DataGridView при создании или редактировании таблицы
        /// </summary>
        public static void PullDataGridCreateTable()
        {
            DataGridView d = Cm.CreateTable.d;

            d.Columns.Add("", "Название");

            var col1 = new DataGridViewComboBoxColumn();
            col1.Name = "Тип";

            foreach (var type in StaticModels.Library.types.OrderBy(c => c.Name).Select(c => c.Name))
            {
                col1.Items.Add(type);
            }

            d.Columns.Add(col1);

            var col2 = new DataGridViewCheckBoxColumn();
            col2.Name = "Null?";
            col2.Width = 70;
            d.Columns.Add(col2);

            var col4 = new DataGridViewComboBoxColumn();
            col4.Name = "Автозаполнение?";
            col4.Width = 150;
            d.Columns.Add(col4);

            var col3 = new DataGridViewCheckBoxColumn();
            col3.Name = "Ключ?";
            col3.Width = 70;
            d.Columns.Add(col3);

            d.Columns.Add("id", "id");
            d.Columns[5].Visible = false;

        }

        /// <summary>
        /// Проверка на корректность ввода данных при создании или редактировании таблицы
        /// </summary>
        /// <returns> 
        /// При ошибке - false, в остальных случаях - true
        /// </returns>
        /// <param name = "IsUpdate"> Обновление?</param>
        private static bool CheckTable(bool IsUpdate)
        {
            var Name = Cm.CreateTable.Name.Text;
            var d = Cm.CreateTable.d;
            var listName = new List<string>();

            if (string.IsNullOrEmpty(Name))
            {
                ErrorHelper.CreateError("Имя должно быть заполнено", "При создании таблицы не указанно имя");
                return false;
            }

            if(!IsUpdate && User.Tables.Count(c => c.Name.Equals(Name)) != 0)
            {
                ErrorHelper.CreateError("Таблица с таким названием уже существует", "При создании таблицы было указанно имя, уже существующеей таблицы");
                return false;
            }
            
            for (int i = 0; i < d.Rows.Count - 1; i++)
            {
                if (d.Rows[i].Cells[0].Value == null || string.IsNullOrEmpty(d.Rows[i].Cells[0].Value.ToString()))
                {
                    ErrorHelper.CreateError("Имя всех полей должно быть заполнено", "При создании таблицы не указанно имя поля. Таблица: " + Name);
                    return false;
                }

                if ((d.Rows[i].Cells[1].Value != null && string.IsNullOrEmpty(d.Rows[i].Cells[1].Value.ToString())) || d.Rows[i].Cells[1].Value == null)
                {
                    ErrorHelper.CreateError("Тип данных всех полей должнен быть заполнен", "При создании таблицы не указанно тип данных поля: " + d.Rows[i].Cells[0].Value.ToString() + ". Таблица: " + Name);
                    return false;
                }
                
                var type = SystemHelper.GetType(d.Rows[i].Cells[1].Value.ToString());

                if (Convert.ToBoolean(d.Rows[i].Cells[4].Value) && type.Type != Models.Enums.TypeEnum.Number)
                {
                    ErrorHelper.CreateError("Ключевое поле должно иметь целочисленный тип", "При создании таблицы ключевое поле указано не на целочисленный тип данных. Поле: " + d.Rows[i].Cells[0].Value.ToString() + ". Таблица: " + Name);
                    return false;
                }

                listName.Add(d.Rows[i].Cells[0].Value.ToString());
            }

            foreach (var name in listName)
            {
                var count = listName.Count(c => c.Equals(name));

                if(count > 1)
                {
                    ErrorHelper.CreateError("В одной таблице не должно быть полей с идентичными именами", "При создании/обновлении таблицы указаны 2 поля с одинаковыми названиями: " + name + ". Таблица: " + Name);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Отчистка формы создания или редактирования таблицы
        /// </summary>
        public static void ClearCreateTableForm()
        {
            Cm.CreateTable.d.Rows.Clear();
            Cm.CreateTable.d.Columns.Clear();
            Cm.CreateTable.Name.Clear();
            User.SelectedTableId = -1;
        }

        /// <summary>
        /// Выводит список типов автозаполнений в столбец "Автозаполнение", если был выбран тип данных.
        /// </summary>
        /// <param name = "rowid"> Порядковый номер строки, где был отредактирован тип </param>
        public static void UpdateFillingType(int rowid)
        {
            var name = Cm.CreateTable.d.Rows[rowid].Cells[1].Value.ToString();
            var fillingType = SystemHelper.GetListFilling(name);

            (Cm.CreateTable.d.Rows[rowid].Cells[3] as DataGridViewComboBoxCell).Items.Clear();
            foreach (var item in fillingType)
            {
                (Cm.CreateTable.d.Rows[rowid].Cells[3] as DataGridViewComboBoxCell).Items.Add(item.Name);
            }
        }

        #endregion

        #region Удаление 

        /// <summary>
        /// Удаление таблицы
        /// </summary>
        /// <returns> 
        /// При ошибке - false, в остальных случаях - true
        /// </returns>
        public static bool DeleteTable()
        {
            try
            {
                var table = TableHelper.GetTableById(User.SelectedTableId);
                User.Tables.Remove(table);
                MessageHelper.InfoMessage("Таблица " + table.Name + " была удалена");

            }
            catch (Exception e)
            {
                ErrorHelper.CreateError("Ошибка при удалении", "При удалении таблицы произошла ошибка.", e.Message);
                return false;
            }
            User.SelectedTableId = -1;
            SystemController.UpdateInfoTree();
            return true;

        }

        /// <summary>
        /// Удаление всех таблиц
        /// </summary>
        public static void DeleteTablesList()
        {
            if(MessageHelper.QueMessage("Удалить все таблицы?"))
            {
                User.Tables.Clear();
                SystemController.UpdateInfoTree();
            }
        }

        #endregion

        #region Редактирование

        /// <summary>
        /// Заполнение данными DataGridView из таблицы при редактировании
        /// </summary>
        /// <param name = "tableId"> Id таблицы </param>
        public static void PullUpdateTable(int tableId)
        {
            var table = TableHelper.GetTableById(tableId);
            var d = Cm.CreateTable.d;

            Cm.CreateTable.Name.Text = table.Name;

            foreach (var col in table.Columns)
            {
                d.Rows.Add(col.Name, null, col.IsNullable, null, col.IsPrimaryKey , col.Id);
                DataGridViewCell cell = d.Rows[d.Rows.Count - 2].Cells[1];
                (cell as DataGridViewComboBoxCell).Value = col.Type.Name;

                cell = d.Rows[d.Rows.Count - 2].Cells[3];
                (cell as DataGridViewComboBoxCell).Value = col.Filling.Name;
            }
        }

        /// <summary>
        /// Создание или обновление таблицы
        /// </summary>
        /// <returns> 
        /// При ошибке - false, в остальных случаях - true
        /// </returns>
        /// <param name = "IsUpdate"> Обновление?</param>
        public static bool UpdateTable(bool IsUpdate)
        {
            if (CheckTable(IsUpdate))
            {
                var table = new Table();

                if (User.SelectPage == Enums.PagesEnum.UpdateTable)
                {
                    table = TableHelper.GetTableById(User.SelectedTableId);
                }

                var d = Cm.CreateTable.d;
                var listCol = new List<Column>();
                listCol.AddRange(table.Columns);

                table.Name = Cm.CreateTable.Name.Text;

                for (int i = 0; i < d.Rows.Count - 1; i++)
                {
                    var col = new Column();

                    if (d.Rows[i].Cells[5].Value != null)
                    {
                        col = TableHelper.GetColumnsById(Convert.ToInt32(d.Rows[i].Cells[5].Value), table.Id);
                        listCol.Remove(col);
                    }

                    col.Name = d.Rows[i].Cells[0].Value.ToString();
                    var nameType = d.Rows[i].Cells[1].Value.ToString();
                    col.Type = SystemHelper.GetType(nameType);
                    col.IsNullable = Convert.ToBoolean(d.Rows[i].Cells[2].Value);
                    col.IsPrimaryKey = Convert.ToBoolean(d.Rows[i].Cells[4].Value);

                     if (d.Rows[i].Cells[3] != null && d.Rows[i].Cells[3].Value != null)
                     {
                        var typeFilling = SystemHelper.GetTypeFilling(d.Rows[i].Cells[3].Value.ToString());

                        if(IsUpdate && (col.Filling == null || col.Filling.Id != typeFilling.Id))
                        {
                            col.Settings = new FillingSettings();
                        }
                        else if(!IsUpdate)
                        {
                            col.Settings = new FillingSettings();
                        }

                        col.Filling = typeFilling;
                     }
                    else
                    {
                        col.Filling = StaticModels.Library.typeFilling[0];
                        col.Settings = new FillingSettings();
                    }

                    if (d.Rows[i].Cells[5].Value == null)
                    {
                        table.Columns.Add(col);
                    }

                    if(col.IsPrimaryKey && col.Filling.Id != 0)
                    {
                        MessageHelper.WarningMessage("Автозаполнение невозможно при параметре: Ключ");
                        col.Settings = new FillingSettings();
                        col.Filling = Library.typeFilling.First(c => c.Id == 0);
                    }

                }

                foreach (var item in listCol)
                {
                    table.Columns.Remove(item);
                }

                if (User.SelectPage == Enums.PagesEnum.CreateTable)
                {
                    User.Tables.Add(table);
                }

                SystemController.UpdateInfoTree();
                User.SelectedTableId = -1;
                return true;
            }

            return false;
        }

        #endregion

        #region Автозаполнение


        /// <summary>
        /// Заполнения списка таблиц в Автозаполнении
        /// </summary>
        public static void PullTableList()
        {
            Cm.Filling.Tables.Items.Clear();

            foreach (var item in User.Tables)
            {
                Cm.Filling.Tables.Items.Add(item.Name);
            }

            if (Cm.Filling.Tables.Items.Count > 0)
            {
                var table = TableHelper.GetTableById(User.SelectedTableId);

                if (table != null)
                {
                    SystemHelper.ComboBoxSelect(Cm.Filling.Tables, table.Name);
                }
                else
                    Cm.Filling.Tables.SelectedIndex = 0;
            }
        }


        /// <summary>
        /// Заполнения списка полей в Автозаполнении
        /// </summary>
        public static void PullColumnsList()
        {
            if(CheckUpdateFilling())
            {
                var name = Cm.Filling.Tables.SelectedItem as string;
                var table = TableHelper.GetTableByName(name);

                Cm.Filling.CountText.Text = table.Count.ToString();
                Cm.Filling.CountText.Enabled = true;

                Cm.Filling.Columns.Items.Clear();

                foreach (var item in table.Columns)
                {
                    Cm.Filling.Columns.Items.Add(item.Name);
                }

                User.SelectedColumnId = -1;
                User.SelectedTableId = table.Id;
                ClearFillingTabs();

                if (Cm.Filling.Columns.Items.Count > 0)
                    Cm.Filling.Columns.SelectedIndex = 0;
                else
                    NavigationHelper.FillingOpenWindow(0);

            }
            else
            {
                var table = TableHelper.GetTableById(User.SelectedTableId);
                Cm.Filling.Tables.Text = table.Name;
            }
        }

        /// <summary>
        /// Сохрание изменений в Автозаполнении
        /// </summary>
        /// <returns> 
        /// При ошибке - false, в остальных случаях - true
        /// </returns>
        public static bool CheckUpdateFilling()
        {
           
            if (User.SelectedColumnId != -1)
            {
                if (UpdateFillingValue() == false)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Обновление формы при выборе поля в Автозаполнении
        /// </summary>
        public static void OpenWindowFilling()
        {
            if(Cm.Filling.Columns.SelectedIndex >= 0 && CheckUpdateFilling())
            {
                ClearFillingTabs();
                var col = TableHelper.GetColumnByName(Cm.Filling.Columns.SelectedItem as string, Cm.Filling.Tables.Text);
                User.SelectedColumnId = col.Id;
                UpdateFillingTab(col);

                NavigationHelper.FillingOpenWindow(col.Filling.Id);

                Cm.Filling.bDown.Enabled = true;
                Cm.Filling.bUp.Enabled = true;

                if (Cm.Filling.Tables.SelectedIndex == 0 && Cm.Filling.Columns.SelectedIndex == 0)
                {
                    Cm.Filling.bDown.Enabled = false;
                }
                else if (Cm.Filling.Tables.SelectedIndex == Cm.Filling.Tables.Items.Count - 1 && Cm.Filling.Columns.SelectedIndex == Cm.Filling.Columns.Items.Count - 1)
                {
                    Cm.Filling.bUp.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Функционал кнопок перемещения между таблицами и полями в автозаполнении
        /// </summary>
        /// <param name="st">1 - вперед , 2 - назад</param>
        public static void FiilingUpdateStatusButton(byte st)
        {
            try
            {
                if (st == 1)
                {
                    if (Cm.Filling.Columns.SelectedIndex > 0)
                    {
                        Cm.Filling.Columns.SelectedIndex = Cm.Filling.Columns.SelectedIndex - 1;
                    }
                    else
                    {
                        Cm.Filling.Tables.SelectedIndex = Cm.Filling.Tables.SelectedIndex - 1;
                        Cm.Filling.Columns.SelectedIndex = Cm.Filling.Columns.Items.Count - 1;
                    }
                }
                else if (st == 2)
                {
                    if (Cm.Filling.Columns.SelectedIndex < Cm.Filling.Columns.Items.Count - 1)
                    {
                        Cm.Filling.Columns.SelectedIndex = Cm.Filling.Columns.SelectedIndex + 1;
                    }
                    else
                    {
                        Cm.Filling.Tables.SelectedIndex = Cm.Filling.Tables.SelectedIndex + 1;
                    }
                }
            }
            catch (Exception)
            {

            }
            
        }

        /// <summary>
        /// Отчистка форм в Автозаполнении
        /// </summary>
        public static void ClearFillingTabs()
        {
            
            Cm.Filling.IntConst.Value.Value = 0;
            Cm.Filling.IntRand.ValueFrom.Value = 0;
            Cm.Filling.IntRand.ValueTo.Value = 0;
            Cm.Filling.IntIdConst.Table.Items.Clear();
            Cm.Filling.IntIdConst.Column.Items.Clear();
            Cm.Filling.IntIdConst.Table.Text = string.Empty;
            Cm.Filling.IntIdConst.Column.Text = string.Empty;
            Cm.Filling.IntIdRand.Table.Items.Clear();
            Cm.Filling.IntIdRand.Column.Items.Clear();
            Cm.Filling.IntIdRand.Table.Text = string.Empty;
            Cm.Filling.IntIdRand.Column.Text = string.Empty;
            Cm.Filling.StringConst.Value.Clear();
            Cm.Filling.FloatConst.Value.Value = 0;
            Cm.Filling.FloatRand.ValueFrom.Value = 0;
            Cm.Filling.FloatRand.ValueTo.Value = 0;
            Cm.Filling.DateConst.Value.Value = DateTime.Today;
            Cm.Filling.DateRand.ValueFrom.Value = DateTime.Today;
            Cm.Filling.DateRand.ValueTo.Value = DateTime.Today;
            Cm.Filling.BoolConst.Value.Checked = false;
            Cm.Filling.StringFile.File.Items.Clear();
            Cm.Filling.ImgConst.File.Items.Clear();
            Cm.Filling.ImgFile.File.Items.Clear();
            Cm.Filling.ConditionList.Items.Clear();
            Cm.Filling.ConditionText.Clear();
            User.SelectedColumnId = -1;
            

            
        }

        /// <summary>
        /// Отчистка Автозаполнения
        /// </summary>
        public static void ClearFilling()
        {
            Cm.Filling.Columns.Items.Clear();
            Cm.Filling.Tables.Items.Clear();
            Cm.Filling.Tables.Text = string.Empty;
            Cm.Filling.Tab.SelectedIndex = 0;
            User.SelectedTableId = -1;
            User.SelectedColumnId = -1;
            Cm.Filling.CountText.Enabled = false;
            Cm.Filling.CountText.Text = "0";
        }

        /// <summary>
        /// Обновление содержимого формы сохраненными данными при выборе поля в Автозаполнении
        /// </summary>
        /// <param name = "col"> Поле таблицы </ param >
        private static void UpdateFillingTab(Column col)
        {
            var table = TableHelper.GetTableById(User.SelectedTableId);

            Cm.Filling.Type.Text = $"Тип: {col.Filling.Name}";

            foreach (var item in User.Tables.Where(t => !t.Name.Equals(table.Name)))
            {
                Cm.Filling.IntIdConst.Table.Items.Add(item.Name);
                Cm.Filling.IntIdRand.Table.Items.Add(item.Name);
            }

            switch (col.Filling.Id)
            {
                case 1:

                    if (col.Settings.IntConst != null)
                    {
                        Cm.Filling.IntConst.Value.Value = col.Settings.IntConst.Value;
                    }
                    break;

                case 2:

                    if (col.Settings.IntRandFrom != null)
                    {
                        Cm.Filling.IntRand.ValueFrom.Value = col.Settings.IntRandFrom.Value;
                    }

                    if (col.Settings.IntRandTo != null)
                    {
                        Cm.Filling.IntRand.ValueTo.Value = col.Settings.IntRandTo.Value;
                    }

                    switch (col.Settings.IntRandSort)
                    {
                        case SortEnum.Defoult: Cm.Filling.IntRand.R1.Checked = true; break;
						case SortEnum.Increasing: Cm.Filling.IntRand.R2.Checked = true; break;
						case SortEnum.Descending: Cm.Filling.IntRand.R3.Checked = true; break;
					}
                    break;

                case 3:

                    if (col.Settings.IntIdConstTable != null)
                    {
                        var t = TableHelper.GetTableById(col.Settings.IntIdConstTable.Value);
                        SystemHelper.ComboBoxSelect(Cm.Filling.IntIdConst.Table, t.Name);
                        if (col.Settings.IntIdConstColumn != null)
                        {
                            var c = TableHelper.GetColumnsById(col.Settings.IntIdConstColumn.Value, t.Id);
                            SystemHelper.ComboBoxSelect(Cm.Filling.IntIdConst.Column, c.Name);
                        }
                    }

                    break;

                case 4:

                    if (col.Settings.IntIdRandTable != null)
                    {
                        var t = TableHelper.GetTableById(col.Settings.IntIdRandTable.Value);
                        SystemHelper.ComboBoxSelect(Cm.Filling.IntIdRand.Table, t.Name);

                        if (col.Settings.IntIdRandColumn != null)
                        {
                            foreach (var item in t.Columns)
                            {
                                Cm.Filling.IntIdRand.Column.Items.Add(item.Name);
                            }

                            var c = TableHelper.GetColumnsById(col.Settings.IntIdRandColumn.Value, t.Id);
                            SystemHelper.ComboBoxSelect(Cm.Filling.IntIdRand.Column, c.Name);
                        }
                    }

                    break;

                case 5:

                    if (!string.IsNullOrEmpty(col.Settings.StringConst))
                    {
                        Cm.Filling.StringConst.Value.Text = col.Settings.StringConst;
                    }
                    break;

                case 6:

                    var fileList = User.Files.Where(f => f.Type == Enums.FileTypeEnum.String);

                    foreach (var file in fileList)
                    {
                        Cm.Filling.StringFile.File.Items.Add(file.Name);
                    }

                    if(col.Settings.StringFile != 0)
                    {
                        var file1 = FileHelper.GetFileById(col.Settings.StringFile);
                        SystemHelper.ComboBoxSelect(Cm.Filling.StringFile.File, file1.Name);
                    }

					switch (col.Settings.StringFileSort)
					{
						case SortEnum.Defoult: Cm.Filling.StringFile.R1.Checked = true; break;
						case SortEnum.Increasing: Cm.Filling.StringFile.R2.Checked = true; break;
						case SortEnum.Descending: Cm.Filling.StringFile.R3.Checked = true; break;
					}

					break;

                case 7:

                    if (col.Settings.FloatConst != null)
                    {
                        Cm.Filling.FloatConst.Value.Value = (decimal)col.Settings.FloatConst.Value;
                    }
                    break;

                case 8:

                    if (col.Settings.FloatRandFrom != null)
                    {
                        Cm.Filling.FloatRand.ValueFrom.Value = (decimal)col.Settings.FloatRandFrom.Value;
                    }

                    if (col.Settings.FloatRandTo != null)
                    {
                        Cm.Filling.FloatRand.ValueTo.Value = (decimal)col.Settings.FloatRandTo.Value;
                    }

					switch (col.Settings.FloatRandSort)
					{
						case SortEnum.Defoult: Cm.Filling.FloatRand.R1.Checked = true; break;
						case SortEnum.Increasing: Cm.Filling.FloatRand.R2.Checked = true; break;
						case SortEnum.Descending: Cm.Filling.FloatRand.R3.Checked = true; break;
					}

					break;

                case 9:

                    if (col.Settings.DateConst != null)
                    {
                        Cm.Filling.DateConst.Value.Value = col.Settings.DateConst.Value;
                    }

                    break;

                case 10:

                    if (col.Settings.DateRandFrom != null)
                    {
                        Cm.Filling.DateRand.ValueFrom.Value = col.Settings.DateRandFrom.Value;
                    }

                    if (col.Settings.DateRandTo != null)
                    {
                        Cm.Filling.DateRand.ValueTo.Value = col.Settings.DateRandTo.Value;
                    }

					switch (col.Settings.DateRandSort)
					{
						case SortEnum.Defoult: Cm.Filling.DateRand.R1.Checked = true; break;
						case SortEnum.Increasing: Cm.Filling.DateRand.R2.Checked = true; break;
						case SortEnum.Descending: Cm.Filling.DateRand.R3.Checked = true; break;
					}

					break;

                case 11:

                    var fileList2 = User.Files.Where(f => f.Type == Enums.FileTypeEnum.Image);

                    foreach (var file in fileList2)
                    {
                        Cm.Filling.ImgConst.File.Items.Add(file.Name);
                    }

                    if(col.Settings.ImgConst != 0)
                    {
                        var file1 = FileHelper.GetFileById(col.Settings.ImgConst);
                        SystemHelper.ComboBoxSelect(Cm.Filling.ImgConst.File, file1.Name);
                    }
                    break;

                case 12:

                    var fileList3 = User.Files.Where(f => f.Type == Enums.FileTypeEnum.ImageList);

                    foreach (var file in fileList3)
                    {
                        Cm.Filling.ImgFile.File.Items.Add(file.Name);
                    }

                    if(col.Settings.ImgRand != 0)
                    {
                        var file1 = FileHelper.GetFileById(col.Settings.ImgRand);
                        SystemHelper.ComboBoxSelect(Cm.Filling.ImgFile.File, file1.Name);
                    }
                    break;

                case 13:

                    if (col.Settings.BoolConst != null)
                    {
                        Cm.Filling.BoolConst.Value.Checked = col.Settings.BoolConst.Value;
                    }
                    break;

            }

            foreach (var item in col.Settings.ConditionList)
            {
                Cm.Filling.ConditionList.Items.Add(item.Text);
            }

        }

        /// <summary>
        /// Сохрание данных с формы в Автозаполнении
        /// </summary>
        /// <returns> 
        /// При ошибке - false, в остальных случаях - true
        /// </returns>
        private static bool UpdateFillingValue()
        {
            var table = TableHelper.GetTableById(User.SelectedTableId);
            var col = TableHelper.GetColumnsById(User.SelectedColumnId , table.Id);

            switch (col.Filling.Id)
            {
                case 1:

                   if(Cm.Filling.IntConst.Value.Value != 0)
                   {
                        if(ValueHelper.IsInt(Cm.Filling.IntConst.Value.Value))
                        {
                            col.Settings.IntConst = (int)Cm.Filling.IntConst.Value.Value;
                        }
                        else
                        {
                            ErrorHelper.CreateError("Значение должно быть целочисленным", "При настройки автозаполнения поля: " + col.Name + " указан не целочисленный тип");
                            return false;
                        }
                   }
                   break;

                case 2:

                    if (Cm.Filling.IntRand.ValueFrom.Value != 0)
                    {
                        if (ValueHelper.IsInt(Cm.Filling.IntRand.ValueFrom.Value))
                        {
                            col.Settings.IntRandFrom = (int)Cm.Filling.IntRand.ValueFrom.Value;
                        }
                        else
                        {
                            ErrorHelper.CreateError("Значение должно быть целочисленным", "При настройки автозаполнения поля: " + col.Name + " указан не целочисленный тип");
                            return false;
                        }

                    }

                    if (Cm.Filling.IntRand.ValueTo.Value != 0)
                    {
                        if (ValueHelper.IsInt(Cm.Filling.IntRand.ValueTo.Value))
                        {
                            col.Settings.IntRandTo = (int)Cm.Filling.IntRand.ValueTo.Value;
                        }
                        else
                        {
                            ErrorHelper.CreateError("Значение должно быть целочисленным", "При настройки автозаполнения поля: " + col.Name + " указан не целочисленный тип");
                            return false;
                        }
                    }

                    if (Cm.Filling.IntRand.R2.Checked)
                        col.Settings.IntRandSort = SortEnum.Increasing;
                    else if(Cm.Filling.IntRand.R3.Checked)
						col.Settings.IntRandSort = SortEnum.Descending;
                    else
						col.Settings.IntRandSort = SortEnum.Defoult;
					break;

                case 3:

                    if(Cm.Filling.IntIdConst.Table.Text.Length > 0)
                    {
                        var t = TableHelper.GetTableByName(Cm.Filling.IntIdConst.Table.Text);
                        col.Settings.IntIdConstTable = t.Id;

                        if(Cm.Filling.IntIdConst.Column.Text.Length > 0)
                        {
                            var c = TableHelper.GetColumnByName(Cm.Filling.IntIdConst.Column.Text, t.Name);

                            if(c.Type.Id > col.Type.Id)
                            {
                                ErrorHelper.CreateError("Вы не можете привязать данные поля, так как диапазон значений выбранного поля меньше, чем диапазон значений редактируемого поля", "Поле: " + col.Name + " с типом " + col.Type.Name + " не может свзяаться с полем " + c.Name + " типом " + c.Type.Name);
                                return false;
                            }

                            col.Settings.IntIdConstColumn = c.Id;
                        }
                    }
                    break;

                case 4:

                    if (Cm.Filling.IntIdRand.Table.Text.Length > 0)
                    {
                        var t = TableHelper.GetTableByName(Cm.Filling.IntIdRand.Table.Text);
                        col.Settings.IntIdRandTable = t.Id;

                        if (Cm.Filling.IntIdRand.Column.Text.Length > 0)
                        {
                            var c = TableHelper.GetColumnByName(Cm.Filling.IntIdRand.Column.Text, t.Name);

                            if (c.Type.Id > col.Type.Id)
                            {
                                ErrorHelper.CreateError("Вы не можете привязать данные поля, так как диапазон значений выбранного поля меньше, чем диапазон значений редактируемого поля", "Поле: " + col.Name + " с типом " + col.Type.Name + " не может свзяаться с полем " + c.Name + " типом " + c.Type.Name);
                                return false;
                            }

                            col.Settings.IntIdRandColumn = c.Id;
                        }
                    }
                    break;

                case 5:

                    if(!string.IsNullOrEmpty(Cm.Filling.StringConst.Value.Text))
                    {
                        col.Settings.StringConst = Cm.Filling.StringConst.Value.Text;
                    }
                    break;

                case 6:

                    if(!string.IsNullOrEmpty(Cm.Filling.StringFile.File.Text))
                    {
                        var file1 = FileHelper.GetFileByName(Cm.Filling.StringFile.File.Text);
                        col.Settings.StringFile = file1.Id;
                    }

					if (Cm.Filling.StringFile.R2.Checked)
						col.Settings.StringFileSort = SortEnum.Increasing;
					else if (Cm.Filling.StringFile.R3.Checked)
						col.Settings.StringFileSort = SortEnum.Descending;
					else
						col.Settings.StringFileSort = SortEnum.Defoult;
					break;

                case 7:
                    
                    if(Cm.Filling.FloatConst.Value.Value != 0)
                    {
                        col.Settings.FloatConst = (double)Cm.Filling.FloatConst.Value.Value;
                    }
                    break;

                case 8:

                    if (Cm.Filling.FloatRand.ValueFrom.Value != 0)
                    {
                        col.Settings.FloatRandFrom = (double)Cm.Filling.FloatRand.ValueFrom.Value;
                    }

                    if (Cm.Filling.FloatRand.ValueTo.Value != 0)
                    {
                        col.Settings.FloatRandTo = (double)Cm.Filling.FloatRand.ValueTo.Value;
                    }

					if (Cm.Filling.FloatRand.R2.Checked)
						col.Settings.FloatRandSort = SortEnum.Increasing;
					else if (Cm.Filling.FloatRand.R3.Checked)
						col.Settings.FloatRandSort = SortEnum.Descending;
					else
						col.Settings.FloatRandSort = SortEnum.Defoult;
					break;

                case 9:

                    col.Settings.DateConst = Cm.Filling.DateConst.Value.Value;

                    break;

                case 10:

                    col.Settings.DateRandFrom = Cm.Filling.DateRand.ValueFrom.Value;
                    col.Settings.DateRandTo = Cm.Filling.DateRand.ValueTo.Value;

					if (Cm.Filling.DateRand.R2.Checked)
						col.Settings.DateRandSort = SortEnum.Increasing;
					else if (Cm.Filling.DateRand.R3.Checked)
						col.Settings.DateRandSort = SortEnum.Descending;
					else
						col.Settings.DateRandSort = SortEnum.Defoult;
					break;

                case 11:

                    if (!string.IsNullOrEmpty(Cm.Filling.ImgConst.File.Text))
                    {
                        var file1 = FileHelper.GetFileByName(Cm.Filling.ImgConst.File.Text);
                        col.Settings.ImgConst = file1.Id;
                    }

                    break;

                case 12:

                    if (!string.IsNullOrEmpty(Cm.Filling.ImgFile.File.Text))
                    {
                        var file1 = FileHelper.GetFileByName(Cm.Filling.ImgFile.File.Text);
                        col.Settings.ImgRand = file1.Id;
                    }

                    break;

                case 13:

                    col.Settings.BoolConst = Cm.Filling.BoolConst.Value.Checked;
                    break;

            }

            table.Count = Convert.ToInt32(Cm.Filling.CountText.Text);
            return true;
        }

        /// <summary>
        /// Вывод полей при типе заполнения "Id Ключ" в Автозаполнении
        /// </summary>
        /// <param name = "st"> 1 - Постоянный ключ, 2 - Рандомный ключ </param>
        public static void PullColumnsListIdFilling(byte st)
        { 
            var colEdit = TableHelper.GetColumnsById(User.SelectedColumnId , User.SelectedTableId);
            Table table = null;

            if(st == 1 && Cm.Filling.IntIdConst.Table.Text.Length > 0)
            {
                table = TableHelper.GetTableByName(Cm.Filling.IntIdConst.Table.Text);
                Cm.Filling.IntIdConst.Column.Items.Clear();
            }

            else if(Cm.Filling.IntIdRand.Table.Text.Length > 0)
            {
                table = TableHelper.GetTableByName(Cm.Filling.IntIdRand.Table.Text);
                Cm.Filling.IntIdRand.Column.Items.Clear();
            }

            var colList = table.Columns
                .Where(c => c.Type.Type == colEdit.Type.Type);

            foreach (var item in colList)
            {
                if(st == 1)
                    Cm.Filling.IntIdConst.Column.Items.Add(item.Name);
                else if(st == 2)
                    Cm.Filling.IntIdRand.Column.Items.Add(item.Name);
            }
        }

		#endregion

		#region Условия

        /// <summary>
        /// Добавление условия в настройки автозаполнения колонки
        /// </summary>
        public static void AddConditionInList()
        {
            int errorRow = -2;
			var words = Cm.Filling.ConditionText.Text.Split(' ');
			var table = TableHelper.GetTableById(User.SelectedTableId);
			var col = TableHelper.GetColumnsById(User.SelectedColumnId, table.Id);

			var cont = GetCondition(Cm.Filling.ConditionText.Text , col , table, out errorRow);

			if (cont != null && CheckCondition(cont, col, table))
            {
				if (SystemHelper.ListBoxSearch(Cm.Filling.ConditionList, Cm.Filling.ConditionText.Text) != -1)
				{
					ErrorHelper.CreateError("Данное условие уже было примененно к данному полю");
				}
                else
                {
                    var colm = TableHelper.GetColumnsById(User.SelectedColumnId, User.SelectedTableId);
                    colm.Settings.ConditionList.Add(cont);
					Cm.Filling.ConditionList.Items.Add(Cm.Filling.ConditionText.Text);
					Cm.Filling.ConditionText.Clear();
				}
			}
            else
            {
				if (errorRow > -1)
				{
					ErrorHelper.CreateError($"Не удалось прочитать строку условия. Примерно место ошибки: {words[errorRow]}", $"Таблица {table.Name} Поле {col.Name}");
				}
                else if(errorRow == -1)
                {
					ErrorHelper.CreateError("Не удалось прочитать строку условия");
				}
			}

		}

        /// <summary>
        /// Получение модели условия из строки условия
        /// </summary>
        /// <param name="text">Строка условия</param>
        /// <param name="col">Колонка условия</param>
        /// <param name="table">Таблица колонки условия</param>
        /// <param name="IsError">Строка ошибки при чтении строки условия</param>
        /// <returns>Объект условия</returns>
        public static Condition GetCondition(string text , Column col , Table table , out int IsError)
        {
            if(string.IsNullOrEmpty(text))
            {
                IsError = -1;
                return null;
            }

            var condition = new Condition();
            var words = text.Split(' ');
            var i = 0;

            // 1 слово(Esc)

            if (words[i].ToLower().Equals("esc") && col.Settings.ConditionList.Any())
            {
                if(col.Settings.ConditionList.Count - 1 == -1)
                {
                    IsError = i;
                    return null;
                }
                else
                {
                    col.Settings.ConditionList[col.Settings.ConditionList.Count - 1].IdConditionNext = condition.Id;
                    i++;
                }
                
            }
            else if(words[i].ToLower().Equals("esc"))
            {
                IsError = i;
				return null;
			}

            // 1 слово (столбец)

            if(words.Length > i)
            {
                var column = TableHelper.GetColumnByName(words[i], table.Name);

                if (column != null)
                {
                    condition.ColumnIdIn = column.Id;
                }
                else
                {
                    IsError = i;
					return null;
				}
            }

            i++;

            // 2 слово (знак)

            if (words.Length > i)
            {

                if (words[i].Equals(">"))
                {
                    condition.Sign = ">";
                }
                else if (words[i].Equals("<"))
                {
                    condition.Sign = "<";
                }
                else if (words[i].Equals("=="))
                {
                    condition.Sign = "==";
                }
                else
                {
					IsError = i;
					return null;
				}
					
			}

            i++;

            // 3 слово (столбец)

            if (words.Length > i && TableHelper.GetColumnByName(words[i], table.Name) != null)
            {
                condition.ColumnIdOut = TableHelper.GetColumnByName(words[i], table.Name).Id;
            }

			// 3 слово (значение)

			else if (words.Length > i)
			{
				condition.ValueOut = words[i];
			}

			else 
            {
				IsError = i;
				return null;
			}

			i++;

            // 4 слово ( = )

            if(!(words.Length > i && words[i].Equals("=")))
            {
				IsError = i;
				return null;
			}

            i++;

            var word = string.Empty;

            for(int j = i; j < words.Length; j++)
            {
                word += words[j];

                if(j + 1 != words.Length)
                    word += " ";
            }

			
            // 5 слово (столбец)

            if(words.Length > i && TableHelper.GetColumnByName(words[i] , table.Name) != null)
            {
                condition.ColumnIdValue = TableHelper.GetColumnByName(words[i], table.Name).Id;
			}

            // 5 слово (файл)

            else if(words.Length > i && FileHelper.GetFileByName(word) != null)
            {
                condition.IdFile = FileHelper.GetFileByName(word).Id;
            }

			// 5 слово (значение)

			else if (words.Length > i)
			{
				condition.Value = words[i];
			}

			else
            {
				IsError = i;
				return null;
			}

            IsError = -2;

            condition.Text = text;
			condition.TableId = table.Id;
			return condition;
            
		}

        /// <summary>
        /// Проверка условия
        /// </summary>
        /// <param name="condition">Объект условия</param>
        /// <param name="col">Колонка условия</param>
        /// <param name="tbl">Таблица колонки условия</param>
        /// <returns>false - при ошибке</returns>
        private static bool CheckCondition(Condition condition, Column col, Table tbl)
        {
            if (!string.IsNullOrEmpty(condition.ValueOut))
            {
                bool IsError = false;
                var column = TableHelper.GetColumnsById(condition.ColumnIdIn, tbl.Id);

                switch (column.Type.Type)
                {
                    case Models.Enums.TypeEnum.Number:
                        if (!ValueHelper.IsInt(condition.ValueOut))
                        {
                            IsError = true;
                        }
                        break;

                    case Models.Enums.TypeEnum.Float:
                        if (!ValueHelper.IsFloat(condition.ValueOut))
                        {
                            IsError = true;
                        }
                        break;

                    case Models.Enums.TypeEnum.Date:
                        if (!ValueHelper.IsDate(condition.ValueOut))
                        {
                            IsError = true;
                        }
                        break;

                    case Models.Enums.TypeEnum.Bool:
                        if (!ValueHelper.IsBool(condition.ValueOut))
                        {
                            IsError = true;
                        }
                        break;

                }

                if (IsError)
                {
                    MessageHelper.ErrorMessage($"Значение {condition.ValueOut} невозможно ставнить с полем {column.Name}, т.к оно другого типа данных");
                    return false;
                }
            }

            else if (condition.ColumnIdOut != 0 && TableHelper.GetColumnsById(condition.ColumnIdIn, tbl.Id).Type.Type != TableHelper.GetColumnsById(condition.ColumnIdOut, tbl.Id).Type.Type)
            {
                ErrorHelper.CreateError($"Значение колонки {TableHelper.GetColumnsById(condition.ColumnIdOut, tbl.Id).Name} невозможно записать в данное поле, т.к оно другого типа данных", $"Таблица {tbl.Name} Поле {col.Name}");
                return false;
            }

            if (!string.IsNullOrEmpty(condition.Value))
            {
                bool IsError = false;

                switch (col.Type.Type)
                {
                    case Models.Enums.TypeEnum.Number:
                        if (!ValueHelper.IsInt(condition.Value))
                        {
                            IsError = true;
                        }
                        break;

                    case Models.Enums.TypeEnum.Float:
                        if (!ValueHelper.IsFloat(condition.Value))
                        {
                            IsError = true;
                        }
                        break;

                    case Models.Enums.TypeEnum.Date:
                        if (!ValueHelper.IsDate(condition.Value))
                        {
                            IsError = true;
                        }
                        break;

                    case Models.Enums.TypeEnum.Bool:
                        if (!ValueHelper.IsBool(condition.Value))
                        {
                            IsError = true;
                        }
                        break;

                }

                if (IsError)
                {
                    MessageHelper.ErrorMessage($"Значение {condition.Value} невозможно присвоить данному полю, т.к оно другого типа данных");
                    return false;
                }

            }
            
            else if (condition.ColumnIdValue != 0 && col.Type.Type != TableHelper.GetColumnsById(condition.ColumnIdValue, tbl.Id).Type.Type)
            {
                ErrorHelper.CreateError($"Значение колонки {TableHelper.GetColumnsById(condition.ColumnIdValue, tbl.Id).Name} невозможно записать в данное поле, т.к оно другого типа данных", $"Таблица {tbl.Name} Поле {col.Name}");
                return false;
            }

            if(condition.IdFile != 0)
            {
                bool IsError = false;   

                switch (FileHelper.GetFileById(condition.IdFile).Type)
                {
                    case FileTypeEnum.String:
                        if (col.Type.Type != Models.Enums.TypeEnum.String)
                            IsError = true;
                        break;
                    case FileTypeEnum.Image:
						if (col.Type.Type != Models.Enums.TypeEnum.Image)
							IsError = true;
						break;
                    case FileTypeEnum.ImageList:
						if (col.Type.Type != Models.Enums.TypeEnum.Image)
							IsError = true;
						break;
                }

                if(IsError)
                {
                    ErrorHelper.CreateError($"Невозможно присвоить файл {FileHelper.GetFileById(condition.IdFile).Name} в это поле");
                    return false;
                }

            }

            if((col.Filling.Id == 6 || col.Filling.Id == 11 || col.Filling.Id == 12) && condition.IdFile == 0)
            {
                ErrorHelper.CreateError("Имя файло было не опознанно или опознанно не правильно");
                return false;
            }
           
            return true;
        }

        /// <summary>
        /// Удаление условия
        /// </summary>
        public static void DeleteCondition()
        {
            if(Cm.Filling.ConditionList.SelectedIndex != -1 && MessageHelper.QueMessage("Удалить условие?"))
            {
                var table = TableHelper.GetTableById(User.SelectedTableId);
                var col = TableHelper.GetColumnsById(User.SelectedColumnId, table.Id);

                var con = col.Settings.ConditionList.FirstOrDefault(c => c.Text.ToLower().Equals(Cm.Filling.ConditionList.Items[Cm.Filling.ConditionList.SelectedIndex].ToString().ToLower()));
                col.Settings.ConditionList.Remove(con);

                Cm.Filling.ConditionList.Items.Clear();

                foreach (var item in col.Settings.ConditionList)
                {
                    Cm.Filling.ConditionList.Items.Add(item.Text);
                }
            }
            else
            {
                ErrorHelper.CreateError("Не выбрано ни одно условие", "При удалении условия пользователь не выбрал условие");
            }
        }

		#endregion

		#region Проверка таблиц

        /// <summary>
        /// Проверка таблиц перед сохранием конфигурации или при заполнении БД
        /// </summary>
        /// <param name="IsConf">true - проверка для конфигурации  false - для проверки для заполнения БД</param>
		public static bool CheckTableList(bool IsConf)
        {
            Cm.StartPage.List.Items.Clear();
            Cm.StartPage.Text.Text = string.Empty;
            User.CreateTableErrors.Clear();

            SystemHelper.WriteInfoStartPage("Начало проверки");
            SystemHelper.WriteInfoStartPage("Старт...");

            if(!IsConf)
            {
                SystemHelper.WriteInfoStartPage("Обновление приоритета");
                TableHelper.ChangePriorityTable(User.Tables);
                SystemHelper.WriteInfoStartPage("Сортировка по приоритету");

                User.Tables = User.Tables.OrderByDescending(c => c.Priority).ToList();

                foreach (var table in User.Tables)
                {
                    table.Columns = table.Columns.OrderByDescending(c => c.Priority).ToList();
                }

                SystemHelper.WriteInfoStartPage("Обновления дерева по приоритету");
                SystemController.UpdateInfoTree();
            }
           
            SystemHelper.WriteInfoStartPage("Проверка списка таблиц");

            if (User.Tables.Count == 0 && IsConf)
            {
                CreateTableWarning("Список таблиц пуст");
            }
            else if(User.Tables.Count == 0 && !IsConf)
            {
                CreateTableError("Список таблиц пуст", 0, 0, 0, CreateTableErrorTypeEnum.None);
            }
            else
            {
                SystemHelper.WriteInfoStartPage($"Количество таблиц: " + User.Tables.Count.ToString());
            }

            SystemHelper.WriteInfoStartPage("Проверка списка файлов");

            if (User.Files.Count == 0)
            {
                CreateTableWarning("Список файлов пуст");
            }
            else
            {
                SystemHelper.WriteInfoStartPage($"Количество файлов: " + User.Files.Count.ToString());
            }

            SystemHelper.WriteInfoStartPage("Старт проверки таблиц");

            foreach (var table in User.Tables)
            {
                int countFillingColumn = table.Columns.Count(c => c.Filling.Id != 0);

                if (table.Name.Length == 0)
                {
                    CreateTableError("Одна из таблиц не имеет название" , table.Id , 0 , 0 , CreateTableErrorTypeEnum.TableName);
                    continue;
                }
                else
                    SystemHelper.WriteInfoStartPage($"Таблица {table.Name}: Проверка на название - Успешно");

                SystemHelper.WriteInfoStartPage($"Таблица {table.Name}: Проверка на количество строк");

                var collist = table.Columns.Count(c => c.Filling.Id != 0);

                if (collist != 0 && table.Count == 0)
                {
                    CreateTableError($"Таблица {table.Name}: Не указано число строк", table.Id, 0, 0, CreateTableErrorTypeEnum.TableCount);
                }
                else if(collist == 0 && table.Count > 0)
                {
                    CreateTableError($"Таблица {table.Name}: Таблица без настроек автозаполнения имеет количество строк", table.Id, 0, 0, CreateTableErrorTypeEnum.TableCount);
                }
               
                collist = table.Columns.Count(c => c.IsPrimaryKey == true);

                SystemHelper.WriteInfoStartPage($"Таблица {table.Name}: Проверка на ключевое поле");

                if (collist > 1)
                {
                    CreateTableError($"Таблица {table.Name}: Некорректное ключевое поле", table.Id, 0, 0, CreateTableErrorTypeEnum.TableKey);
                }
                else
                {
                    var col = table.Columns.FirstOrDefault(c => c.IsPrimaryKey == true);

                    if (col != null && col.Type.Type != Models.Enums.TypeEnum.Number)
                    {
                        CreateTableError($"Таблица {table.Name}: Некорректное ключевое поле", table.Id, 0, 0, CreateTableErrorTypeEnum.TableKey);
                    }
                    else
                        SystemHelper.WriteInfoStartPage($"Таблица {table.Name}: Проверка на ключевое поле - Успешно");
                }

                SystemHelper.WriteInfoStartPage($"Таблица {table.Name}: Проверка на поля");

                foreach (var col in table.Columns)
                {
                    SystemHelper.WriteInfoStartPage($"Таблица {table.Name}: Проверка на название поля");

                    if(col.Name.Length == 0)
                    {
                        CreateTableError($"Таблица {table.Name}: Поле не имеет названия", table.Id, col.Id, 0, CreateTableErrorTypeEnum.ColName);
                    }
                    else
                        SystemHelper.WriteInfoStartPage($"Таблица {table.Name} Поле: {col.Name}: Проверка на название - Успешно");

                    SystemHelper.WriteInfoStartPage($"Таблица {table.Name} Поле: {col.Name}: Проверка на Автозаполнение");

                    if(col.Filling.Id == 0)
                    {
                        if(col.IsNullable == false && countFillingColumn != 0 && !col.IsPrimaryKey)
                        {
                            CreateTableError($"Таблица {table.Name} Поле: {col.Name}: Невозможное значение null при отсутствии автозаполнения", table.Id, col.Id, 0, CreateTableErrorTypeEnum.ColIsNull);
                        }
                        else
                        {
                            SystemHelper.WriteInfoStartPage($"Таблица {table.Name} Поле: {col.Name}: Проверка на Автозаполнение - Не требуется");
                        }
                       
                    }
                    else
                    {
                        bool isError = false;
                        switch (col.Filling.Id)
                        {
                            case 1:
                                if (!col.Settings.IntConst.HasValue)
                                    isError = true;
                                break;

                            case 2:
                                if (!col.Settings.IntRandFrom.HasValue || !col.Settings.IntRandTo.HasValue)
                                    isError = true;
                                else if(col.Settings.IntRandFrom.Value >= col.Settings.IntRandTo.Value)
                                    isError = true;
                                break;

                            case 3:
                                if ((!col.Settings.IntIdConstTable.HasValue || col.Settings.IntIdConstTable == 0) || (!col.Settings.IntIdConstColumn.HasValue || col.Settings.IntIdConstColumn == 0))
                                    isError = true;
                                break;

                            case 4:
                                if ((!col.Settings.IntIdRandTable.HasValue || col.Settings.IntIdRandTable == 0) || (!col.Settings.IntIdRandColumn.HasValue || col.Settings.IntIdRandColumn == 0))
                                    isError = true;
                                break;

                            case 5:
                                if (string.IsNullOrEmpty(col.Settings.StringConst))
                                    isError = true;
                                break;

                            case 6:
                                if (col.Settings.StringFile == 0)
                                    isError = true;
                                break;

                            case 7:
                                if (!col.Settings.FloatConst.HasValue)
                                    isError = true;
                                break;

                            case 8:
                                if (!col.Settings.FloatRandFrom.HasValue || !col.Settings.FloatRandTo.HasValue)
                                    isError = true;
                                else if (col.Settings.FloatRandFrom.Value >= col.Settings.FloatRandTo.Value)
                                    isError = true;
                                break;

                            case 9:
                                if (!col.Settings.DateConst.HasValue)
                                    isError = true;
                                break;

                            case 10:
                                if (!col.Settings.DateRandFrom.HasValue || !col.Settings.DateRandTo.HasValue)
                                    isError = true;
                                else if (col.Settings.DateRandFrom.Value.Ticks >= col.Settings.DateRandTo.Value.Ticks)
                                    isError = true;
                                break;

                            case 11:
                                if (col.Settings.ImgConst == 0)
                                    isError = true;
                                break;

                            case 12:
                                if (col.Settings.ImgRand == 0)
                                    isError = true;
                                break;

                            case 13:
                                if (!col.Settings.BoolConst.HasValue)
                                    isError = true;
                                break;

                        }

                        if(isError)
                        {
                            CreateTableError($"Таблица {table.Name} Поле: {col.Name}: При типе автозаполнения {col.Filling.Name} не указано значение или указано некорректно", table.Id, col.Id, 0, CreateTableErrorTypeEnum.ColFilling);
                        }
                        else
                        {
                            SystemHelper.WriteInfoStartPage($"Таблица {table.Name} Поле: {col.Name}: Проверка на Автозаполнение - Успешно");
                        }
                        
                    }

                }
                
            }

            SystemHelper.WriteInfoStartPage("Старт проверки файлов");

            foreach (var file in User.Files)
            {
                bool fileError = false;

                SystemHelper.WriteInfoStartPage("Проверка файла на название");

                if(file.Name.Length == 0)
                {
                    CreateTableError("Файл не имеет названия", 0, 0, file.Id, CreateTableErrorTypeEnum.FileName);
                }
                else
                    SystemHelper.WriteInfoStartPage($"Файл {file.Name}: Проверка на название - Успешно");

                SystemHelper.WriteInfoStartPage($"Файл {file.Name}: Проверка на содержимое");

                if(file.FileName.Length == 0 && file.FileNames.Count == 0 && !IsConf)
                {
                    CreateTableError($"Файл {file.Name}: Пустой файл", 0, 0, file.Id, CreateTableErrorTypeEnum.FileNull);
                    fileError = true;
                }
                else
                    SystemHelper.WriteInfoStartPage($"Файл {file.Name}: Проверка на содержимое - Успешно");

                var count = User.Files.Count(f => f.FileName.Equals(file.FileName));

                if(count > 1 && file.FileName.Length > 0)
                {
                    CreateTableWarning($"Файл {file.Name}: Данный файл уже используется в другом файле");
                }

                if(!fileError)
                {
                    foreach (var table in User.Tables)
                    {
                        int countFil = int.MaxValue;

                        foreach (var item in table.Columns)
                        {
                            if (item.Filling.Id == 6)
                            {
                                var count1 = File.ReadLines(FileHelper.GetFileById(item.Settings.StringFile).FileName).Count();

                                if (count1 < countFil)
                                {
                                    countFil = count1;
                                }
                            }
                            else if (item.Filling.Id == 12)
                            {
                                var count1 = FileHelper.GetFileById(item.Settings.ImgRand).FileNames.Count;

                                if (count1 < countFil)
                                {
                                    countFil = count1;
                                }
                            }
                        }

                        if (countFil < table.Count)
                        {
                            CreateTableError($"Таблица {table.Name}: Число строк в таблице больше, чем расчитано в автозаполнении", table.Id, 0, 0, CreateTableErrorTypeEnum.TableCount);
                        }
                    }
                }


            }

            return WriteCheckTableListError();
        }

		/// <summary>
		/// Вывод списка ошибок и предупреждений при проверке перед сохранием конфигурации или при заполнении БД
		/// </summary>
		public static bool WriteCheckTableListError()
        {
            bool result = false;

            SystemHelper.WriteInfoStartPage("");
            SystemHelper.WriteInfoStartPage("Список ошибок: ");
            var errorList = User.CreateTableErrors.Where(c => c.IsError == true).ToList();

            if (errorList.Count == 0)
            {
                SystemHelper.WriteInfoStartPage("Пусто");
                result = true;
            }
            else
            {
                for (int i = 0; i < errorList.Count; i++)
                {
                    SystemHelper.WriteInfoStartPage($"{errorList[i].Text}");
                }
            }

            SystemHelper.WriteInfoStartPage("");
            errorList = User.CreateTableErrors.Where(c => c.IsError == false).ToList();
            SystemHelper.WriteInfoStartPage("Список предупреждений: ");

            if (errorList.Count == 0)
                SystemHelper.WriteInfoStartPage("Пусто");
            else
            {
                for (int i = 0; i < errorList.Count; i++)
                {
                    SystemHelper.WriteInfoStartPage($"{i + 1}. {errorList[i].Text}");
                }
            }

            return result;

        }

        /// <summary>
        /// Создание предупреждения при проверке таблиц
        /// </summary>
        /// <param name="text">Строка ошибки</param>
        private static void CreateTableWarning(string text)
        {
            ErrorHelper.CreateTableWarning(text);
            string text1 = "Предупреждение: " + text;
            SystemHelper.WriteInfoStartPage(text);
        }

        /// <summary>
        /// Создание ошибки при проверке таблиц
        /// </summary>
        /// <param name="text">Строка ошибки</param>
        /// <param name="tableId">Индетификатор таблицы ошибки</param>
        /// <param name="columnId">Индетификатор колонки ошибки</param>
        /// <param name="fileId">Индентификатор файла ошибки</param>
        /// <param name="type">Тип ошибки</param>
        private static void CreateTableError(string text, int tableId, int columnId, int fileId, CreateTableErrorTypeEnum type)
        {
            ErrorHelper.CreateTableError(text, tableId, columnId, fileId, type);
            string text1 = "Ошибка: " + text;
            SystemHelper.WriteInfoStartPage(text);
        }

        /// <summary>
        /// Предварительная проверка перед стартом задачи. Так же запускает общую проверку
        /// </summary>
        /// <returns>true - Проверка пройдена</returns>
        public static bool CheckTable()
        {
            bool IsConf = true;
            bool IsCheck = true;

            if(Cm.StartPage.CreateTableCheck.Checked || Cm.StartPage.FillingCheck.Checked)
            {
                if(string.IsNullOrEmpty(User.StringConnection) && !Cm.StartPage.QueryText.Checked)
                {
                    MessageHelper.ErrorMessage("Заполните строку подключения");
                    return false;
                }
                IsCheck = false;
                IsConf = false;
            }

            if(Cm.StartPage.LoadConfCheck.Checked)
            {
                if(string.IsNullOrEmpty(User.ConfPath1))
                {
                    MessageHelper.ErrorMessage("Заполните файл для загрузки конфигурации");
                    return false;
                }
            }

            if (Cm.StartPage.SaveConfCheck.Checked)
            {
                if (string.IsNullOrEmpty(User.ConfPath2))
                {
                    MessageHelper.ErrorMessage("Заполните файл для сохранения конфигурации");
                    return false;
                }

                IsCheck = false;
            }

            if (IsCheck)
            {
                SystemHelper.WriteInfoStartPage("Проверка не требуется");
                return true;
            }
            else
            {
                return CheckTableList(IsConf);
            }
        }

        /// <summary>
        /// Исправление ошибок в таблицах
        /// </summary>
        /// <param name="text">Текст ошибки</param>
        public static void CorrectionTableError(string text)
        {
            var error = User.CreateTableErrors.FirstOrDefault(c => c.Text.Equals(text) && c.IsError);

            if(error != null)
            {
                User.CorrectionErrorStatus = true;

                switch (error.Type)
                {
                    case CreateTableErrorTypeEnum.None:
                        break;
                    case CreateTableErrorTypeEnum.TableName:
                        NavigationHelper.UpdateTable(error.TableId, true);
                        break;
                    case CreateTableErrorTypeEnum.TableCount:
                        NavigationHelper.Filling(error.TableId, true);
                        break;
                    case CreateTableErrorTypeEnum.TableKey:
                        NavigationHelper.UpdateTable(error.TableId, true);
                        break;
                    case CreateTableErrorTypeEnum.ColName:
                        NavigationHelper.UpdateTable(error.TableId, true);
                        break;
                    case CreateTableErrorTypeEnum.ColFilling:
                        NavigationHelper.Filling(error.TableId, true);
                        break;
                    case CreateTableErrorTypeEnum.ColIsNull:
                        NavigationHelper.UpdateTable(error.TableId, true);
                        break;
                    case CreateTableErrorTypeEnum.FileName:
                        NavigationHelper.EditFile(error.FileId, true);
                        break;
                    case CreateTableErrorTypeEnum.FileNull:
                        NavigationHelper.EditFile(error.FileId, true);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

    }

}
