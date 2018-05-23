
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common;
using Kesco.Lib.Entities;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
	public class DSOCurrency : DSOCommon
	{
		/// <summary>
		/// Опция фильтра по имени
		/// </summary>
		[FilterOption("Название")]
		public FOptName Name;

		/// <summary>
		/// Опция фильтра по ID
		/// </summary>
		[FilterOption("КодСклада")]
		public FOptIDs StoreId;

		public DSOCurrency()
		{
			KeyField = "КодВалюты";
			NameField = "Название";
			StoreId = new FOptIDs("КодВалюты");
			Name = new FOptName("Название");
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
