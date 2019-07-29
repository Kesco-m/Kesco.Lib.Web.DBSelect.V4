﻿namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска по указанным кодам Подразделения
    /// </summary>
    public class FOptSubdivisionIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Параметр фильтрации: условие поиска по подразделениям
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        ///     2 - Любое значение (не фильтруем по полю),
        ///     3 - Значение не указано (значение поля NULL)
        /// </summary>
        public string SubdivisionHowSearch { get; set; }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if (string.IsNullOrEmpty(Value) && SubdivisionHowSearch != "3" && SubdivisionHowSearch != "2") return "";
            var clause = "";

            switch (SubdivisionHowSearch)
            {
                case "0":
                    clause = "IN";
                    break;
                case "1":
                    clause = "NOT IN";
                    break;
                case "2":
                    return
                        " EXISTS (SELECT P0.КодДолжности FROM vwДолжности AS P0 WHERE (T1.L >= P0.L AND T1.R <= P0.R) AND (P0.Подразделение IS NOT NULL AND P0.Подразделение <> ''))";
                case "3":
                    return
                        " EXISTS (SELECT P0.КодДолжности FROM vwДолжности AS P0 WHERE (T1.L >= P0.L AND T1.R <= P0.R) AND (P0.Подразделение IS NULL OR P0.Подразделение = ''))";
            }

            return !string.IsNullOrEmpty(Value)
                ? string.Format(
                    " EXISTS (SELECT P0.КодДолжности FROM vwДолжности AS P0 WHERE (T1.L >= P0.L AND T1.R <= P0.R) AND P0.Подразделение {1} ({0}))",
                    Value, clause)
                : "";
        }
    }
}