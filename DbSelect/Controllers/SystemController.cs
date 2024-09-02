using DbSelect.Helpers;
using DbSelect.Models.StaticModels;
using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Controllers
{
    public class SystemController
    {
        /// <summary>
        /// Обновления данных о таблицах в TreeView
        /// </summary>
        public static void UpdateInfoTree()
        {
            TreeView tree = Cm.System.Menu;
            ContextMenuStrip tables = Cm.System.MenuTables;
            ContextMenuStrip table = Cm.System.MenuTable;

            tree.Nodes.Clear();

            var nodeTables = new TreeNode();
            nodeTables.Text = "Таблицы";
            nodeTables.ImageIndex = 0;

            foreach (var item in User.Tables)
            {
                var nodeTable = new TreeNode();
                nodeTable.Text = item.Name;
                nodeTable.ContextMenuStrip = table;
                nodeTable.ImageIndex = 1;

                foreach (var item1 in item.Columns)
                {
                    nodeTable.Nodes.Add(item1.Name);
                }

                nodeTables.Nodes.Add(nodeTable);
            }

            nodeTables.ContextMenuStrip = tables;
            tree.Nodes.Add(nodeTables);

            var nodeFiles = new TreeNode();
            nodeFiles.Text = "Файлы";
            nodeFiles.ContextMenuStrip = Cm.System.MenuFiles;
            nodeFiles.ImageIndex = 2;

            foreach (var item in User.Files.OrderBy(f => f.Type).OrderBy(f => f.Name))
            {
                var node2 = new TreeNode();
                node2.Text = item.Name;
                node2.ContextMenuStrip = Cm.System.MenuFile;

                switch (item.Type)
                {
                    case Enums.FileTypeEnum.String:
                        node2.ImageIndex = 3;
                        break;
                    case Enums.FileTypeEnum.Image:
                        node2.ImageIndex = 4;
                        break;
                    case Enums.FileTypeEnum.ImageList:
                        node2.ImageIndex = 5;
                        break;
                }

                if(string.IsNullOrEmpty(item.FileName) && !item.FileNames.Any())
                {
                    node2.ForeColor = Color.Gray;
                }

                nodeFiles.Nodes.Add(node2);
            }

            tree.Nodes.Add(nodeFiles);

            tree.ExpandAll();

        }

        /// <summary>
        /// Запуск создания таблицы без проверки на сохрание при переходе.
        /// </summary>
        public static void CreateTable()
        {
            if(TableController.UpdateTable(false))
            {
                if(!User.CorrectionErrorStatus)
                {
                    NavigationHelper.StartWindow(true);
                }
                else
                {
                    User.CorrectionErrorStatus = false;
                    NavigationHelper.StartA(true);
                }
            }
        }

        /// <summary>
        /// Запуск обновления таблицы без проверки на сохрание при переходе.
        /// </summary>
        public static void UpdateTable()
        {
            if (TableController.UpdateTable(true))
            {
                if (!User.CorrectionErrorStatus)
                {
                    NavigationHelper.StartWindow(true);
                }
                else
                {
                    User.CorrectionErrorStatus = false;
                    NavigationHelper.StartA(true);
                }
            }
        }

        /// <summary>
        /// Запуск удаления таблицы
        /// </summary>
        public static void DeleteTable()
        {
            if(MessageHelper.QueMessage("Вы действительно хотите удалить таблицу?") && TableController.DeleteTable())
            {
                UpdateInfoTree();
            }

        }

        /// <summary>
        /// Создание файла типа Тексовый
        /// </summary>
        public static void CreateFileString()
        {
            if(FileController.FileStringSave())
            {
                if (!User.CorrectionErrorStatus)
                {
                    NavigationHelper.StartWindow(true);
                }
                else
                {
                    User.CorrectionErrorStatus = false;
                    NavigationHelper.StartA(true);
                }
            }
        }

        /// <summary>
        /// Создание файла типа Картинка
        /// </summary>
        public static void CreateFileImg() 
        {
            if(FileController.FileImgSave())
            {
                if (!User.CorrectionErrorStatus)
                {
                    NavigationHelper.StartWindow(true);
                }
                else
                {
                    User.CorrectionErrorStatus = false;
                    NavigationHelper.StartA(true);
                }
            }
        }

        /// <summary>
        /// Создание файла Группа изображений
        /// </summary>
        public static void CreateFileGroup()
        {
            if(FileController.FileGroupSave())
            {
                if (!User.CorrectionErrorStatus)
                {
                    NavigationHelper.StartWindow(true);
                }
                else
                {
                    User.CorrectionErrorStatus = false;
                    NavigationHelper.StartA(true);
                }
            }
        }

        /// <summary>
        /// Отчистка всех компонентов
        /// </summary>
        public static void ClearAll()
        {
            TableController.ClearCreateTableForm();
            TableController.ClearFillingTabs();
            TableController.ClearFilling();

            FileController.FileStringClear();
            FileController.FileImageClear();
            FileController.FileGroupClear();
        }

        /// <summary>
        /// Проверка на сохранине, при переходе с одной формы на другую
        /// </summary>
        /// <returns> 
        /// При ошибке - false, в остальных случаях - true
        /// </returns>
        public static bool CheckSaveChanges()
        {
            if (User.SelectPage == Enums.PagesEnum.None)
                return true;

            var result = MessageHelper.QueMessageCancel("При выходе с данной формы возможно некоторые изменения не будут сохранены. Сохранить изменения?");

            if (result == DialogResult.No)
                return true;
            if (result == DialogResult.Cancel)
                return false;

            switch (User.SelectPage)
            {
                case Enums.PagesEnum.CreateTable:
                    return TableController.UpdateTable(false);
                case Enums.PagesEnum.UpdateTable:
                    return TableController.UpdateTable(true);
                case Enums.PagesEnum.Filling:
                    return TableController.CheckUpdateFilling();
                case Enums.PagesEnum.FileString:
                    return FileController.FileStringSave();
                case Enums.PagesEnum.FileImg:
                    return FileController.FileImgSave();
                case Enums.PagesEnum.FileImgList:
                    return FileController.FileGroupSave();
                    
                    
            }

            return false;
        }

        /// <summary>
        /// Изменения статуса админки пользователя
        /// </summary>
        public static void ChangeAdminStatus(bool IsAdmin)
        {
            User.IsAdmin = IsAdmin;

            Cm.System.ErrorList.Enabled = IsAdmin;
            Cm.System.AdminPanel.Enabled = IsAdmin;
            Cm.System.SaveConfig.Enabled = IsAdmin;

            Cm.StartPage.SaveConfCheck.Enabled = IsAdmin;
        }

        /// <summary>
        /// Вывод списка ошибок
        /// </summary>
        public static void ErrorListPull()
        {
            var d = Cm.System.ErrorListData;

            d.Rows.Clear();
            d.Columns.Clear();

            d.Columns.Add("", "");
            d.Columns.Add("", "");
            d.Columns.Add("", "");

            d.Columns[1].Width = 200;
            d.Columns[2].Width = 200;

            d.Columns[0].Visible = false;

            foreach (var er in User.ExeptionsList.OrderByDescending(c => c.DateTime))
            {
                d.Rows.Add(er.Id, er.DateTime, er.Text);
            }

            Cm.System.ErrorListText.Text = string.Empty;
        }

        /// <summary>
        /// Отчистка страницы заполнения
        /// </summary>
        /// <param name="st">Что имеено делаем</param>
        public static void ClearStartWindow(byte st)
        {
            Cm.StartPage.CreateTableCheck.Checked = false;
            Cm.StartPage.SaveConfCheck.Checked = false;
            Cm.StartPage.LoadConfCheck.Checked = false;
            Cm.StartPage.FillingCheck.Checked = false;

            switch (st)
            {
                case 1:
                    Cm.StartPage.LoadConfCheck.Checked = true;
                    break;
                case 2:
                    Cm.StartPage.SaveConfCheck.Checked = true;
                    break;
                case 3:
                    Cm.StartPage.CreateTableCheck.Checked = true;
                    break;
                case 4:
                    Cm.StartPage.FillingCheck.Checked = true;
                    break;
            }

            Cm.StartPage.List.Items.Clear();
        }
    }
}
