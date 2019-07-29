using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для контрола выбора лица
    /// </summary>
    public class DSOPersonNickName : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по наименованию лица
        /// </summary>
        [FilterOption("Search", true, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPersonNickName()
        {
            KeyField = "Name";
            NameField = "Name";

            Name = new FOptName();

            IsAddExcludeCondition = false;
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Empty;
    }
}