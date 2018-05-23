namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    ///     Класс опции поиска документов по изменившему пользователю
    /// </summary>
    public class FOptChangeBy : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? string.Format("(T0.Изменил IN ({0})) ", Value) : string.Empty;
        }
    }
}