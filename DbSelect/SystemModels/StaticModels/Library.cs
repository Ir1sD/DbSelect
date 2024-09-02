using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbSelect.Models;
using DbSelect.Models.Enums;
using System.Windows.Forms;
using System.Drawing.Text;

namespace DbSelect.StaticModels
{
    public static class Library
    {
        // Типы данных

        public static readonly List<TypeOf> types = new List<TypeOf>
        {
            new TypeOf {Id = 1, Name = "smallint" , Type = TypeEnum.Number},
            new TypeOf {Id = 2, Name = "int" , Type = TypeEnum.Number},
            new TypeOf {Id = 3, Name = "bigint" , Type = TypeEnum.Number},
            new TypeOf {Id = 4, Name = "varchar(30)" , Type = TypeEnum.String , MaxSizeString = 30},
            new TypeOf {Id = 5, Name = "varchar(80)" , Type = TypeEnum.String , MaxSizeString = 80},
            new TypeOf {Id = 6, Name = "varchar(MAX)" , Type = TypeEnum.Image},
            new TypeOf {Id = 7, Name = "nvarchar(30)" , Type = TypeEnum.String , MaxSizeString = 30},
            new TypeOf {Id = 8, Name = "nvarchar(80)" , Type = TypeEnum.String , MaxSizeString = 80},
            new TypeOf {Id = 9, Name = "nvarchar(MAX)" , Type = TypeEnum.String},
            new TypeOf {Id = 10, Name = "bit" , Type = TypeEnum.Bool},
            new TypeOf {Id = 11, Name = "date" , Type = TypeEnum.Date},
            new TypeOf {Id = 12, Name = "datetime" , Type = TypeEnum.Date},
            new TypeOf {Id = 13, Name = "float" , Type = TypeEnum.Float},
            new TypeOf {Id = 14, Name = "real" , Type = TypeEnum.Float}
        };

        // Типы заполнения

        public static readonly List<TypeFilling> typeFilling = new List<TypeFilling>
        {
            new TypeFilling {Id = 0, Name = "Пусто" , Type = TypeEnum.All },
            new TypeFilling {Id = 1, Name = "Константа(число)" , Type = TypeEnum.Number },
            new TypeFilling {Id = 2, Name = "Случайное(число)" , Type = TypeEnum.Number },
            new TypeFilling {Id = 3, Name = "Ключ-уникальный(число)" , Type = TypeEnum.Number },
            new TypeFilling {Id = 4, Name = "Ключ-случайный(число)" , Type = TypeEnum.Number },
            new TypeFilling {Id = 5, Name = "Константа(строка)" , Type = TypeEnum.String },
            new TypeFilling {Id = 6, Name = "Из файла(строка)" , Type = TypeEnum.String },
            new TypeFilling {Id = 7, Name = "Константа(число с запятой)" , Type = TypeEnum.Float },
            new TypeFilling {Id = 8, Name = "Случайно(число с запятой)" , Type = TypeEnum.Float },
            new TypeFilling {Id = 9, Name = "Константа(дата)" , Type = TypeEnum.Date },
            new TypeFilling {Id = 10, Name = "Случайно(дата)" , Type = TypeEnum.Date },
            new TypeFilling {Id = 11, Name = "Константа(картинка)" , Type = TypeEnum.Image },
            new TypeFilling {Id = 12, Name = "Из файла(картинка)" , Type = TypeEnum.Image },
            new TypeFilling {Id = 13, Name = "Константа(bool)" , Type = TypeEnum.Bool },
            new TypeFilling {Id = 14, Name = "Случайное(bool)" , Type = TypeEnum.Bool },
        };

        // Иконки в TreeView

        public static ImageList Icons = new ImageList();

        // Авторизация
        public static bool IsPasswordAdmin(string password)
        {
            string pas = "qwe";
            return pas.Equals(password);
        }

    }
}
