namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PhoneAttributeType
{ 
    /// <summary>
    ///     Класс опции поиска типов атрибутов телефонов по источнику значения
    /// </summary>
    public class FOptValuesSource : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? string.Format(" (ИсточникЗначений = {0})", Value) : string.Empty;
        }
    }
}