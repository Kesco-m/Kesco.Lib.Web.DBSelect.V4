namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StoreReportType
{
    /// <summary>
    ///     Класс опции поиска по имени
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по имени
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по имени</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords("ТипОтчётаПоСкладам", WordsGroup) : "";
        }
    }
}