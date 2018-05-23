using Kesco.Lib.Entities;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    public class DSOPersonDepartment : DSOCommon
    {
        /// <summary>
        /// Опция фильтра по имени
        /// </summary>
        [FilterOption("Подразделение")]
        public FOpt.PersonDepartment.FOptName Name;

        /// <summary>
        /// Опция фильтра по ID
        /// </summary>
        [FilterOption("КодЛица")]
        public FOpt.PersonDepartment.FOptIDs PcId;

        /// <summary>
        /// 
        /// </summary>
        public DSOPersonDepartment()
        {
            KeyField = "КодПодразделенияЛица";
            NameField = "Подразделение";
            PcId = new FOpt.PersonDepartment.FOptIDs();
            Name = new FOpt.PersonDepartment.FOptName();
        }

        public override string SQLBatch
        {
            get
            {
                return SQLQueries.SELECT_ПодразделенияЛиц;
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
