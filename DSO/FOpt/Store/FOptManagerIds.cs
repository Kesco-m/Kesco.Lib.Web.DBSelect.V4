namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Store
{
    /// <summary>
    ///     Класс опции поиска по распорядителям складов
    /// </summary>
    public class FOptManagerIds : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Добавить к поиску виртуальный склад
        /// </summary>
        public bool AddVirtual { get; set; }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if (string.IsNullOrEmpty(Value) && !AddVirtual) return "";

            var whereStr =
                $"({(!string.IsNullOrEmpty(Value) ? $"T0.КодРаспорядителя IN({Value})" : "")}{(AddVirtual ? " OR T0.КодРаспорядителя IS NULL" : "")})";

            return whereStr;
        }
    }
}