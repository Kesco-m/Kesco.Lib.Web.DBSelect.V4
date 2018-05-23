using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonTheme;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для контрола выбора лица
    /// </summary>
    public class DSOPersonTheme : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по наименованию темы лица
        /// </summary>
        [FilterOption("ТемаЛица", false, "Search")] public FOptPersonTheme Name;

        /// <summary>
        /// Опция фильтра по каталогу
        /// </summary>
        [FilterOption("Каталог", false, "CatId")] public FOptCatalog Catalog;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPersonTheme()
        {
            KeyField = "КодТемыЛица";
            NameField = "ТемаЛица";
            Name = new FOptPersonTheme();
            Catalog = new FOptCatalog();
            
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_ТемыЛиц; }
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