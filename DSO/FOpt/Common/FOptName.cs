namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common
{
	/// <summary>
	///     Класс опции поиска по имени
	/// </summary>
	public class FOptName : FOptBase, IFilterOption
	{
        /// <summary>
        /// Конструктор
        /// </summary>
        public FOptName()
        {
        }

        /// <summary>
        ///     Фильтр по имени атрибута
        /// </summary>
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
        ///     Построение условия WHERE для ограничения по типу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по типу</returns>
		public string SQLGetClause()
		{
			if (string.IsNullOrEmpty(Column)) return string.Empty;
			if (string.IsNullOrEmpty(Value)) return string.Empty;

			return GetWhereStrBySearchWords("T0." + Column, WordsGroup);
		}
	}
}