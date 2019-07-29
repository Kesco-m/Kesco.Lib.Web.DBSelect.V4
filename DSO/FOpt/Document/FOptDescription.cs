namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    ///     Класс опции поиска документов по примечанию
    /// </summary>
    public class FOptDescription : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            var whereStr = string.Empty;

            if (!string.IsNullOrEmpty(Value))
            {
                var words = Value.Split(',');
                for (var i = 0; i < words.Length; i++)
                    whereStr = string.Concat(whereStr,
                        string.Format(" T0.Описание LIKE '%{0}%' {1}", words[i].Trim(),
                            i < words.Length - 1 ? "OR" : string.Empty));
                return string.Format("({0})", whereStr);
            }

            return string.Empty;
        }
    }
}