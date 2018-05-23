using System.Globalization;
using Kesco.Lib.BaseExtention.Enums.Corporate;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска по состоянию сотрудника, отбираются сотрудники с состоянием меньше указанного
    /// </summary>
    public class FOptStatus : FOptBase, IFilterOption
    {
        private СотоянияСотрудника _value = СотоянияСотрудника.Неважно;

        /// <summary>
        ///     Значение
        /// </summary>
        public override string Value
        {
            get
            {
                if (_value != СотоянияСотрудника.Неважно)
                    return ((int)_value).ToString(CultureInfo.InvariantCulture);
                return "";
            }
        }

        /// <summary>
        ///     Значение
        /// </summary>
        public object ValueStatus
        {
            get { return _value; }
            set { _value = (СотоянияСотрудника)value; }
        }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            return ValueStatus.Equals(СотоянияСотрудника.Неважно) ? "" : string.Format(" T0.Состояние <= {0}", Value);
        }
    }
}