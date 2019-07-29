using Kesco.Lib.BaseExtention.Enums.Controls;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    ///     Класс опции поиска документов по дате
    /// </summary>
    public class FOptDate : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Cпособ задания даты
        /// </summary>
        public DateSearchType DateSearchType { get; set; }

        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            var whereStr = string.Empty;

            if (!string.IsNullOrEmpty(Value))
            {
                var dates = Value.Split(',');

                switch (DateSearchType)
                {
                    case DateSearchType.Equals:
                        for (var i = 0; i < dates.Length; i++)
                            whereStr = string.Concat(whereStr,
                                string.Format(
                                    @"(T0.ДатаДокумента >= '{0}' AND T0.ДатаДокумента < DATEADD(day, 1, '{0}') ) {1}",
                                    dates[i], i < dates.Length - 1 ? " OR " : string.Empty));
                        return string.Format("({0})", whereStr);

                    case DateSearchType.MoreThan:
                        return string.Format(@"(T0.ДатаДокумента >= '{0}')", dates[0]);

                    case DateSearchType.LessThan:
                        return string.Format(@"(T0.ДатаДокумента <= '{0}')", dates[0]);
                }
            }

            return string.Empty;
        }
    }
}