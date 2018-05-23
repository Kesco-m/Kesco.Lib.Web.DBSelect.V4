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
        [FilterOption("IDs", false)]
        public FOptIDs IDs;

        /// <summary>
        ///     Опция фильтра по наименованию типа склада
        /// </summary>
        [FilterOption("Name", false)]
        public FOptName Name;

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
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_ТипыСкладов; }
        }

        /// <summary>
        ///     Запрос получения типа склада по ID
        /// </summary>
        public override string SQLEntityById
        {
            get { return SQLQueries.SELECT_ID_ТипСклада; }
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy
        {
            get { return string.Format("T0.{0}", KeyField); }
        }
    }
}