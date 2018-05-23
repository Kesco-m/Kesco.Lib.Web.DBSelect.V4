
namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common
{
    /// <summary>
    /// Класс опции поиска по указанным кодам компаний
    /// </summary>
    public class FOptIDs : FOptBase, IFilterOption
    {
		public FOptIDs(string column)
		{
			_column = column;
			CompanyHowSearch = "0";
		}

        /// <summary>
        /// Параметр фильтрации: условие поиска по компаниям
        /// 0 - Элементы из списка, 
        /// 1 - Элементы за исключением, 
        /// 2 - Любое значение (не фильтруем по полю), 
        /// 3 - Значение не указано (значение поля NULL)
        /// </summary>
        public string CompanyHowSearch { get; set; }

		//Поле для свойства Column
		string _column = "КодСклада";

		//Назавание колонки таблицы с идентификатором лица
		public string Column { get { return _column; } set { _column = value; } }

        /// <summary>
        /// Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if (string.IsNullOrEmpty(Value) && CompanyHowSearch != "3") return "";
            string clause = "";
            switch (CompanyHowSearch)
            {
                case "0": clause = "IN"; break;
                case "1": clause = "NOT IN"; break;
                case "2": return "";
				case "3": return " T0.КодСклада IS NULL";
            }
			return !string.IsNullOrEmpty(Value) ? string.Format(" T0.{0} {2} ({1})", Column, Value, clause) : "";
        }
    }
}
