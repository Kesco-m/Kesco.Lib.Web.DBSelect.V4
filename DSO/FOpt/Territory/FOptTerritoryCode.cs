using System;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Territory
{
    public class FOptTerritoryCode : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по типу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по типу</returns>
        public string SQLGetClause()
        {
            string[] fields = { "Территория", "КодТТерритории" };
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }

    }
}