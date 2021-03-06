﻿namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска по указанным кодам
    /// </summary>
    public class FOptIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Параметр фильтрации: условие поиска
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        ///     2 - Любое значение (не фильтруем по полю),
        ///     3 - Значение не указано (значение поля NULL)
        /// </summary>
        public string EmployeeHowSearch { get; set; }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if (string.IsNullOrEmpty(Value) && EmployeeHowSearch != "3") return "";
            var clause = "";
            switch (EmployeeHowSearch)
            {
                case "0":
                    clause = "IN";
                    break;
                case "1":
                    clause = "NOT IN";
                    break;
                case "2":
                    return "";
                case "3":
                    return " T0.КодСотрудника IS NULL";
            }

            return !string.IsNullOrEmpty(Value) ? string.Format(" T0.КодСотрудника {1} ({0})", Value, clause) : "";
        }
    }
}