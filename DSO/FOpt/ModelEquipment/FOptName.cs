namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.ModelEquipment
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
            var whereStr = WordsGroup.Count > 0
                ? GetWhereStrBySearchWords("МодельОборудования", WordsGroup)
                : string.Empty;

            return whereStr;
        }
    }
}