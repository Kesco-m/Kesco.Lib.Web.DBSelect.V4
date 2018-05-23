namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common
{
	/// <summary>
	///     Класс опции поиска по имени
	/// </summary>
	public class FOptName : FOptBase, IFilterOption
	{
		public FOptName()
		{
		}

		public FOptName(string column)
		{
			_column = column;
		}

		//Поле для свойства Column
		string _column;

		//Назавание колонки таблицы с названием сущности
		public string Column { get { return _column; } set { _column = value; } }

		public string SQLGetClause()
		{
			if (string.IsNullOrEmpty(Column)) return string.Empty;
			if (string.IsNullOrEmpty(Value)) return string.Empty;

			return GetWhereStrBySearchWords("T0." + Column, WordsGroup);
		}
	}
}