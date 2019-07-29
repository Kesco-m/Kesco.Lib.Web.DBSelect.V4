namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Store
{
    /// <summary>
    ///     Класс опции поиска по указанной дате
    /// </summary>
    public class FOptValidAt : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if (!string.IsNullOrEmpty(Value))
                return string.Format(
                    @"((T0.От IS NULL OR T0.От <= '{0}')
AND (T0.До IS NULL OR T0.До > '{0}')
AND (T0.КодХранителя IS NULL OR EXISTS( SELECT NULL FROM fn_ДействующиеЛица('{0}') WHERE T0.КодХранителя=КодЛица))
AND (T0.КодРаспорядителя IS NULL OR EXISTS( SELECT NULL FROM fn_ДействующиеЛица('{0}') WHERE T0.КодРаспорядителя=КодЛица)))"
                    , SqlEscape(Value));
            return string.Empty;
        }
    }
}