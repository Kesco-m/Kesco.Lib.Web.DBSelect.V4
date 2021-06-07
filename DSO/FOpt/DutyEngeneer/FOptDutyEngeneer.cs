namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.DutyEngeneer
{   
    /// <summary>
    ///     Класс опции поиска по дежурному инженеру
    /// </summary>
    public class FOptDutyEngeneer : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по сотруднику
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по сотруднику</returns>
        public string SQLGetClause()
        {
            string[] fields = { "Сотрудник", "КодСотрудника" };
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }
    }
}
