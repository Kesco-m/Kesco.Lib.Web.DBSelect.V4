namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Currency
{
	/// <summary>
	///     Класс опции поиска по имени
	/// </summary>
	public class FOptName : FOptBase, IFilterOption
	{
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="column">Название колонки</param>
		public FOptName(string column)
		{
			_column = column;
		}

		//Поле для свойства Column
		string _column;

        /// <summary>
        /// Назавание колонки таблицы с названием сущности
        /// </summary>
		public string Column { get { return _column; } set { _column = value; } }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
		public string SQLGetClause()
		{
			if (string.IsNullOrEmpty(Column)) return string.Empty;
			if (string.IsNullOrEmpty(Value)) return string.Empty;

			return GetWhereStrBySearchWords(Column, WordsGroup);
		}
	}
}