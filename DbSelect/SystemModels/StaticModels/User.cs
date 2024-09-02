using DbSelect.Enums;
using DbSelect.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Models.StaticModels
{
    public static class User
    {
        public static List<Table> Tables = new List<Table>();
        public static List<Files> Files = new List<Files>();
        public static List<Exeptions> ExeptionsList = new List<Exeptions>();
        public static List<CreateTableError> CreateTableErrors = new List<CreateTableError>();
        public static PagesEnum SelectPage = PagesEnum.None;

        public static string ConfPath1 = string.Empty;
        public static string ConfPath2 = string.Empty;

        public static int SelectedTableId = -1;
        public static int SelectedColumnId = -1;
        public static int SelectedFileId = -1;
        public static int SelectedImageFromFile = -1;

        public static bool CorrectionErrorStatus = false;
        public static bool IsAdmin = false;

        public static List<string> FileGroupFileName = new List<string>();

		public static int TimeSleep = 40;
        public static string StringConnection = string.Empty;
    }
}
