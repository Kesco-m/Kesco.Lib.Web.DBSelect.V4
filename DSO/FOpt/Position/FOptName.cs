namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Position
{
    /// <summary>
    ///     Класс опции поиска
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по типу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по типу</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords("T0.Должность", WordsGroup) : "";
        }
    }
}