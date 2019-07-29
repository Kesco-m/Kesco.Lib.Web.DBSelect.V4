using System;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StavkaNDS
{
    /// <summary>
    ///     Класс опции поиска по коду территории
    /// </summary>
    public class FOptValid : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по коду территории
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по коду территории</returns>
        public string SQLGetClause()
        {
            if (!string.IsNullOrEmpty(Value) && Value != DateTime.MinValue.ToString("yyyyMMdd"))
                return !string.IsNullOrEmpty(Value)
                    ? string.Format(" (CONVERT(date, CONVERT(varchar, Действует) + '1231') >= '{0}')", Value)
                    : string.Empty;
            return string.Empty;
        }
    }
}