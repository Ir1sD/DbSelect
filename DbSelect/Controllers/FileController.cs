using DbSelect.Helpers;
using DbSelect.Models.StaticModels;
using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DbSelect.Controllers
{
    public static class FileController
    {
		
		#region Удаление

		/// <summary>
		/// Удаление выбранного файла
		/// </summary>
		public static void DeleteFile()
        {
            if (MessageHelper.QueMessage("Вы действительно хотите удалить данный файл?"))
            {
                var file = FileHelper.GetFileByName(SystemHelper.GetSelectedItem().Text);

                if(DeleteFileInTableFilling(file))
                {
					MessageHelper.WarningMessage("Данный файл был привязан к автозаполнению. Необходимо привязать новый файл");
				}

                User.Files.Remove(file);
                SystemController.UpdateInfoTree();
                MessageHelper.InfoMessage("Файл " + file.Name + " был удален");
                NavigationHelper.StartWindow(true);
            }
        }

        /// <summary>
        /// Удаление всех файлов
        /// </summary>
        public static void DeleteFilesList()
        {
			if (MessageHelper.QueMessage("Удалить все файлы?"))
			{
                bool IsDel = false;
                foreach (var file in User.Files)
                {
					if (DeleteFileInTableFilling(file))
					{
                        IsDel = true;
					}
				}

				if (IsDel)
				{
					MessageHelper.WarningMessage("Один или несколько файлов были привязанны к автозаполнению. Необходимо привязать новые файлы");
				}

				User.Files.Clear();
				SystemController.UpdateInfoTree();
			}
		}

        /// <summary>
        /// Удаление файла из настроек автозаполнения колонок
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns>Было ли произведенно удаление</returns>
        private static bool DeleteFileInTableFilling(Files file)
        {
            bool IsDel = false;

            foreach (var table in User.Tables)
            {
                foreach (var col in table.Columns)
                {
                    if(col.Settings.StringFile == file.Id)
                    {
                       col.Settings.StringFile = 0;
                        IsDel = true;
					}

                    if(col.Settings.ImgConst == file.Id)
                    {
                        col.Settings.ImgConst = 0;
						IsDel = true;
					}

                    if (col.Settings.ImgRand == file.Id )
                    {
                        col.Settings.ImgRand = 0;
						IsDel = true;
					}
                }
            }

            return IsDel;
        }
		#endregion

		#region Файл текст

		/// <summary>
		/// Получения файла - строк
		/// </summary>
		public static void PushFileString()
        {
            if(Cm.CreateFile.FileStringFileName.Text.Length < 2)
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                dialog.Title = "Выбор файла";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Cm.CreateFile.FileStringFileName.Text = dialog.FileName;
                    Cm.CreateFile.FileStringValues.Text = File.ReadAllText(dialog.FileName);
                    Cm.CreateFile.FileStringAction.Text = "Файл загружен";
                    Cm.CreateFile.FileStringAction.ForeColor = Color.Green;
                    Cm.CreateFile.FileStringValues.Enabled = true;

                    if(Cm.CreateFile.FileStringName.Text.Length == 0)
                    {
                        Cm.CreateFile.FileStringName.Text = new FileInfo(dialog.FileName).Name;
                    }

                    var files = FileHelper.GetFileByFileName(Cm.CreateFile.FileStringFileName.Text);

                    if (files != null && User.SelectedFileId == -1)
                    {
                        MessageHelper.WarningMessage("Данный файл был уже добавлен в " + files.Name + ". Если вы не хотели дублировать файлы, то измените источник");
                    }
                }

            }
            else if (MessageHelper.QueMessage("Сбросить выбранный файл?"))
            {
                Cm.CreateFile.FileStringFileName.Text = string.Empty;
                Cm.CreateFile.FileStringValues.Clear();
                Cm.CreateFile.FileStringAction.Text = "Загрузить файл...";
                Cm.CreateFile.FileStringAction.ForeColor = Color.Black;
                Cm.CreateFile.FileStringValues.Enabled = false;
            }
           
        }

        /// <summary>
        /// Отчищение формы для файла - строк
        /// </summary>
        public static void FileStringClear()
        {
            Cm.CreateFile.FileStringFileName.Text = string.Empty;
            Cm.CreateFile.FileStringValues.Clear();
            Cm.CreateFile.FileStringAction.Text = "Загрузить файл...";
            Cm.CreateFile.FileStringAction.ForeColor = Color.Black;
            Cm.CreateFile.FileStringName.Clear();
            Cm.CreateFile.FileStringValues.Enabled = false;

            User.SelectedFileId = -1;
        }

        /// <summary>
        /// Сохранения файла - строк
        /// </summary>
        /// <returns> 
        /// При ошибке - false , в остальных случаях true
        /// </returns>
        public static bool FileStringSave()
        {
            bool IsUpdate = false;

            if(User.SelectedFileId > -1)
            {
                IsUpdate = true;
            }

            if(CheckStringFile(IsUpdate))
            {
                var file = new Files();
                
                if(IsUpdate)
                {
                    file = FileHelper.GetFileById(User.SelectedFileId);
                }

                if (Cm.CreateFile.FileStringFileName.Text.Length > 1)
                {
                    file.FileName = Cm.CreateFile.FileStringFileName.Text;
                }
                else
                {
                    file.FileName = string.Empty;
                }

                file.Name = Cm.CreateFile.FileStringName.Text;
                file.Type = Enums.FileTypeEnum.String;

                if (!IsUpdate)
                {
                    User.Files.Add(file);
                }
                
                SystemController.UpdateInfoTree();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверка при создании файла
        /// </summary>
        /// <returns> 
        /// Объект Таблицы
        /// </returns>
        /// <param name = "IsUpdate"> Обновление?</param>
        private static bool CheckStringFile(bool IsUpdate)
        {
            if(Cm.CreateFile.FileStringName.Text.Length == 0)
            {
                ErrorHelper.CreateError("Название файла должно быть заполнено");
                return false;
            }

            var count = User.Files.Count(f => f.Name.Equals(Cm.CreateFile.FileStringName.Text));

            if (!IsUpdate && count > 0)
            {
                ErrorHelper.CreateError("Файл с данным названием уже существует");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Заполнения данных при переходе на форму
        /// </summary>
        /// <returns> 
        /// Объект Таблицы
        /// </returns>
        /// <param name = "fileId"> Id файла</param>
        public static void PullDataStringFile(int fileId)
        {
            var file = FileHelper.GetFileById(fileId);

            Cm.CreateFile.FileStringName.Text = file.Name;

            if(file.FileName.Length > 0)
            {
                Cm.CreateFile.FileStringFileName.Text = file.FileName;
                Cm.CreateFile.FileStringAction.Text = "Файл загружен";
                Cm.CreateFile.FileStringAction.ForeColor = Color.Green;
                Cm.CreateFile.FileStringValues.Text = File.ReadAllText(file.FileName);
                Cm.CreateFile.FileStringValues.Enabled = true;
            }
        }

        #endregion

        #region Файл картинка

        /// <summary>
        /// Отчистка данных формы с файлом - изображением
        /// </summary>
        public static void FileImageClear()
        {
            Cm.CreateFile.FileImgValue.Image = null;
            Cm.CreateFile.FileImgName.Text = string.Empty;
            Cm.CreateFile.FileImgFileName.Text = string.Empty;

            User.SelectedFileId = -1;
        }

        /// <summary>
        /// Выбор файла - изображение
        /// </summary>
        public static void FilePullImage()
        {
            var file = new Files();

            if(Cm.CreateFile.FileImgFileName.Text.Length == 0)
            {
                OpenFileDialog dialog = new OpenFileDialog();

                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.tif;...";
                dialog.Multiselect = false;
                dialog.Title = "Выбор изображения";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Cm.CreateFile.FileImgFileName.Text = dialog.FileName;
                    Cm.CreateFile.FileImgValue.Image = Image.FromFile(dialog.FileName);

                    var files = FileHelper.GetFileByFileName(dialog.FileName);

                    if(files != null)
                    {
                        MessageHelper.WarningMessage("Данный файл был уже добавлен в " + files.Name + ". Если вы не хотели дублировать файлы, то измените источник");
                    }

                    if(Cm.CreateFile.FileImgName.Text.Length == 0)
                    {
                        Cm.CreateFile.FileImgName.Text = new FileInfo(dialog.FileName).Name;
                    }

                }
            }
            else if(MessageHelper.QueMessage("Сбросить выбранный файл?"))
            {
                Cm.CreateFile.FileImgFileName.Text = string.Empty;
                Cm.CreateFile.FileImgValue.Image = null;
            }
        }

        /// <summary>
        /// Сохраниние файла - изображения
        /// </summary>
        /// <returns> 
        /// При ошибке - false
        /// </returns>
        public static bool FileImgSave()
        {
            bool IsUpdate = false;

            if (User.SelectedFileId != -1)
            {
                IsUpdate = true;
            }

            if (CheckFileImage(IsUpdate))
            {
                var file = new Files();

                if (IsUpdate)
                {
                    file = FileHelper.GetFileById(User.SelectedFileId);
                }

                file.Type = Enums.FileTypeEnum.Image;
                file.Name = Cm.CreateFile.FileImgName.Text;

                file.FileName = Cm.CreateFile.FileImgFileName.Text;

                if (!IsUpdate)
                {
                    User.Files.Add(file);
                }

                SystemController.UpdateInfoTree();
                NavigationHelper.StartWindow(true);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Заполнения данных при переходе на форму
        /// </summary>
        /// <returns> 
        /// При ошибке false
        /// </returns>
        /// <param name = "IsUpdate"> Обновляем? </param>

        public static bool CheckFileImage(bool IsUpdate)
        {
            if(Cm.CreateFile.FileImgName.Text.Length == 0)
            {
                ErrorHelper.CreateError("Название файла должно быть заполнено");
                return false;
            }

            var count = User.Files.Count(f => f.Name.Equals(Cm.CreateFile.FileImgName.Text));

            if(count > 0 && !IsUpdate)
            {
                ErrorHelper.CreateError("Файл с данным названием уже существует");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Заполнения данных при переходе на форму
        /// </summary>
        public static void FileImgPullData()
        {
            var file = FileHelper.GetFileById(User.SelectedFileId);

            Cm.CreateFile.FileImgName.Text = file.Name;

            if(file.FileName.Length > 0)
            {
                Cm.CreateFile.FileImgFileName.Text = file.FileName;
                Cm.CreateFile.FileImgValue.Image = Image.FromFile(file.FileName);
            }
        }

        #endregion

        #region Файл группа


        /// <summary>
        /// Отчистка формы файла - группы
        /// </summary>
        public static void FileGroupClear()
        {
            Cm.CreateFile.FileGroupButtonNext.Enabled = false;
            Cm.CreateFile.FileGroupButtonPrev.Enabled = false;
            Cm.CreateFile.FileGroupName.Text = string.Empty;

            foreach (var item in Cm.CreateFile.FileGroupValues)
            {
                item.Image = null;
            }

            User.SelectedFileId = -1;
            User.FileGroupFileName.Clear();
            User.SelectedImageFromFile = -1;
        }

        /// <summary>
        /// Выбор файлов в файл - группу
        /// </summary>
        public static void FileGroupPullList()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            dialog.Multiselect = true;
            dialog.Title = "Выбор изображений";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                User.FileGroupFileName.AddRange(dialog.FileNames);

                if(Cm.CreateFile.FileGroupName.Text.Length == 0)
                {
                    var count = User.Files.Count(f => f.Type == Enums.FileTypeEnum.ImageList);
                    Cm.CreateFile.FileGroupName.Text = "Группа" + (count + 1).ToString();
                }

                var files = FileHelper.GetFileByFileName(dialog.FileName);

                if(files != null)
                {
                    MessageHelper.WarningMessage("Данный файл был уже добавлен в " + files.Name + ". Если вы не хотели дублировать файлы, то измените источник");
                }

                User.SelectedImageFromFile = 0;
                FileGroupPullData();
            }
        }

        /// <summary>
        /// Отображения изображений на форме файла - группе
        /// </summary>
        public static void FileGroupPullData()
        {
            foreach (var item in Cm.CreateFile.FileGroupValues)
            {
                item.Image = null;
            }

            if (User.SelectedImageFromFile == -1)
            {
                User.SelectedImageFromFile = 0;
            }

            byte arrayIndex = 0;
            int fileIndex;

            for(fileIndex = User.SelectedImageFromFile; fileIndex < User.SelectedImageFromFile + 12; fileIndex++)
            {
                if(fileIndex < User.FileGroupFileName.Count)
                {
                    Cm.CreateFile.FileGroupValues[arrayIndex].Image = Image.FromFile(User.FileGroupFileName[fileIndex]);
                    Cm.CreateFile.FileGroupValues[arrayIndex].Tag = User.FileGroupFileName[fileIndex];
                    arrayIndex++;
                }
            }

            if(fileIndex >= User.FileGroupFileName.Count)
            {
                Cm.CreateFile.FileGroupButtonNext.Enabled = false;
            }
            else
            {
                Cm.CreateFile.FileGroupButtonNext.Enabled = true;
            }

            if(fileIndex - 12 == 0)
            {
                Cm.CreateFile.FileGroupButtonPrev.Enabled = false;
            }
            else
            {
                Cm.CreateFile.FileGroupButtonPrev.Enabled = true;
            }

            User.SelectedImageFromFile = fileIndex;
        }

        /// <summary>
        /// Сохранение файла - группа
        /// </summary>
        /// <returns> 
        /// При ошибке false
        /// </returns>
        public static bool FileGroupSave()
        {
            bool IsUpdate = false;

            if (User.SelectedFileId != -1)
            {
                IsUpdate = true;
            }

            if(FileGroupCheck(IsUpdate))
            {
                var file = new Files();

                if (IsUpdate)
                {
                    file = FileHelper.GetFileById(User.SelectedFileId);
                }

                file.Name = Cm.CreateFile.FileGroupName.Text;
                file.Type = Enums.FileTypeEnum.ImageList;
                
                if(User.FileGroupFileName.Count > 0)
                {
                    file.FileNames.AddRange(User.FileGroupFileName);
                }

                if(!IsUpdate)
                {
                    User.Files.Add(file);
                }

                SystemController.UpdateInfoTree();
                NavigationHelper.StartWindow(true);
                return true;

            }

            return false;
        }

        /// <summary>
        /// Проверка файла - группу
        /// </summary>
        /// <returns> 
        /// При ошибке false
        /// </returns>
        /// <param name = "IsUpdate"> Обновляем? </param>
        public static bool FileGroupCheck(bool IsUpdate)
        {
            if(string.IsNullOrEmpty(Cm.CreateFile.FileGroupName.Text))
            {
                ErrorHelper.CreateError("Название файла должно быть заполнено");
                return false;
            }

            var count = User.Files.Count(f => f.Name.Equals(Cm.CreateFile.FileGroupName.Text));

            if (count > 0 && !IsUpdate)
            {
                ErrorHelper.CreateError("Файл с данным названием уже существует");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Заполнения данных при переходе на форму
        /// </summary>
        public static void FileGroupPull()
        {
            var file = FileHelper.GetFileById(User.SelectedFileId);

            Cm.CreateFile.FileGroupName.Text = file.Name;
            User.FileGroupFileName.AddRange(file.FileNames);
            FileGroupPullData();
        }

        #endregion

        /// <summary>
        /// Быстрая загрузка файлов
        /// </summary>
        /// <returns>true - Успешнр  false - При ошибке</returns>
        public static bool SelectAllFile()
        {
            string result = "Файлы: ";

            if(User.Files.Count == 0)
            {
                MessageHelper.ErrorMessage("Нет доступных файлов");
                return false;
            }

            var dialog = new OpenFileDialog();
          //  dialog.Filter = "Text files(*.txt)|*.txt | Image Files|*.jpg;*.jpeg;*.png";
            dialog.Title = "Выбор файла";
            dialog.Multiselect = true;

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var filename in dialog.FileNames)
                {
                    var info = new FileInfo(filename);

                    var file = FileHelper.GetFileByName(info.Name);

                    if(file != null)
                    {
                        file.FileName = filename;
                        result += info.Name + " ";
                    }
                }

                result += "загружены";
                MessageHelper.InfoMessage(result);
                SystemController.UpdateInfoTree();
                return true;
            }
            return false;
        }
    }

}
