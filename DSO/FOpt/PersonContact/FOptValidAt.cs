using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonContact
{
	/// <summary>
	///     Класс опции поиска по указанной дате
	/// </summary>
	public class FOptValidAt : FOptBase, IFilterOption
	{
		/// <summary>
		///     Построение запроса
		/// </summary>
		/// <returns>Построенный запрос</returns>
		public string SQLGetClause()
		{
            if (!string.IsNullOrEmpty(Value) && Value != DateTime.MinValue.ToString("yyyyMMdd"))
			{
				return string.Format(@"((T0.От IS NULL OR T0.От <= '{0}') AND (T0.До IS NULL OR T0.До > '{0}'))", SqlEscape(Value));
			}
			return string.Empty;
		}
	}
}
