using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StoreType;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Класс объекта сущности "Тип склада"
    /// </summary>
    public class DSOStoreType : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра ID типа склада
        /// </summary>
        [FilterOption("IDs", false)] public FOptIDs IDs;


        /// <summary>
        ///     Опция фильтра по банковским счетам
        /// </summary>
        [FilterOption("TypeStoreIsAccount", false)]
        public FOptTypeStoreIsAccount IsAccountType;

        /// <summary>
        ///     Опция фильтра по банковским счетам
        /// </summary>
        [FilterOption("TypeStoreIsWarehouse", false)]
        public FOptTypeStoreIsWarehouse IsWarehouseType;

        /// <summary>
        ///     Опция фильтра по наименованию типа склада
        /// </summary>
        [FilterOption("Name", false)] public FOptName Name;

        /// <summary>
        ///     Опция фильтра по корневому ресурсу типа склада
        /// </summary>
        [FilterOption("RootResourceID", false)]
        public FOptRootResourceIDs RootResourceIDs;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOStoreType()
        {
            KeyField = "КодТипаСклада";
            NameField = "ТипСклада";

            IDs = new FOptIDs();
            Name = new FOptName();
            RootResourceIDs = new FOptRootResourceIDs();
            IsAccountType = new FOptTypeStoreIsAccount();
            IsWarehouseType = new FOptTypeStoreIsWarehouse();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ТипыСкладов;

        /// <summary>
        ///     Запрос получения типа склада по ID
        /// </summary>
        public override string SQLEntityById => SQLQueries.SELECT_ID_ТипСклада;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("T0.{0}", KeyField);
    }
}