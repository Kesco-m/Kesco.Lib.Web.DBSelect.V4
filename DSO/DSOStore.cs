using Kesco.Lib.Entities;

using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для контрола выбора склада
    /// </summary>
    public class DSOStore : DSOCommon
    {
        /// <summary>
        /// Опция фильтра по имени
        /// </summary>
        [FilterOption("Название", false, "StoreSrchNameMode=11&_StoreSrchName")]
        public FOptName Name;

        /// <summary>
        /// Опция фильтра по ID
        /// </summary>
        [FilterOption("КодСклада")]
        public FOptIDs StoreId;

        /// <summary>
        /// Опция фильтра по ID
        /// </summary>
        [FilterOption("КодСклада", false, "StoreSrchExceptions")]
        public FOptIDs Exceptions;

		/// <summary>
		/// Опция фильтра по коду типа склада
		/// </summary>
		[FilterOption("КодТипаСклада", true, "StoreType")]
		public FOptIDs StoreTypeId;

		/// <summary>
		/// Опция фильтра по коду ресурса склада
		/// </summary>
        [FilterOption("КодРесурса", true, "StoreResource")]
		public FOptIDs StoreResourceId;

		/// <summary>
		/// Опция фильтра по коду распорядителя
		/// </summary>
        [FilterOption("КодРаспорядителя", true, "StoreManager")]
		public FOptIDs ManagerId;

		/// <summary>
		/// Опция фильтра по дате действия
		/// </summary>
        [FilterOption("Действует")]
		public FOpt.Store.FOptValidAt ValidAt;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOStore()
        {
            KeyField = "КодСклада";
            NameField = "Название";
			StoreId = new FOptIDs("КодСклада");
            Name = new FOptName("Склад");
			Exceptions = new FOptIDs("КодСклада");
			Exceptions.CompanyHowSearch = "1"; //NOT IN

			StoreTypeId = new FOptIDs("КодТипаСклада");
			ManagerId = new FOptIDs("КодРаспорядителя");
			StoreResourceId = new FOptIDs("КодРесурса");

			ValidAt = new FOpt.Store.FOptValidAt();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_Склад_Ext; }
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy
        {
            get
            {
                return "T0.Sort";
            }
        }
    }
}