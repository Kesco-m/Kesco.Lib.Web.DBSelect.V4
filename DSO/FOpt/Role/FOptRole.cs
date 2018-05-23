namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Role
{
    public class FOptRole : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по роли
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по роли</returns>
        public string SQLGetClause()
        {
            string[] fields = {"Роль", "КодРоли"};
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }
    }
}