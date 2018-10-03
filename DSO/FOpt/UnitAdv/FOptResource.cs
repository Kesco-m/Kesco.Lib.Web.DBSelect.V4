namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.UnitAdv
{
    /// <summary>
    ///     Класс опции поиска
    /// </summary>
    public class FOptResource : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по ресурсу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по ресурсу</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? string.Format(" (КодРесурса = {0})", Value) : string.Empty;
        }
    }
}