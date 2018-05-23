namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonTheme
{
    /// <summary>
    /// Класс опции поиска темы по названию
    /// </summary>
    public class FOptPersonTheme : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по имени
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по имени</returns>
        public string SQLGetClause()
        {
            string[] fields = {"ТемаЛица"};
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }
    }
}