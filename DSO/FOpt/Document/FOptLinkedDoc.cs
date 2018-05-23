using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Kesco.Lib.BaseExtention.Enums.Docs;
using Kesco.Lib.Entities.Documents;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    ///     Класс опции поиска по связанному документу
    /// </summary>
    public class FOptLinkedDoc : IFilterOption
    {
        /// <summary>
        ///     Контейнер параметров связанных документов с типами запросов
        /// </summary>
        public List<LinkedDocParam> LinkedDocParams = new List<LinkedDocParam>();

        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
		public string SQLGetClause()
		{
			var sb = new StringBuilder(" (");

			LinkedDocParams.ForEach(doc =>
				{
					switch (doc.QueryType)
					{
						case LinkedDocsType.AllReasons:
							if (sb.Length > 2) sb.Append(" OR ");
							sb.AppendFormat("(EXISTS (SELECT * FROM Документы.dbo.fn_ВсеОснования({0}) T1 WHERE T1.КодДокумента = T0.КодДокумента))",doc.DocID);
							break;

						case LinkedDocsType.DirectReasons:
							if (sb.Length > 2) sb.Append(" OR ");
							sb.AppendFormat("(EXISTS (SELECT * FROM Документы.dbo.vwСвязиДокументов T1 (nolock) WHERE T1.КодДокументаВытекающего = {0} AND T1.КодДокументаОснования = T0.КодДокумента) )", doc.DocID);
							break;

						case LinkedDocsType.AllСonsequences:
							if (sb.Length > 2) sb.Append(" OR ");
							sb.AppendFormat("(EXISTS (SELECT * FROM Документы.dbo.fn_ВсеВытекающие({0}) T1 WHERE T1.КодДокумента = T0.КодДокумента) )", doc.DocID);
							break;

						case LinkedDocsType.DirectСonsequences:
							if (sb.Length > 2) sb.Append(" OR ");
							sb.AppendFormat("(EXISTS (SELECT * FROM Документы.dbo.vwСвязиДокументов T1 (nolock) WHERE T1.КодДокументаОснования = {0} AND T1.КодДокументаВытекающего = T0.КодДокумента) )", doc.DocID);
							break;
					}
				}
			);

			if (sb.Length > 2) return sb.Append(") ").ToString();

			return string.Empty;
		}

        /// <summary>
        ///     Очистить
        /// </summary>
        public void Clear()
        {
            LinkedDocParams.Clear();
        }

        /// <summary>
        ///     Добавление параметра по связанным документам
        /// </summary>
        public void Add(LinkedDocParam par)
        {
            LinkedDocParams.Add(par);
        }

        /// <summary>
        ///     Добавление параметра по связанным документам
        /// </summary>
        public void Add(string id, LinkedDocsType type)
        {
            var link = new LinkedDocParam {DocID = id, QueryType = type};
            LinkedDocParams.Add(link);
        }
    }
}