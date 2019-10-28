namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Location
{
    /// <summary>
    ///     Класс опции поиска документов по документам в работе заданного пользователя
    /// </summary>
    public class FOptOnHand : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public FOptOnHand()
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
            return !Enabled ? "КодРасположения <> 55" : string.Empty;
        }
    }
}