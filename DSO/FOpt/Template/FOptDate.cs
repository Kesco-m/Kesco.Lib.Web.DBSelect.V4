namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Template
{
    /// <summary>
    ///     Класс опции поиска шаблонов документов по дате
    /// </summary>
    public class FOptDate : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                return string.Format(@"(От <= '{0}' AND До <= '{0}')", Value);
            }
            return string.Empty;
        }
    }
}