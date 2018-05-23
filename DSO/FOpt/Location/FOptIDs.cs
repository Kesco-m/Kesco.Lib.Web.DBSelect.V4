namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Location
{
    /// <summary>
    ///     Класс опции поиска по указанным кодам мест хранения
    /// </summary>
    public class FOptIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            var whereStr = Value != null && !string.IsNullOrEmpty(Value)
                ? string.Format(" (T0.КодРасположения IN({0}))", Value)
                : string.Empty;

            return whereStr;
        }
    }
}