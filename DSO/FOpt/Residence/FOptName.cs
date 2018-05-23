namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Residence
{
    /// <summary>
    ///     Класс опции поиска по наименованию места хранения
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по месту хранения
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по месту хранения</returns>
        public string SQLGetClause()
        {
            var whereStr = Value != null && !string.IsNullOrEmpty(Value)
                ? GetWhereStrBySearchWords("T0.МестоХранения", WordsGroup)
                : string.Empty;
            return whereStr;
        }
    }
}