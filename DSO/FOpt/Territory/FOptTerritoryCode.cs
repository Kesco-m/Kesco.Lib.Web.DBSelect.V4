using Kesco.Lib.BaseExtention;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Territory
{
    /// <summary>
    ///     Класс опции поиска по коду территории
    /// </summary>
    public class FOptTerritoryCode : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по коду территории
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по коду территории</returns>
        public string SQLGetClause()
        {
            //string[] fields = {"КодТТерритории"};
            //return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";

            if (Value.IsNullEmptyOrZero()) return string.Empty;

            return $" КодТТерритории IN ({Value})";
        }
    }
}