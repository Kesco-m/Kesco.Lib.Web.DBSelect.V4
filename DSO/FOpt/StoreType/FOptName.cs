namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StoreType
{
    /// <summary>
    ///     Класс опции поиска по наименованию типа склада
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по типу склада
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по типу склада</returns>
        public string SQLGetClause()
        {
            var whereStr = WordsGroup.Count > 0
                ? GetWhereStrBySearchWords("T0.ТипСклада", WordsGroup)
                : string.Empty;

            return whereStr;
        }
    }
}