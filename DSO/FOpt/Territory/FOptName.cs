using System;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Territory
{
    /// <summary>
    ///     Класс опции поиска по имени
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по типу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по типу</returns>
        public string SQLGetClause()
        {
            int res;
            string[] fields = {"Территория", "Caption"};

            var isInt = Int32.TryParse(Value, out res);

            if (isInt)
            {
                return SQLGetStringForTelephoneCodeSelect(res);
            }


            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }

        /// <summary>
        ///     Формирует строку для SQL запроса, для выбора Территории по телефонному коду территории
        /// </summary>
        /// <param name="telephoneCode">Телефонный код территории</param>
        /// <returns>Строка с условием WHERE для ограничения по типу</returns>
        private string SQLGetStringForTelephoneCodeSelect(int telephoneCode)
        {
            return string.Concat("(ТелКодСтраны=", telephoneCode, ")");
        }
    }
}