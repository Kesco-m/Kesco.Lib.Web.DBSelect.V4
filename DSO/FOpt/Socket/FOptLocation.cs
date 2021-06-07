namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Socket
{
    /// <summary>
    ///     Класс опции поиска розетки расположения
    /// </summary>
    public class FOptLocation : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public FOptLocation()
        {
            Child = false;
        }

        /// <summary>
        ///     Использовать опцию при поиске
        /// </summary>
        public bool Child { get; set; }

        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            if (Child)
                return $@"(EXISTS(SELECT Child.* FROM vwРасположения Parent
                            INNER JOIN vwРасположения Child ON Parent.L <= Child.L AND Parent.R >= Child.R
                            WHERE Parent.КодРасположения = {Value} AND Child.КодРасположения = T0.КодРасположения))"; 
            else
                return !string.IsNullOrEmpty(Value) ? $"T0.КодРасположения = {Value}" : string.Empty;

        }
    }
}