namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.SimCard
{
    /// <summary>
    ///     Класс опции поиска моделей оборудования по названию
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            //return !string.IsNullOrEmpty(Value) ? $"(МодельОборудования LIKE '%{Value}%')" : string.Empty;
            var whereStr = WordsGroup.Count > 0
                ? GetWhereStrBySearchWords("НомерТелефона", WordsGroup)
                : string.Empty;

            return whereStr;
        }
    }
}