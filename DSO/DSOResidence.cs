using Kesco.Lib.BaseExtention.Enums.Controls;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Residence;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Класс объекта базы данных "Место хранения"
    /// </summary>
    public class DSOResidence : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра: все подчиненные места хранения
        /// </summary>
        [FilterOption("AllChildrenWithoutParentIDs", false)] public FOptParentsIDs AllChildrenWithoutParentIDs;

        /// <summary>
        ///     Опция фильтра: все подчиненные места хранения включая родителя
        /// </summary>
        [FilterOption("AllChildrenWithParentIDs", false)] public FOptParentsIDs AllChildrenWithParentIDs;

        /// <summary>
        ///     Опция фильтра: все непосредственные подчиненные места хранения
        /// </summary>
        [FilterOption("ChildrenWithoutParentIDs", false)] public FOptParentsIDs ChildrenWithoutParentIDs;

        /// <summary>
        ///     Опция фильтра: все непосредственные подчиненные места хранения включая родителя
        /// </summary>
        [FilterOption("ChildrenWithParentIDs", false)] public FOptParentsIDs ChildrenWithParentIDs;

        /// <summary>
        ///     Опция фильтра по ID мест хранения
        /// </summary>
        [FilterOption("IDs", false, "IDs")] public FOptIDs IDs;

        /// <summary>
        ///     Опция фильтра по наименованию места хранения
        /// </summary>
        [FilterOption("Name", false, "Search")] public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOResidence()
        {
            var tableName = "МестаХранения";

            KeyField = "КодМестаХранения";
            NameField = "МестоХранения";

            IDs = new FOptIDs();
            Name = new FOptName();
            AllChildrenWithParentIDs = new FOptParentsIDs(TreeQueryType.AllChildrenWithoutParent, tableName, KeyField,
                NameField);
            AllChildrenWithoutParentIDs = new FOptParentsIDs(TreeQueryType.AllChildrenWithParent, tableName, KeyField,
                NameField);
            ChildrenWithParentIDs = new FOptParentsIDs(TreeQueryType.ChildrenWithoutParent, tableName, KeyField);
            ChildrenWithoutParentIDs = new FOptParentsIDs(TreeQueryType.ChildrenWithParent, tableName, KeyField);
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_МестаХранения; }
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy
        {
            get { return string.Format("T0.{0}", NameField); }
        }
    }
}