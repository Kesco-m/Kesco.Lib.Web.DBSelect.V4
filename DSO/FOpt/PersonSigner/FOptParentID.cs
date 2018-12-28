namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonSigner
{
    /// <summary>
    ///     Класс опции поиска
    /// </summary>
    public class FOptParentID : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? string.Format(" КодЛицаРодителя = {0}", Value) : "";
        }
    }
}