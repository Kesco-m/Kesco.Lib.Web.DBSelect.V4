namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    ///     Класс опции поиска документов по коду
    /// </summary>
    public class FOptIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     исключает указанные Ids
        /// </summary>
        public bool Inverse { get; set; }

        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            if (Inverse)
                return !string.IsNullOrEmpty(Value)
                    ? string.Format(" (T0.КодДокумента NOT IN({0}))", Value)
                    : string.Empty;

            return !string.IsNullOrEmpty(Value) ? string.Format(" (T0.КодДокумента IN({0}))", Value) : string.Empty;
        }
    }
}