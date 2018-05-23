using Kesco.Lib.BaseExtention.Enums.Controls;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Location
{
    /// <summary>
    ///     Класс опции поиска по указанным родительским кодам мест хранения, включая родителей
    /// </summary>
    public class FOptParentsIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построитель строк запросов к древовидным таблицам
        /// </summary>
        private readonly TreeQueryBuilder qBuilder;

        /// <summary>
        ///     Конструктор опции
        /// </summary>
        /// <param name="qType">Тип запроса</param>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="keyField">Имя ключевого поля таблицы</param>
        /// <param name="nameField">Имя поля наименования</param>
        public FOptParentsIDs(TreeQueryType qType, string tableName, string keyField, string nameField = "")
        {
            qBuilder = new TreeQueryBuilder(qType, tableName, keyField, nameField);
        }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            return Value != null && !string.IsNullOrEmpty(Value)
                ? qBuilder.GetQueryString(Value)
                : string.Empty;
        }
    }
}