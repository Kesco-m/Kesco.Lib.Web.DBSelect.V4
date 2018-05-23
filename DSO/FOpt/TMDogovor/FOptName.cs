namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.TMDogovor
{
    /// <summary>
    ///     Класс опции поиска по введенному в контрол тексту
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            var sql = Value == null && string.IsNullOrEmpty(Value)
                ? ""
                : string.Format("(T0.Договор LIKE '%{0}%')", Value);

            return sql;
        }
    }
}