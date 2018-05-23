using System.Globalization;
using Kesco.Lib.BaseExtention.Enums.Corporate;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска по сотрудникам, имеющим учетную запись
    /// </summary>
    public class FOptHasEmail : FOptBase, IFilterOption
    {
        private НаличиеEmail _value = НаличиеEmail.Неважно;

        /// <summary>
        ///     Значение
        /// </summary>
        public object ValueHasEmail
        {
            get { return _value; }
            set { _value = (НаличиеEmail) value; }
        }

        /// <summary>
        ///     Приведение значения enum НаличиеEmail к строке
        /// </summary>
        public override string Value
        {
            get
            {
                if (_value != НаличиеEmail.Неважно)
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
            if (_value.Equals(НаличиеEmail.ЕстьEmail))
                sql = " (T0.Email <> '')";
            else if (_value.Equals(НаличиеEmail.НетEmail))
                sql = " (T0.Email = '')";
            return sql;
        }
    }
}