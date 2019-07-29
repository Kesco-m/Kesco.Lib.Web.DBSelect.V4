using System.Globalization;
using Kesco.Lib.BaseExtention;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Resource
{
    /// <summary>
    ///     Класс опции поиска по наименованию ресурса
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по ресурсу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по ресурсу</returns>
        public string SQLGetClause()
        {
            var whereStr = Value != null && !string.IsNullOrEmpty(Value.ToString(CultureInfo.InvariantCulture))
                ? GetWhereStrBySearchWords("T0.РесурсRL", WordsGroup, true)
                : string.Empty;

            if (Value != null && !string.IsNullOrEmpty(Value.ToString(CultureInfo.InvariantCulture)) && Value.IsDigit())
                whereStr = $"({whereStr} OR T0.КодРесурса={Value})";

            return whereStr;
        }
    }
}