namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonRole
{
    /// <summary>
    ///     Класс опции поиска
    /// </summary>
    public class FOptPersonRole : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по типу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по типу</returns>
        public string SQLGetClause()
        {
            var subQuery =
                "(EXISTS(SELECT * FROM Инвентаризация.dbo.РолиСотрудников РолиСотрудников WHERE T0.КодЛица = РолиСотрудников.КодЛица";
            if (!string.IsNullOrEmpty(Value))
                subQuery += string.Format(" AND КодРоли={0}", Value);
            subQuery += "))";

            return subQuery;
        }
    }
}