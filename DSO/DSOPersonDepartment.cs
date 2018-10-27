using Kesco.Lib.Entities;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    /// 
    /// </summary>
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
        /// Конструктор класса
        /// </summary>
        public DSOPersonDepartment()
        {
            KeyField = "КодПодразделенияЛица";
            NameField = "Подразделение";
            PcId = new FOpt.PersonDepartment.FOptIDs();
            Name = new FOpt.PersonDepartment.FOptName();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>        
        public override string SQLBatch
        {
            get
            {
                return SQLQueries.SELECT_ПодразделенияЛиц;
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
