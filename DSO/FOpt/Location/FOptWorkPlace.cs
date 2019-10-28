using System.Globalization;
using Kesco.Lib.BaseExtention.Enums.Corporate;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Location
{
    /// <summary>
    ///     Класс опции поиска по типу рабочего места расположения
    /// </summary>
    public class FOptWorkPlace : FOptBase, IFilterOption
    {
        private int _valueType = 1;
        private string _value;

        /// <summary>
        ///     Значение
        /// </summary>
        //public override string Value => _value.ToString(CultureInfo.InvariantCulture);
        public override string Value {
            get
            {
                if (string.IsNullOrEmpty(_value))
                    return "1";
                return _value.ToString(CultureInfo.InvariantCulture);
            }
            set { _value = value; }
        }

        /// <summary>
        ///     Значение через Enum ТипыРабочихМест
        /// </summary>
        public object ValueType
        {
            get { return (ТипыРабочихМест) _valueType; }
            set { _valueType = (int) value; }
        }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            return string.Format(" T0.РабочееМесто IN ({0})", Value);
        }
    }
}