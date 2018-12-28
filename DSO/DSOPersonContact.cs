using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
	/// <summary>
	///     Источник данных для контрола выбора склада
	/// </summary>
	public class DSOPersonContact : DSOCommon
	{
	    /// <summary>
        /// Источник
	    /// </summary>
	    public enum Source
	    {
	        /// <summary>
            /// Контакты
	        /// </summary>
	        Контакты = 1, 
            /// <summary>
            /// КарточкиЮрЛиц
            /// </summary>
            КарточкиЮрЛиц = 2, 
            /// <summary>
            /// КарточкиФизЛиц
            /// </summary>
            КарточкиФизЛиц = 4, 
            /// <summary>
            /// Пусто
            /// </summary>
            Пусто = 0
	    };

		/// <summary>
		/// Опция фильтра по имени
		/// </summary>
		[FilterOption("Контакт")]
		public FOptName Name;

		/// <summary>
		/// Опция фильтра по ID
		/// </summary>
		[FilterOption("КодКонтакта")]
		public FOptIDs ContactId;

		/// <summary>
		/// Опция фильтра по типу контакта
		/// </summary>
		[FilterOption("КодТипаКонтакта")]
		public FOptIDs ContactTypeId;

		/// <summary>
		/// Опция фильтра по ID лица
		/// </summary>
        [FilterOption("КодЛица", true, "idclient")]
		public FOptIDs PersonId;

		/// <summary>
		/// Опция фильтра по дате действия
		/// </summary>
		[FilterOption("Действует")]
		public FOpt.PersonContact.FOptValidAt ValidAt;

		/// <summary>
		/// Опция фильтра по источнику выбора контактов
		/// </summary>
		[FilterOption("ИсточникКонтакта", true)]
		public Source SourceType { get; set; }

		/// <summary>
		///     Конструктор класса
		/// </summary>
		public DSOPersonContact()
		{
			KeyField = "КодКонтакта";
			NameField = "Контакт";

			ContactId = new FOptIDs("КодКонтакта");
			Name = new FOptName("Контакт");
			PersonId = new FOptIDs("КодЛица");
			ContactTypeId = new FOptIDs("КодТипаКонтакта");

			ValidAt = new FOpt.PersonContact.FOptValidAt();

			SourceType = Source.Контакты | Source.КарточкиЮрЛиц | Source.КарточкиФизЛиц;
		}

		/// <summary>
		///     Запрос выборки данных
		/// </summary>
		public override string SQLBatch
		{
			get
			{
				if (null == PersonId || PersonId.CompanyHowSearch!="2" && PersonId.CompanyHowSearch!="3" && string.IsNullOrEmpty(PersonId.Value))
					return SQLQueries.SELECT_ПустыеКонтактыЛица;

				return SQLQueries.SELECT_КонтактыЛица;
			}
		}

		/// <summary>
		///     Задание сортировки выборки
		/// </summary>
		public override string SQLOrderBy
		{
			get
			{
				return "Контакт";
			}
		}
	}
}
