using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Territory;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Бизнес проект
    /// </summary>
    public class DSOBusinessProject : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени бизнес проекта
        /// </summary>
        [FilterOption("БизнесПроект", false, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOBusinessProject()
        {
            KeyField = "КодБизнесПроекта";
            NameField = "БизнесПроект";
            Name = new FOptName();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ID_БизнесПроект;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", NameField);
    }
}