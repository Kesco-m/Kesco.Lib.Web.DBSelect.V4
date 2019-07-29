namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Store
{
    /// <summary>
    ///     Класс опции поиска: склад является складом продуктов
    /// </summary>
    public class FOptTypeStoreIsWarehouse : FOptBase, IFilterOption
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
            Value = Enabled ? "2" : "";
            if (AddVirtual && Enabled) Value = "-2";
            var whereStr = Enabled
                ? $" (T0.КодТипаСклада >=20 {(AddVirtual ? " OR T0.КодТипаСклада = -1 AND T0.КодСклада < 0 AND T0.КодРесурса=2" : "")})"
                : string.Empty;
            return whereStr;
        }
    }
}