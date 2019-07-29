using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Kesco.Lib.BaseExtention.Enums.Docs;
using Kesco.Lib.Entities.Documents;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    ///     Класс опции поиска документов по номеру документа
    /// </summary>
    public class FOptDocNumber : IFilterOption
    {
        /// <summary>
        ///     Список параметров поиска по номеру документа
        /// </summary>
        public List<DocNumberParam> DocNumberParams = new List<DocNumberParam>();

        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            var sb = new StringBuilder();

            DocNumberParams.ForEach(p =>
            {
                switch (p.DocNumberQueryType)
                {
                    case SearchType.Equals:
                        sb.AppendFormat("(T0.НомерДокумента = N'{0}') {1} ", p.Number, "OR");
                        break;

                    case SearchType.StartWith:
                        sb.AppendFormat("(T0.НомерДокумента LIKE N'% {0}%') {1} ", p.Number, "OR");
                        break;

                    case SearchType.Contain:
                        sb.AppendFormat("(T0.НомерДокумента LIKE N'%{0}%') {1} ", p.Number, "OR");
                        break;
                }
            });

            var whereStr = sb.ToString().Trim();
            const string endOrPattern = @"OR[\s]{0,}$";

            if (whereStr.Length > 2 && Regex.IsMatch(whereStr, endOrPattern, RegexOptions.IgnoreCase))
                whereStr = Regex.Replace(whereStr, endOrPattern, string.Empty);

            return whereStr;
        }
    }
}