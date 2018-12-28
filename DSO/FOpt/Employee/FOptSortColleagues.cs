using System.Globalization;
using Kesco.Lib.BaseExtention.Enums.Corporate;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска по сотрудникам, включая виртуальных
    /// </summary>
    public class FOptSortColleagues : FOptBase, IFilterOption
    {
        /// <summary>
        /// Коллеги
        /// </summary>
        public FOptSortColleagues()
        {
            ValueSortColleagues = false;
        }

        /// <summary>
        /// Значение
        /// </summary>
        public bool ValueSortColleagues { get; set; }


        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            var sql = "";
            return sql;
        }
    }
}