namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonTheme
{
    /// <summary>
    ///     Класс опции ограничений тем лиц по каталогам
    /// </summary>
    public class FOptCatalog : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Подзапрос, получающий ограничение по каталогу
        /// </summary>
        /// <returns>Возврат строки подзапроса</returns>
        public string SQLGetClause()
        {
            var sql = "";
            if (Value.Length > 0)
                sql = string.Format(
                    "T0.КодТемыЛица IN (SELECT КодТемыЛица FROM ТипыЛиц WHERE КодКаталога IN({0})) OR T0.КодТемыЛица=0",
                    Value);
            return sql;
        }
    }
}