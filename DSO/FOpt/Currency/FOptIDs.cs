
namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Currency
{
    /// <summary>
    /// Класс опции поиска по указанным кодам валют
    /// </summary>
    public class FOptIDs : FOptBase, IFilterOption
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="column">Наименовние колонки</param>
		public FOptIDs(string column)
		{
			_column = column;
			HowSearch = BaseExtention.Enums.Controls.SearchIds.In;
		}

        /// <summary>
        /// Параметр фильтрации: условие поиска по компаниям
        /// 0 - Элементы из списка, 
        /// 1 - Элементы за исключением,              
        /// </summary>
        public BaseExtention.Enums.Controls.SearchIds HowSearch { get; set; }

		/// <summary>
        /// Поле для свойства Column
		/// </summary>
        string _column = "КодВалюты";

        /// <summary>
        /// Назавание колонки таблицы с идентификатором лица
        /// </summary>
		public string Column { get { return _column; } set { _column = value; } }

        /// <summary>
        /// Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if (string.IsNullOrEmpty(Value)) return "";
            string clause = "";
            switch (HowSearch)
            {                
                case BaseExtention.Enums.Controls.SearchIds.NotIn: clause = "NOT IN"; break;
                default: clause = "IN"; break;

            }
			return !string.IsNullOrEmpty(Value) ? string.Format(" {0} {1} ({2})", Column,  clause, Value) : "";
        }
    }
}
