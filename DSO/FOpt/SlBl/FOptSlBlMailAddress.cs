namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.SlBl
{
    /// <summary>
    ///     Класс опции поиска по адрсеу SlBl
    /// </summary>
    public class FOptSlBlMailAddress : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по адресу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по адресу</returns>
        public string SQLGetClause()
        {
            string[] fields = { "Address"};
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }
    }

}

