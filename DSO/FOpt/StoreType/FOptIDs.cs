namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StoreType
{
    /// <summary>
    ///     Класс опции поиска по указанным кодам типа склада
    /// </summary>
    public class FOptIDs : FOptBase, IFilterOption
    {
        public bool IsException {get; set;}

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if(Value == null || Value.Length <1) return string.Empty;

            string whereStr = IsException ? string.Format(" (T0.КодТипаСклада NOT IN({0}))", Value) : string.Format(" (T0.КодТипаСклада IN({0}))", Value);

            return whereStr;
        }
    }
}