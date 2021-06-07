namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Socket
{
    /// <summary>
    ///     Класс опции поиска розетки по состоянию
    /// </summary>
    public class FOptAlive : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public FOptAlive()
        {
            Enabled = true;
        }

        /// <summary>
        ///     Использовать опцию при поиске
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        ///     Значение
        /// </summary>
        public override string Value
        {
            get
            {
                if (Enabled)
                    return "1";
                return "0";
            }
            set { Enabled = value == "1"; }
        }

        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            return Enabled ? "(Работает = 1)" : string.Empty;
        }
    }
}