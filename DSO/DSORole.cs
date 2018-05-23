using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Role;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для котроа выбора роли
    /// </summary>
    public class DSORole : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по роли
        /// </summary>
        [FilterOption("Роль", false, "Search")] public FOptRole Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSORole()
        {
            KeyField = "КодРоли";
            NameField = "Роль";
            Name = new FOptRole();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_Роли; }
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// Доделать управляемую сортировку
        /// </summary>
        public override string SQLOrderBy
        {
            get { return string.Format("{0}", KeyField); }
        }
    }
}