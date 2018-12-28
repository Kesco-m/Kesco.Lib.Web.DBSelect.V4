using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Kesco.Lib.BaseExtention.Enums.Docs;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Documents;
using Kesco.Lib.Log;
using Kesco.Lib.Web.Settings;
using Convert = Kesco.Lib.ConvertExtention.Convert;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    ///     Класс опции поиска по типу документа
    /// </summary>
    public class FOptDocType : IFilterOption
    {
        /// <summary>
        ///     Контейнер параметров типов складов с типами запросов
        /// </summary>
        public List<DocTypeParam> DocTypeParams = new List<DocTypeParam>();

        /// <summary>
        ///     Строка параметров типов документа
        ///     Формат: "1111,2222s,3333c,4444sc"
        ///     xxxx - ID типа документа
        ///     s - с синонимами
        ///     с - с потомками
        ///     sc - с синонимами и потомками
        ///     Пример: 4444sc - тип документа с ID 4444 с синонимами и потомками
        /// </summary>
        public string DocTypeParamsStr
        {
            get { return GetDocTypeParamString(DocTypeParams); }
        }

        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            return DocTypeParams.Count > 0
                ? string.Format(@" T0.КодТипаДокумента IN ({0}) ", GetDocTypeIDsStr())
                : string.Empty;
        }

        /// <summary>
        ///     Добавить параметр поиска по типу документа
        /// </summary>
        public void Add(DocTypeParam param)
        {
            var contains = DocTypeParams.Any(p => p.DocTypeID == param.DocTypeID && p.QueryType == param.QueryType);
            if (contains) return;
            DocTypeParams.Add(param);
        }

        /// <summary>
        ///     Добавить параметр поиска по типу документа
        /// </summary>
        public void Add(string docTypeId, DocTypeQueryType qType)
        {
            var contains = DocTypeParams.Any(p => p.DocTypeID == docTypeId && p.QueryType == qType);
            if (contains) return;

            var param = new DocTypeParam {DocTypeID = docTypeId, QueryType = qType};
            DocTypeParams.Add(param);
        }

        /// <summary>
        ///     Очистить
        /// </summary>
        public void Clear()
        {
            DocTypeParams.Clear();
        }

        #region Privates

        /// <summary>
        ///     Формирование и возврат строки с параметрами типов и типами запроса
        /// </summary>
        /// <returns></returns>
        private string GetDocTypeParamString(List<DocTypeParam> docTypeParams)
        {
            var sb = new StringBuilder();
            var strFieldPat = "{0}{1}{2}";

            docTypeParams.ForEach(p =>
            {
                switch (p.QueryType)
                {
                    case DocTypeQueryType.Equals:
                        sb.AppendFormat(strFieldPat, p.DocTypeID, string.Empty, ",");
                        break;
                    case DocTypeQueryType.WithChildren:
                        sb.AppendFormat(strFieldPat, p.DocTypeID, "c", ",");
                        break;
                    case DocTypeQueryType.WithSynonyms:
                        sb.AppendFormat(strFieldPat, p.DocTypeID, "s", ",");
                        break;
                    case DocTypeQueryType.WithChildrenSynonyms:
                        sb.AppendFormat(strFieldPat, p.DocTypeID, "sc", ",");
                        break;
                }
            }
                );
            if (sb.Length > 0 && sb[sb.Length - 1].Equals(','))
                sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        ///     Получение строки с ID типов документа
        /// </summary>
        /// <returns>Строка с ID типов документа</returns>
        private string GetDocTypeIDsStr()
        {
            if (DocTypeParams.Count > 0)
            {
                var typeIDs = new StringCollection();

                var selectLRSql = "SELECT КодТипаДокумента FROM ТипыДокументов WHERE L >= @L AND R <= @R; ";
                var selectParentSql = "SELECT КодТипаДокумента FROM ТипыДокументов WHERE Parent = @Parent; ";

                DocTypeParams.ForEach(
                    type =>
                    {
                        var sql = new StringBuilder();
                        switch (type.QueryType)
                        {
                            case DocTypeQueryType.Equals:
                                typeIDs.Add(type.DocTypeID);
                                break;

                            case DocTypeQueryType.WithChildren:
                                sql.Append(@"
DECLARE @L int, @R int; ");
                                sql.AppendFormat(@"
SELECT @L = L, @R = R FROM ТипыДокументов WHERE КодТипаДокумента = {0}; ", type.DocTypeID);
                                sql.Append(selectLRSql);
                                break;

                            case DocTypeQueryType.WithSynonyms:
                                sql.AppendFormat(@"
DECLARE @Parent int; 
SELECT @Parent = Parent FROM ТипыДокументов WHERE КодТипаДокумента = {0}; ", type.DocTypeID);
                                sql.Append(selectParentSql);
                                break;

                            case DocTypeQueryType.WithChildrenSynonyms:
                                sql.Append(@"
DECLARE @L int, @R int; ");
                                sql.AppendFormat(@"
SELECT @L = ISNULL(Pr.L, Ch.L), @R = ISNULL(Pr.R, Ch.R) 
FROM ТипыДокументов Ch LEFT JOIN ТипыДокументов Pr ON Pr.КодТипаДокумента = Ch.Parent WHERE Ch.КодТипаДокумента = {0}; ",
                                    type.DocTypeID);
                                sql.Append(selectLRSql);
                                break;
                        }

                        if (type.QueryType != DocTypeQueryType.Equals)
                        {
                            var dt = new DataTable();
                            try
                            {
                                dt = DBManager.GetData(sql.ToString(), Config.DS_document);
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteEx(new DetailedException("Ошибка при получении типов документов в DSODoc", ex));
                                throw ex;
                            }

                            for (var i = 0; i < dt.Rows.Count; i++)
                            {
                                var typeID = dt.Rows[i][0].ToString();
                                if (!typeIDs.Contains(typeID)) typeIDs.Add(typeID);
                            }
                        }
                    }
                    );
                return Convert.Collection2Str(typeIDs);
            }
            return string.Empty;
        }

        #endregion
    }
}