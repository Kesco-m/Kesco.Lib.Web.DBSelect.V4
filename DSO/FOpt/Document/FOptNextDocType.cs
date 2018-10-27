using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    /// Класс опции поиска по типу
    /// </summary>
    public class FOptNextDocType : FOptBase, IFilterOption
	{
		/// <summary>
		/// Идентификатор поля
		/// </summary>
        public int FieldId { get; set; }

		/// <summary>
		/// Построение запроса
		/// </summary>
		/// <returns>Построенный запрос</returns>
		public string SQLGetClause()
		{
			//return !string.IsNullOrEmpty(Value) ? string.Format(" (T0.КодТипаДокумента IN (SELECT КодТипаДокументаОснования FROM СвязиТиповДокументов WHERE КодПоляДокумента={0:D} AND КодТипаДокументаВытекающего={1:D}))", FieldId, Value) : "";

			return !string.IsNullOrEmpty(Value) ? string.Format(@" (T0.КодТипаДокумента IN (SELECT T3.КодТипаДокумента FROM ТипыДокументов AS T1
CROSS APPLY (SELECT КодТипаДокумента FROM ТипыДокументов AS T2 WHERE T2.L >= T1.L AND T2.R <= T1.R) AS T3
WHERE T1.КодТипаДокумента IN (SELECT КодТипаДокументаОснования FROM СвязиТиповДокументов WHERE КодПоляДокумента={0:D} AND КодТипаДокументаВытекающего={1:D})))", FieldId, Value) : "";
		}
	}
}
