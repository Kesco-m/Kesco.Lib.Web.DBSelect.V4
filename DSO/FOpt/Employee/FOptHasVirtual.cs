using System.Globalization;
using Kesco.Lib.BaseExtention.Enums.Corporate;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска по сотрудникам, включая виртуальных
    /// </summary>
    public class FOptHasVirtual : FOptBase, IFilterOption
    {
        private ВиртуальныйСотрудник _value = ВиртуальныйСотрудник.Неважно;

        /// <summary>
        ///     Значение
        /// </summary>
        public object ValueHasVirtual
        {
            get { return _value; }
            set { _value = (ВиртуальныйСотрудник) value; }
        }

        /// <summary>
        ///     Приведение значения enum ВиртуальныйСотрудник к строке
        /// </summary>
        public override string Value
        {
            get
            {
                if (_value != ВиртуальныйСотрудник.Неважно)
                    return ((int) _value).ToString(CultureInfo.InvariantCulture);
                return "";
            }
        }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            var sql = "";
            if (_value.Equals(ВиртуальныйСотрудник.ИсключитьВиртуальныхСотрудников))
                sql =
                    " NOT EXISTS (SELECT T1.КодСотрудника FROM Сотрудники AS T1 WHERE T0.КодСотрудника = T1.КодСотрудника AND T1.КодЛица IS NULL AND T1.Состояние = 0)";
            return sql;
        }
    }
}