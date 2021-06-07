namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Equipment
{
    /// <summary>
    ///     Класс опции поиска оборудования по типу оборудования
    /// </summary>
    public class FOptTypeIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? string.Format(" (T2.КодТипаОборудования IN ({0}))", Value) : string.Empty;
        }
    }
}


