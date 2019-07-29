namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StoreReportType
{
    /// <summary>
    ///     Класс опции поиска
    /// </summary>
    public class FOptResourceIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if (string.IsNullOrEmpty(Value)) return string.Empty;
            return string.Format(
                "ТО.КодРесурса IN ( SELECT Root.КодРесурса FROM Ресурсы Root INNER JOIN Ресурсы Child ON Root.L<=Child.L AND Root.R>=Child.R WHERE Child.КодРесурса IN ({0}))",
                Value);
        }
    }
}