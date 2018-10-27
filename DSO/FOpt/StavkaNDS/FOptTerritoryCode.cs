namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StavkaNDS
{
    /// <summary>
    /// Класс опции поиска по коду территории
    /// </summary>
    public class FOptTerritoryCode : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по коду территории
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по коду территории</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? string.Format(" (КодТерритории = {0})", Value) : string.Empty;
        }
    }
}