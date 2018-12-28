using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonSigner;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Связи лиц
    /// </summary>
    public class DSOPersonSigner : DSOCommon
    {
        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPersonSigner()
        {
            KeyField = "КодСвязиЛиц";
            NameField = "Описание";

            ParentID = new FOptParentID();
            ChildID = new FOptChildID();
            LinkTypeID = new FOptLinkTypeID();
            Parametr = new FOptParametr();
        }

        /// <summary>
        ///     Опция фильтра по коду родителя
        /// </summary>
        [FilterOption("КодЛицаРодителя")]
        public FOptParentID ParentID;

        /// <summary>
        ///     Опция фильтра по коду потомка
        /// </summary>
        [FilterOption("КодЛицаПотомка")]
        public FOptChildID ChildID;

        /// <summary>
        ///     Опция фильтра по коду тип связи лиц
        /// </summary>
        [FilterOption("КодТипаСвязиЛиц")]
        public FOptLinkTypeID LinkTypeID;

        /// <summary>
        ///     Опция фильтра по параметру
        /// </summary>
        [FilterOption("Параметр")]
        public FOptParametr Parametr;

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_СвязиЛиц; }
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy
        {
            get { return "Описание"; }
        }
    }
}