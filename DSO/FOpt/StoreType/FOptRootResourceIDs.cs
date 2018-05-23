namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StoreType
{
    /// <summary>
    ///     Класс опции поиска по указанным кодам корневых ресурсов типа склада
    /// </summary>
    public class FOptRootResourceIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            var whereStr = Value != null && !string.IsNullOrEmpty(Value)
                ? string.Format(" (T0.КорневойРесурс IN({0}))", Value)
                : string.Empty;
            return whereStr;
        }
    }
}