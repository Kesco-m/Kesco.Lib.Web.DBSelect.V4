namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Resource
{
    /// <summary>
    ///     Класс опции поиска по указанным кодам ресурсов
    /// </summary>
    public class FOptIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? string.Format(" (T0.КодРесурса IN({0}))", Value) : string.Empty;
        }
    }
}