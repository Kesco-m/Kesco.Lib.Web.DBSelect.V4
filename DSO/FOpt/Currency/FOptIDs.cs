using Kesco.Lib.BaseExtention.Enums.Controls;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Currency
{
    /// <summary>
    ///     Класс опции поиска по указанным кодам валют
    /// </summary>
    public class FOptIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Поле для свойства Column
        /// </summary>
        private string _column = "КодВалюты";

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        /// <param name="column">Наименовние колонки</param>
        public FOptIDs(string column)
        {
            _column = column;
            HowSearch = SearchIds.In;
        }

        /// <summary>
        ///     Параметр фильтрации: условие поиска по компаниям
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        /// </summary>
        public SearchIds HowSearch { get; set; }

        /// <summary>
        ///     Назавание колонки таблицы с идентификатором лица
        /// </summary>
        public string Column
        {
            get { return _column; }
            set { _column = value; }
        }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if (string.IsNullOrEmpty(Value)) return "";
            var clause = "";
            switch (HowSearch)
            {
                case SearchIds.NotIn:
                    clause = "NOT IN";
                    break;
                default:
                    clause = "IN";
                    break;
            }

            return !string.IsNullOrEmpty(Value) ? string.Format(" {0} {1} ({2})", Column, clause, Value) : "";
        }
    }
}