using System.Text;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    ///     Опция поиска по лицам
    /// </summary>
    public class FOptPersonIDs : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Строка параметров для архива документов
        /// </summary>
        /// <remarks>
        ///     Если в конце стоит "A" то используется сочитание через оператор "И"
        ///     по умолчанию ставится "ИЛИ"
        /// </remarks>
        public string DocViewParamsStr
        {
            get
            {
                if (Count > 1 && UseAndOperator)
                    return Value + "A";
                return Value;
            }
        }

        /// <summary>
        ///     Использовать оператор "И" при объединении
        /// </summary>
        public bool UseAndOperator { get; set; }

        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            if (UseAndOperator)
                return Count > 0
                    ? string.Format(@" ({0})", GetAndClause())
                    // такой запрос может параллелится это может привести к пожиранию ресурсов или дедлокам, если такое будет, то в самом конце _всего_ запроса нужно поставить OPTION(MAXDOP 1) 
                    : string.Empty;


            return !string.IsNullOrEmpty(Value)
                ? string.Format(
                    " (EXISTS (SELECT КодДокумента FROM Документы.dbo.vwЛицаДокументов T1 (nolock) WHERE T1.КодДокумента = T0.КодДокумента AND T1.КодЛица IN ({0})))",
                    Value)
                : string.Empty;
        }

        /// <summary>
        ///     Получить запрос используя оператор И
        /// </summary>
        public string GetAndClause()
        {
            var values = GetValuesCollection();
            var b = new StringBuilder();
            for (var i = 0; i < values.Count; i++)
            {
                if (i > 0) b.Append(" AND ");
                b.Append("(EXISTS (SELECT * FROM Документы.dbo.vwЛицаДокументов TI WITH(nolock)" +
                         " WHERE TI.КодДокумента=T0.КодДокумента AND TI.КодЛица =");
                b.Append(values[i]);
                b.Append("))");
            }

            return b.ToString();
        }
    }
}