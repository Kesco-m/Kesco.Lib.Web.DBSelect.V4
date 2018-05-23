namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.VacationType
{
    /// <summary>
    /// Класс опции поиска по указанным кодам компаний
    /// </summary>
    public class FOptIDs : FOptBase, IFilterOption
    {
        /// <summary>
        /// Параметр фильтрации: условие поиска по компаниям
        /// 0 - Элементы из списка, 
        /// 1 - Элементы за исключением, 
        /// 2 - Любое значение (не фильтруем по полю), 
        /// 3 - Значение не указано (значение поля NULL)
        /// </summary>
        public string HowSearch { get; set; }

        /// <summary>
        /// Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if (string.IsNullOrEmpty(Value) && HowSearch != "3") return "";
            string clause = "";
            switch (HowSearch)
            {
                case "0": clause = "IN"; break;
                case "1": clause = "NOT IN"; break;
                case "2": return "";
                case "3": return " КодВидаОтпуска IS NULL";
            }
            return !string.IsNullOrEmpty(Value) ? string.Format(" КодВидаОтпуска {1} ({0})", Value, clause) : "";
        }
    }
}
