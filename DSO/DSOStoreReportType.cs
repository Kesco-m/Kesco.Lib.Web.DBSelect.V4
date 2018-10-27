using Kesco.Lib.Entities;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    /// Источник данных для контрола выбора типа отчета по складам
    /// </summary>
    public class DSOStoreReportType : DSOCommon
    {
        /// <summary>
        /// Опция фильтра по коду ресурса
        /// </summary>
        [FilterOption("КодРесурса")]
        public FOpt.StoreReportType.FOptResourceIDs ResourceId;

        /// <summary>
        /// Опция фильтра по коду лица
        /// </summary>
        [FilterOption("КодЛица")]
        public FOpt.StoreReportType.FOptPersonIDs PcId;

        /// <summary>
        /// Опция фильтра по имени
        /// </summary>
        [FilterOption("ТипОтчётаПоСкладам")]
        public FOpt.StoreReportType.FOptName Name;

        /// <summary>
        /// 
        /// </summary>
        public DSOStoreReportType()
        {
            PcId = new FOpt.StoreReportType.FOptPersonIDs();
            ResourceId = new FOpt.StoreReportType.FOptResourceIDs();
            Name = new FOpt.StoreReportType.FOptName();

            IsAddExcludeCondition = false;
            KeyField = "КодТипаОтчётаПоСкладам";
            NameField = "ТипОтчётаПоСкладам";
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get
            {
                return SQLQueries.SELECT_ТипОтчётаПоСкладам;
            }
        }

        /// <summary>
        /// Поле сортировки
        /// </summary>
        public override string SQLOrderBy
        {
            get
            {
                return NameField;
            }
        }
    }
}
