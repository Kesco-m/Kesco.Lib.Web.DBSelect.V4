
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Currency;
using Kesco.Lib.Entities;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
	public class DSOCurrency : DSOCommon
	{
		/// <summary>
		/// Опция фильтра по имени
		/// </summary>
        [FilterOption("РесурсРус")]
		public FOptName Name;

		/// <summary>
		/// Опция фильтра по ID
		/// </summary>
        [FilterOption("КодВалюты")]
		public FOptIDs StoreId;

		public DSOCurrency()
		{
			KeyField = "КодВалюты";
            NameField = "Название";
			StoreId = new FOptIDs("КодВалюты");
            Name = new FOptName("РесурсРус");
		}

		/// <summary>
		///     Запрос выборки данных
		/// </summary>
		public override string SQLBatch
		{
			get { return SQLQueries.SELECT_Валюты; }
		}

		/// <summary>
		///     Задание сортировки выборки
		/// </summary>
		public override string SQLOrderBy
		{
			get
			{
				return "КодВалюты";
			}
		}
	}
}
