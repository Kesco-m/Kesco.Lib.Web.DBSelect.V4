namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    ///     Класс опции поиска документов по документам в работе заданного пользователя
    /// </summary>
    public class FOptAtWork : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value)
                ? string.Format(
                    "( EXISTS (SELECT T1.КодДокумента FROM Документы.dbo.vwДокументыВРаботе T1 (nolock) WHERE T1.КодДокумента = T0.КодДокумента AND КодСотрудника = {0}) )",
                    Value)
                : string.Empty;
        }
    }
}