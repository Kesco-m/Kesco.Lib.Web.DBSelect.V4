namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common
{
    /// <summary>
    ///     Класс опции поиска по имени
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        //Поле для свойства Column

        /// <summary>
        ///     Конструктор
        /// </summary>
        public FOptName()
        {
        }

        /// <summary>
        ///     Фильтр по имени атрибута
        /// </summary>
        public FOptName(string column)
        {
            Column = column;
        }

        /// <summary>
        ///     Назавание колонки таблицы с названием сущности
        /// </summary>
        public string Column { get; set; }

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