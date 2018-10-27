namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonLink
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
            string[] fields = {"Описание"};

            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }
    }
}