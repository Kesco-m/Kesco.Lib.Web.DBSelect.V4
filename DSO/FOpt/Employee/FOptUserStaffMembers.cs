using System.Globalization;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска сотрудника, включая его должность
    /// </summary>
    public class FOptUserStaffMembers : FOptBase, IFilterOption
    {
        private string _value = "false";

        /// <summary>
        ///     Значение
        /// </summary>
        public override string Value
        {
            get { return _value.ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>
        ///     Значение
        /// </summary>
        public object ValueStatus
        {
            get { return _value; }
            set { _value = (string) value; }
        }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            return "";
        }
    }
}