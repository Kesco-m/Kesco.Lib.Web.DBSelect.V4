using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Subdivision;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Компания
    /// </summary>
    public class DSOSubdivision : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени Подразделения
        /// </summary>
        [FilterOption("Подразделение", false, "Search")] public FOptName Name;

        /// <summary>
        ///     Опция фильтра по ID компаний
        /// </summary>
        [FilterOption("КодЛица")] public FOptIDs PcId;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOSubdivision()
        {
            KeyField = "Подразделение";
            NameField = "Подразделение";
            Name = new FOptName();
            PcId = new FOptIDs();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_Подразделения; }
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