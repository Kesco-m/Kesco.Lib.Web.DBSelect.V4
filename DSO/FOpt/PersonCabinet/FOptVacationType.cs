namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonCabinetType
{
    /// <summary>
    ///     Класс опции поиска по типу личного кабинета
    /// </summary>
    public class FOptPersonCabinetType : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по типу личного кабинета
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по типу личного кабинета</returns>
        public string SQLGetClause()
        {
            string[] fields = { "ТипЛичногоКабинета", "КодТипаЛичногоКабинета" };
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }
    }
}