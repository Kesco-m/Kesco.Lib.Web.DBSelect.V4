using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonLink;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Связи лиц
    /// </summary>
    public class DSOPersonLink : DSOCommon
    {
        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPersonLink()
        {
            KeyField = "КодСвязиЛиц";
            NameField = "Описание";

            Name = new FOptName();
            IsAddExcludeCondition = false;
        }

        /// <summary>
        ///     Опция фильтра по имени типа формата атрибута
        /// </summary>
        [FilterOption("Описание", false, "Описание")]
        public FOptName Name { get; set; }

        /// <summary>
        ///     Опция фильтра по коду родителя
        /// </summary>
        [FilterOption("КодЛицаРодителя", false, "КодЛицаРодителя")]
        public string ParentID { get; set; }

        /// <summary>
        ///     Опция фильтра по коду потомка
        /// </summary>
        [FilterOption("КодЛицаПотомка", false, "КодЛицаПотомка")]
        public string ChildID { get; set; }

        /// <summary>
        ///     Опция фильтра по коду тип связи лиц
        /// </summary>
        [FilterOption("КодТипаСвязиЛиц", false, "КодТипаСвязиЛиц")]
        public string LinkTypeID { get; set; }

        /// <summary>
        ///     Опция фильтра по параметр связи лиц
        /// </summary>
        [FilterOption("Параметр", false, "Параметр")]
        public string Parameter { get; set; }

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
            get { return string.Format("{0}", NameField); }
        }
    }
}