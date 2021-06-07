namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Socket
{
    /// <summary>
    ///     Класс опции поиска розетки по названию
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            var whereStr = Value != null && !string.IsNullOrEmpty(Value)
                ? GetWhereStrBySearchWords("Розетка", WordsGroup)
                : string.Empty;
            return whereStr;
        }
    }
}