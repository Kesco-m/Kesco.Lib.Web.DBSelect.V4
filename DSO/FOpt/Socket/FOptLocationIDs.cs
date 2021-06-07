namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Socket
{
    /// <summary>
    ///     Класс опции поиска розетки по расположению
    /// </summary>
    public class FOptLocationIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? string.Format(" (T0.КодРасположения IN ({0}))", Value) : string.Empty;
        }
    }
}


