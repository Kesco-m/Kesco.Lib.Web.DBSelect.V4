namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Template
{
    /// <summary>
    ///     Опция поиска по Код типа документа
    /// </summary>
    public class FOptCodeTypeDoc : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? string.Format(" КодТипаДокумента = {0}", Value) : string.Empty;
        }
    }
}