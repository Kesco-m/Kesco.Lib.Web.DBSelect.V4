namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Location
{
    /// <summary>
    /// Класс опции поиска по сотрудникам
    /// </summary>
    public class FOptEmployee : FOptBase, IFilterOption
    {
        private string _value = "";

        /// <summary>
        ///     Значение
        /// </summary>
        public override string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            return Value.Length == 0
                ? ""
                : string.Format(
                    " EXISTS(SELECT * FROM РабочиеМеста T1 WHERE T0.КодРасположения = T1.КодРасположения AND T1.КодСотрудника={0})",
                    Value);
        }
    }
}