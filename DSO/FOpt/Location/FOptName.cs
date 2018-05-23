namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Location
{
    /// <summary>
    ///     Класс опции поиска по наименованию места хранения
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по расположению
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по расположению</returns>
        public string SQLGetClause()
        {
            var whereStr = Value != null && !string.IsNullOrEmpty(Value)
                ? GetWhereStrBySearchWords("T0.РасположениеPath1", WordsGroup)
                : string.Empty;
            return whereStr;
        }
    }
}