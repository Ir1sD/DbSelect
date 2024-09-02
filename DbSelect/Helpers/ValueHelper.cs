using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DbSelect.Helpers
{
    public static class ValueHelper
    {
		/// <summary>
		/// Возможно ли преобразовать в int
		/// </summary>
		/// <returns> 
		/// true - Да false - Нет
		/// </returns>
		/// <param name = "val"> Значение</param>
		public static bool IsInt(string val)
        {
            if(val.Length == 0)
                return false;

            return int.TryParse(val, out int numericValue);
        }

		/// <summary>
		/// Возможно ли преобразовать в int
		/// </summary>
		/// <returns> 
		/// true - Да false - Нет
		/// </returns>
		/// <param name = "val"> Значение</param>
		public static bool IsInt(decimal val)
        {
            return IsInt(val.ToString());
        }

		/// <summary>
		/// Возможно ли преобразовать в double
		/// </summary>
		/// <returns> 
		/// true - Да false - Нет
		/// </returns>
		/// <param name = "val"> Значение</param>
		public static bool IsFloat(string val)
        {
            if (val.Length == 0)
                return false;

            return double.TryParse(val, out double numericValue);
        }

		/// <summary>
		/// Возможно ли преобразовать в bool
		/// </summary>
		/// <returns> 
		/// true - Да false - Нет
		/// </returns>
		/// <param name = "val"> Значение</param>
		public static bool IsBool(string val)
        {
            if (val.Length == 0)
                return false;

            if (val.ToLower().Equals("true") || val.ToLower().Equals("false"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		/// <summary>
		/// Преобразование в bool
		/// </summary>
		/// <returns> 
		/// Bool
		/// </returns>
		/// <param name = "val"> Значение</param>
		public static bool InBool(string val)
        {
            if (val.ToLower().Equals("true"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		/// <summary>
		/// Возможно ли преобразовать в DateTime
		/// </summary>
		/// <returns> 
		/// true - Да false - Нет
		/// </returns>
		/// <param name = "val"> Значение</param>
		public static bool IsDate(string val)
        {
            if (val.Length == 0)
                return false;

            return DateTime.TryParse(val, out DateTime numericValue);
        }

		/// <summary>
		/// Получение рандомного int значения
		/// </summary>
		/// <param name="from">Значения от</param>
		/// <param name="to">Значение до</param>
		/// <returns>Значение int</returns>
		public static int Rand(int from , int to)
		{
			Thread.Sleep(1);
			Random random = new Random(DateTime.Now.Millisecond);
			return random.Next(from, to);
		}

        /// <summary>
        /// Получение рандомного double значения
        /// </summary>
        /// <param name="from">Значения от</param>
        /// <param name="to">Значение до</param>
        /// <returns>Значение double</returns>
        public static double Rand(double from, double to)
		{
			Thread.Sleep(1);
			Random random = new Random();

			double result = Rand((int)Math.Floor(from), (int)Math.Ceiling(to - 1));
			result = result += random.NextDouble();

            return Math.Round(result , 2);
		}

        /// <summary>
        /// Получение рандомного DateTime значения
        /// </summary>
        /// <param name="from">Значения от</param>
        /// <param name="to">Значение до</param>
        /// <returns>Значение DateTime</returns>
        public static DateTime Rand(DateTime from , DateTime to)
		{
			int year = Rand(from.Year, to.Year);
			int mounth = Rand(1, 12);
			int day = Rand(1, 30);
			int hour = Rand(1, 23);
			int minute = Rand(1, 59);
			int sec = Rand(1, 59);
            return new DateTime(year, mounth, day, hour, minute , sec);
		}

        /// <summary>
        /// Получение рандомного bool значения
        /// </summary>
        /// <returns>Значение bool</returns>
        public static bool Rand()
		{
			int rand = Rand(0, 2);
			
			if(rand == 0) return false;
			else return true;
		}

		/// <summary>
		/// Конвертация double в строку с заменой запятой на точку
		/// </summary>
		/// <param name="d">Число</param>
		/// <returns>Строка</returns>
		public static string NormalizateDoubleString(double d)
		{
			string result = string.Empty;
			string r = d.ToString();

			for(int i = 0; i < r.Length; i++)
			{
				if (r[i] != ',')
                    result += r[i];
                else
					result += '.';
			}

			return result;
		}
	}
}
