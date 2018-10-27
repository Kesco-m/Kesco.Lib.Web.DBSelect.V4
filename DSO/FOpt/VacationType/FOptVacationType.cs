namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.VacationType
{
    /// <summary>
    ///     Класс опции поиска по виду отпуска
    /// </summary>
    public class FOptVacationType : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по виду отпуска
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по виду отпуска</returns>
        public string SQLGetClause()
        {
            string[] fields = {"ВидОтпуска", "КодВидаОтпуска"};
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }
    }
}