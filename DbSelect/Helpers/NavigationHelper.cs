using DbSelect.Controllers;
using DbSelect.Enums;
using DbSelect.Forms;
using DbSelect.Models.StaticModels;
using DbSelect.Models.SystemModels;


namespace DbSelect.Helpers
{
    public static class NavigationHelper
    {
        /// <summary>
        /// Переадрессация на создание таблиц
        /// </summary>
        /// <param name = "force"> true - без предварительного сохранения </param>
        public static void CreateTable(bool force = false)
        {
            if (force || SystemController.CheckSaveChanges())
            {
                SystemController.ClearAll();
                Cm.System.Tab.SelectedIndex = 1;
                User.SelectPage = PagesEnum.CreateTable;
                TableController.PullDataGridCreateTable();
            }
        }

        /// <summary>
        /// Переадрессация на стартовое окно
        /// </summary>
        /// <param name = "force"> true - без предварительного сохранения </param>
        public static void StartWindow(bool force = false)
        {
            if (force || SystemController.CheckSaveChanges())
            {
                SystemController.ClearAll();
                User.SelectPage = PagesEnum.None;
                Cm.System.Tab.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Переадрессация на автозаполнение
        /// </summary>
        /// <param name = "tableId"> Индетификатор таблицы </param>
        /// <param name = "force"> true - без предварительного сохранения </param>
        public static void Filling(int tableId = 0, bool force = false)
        {
            if (force || SystemController.CheckSaveChanges())
            {
                SystemController.ClearAll();

                if (tableId != 0)
                    User.SelectedTableId = tableId;
                else
                    User.SelectedTableId = -1;

                User.SelectPage = PagesEnum.Filling;
                TableController.PullTableList();
                Cm.System.Tab.SelectedIndex = 2;
            }
        }

        /// <summary>
        /// Переадрессация в автозаполнении
        /// </summary>
        /// <param name = "id"> Индетификатор окна для перехода </param>
        public static void FillingOpenWindow(int id)
        {
            int idTab = 0;

            switch (id)
            {
                case 0:
                    idTab = 0; break;
                case 1:
                    idTab = 1; break;
                case 2:
                    idTab = 2; break;
                case 3:
                    idTab = 3; break;
                case 4:
                    idTab = 4; break;
                case 5:
                    idTab = 5; break;
                case 6:
                    idTab = 6; break;
                case 7:
                    idTab = 7; break;
                case 8:
                    idTab = 8; break;
                case 9:
                    idTab = 9; break;
                case 10:
                    idTab = 10; break;
                case 11:
                    idTab = 11; break;
                case 12:
                    idTab = 12; break;
                case 13:
                    idTab = 13; break;
                case 14:
                    idTab = 14; break;
            }

            Cm.Filling.Tab.SelectedIndex = idTab;
        }


        /// <summary>
        /// Переадрессация на обновление таблиц
        /// </summary>
        /// <param name = "force"> true - без предварительного сохранения </param>
        public static void UpdateTable(int tableId, bool force = false)
        {
            if (force || SystemController.CheckSaveChanges())
            {
                SystemController.ClearAll();
                User.SelectedTableId = tableId;
                Cm.System.Tab.SelectedIndex = 1;
                User.SelectPage = PagesEnum.UpdateTable;
                TableController.PullDataGridCreateTable();
                TableController.PullUpdateTable(tableId);
            }
        }


        /// <summary>
        /// Переадрессация на создание файла
        /// </summary>
        /// <param name = "type"> Тип файла </param>
        /// <param name = "force"> true - без предварительного сохранения </param>
        public static void File(FileTypeEnum type, bool force = false)
        {
            if (force || SystemController.CheckSaveChanges())
            {
                SystemController.ClearAll();
                switch (type)
                {
                    case FileTypeEnum.String:
                        Cm.System.Tab.SelectedIndex = 3;
                        User.SelectPage = PagesEnum.FileString;
                        break;
                    case FileTypeEnum.Image:
                        Cm.System.Tab.SelectedIndex = 4;
                        User.SelectPage = PagesEnum.FileImg;
                        break;
                    case FileTypeEnum.ImageList:
                        Cm.System.Tab.SelectedIndex = 5;
                        User.SelectPage = PagesEnum.FileImgList;
                        break;
                }

            }
        }

        /// <summary>
        /// Переадрессация на обновление файла
        /// </summary>
        /// <param name = "fileId"> Индетификатор файла </param>
        /// <param name = "force"> true - без предварительного сохранения </param>
        public static void EditFile(int fileId, bool force = false)
        {
            if (force || SystemController.CheckSaveChanges())
            {
                SystemController.ClearAll();
                var file = FileHelper.GetFileById(fileId);
                User.SelectedFileId = fileId;

                switch (file.Type)
                {
                    case FileTypeEnum.String:
                        Cm.System.Tab.SelectedIndex = 3;
                        User.SelectPage = PagesEnum.FileString;
                        FileController.PullDataStringFile(fileId);
                        break;
                    case FileTypeEnum.Image:
                        Cm.System.Tab.SelectedIndex = 4;
                        FileController.FileImgPullData();
                        User.SelectPage = PagesEnum.FileImg;
                        break;
                    case FileTypeEnum.ImageList:
                        Cm.System.Tab.SelectedIndex = 5;
                        FileController.FileGroupPull();
                        User.SelectPage = PagesEnum.FileImgList;
                        break;
                }
            }
        }

        /// <summary>
        /// Переадрессация на сохранение конфигурации
        /// </summary>
        /// <param name="force">true - без предварительного сохранения</param>
        public static void SaveConf(bool force = false)
        {
            if (force || SystemController.CheckSaveChanges())
            {
                SystemController.ClearAll();
                SystemController.ClearStartWindow(2);
                Cm.System.Tab.SelectedIndex = 6;
            }
        }

        /// <summary>
        /// Переадрессация на загрузку конфигурации
        /// </summary>
        /// <param name="force">true - без предварительного сохранения</param>

        public static void SelectConf(bool force = false)
        {
            if (force || SystemController.CheckSaveChanges())
            {
                SystemController.ClearAll();
                SystemController.ClearStartWindow(1);
                Cm.System.Tab.SelectedIndex = 6;
            }
        }

        /// <summary>
        /// Переадрессация на старт выполнения запросов
        /// </summary>
        /// <param name="force">true - без предварительного сохранения</param>
        public static void StartAction(bool force = false)
        {
            if (force || SystemController.CheckSaveChanges())
            {
                SystemController.ClearAll();
                SystemController.ClearStartWindow(3);
                Cm.System.Tab.SelectedIndex = 6;
            }
        }

        /// <summary>
        /// Переадрессация на старт
        /// </summary>
        /// <param name="force">true - без предварительного сохранения</param>
        public static void StartA(bool force = false)
        {
            if (force || SystemController.CheckSaveChanges())
            {
                SystemController.ClearAll();
                Cm.System.Tab.SelectedIndex = 6;
            }
        }

        /// <summary>
        /// Переадрессация авторизацию
        /// </summary>
        public static void Autorization()
        {
            StartWindow();
            var form = new DataForm();
            form.Show();
        }

        /// <summary>
        /// Переадрессация на список ошибок
        /// </summary>
        /// <param name="force">true - без предварительного сохранения</param>
        public static void ErrorList(bool force = false)
        {
            if (force || SystemController.CheckSaveChanges())
            {
                SystemController.ErrorListPull();
                Cm.System.Tab.SelectedIndex = 7;
            }
        }
    }
}
