namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StavkaNDS
{
    public class FOptTerritoryCode : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по роли
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по роли</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? string.Format(" (КодТерритории = {0})", Value) : string.Empty;
        }
    }
}