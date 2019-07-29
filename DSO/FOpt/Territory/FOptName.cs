using Kesco.Lib.BaseExtention;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Territory
{
    /// <summary>
    ///     Класс опции поиска по имени
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по типу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по типу</returns>
        public string SQLGetClause()
        {
            string[] fields = {"Территория", "Caption"};

            if (!string.IsNullOrEmpty(Value) && Value.IsDigit())
                return $"(ТелКодСтраны = {Value})";

            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }
    }
}