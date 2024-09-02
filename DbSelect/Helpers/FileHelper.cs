using DbSelect.Models.StaticModels;
using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DbSelect.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// Поиск файла по имени
        /// </summary>
        /// <returns> 
        /// Объект файла
        /// </returns>
        /// <param name = "name"> Имя файла</param>
        public static Files GetFileByName(string name)
        {
            var file = User.Files.FirstOrDefault (f => f.Name.ToLower().Equals(name.ToLower()));
            return file;
        }

        /// <summary>
        /// Поиск файла по Id
        /// </summary>
        /// <returns> 
        /// Объект файла
        /// </returns>
        /// <param name = "id"> Id файла</param>
        public static Files GetFileById(int id)
        {
            var file = User.Files.FirstOrDefault(f => f.Id == id);
            return file;
        }

        /// <summary>
        /// Поиск файла по пути файла
        /// </summary>
        /// <returns> 
        /// Объект файла
        /// </returns>
        /// <param name = "filename"> Путь к файлу</param>
        public static Files GetFileByFileName(string filename)
        {
            var file = User.Files.FirstOrDefault(f => f.FileName != null && f.FileName.Equals(filename));
            return file;
        }

        /// <summary>
        /// Получение значения из файла для автозаполнения
        /// </summary>
        /// <param name="idFile">Id файла</param>
        /// <returns>Значение</returns>
        public static string GetValueByFile(int idFile)
        {
            var file = GetFileById(idFile);

            switch (file.Type)
            {
                case Enums.FileTypeEnum.String:
                    var list = File.ReadAllLines(file.FileName);
                    return list[ValueHelper.Rand(0, list.Count())];
                case Enums.FileTypeEnum.Image:
					return Convert.ToBase64String(System.IO.File.ReadAllBytes(file.FileName));

                case Enums.FileTypeEnum.ImageList:
                    string filepath = file.FileNames[ValueHelper.Rand(0, file.FileNames.Count - 1)];
					return Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath));
            }

            return string.Empty;
        }

    }
}
