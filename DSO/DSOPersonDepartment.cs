using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonDepartment;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    /// </summary>
    public class DSOPersonDepartment : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени
        /// </summary>
        [FilterOption("Подразделение")] public FOptName Name;

        /// <summary>
        ///     Опция фильтра по ID
        /// </summary>
        [FilterOption("КодЛица")] public FOptIDs PcId;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPersonDepartment()
        {
            KeyField = "КодПодразделенияЛица";
            NameField = "Подразделение";
            PcId = new FOptIDs();
            Name = new FOptName();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ПодразделенияЛиц;

        /// <summary>
        ///     Поле сортировки
        /// </summary>
        public override string SQLOrderBy => NameField;
    }
}