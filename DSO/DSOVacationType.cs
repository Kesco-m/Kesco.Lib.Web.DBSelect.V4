using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.VacationType;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для котроа выбора роли
    /// </summary>
    public class DSOVacationType : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по роли
        /// </summary>
        [FilterOption("ВидОтпуска", false, "Search")] public FOptVacationType Name;

        /// <summary>
        ///     Опция фильтра по роли
        /// </summary>
        [FilterOption("КодВидаОтпуска")]
        public FOptIDs VacationTypeId;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOVacationType()
        {
            KeyField = "КодВидаОтпуска";
            NameField = "ВидОтпуска";
            Name = new FOptVacationType();
            VacationTypeId = new FOptIDs();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_ВидыОтпуска; }
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