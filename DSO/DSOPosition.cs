using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Position;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Должность
    /// </summary>
    public class DSOPosition : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени Должность
        /// </summary>
        [FilterOption("Должность", false, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Опция фильтра по ID компаний
        /// </summary>
        [FilterOption("КодЛица", false)] public FOptIDs PcId;

        /// <summary>
        ///     Опция фильтра по Подразделение
        /// </summary>
        [FilterOption("Подразделение", false)] public FOptSubdivisionIDs SubdivisionIDs;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPosition()
        {
            KeyField = "Должность";
            NameField = "Должность";
            Name = new FOptName();
            PcId = new FOptIDs();
            SubdivisionIDs = new FOptSubdivisionIDs();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_Должности;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("T0.{0}", NameField);
    }
}