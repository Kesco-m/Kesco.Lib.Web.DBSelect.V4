using Kesco.Lib.BaseExtention.Enums.Controls;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Resource
{
    /// <summary>
    ///     Класс опции поиска ресурсов по заданным лицам
    /// </summary>
    public class FOptPersonIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Стратегия поиска ресурсов для лиц
        /// </summary>
        public SearchResources SearchStrategy { get; set; }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            var whereStr = string.Empty;

            switch (SearchStrategy)
            {
                case SearchResources.SearchResOnlyForSpecifiedPersons:
                    whereStr = @"(T0.КодРесурса IN (SELECT DISTINCT ResourceID FROM #Resources 
							                        WHERE ResourceID IN (SELECT ResourceID FROM #Resources 
												        GROUP BY ResourceID 
												        HAVING COUNT(*) = (SELECT COUNT(*) FROM @Persons))))";
                    break;

                case SearchResources.SearchAllResForPersons:
                    whereStr =
                        string.Format(
                            @" ( EXISTS(SELECT * FROM РесурсыЛица T1 (nolock) WHERE T0.КодРесурса = T1.КодРесурса AND КодЛица IN({0})) ) ",
                            Value);
                    break;

                default:
                case SearchResources.None:
                    whereStr = string.Empty;
                    break;
            }

            return whereStr;
        }
    }
}