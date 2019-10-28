namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    ///     Класс опции поиска документов по документам
    /// </summary>
    public class FOptHasForm : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? " EXISTS (SELECT * FROM vwДокументыДанные T2 WHERE T2.КодДокумента = T0.КодДокумента )" : string.Empty;
        }
    }
}