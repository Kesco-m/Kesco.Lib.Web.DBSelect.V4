namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Template
{
    /// <summary>
    ///     Опция поиска по Код лица
    /// </summary>
    public class FOptCodePerson : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value)
                ? string.Format(" (КодЛица IS NULL {0})", "OR КодЛица=" + Value)
                : string.Empty;
        }
    }
}