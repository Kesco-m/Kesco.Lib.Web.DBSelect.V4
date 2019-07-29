using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StoreReportType;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для контрола выбора типа отчета по складам
    /// </summary>
    public class DSOStoreReportType : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени
        /// </summary>
        [FilterOption("ТипОтчётаПоСкладам")] public FOptName Name;

        /// <summary>
        ///     Опция фильтра по коду лица
        /// </summary>
        [FilterOption("КодЛица")] public FOptPersonIDs PcId;

        /// <summary>
        ///     Опция фильтра по коду ресурса
        /// </summary>
        [FilterOption("КодРесурса")] public FOptResourceIDs ResourceId;

        /// <summary>
        /// </summary>
        public DSOStoreReportType()
        {
            PcId = new FOptPersonIDs();
            ResourceId = new FOptResourceIDs();
            Name = new FOptName();

            IsAddExcludeCondition = false;
            KeyField = "КодТипаОтчётаПоСкладам";
            NameField = "ТипОтчётаПоСкладам";
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ТипОтчётаПоСкладам;

        /// <summary>
        ///     Поле сортировки
        /// </summary>
        public override string SQLOrderBy => NameField;
    }
}