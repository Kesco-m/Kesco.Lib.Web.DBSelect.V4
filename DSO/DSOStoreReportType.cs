using Kesco.Lib.Entities;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
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

        public override string SQLBatch
        {
            get
            {
                return SQLQueries.SELECT_ТипОтчётаПоСкладам;
            }
        }

        public override string SQLOrderBy
        {
            get
            {
                return NameField;
            }
        }
    }
}
