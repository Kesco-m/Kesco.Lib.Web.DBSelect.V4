using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonCatalog;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для котроа выбора роли
    /// </summary>
    public class DSOPersonCatalog : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по роли
        /// </summary>
        [FilterOption("Каталог", false, "Search")]
        public FOptPersonCatalog Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPersonCatalog()
        {
            KeyField = "КодКаталога";
            NameField = "Каталог";
            Name = new FOptPersonCatalog();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_Каталоги;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", NameField);
    }
}