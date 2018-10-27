namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StavkaNDS
{
    /// <summary>
    ///     Класс опции поиска по ставке НДС
    /// </summary>
    public class FOptStavka : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по роли
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по роли</returns>
        public string SQLGetClause()
        {
            string[] fields = {"СтавкаНДС", "КодСтавкиНДС"};
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }
    }
}