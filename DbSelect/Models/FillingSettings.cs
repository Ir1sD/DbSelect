using DbSelect.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSelect.Models.SystemModels
{

	/// <summary>
	/// Настройки автозаполнения
	/// </summary>
	public class FillingSettings
    {
		/// <summary>
		/// Список условий для поля
		/// </summary>
		public List<Condition> ConditionList = new List<Condition>();


		// INT


		/// <summary>
		/// Постоянное значение для целого числа
		/// </summary>
		public int? IntConst {  get; set; }

		/// <summary>
		/// Минимальное значение для рандомной выборки целого числа
		/// </summary>
		public int? IntRandFrom {  get; set; }

		/// <summary>
		/// Максимальное число для рандомной выборки целого числа
		/// </summary>
		public int? IntRandTo { get; set; }

		/// <summary>
		/// Сортировка для рандомной выборки целого числа
		/// </summary>
		public SortEnum IntRandSort { get; set; }

		/// <summary>
		/// Идентификатор таблицы для заполнения ключом без повторений целого числа
		/// </summary>
		public int? IntIdConstTable { get; set; }

		/// <summary>
		/// Идентификатор колонки для заполнения ключом без повторений целого числа
		/// </summary>
		public int? IntIdConstColumn { get; set; }

		/// <summary>
		/// Идентификатор таблицы для заполнения ключом с повторением целого числа
		/// </summary>
		public int? IntIdRandTable {  get; set; }

		/// <summary>
		/// Идентификатор колонки для заполнения ключом с повторением целого числа
		/// </summary>
		public int? IntIdRandColumn { get; set; }


		// String


		/// <summary>
		/// Постоянное значение для заполнения постоянной строки
		/// </summary>
		public string StringConst { get; set; }

		/// <summary>
		/// Идентификатор файла для заполнения строки из файла
		/// </summary>
		public int StringFile {  get; set; }

		/// <summary>
		/// Сортировка для рандомной выборки строки из файла
		/// </summary>
		public SortEnum StringFileSort {  get; set; }


		// Float


		/// <summary>
		/// Постоянное значения для заполнения числа с запятой
		/// </summary>
		public double? FloatConst {  get; set; }

		/// <summary>
		/// Минимальное значение для рандомной выборки числа с запятой
		/// </summary>
		public double? FloatRandFrom { get; set; }

		/// <summary>
		/// Максимальное значение для рандомной выборки числа с запятой
		/// </summary>
		public double? FloatRandTo { get; set;}

		/// <summary>
		/// Сортировка для рандомной выборки числа с запятой
		/// </summary>
		public SortEnum FloatRandSort {  get; set; }


		// Date


		/// <summary>
		/// Постоянное значение для даты
		/// </summary>
		public DateTime? DateConst { get; set; }

		/// <summary>
		/// Минимальное значение для выборки рандомной даты
		/// </summary>
		public DateTime? DateRandFrom { get; set; }

		/// <summary>
		/// Максимальное значения для выборки рандомной даты
		/// </summary>
		public DateTime? DateRandTo { get; set; }

		/// <summary>
		/// Сортировка для выборки рандомной даты
		/// </summary>
		public SortEnum DateRandSort {  get; set; }


		// Img


		/// <summary>
		/// Идентификатор файла для постоянной картинки
		/// </summary>
		public int ImgConst {  get; set; }

		/// <summary>
		/// Индетификатор файла для рандомной выборки картинки
		/// </summary>
		public int ImgRand {  get; set; }


		// Bool


		/// <summary>
		/// Постоянное значение для постоянного bool выражения
		/// </summary>
		public bool? BoolConst {  get; set; }
    

    }
}
