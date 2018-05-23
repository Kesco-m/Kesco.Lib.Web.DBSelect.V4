namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска по указанным кодам Должности
    /// </summary>
    public class FOptPositionIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Параметр фильтрации: условие поиска по Должности
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        ///     2 - Любое значение (не фильтруем по полю),
        ///     3 - Значение не указано (значение поля NULL)
        /// </summary>
        public string PositionHowSearch { get; set; }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if (string.IsNullOrEmpty(Value) && PositionHowSearch != "3" && PositionHowSearch != "2") return "";
            var clause = "";
            switch (PositionHowSearch)
            {
                case "0":
                    clause = "IN";
                    break;
                case "1":
                    clause = "NOT IN";
                    break;
                case "2":
                    return " (T1.Должность IS NOT NULL AND T1.Должность <> '')";
                case "3":
                    return " (T1.Должность IS NULL OR T1.Должность = '')";
            }
            return !string.IsNullOrEmpty(Value) ? string.Format(" T1.Должность {1} ({0})", Value, clause) : "";
        }
    }
}