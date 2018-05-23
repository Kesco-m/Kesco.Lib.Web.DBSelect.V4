﻿namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.AttributeFormatType
{
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по типу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по типу</returns>
        public string SQLGetClause()
        {
            string[] fields = {"ТипАтрибута", "ТипАтрибутаЛат"};

            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }
    }
}