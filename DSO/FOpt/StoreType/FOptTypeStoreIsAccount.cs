namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StoreType
{
    /// <summary>
    ///     Класс опции поиска: склад является банковским счетом
    /// </summary>
    public class FOptTypeStoreIsAccount : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Использовать опцию при поиске
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        ///     Добавить к поиску виртуальный склад
        /// </summary>
        public bool AddVirtual { get; set; }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            Value = Enabled ? "1" : "";
            if (AddVirtual && Enabled) Value = "-1";
            var whereStr = Enabled
                ? $" (T0.КодТипаСклада >=1 AND T0.КодТипаСклада < 10 {(AddVirtual ? " OR T0.КодТипаСклада = -1" : "")})"
                : string.Empty;
            return whereStr;
        }
    }
}