using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.DutyEngeneer;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для котрола выбора дежурного инженера
    /// </summary>
    public class DSODutyEngeneer : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по роли
        /// </summary>
        [FilterOption("Сотрудник", false, "Search")]
        public FOptDutyEngeneer Name;

        /// <summary>
        ///     Опция фильтра по дате действия
        /// </summary>
        [FilterOption("Действует", true, "StoreActual")]
        public FOptDutyPeriod DutyPeriod;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSODutyEngeneer()
        {
            KeyField = "КодСотрудника";
            NameField = "Сотрудник";
            Name = new FOptDutyEngeneer();
            DutyPeriod = new FOptDutyPeriod();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => string.Format(SQLQueries.SELECT_ДежурныеИнженеры, DutyPeriod.Value);

        /// <summary>
        ///     Задание сортировки выборки
        ///     Доделать управляемую сортировку
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", NameField);
    }
}
