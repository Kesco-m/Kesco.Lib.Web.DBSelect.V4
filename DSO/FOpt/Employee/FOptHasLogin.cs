using System.Globalization;
using Kesco.Lib.BaseExtention.Enums.Corporate;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска по сотрудникам, имеющим учетную запись
    /// </summary>
    public class FOptHasLogin : FOptBase, IFilterOption
    {

        private НаличиеЛогина _value = НаличиеЛогина.Неважно;

        /// <summary>
        ///     Значение
        /// </summary>
        public object ValueHasLogin
        {
            get { return _value; }
            set { _value = (НаличиеЛогина) value; }
        }

        /// <summary>
        ///     Приведение значения enum НаличиеЛогина к строке
        /// </summary>
        public override string Value
        {
            get
            {
                if (_value != НаличиеЛогина.Неважно)
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
            if (_value.Equals(НаличиеЛогина.ЕстьЛогин))
                sql = " (T0.Login <> '' OR EXISTS(SELECT * FROM Сотрудники X1 WHERE X1.КодОбщегоСотрудника = T0.КодСотрудника AND X1.Login <> ''))";
            else if (_value.Equals(НаличиеЛогина.НетЛогина))
                sql = " T0.Login = ''";
            return sql;
        }
    }
}