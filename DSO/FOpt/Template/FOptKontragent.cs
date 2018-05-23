namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Template
{
    /// <summary>
    ///     Опция поиска по Код контрагента
    /// </summary>
    public class FOptKontragent : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value)
                ? string.Format(" (КодКонтрагента IS NULL {0})", "OR КодКонтрагента=" + Value)
                : string.Empty;
        }
    }
}