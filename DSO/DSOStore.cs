using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Store;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для контрола выбора склада
    /// </summary>
    public class DSOStore : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по ID
        /// </summary>
        [FilterOption("КодСклада", false, "StoreSrchExceptions")]
        public FOptIDs Exceptions;

        /// <summary>
        ///     Опция фильтра по банковским счетам
        /// </summary>
        [FilterOption("TypeStoreIsAccount", false, "StoreKindType")]
        public FOptTypeStoreIsAccount IsAccountType;

        /// <summary>
        ///     Опция фильтра по продуктовым складм
        /// </summary>
        [FilterOption("TypeStoreIsWarehouse", false, "StoreKindType")]
        public FOptTypeStoreIsWarehouse IsWarehouseType;

        /// <summary>
        ///     Опция фильтра по коду распорядителя
        /// </summary>
        [FilterOption("КодРаспорядителя", true, "StoreManager")]
        public FOptManagerIds ManagerId;

        /// <summary>
        ///     Опция фильтра по имени
        /// </summary>
        [FilterOption("Название", false, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Опция фильтра по ID
        /// </summary>
        [FilterOption("КодСклада")] public FOptIDs StoreId;

        /// <summary>
        ///     Опция фильтра по коду ресурса склада
        /// </summary>
        [FilterOption("КодРесурса", true, "StoreResource")]
        public FOptIDs StoreResourceId;

        /// <summary>
        ///     Опция фильтра по коду типа склада
        /// </summary>
        [FilterOption("КодТипаСклада", true, "StoreType")]
        public FOptIDs StoreTypeId;

        /// <summary>
        ///     Опция фильтра по дате действия
        /// </summary>
        [FilterOption("Действует", true, "StoreActual")]
        public FOptValidAt ValidAt;

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
            ManagerId = new FOptManagerIds();
            StoreResourceId = new FOptIDs("КодРесурса");

            ValidAt = new FOptValidAt();

            IsAccountType = new FOptTypeStoreIsAccount();
            IsWarehouseType = new FOptTypeStoreIsWarehouse();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_Склад_Ext;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => "T0.Хранитель, T0.Sort";
    }
}