using System.Globalization;
using Kesco.Lib.BaseExtention.Enums.Corporate;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Location
{
    /// <summary>
    ///     Класс опции поиска по типу рабочего места расположения
    /// </summary>
    public class FOptWorkPlace : FOptBase, IFilterOption
    {
        private int _value = 1;

        /// <summary>
        ///     Значение
        /// </summary>
        public override string Value => _value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        ///     Значение через Enum ТипыРабочихМест
        /// </summary>
        public object ValueType
        {
            get { return (ТипыРабочихМест) _value; }
            set { _value = (int) value; }
        }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            return string.Format(" T0.РабочееМесто = {0}", Value);
        }
    }
}