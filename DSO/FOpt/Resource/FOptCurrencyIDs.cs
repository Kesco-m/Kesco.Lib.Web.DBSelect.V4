namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Resource
{
    /// <summary>
    ///     Класс опции поиска по указанным кодам ресурсов, которые представляют собой валюту
    /// </summary>
    public class FOptCurrencyIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) && (Value == "1" || Value.ToLower() == "true")
                ? " (EXISTS(SELECT T1.КодВалюты FROM Валюты T1 WHERE T1.КодВалюты = T0.КодРесурса)) "
                : string.Empty;
        }
    }
}