using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Kesco.Lib.ConvertExtention;
using Kesco.Lib.Entities.Documents;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document
{
    /// <summary>
    ///     Класс опции поиска по названию документа
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            var whereStr = !string.IsNullOrEmpty(Value)
                ? GetWhereStr()
                : string.Empty;
            return whereStr;
        }

        /// <summary>
        ///     Получение строки условия запроса
        /// </summary>
        /// <returns></returns>
        private string GetWhereStr()
        {
            var searchText = Value.Replace("№", "№ ").Trim();

            var words = Regex.Split(searchText, @"\s+");
            var word = string.Empty;
            var namewords = string.Empty;
            var date = string.Empty;

            var sql = string.Empty;
            var typeIDs = string.Empty;
            var isNotExistsNumberWords = true;

            var textParts = new Dictionary<string, string>();

            for (var i = 0; i < words.Length; i++)
            {
                word = words[i].Trim();
                if (Regex.IsMatch(word, "^(от|ot|oт|оt)$", RegexOptions.IgnoreCase))
                    continue;

                if (Regex.IsMatch(word, "№", RegexOptions.IgnoreCase))
                    continue;

                if (IsID(word))
                {
                    if (textParts.ContainsKey("ID"))
                        textParts["ID"] = string.Concat(textParts["Number"], " ", word);
                    else
                        textParts.Add("ID", word);

                    //continue;
                }

                if (IsType(word, ref typeIDs))
                {
                    if (textParts.ContainsKey("TypeID"))
                    {
                        textParts["TypeID"] = string.Concat(textParts["TypeID"], ",", typeIDs);
                    }
                    else
                    {
                        textParts.Add("TypeID", typeIDs);
                    }
                    continue;
                }

                if (i > 0 && words[i - 1].Equals("№"))
                {
                    if (IsNumber(word))
                    {
                        isNotExistsNumberWords = false;
                        if (textParts.ContainsKey("Number"))
                            textParts["Number"] = string.Concat(textParts["Number"], " ", word);
                        else
                            textParts.Add("Number", word);
                    }
                }
                else
                {
                    if (IsDate(word, ref date))
                    {
                        if (textParts.ContainsKey("Date"))
                            textParts["Date"] = string.Concat(textParts["Date"], ",", date);
                        else
                            textParts.Add("Date", date);
                        continue;
                    }
                    if (IsNumber(word))
                    {
                        isNotExistsNumberWords = false;
                        if (textParts.ContainsKey("Number"))
                            textParts["Number"] = string.Concat(textParts["Number"], " ", word);
                        else
                            textParts.Add("Number", word);
                    }
                }
            }

            var numWords = textParts.ContainsKey("ID") && isNotExistsNumberWords
                ? textParts["ID"]
                : textParts.ContainsKey("Number") ? textParts["Number"] : "";

            if (textParts.ContainsKey("Date"))
            {
                var dates = textParts["Date"].Split(',');

                for (var i = 0; i < dates.Length; i++)
                {
                    var di = dates[i];
                    sql +=
                        string.Concat(
                            (sql.Length == 0 ? string.Empty : " OR "),
                            "(T0.ДатаДокумента >= '", di, "'", "AND T0.ДатаДокумента < DATEADD(day, 1, '", di, "')",
                            (!textParts.ContainsKey("Date")
                                ? string.Empty
                                : " OR T0.НомерДокументаRL LIKE N'" + di.Replace(" ", "%") + "%'" +
                                  " OR T0.НомерДокументаRLReverse LIKE N'" +
                                  new string(di.Reverse().ToArray()).Replace(" ", "%") + "%'"
                                )
                            , ")"
                            );
                }
                sql = sql.Length == 0 ? string.Empty : string.Format("({0})", sql);
            }
            if (textParts.ContainsKey("Number"))
            {
                var ws = ReplaceRusLat(SqlEscape(GetWords(numWords, new Regex("[^ ]+", RegexOptions.IgnoreCase))));
                var tp =
                    SqlEscape(GetWords(textParts["Number"],
                        new Regex("[0-9A-ZА-ЯŠŽÕÄÖÜÉÀÈÙÂÊÎÔÛÇËÏŸÆæŒœßŇñ_]+", RegexOptions.IgnoreCase)));

                sql =
                    string.Concat(sql, (sql.Length == 0 ? "" : " AND "),
                        string.Format("({0}{1})",
                            string.Format("T0.НазваниеДокумента LIKE N'{0}%'", tp.Replace(" ", "%")),
                            string.Format(" OR T0.НомерДокументаRL LIKE N'{0}%'", ws.Replace(" ", "%")) +
                            string.Format(" OR T0.НомерДокументаRLReverse LIKE N'{0}%'",
                                new string(ws.Reverse().ToArray()).Replace(" ", "%"))
                            ));
            }

            if (textParts.ContainsKey("TypeID"))
                sql =
                    string.Concat(sql,
                        (sql.Length == 0 ? string.Empty : " AND "),
                        string.Format("(T0.КодТипаДокумента IN ({0}))", textParts["TypeID"]));

            if (textParts.ContainsKey("ID"))
            {
                var wId = string.Format("(T0.КодДокумента IN(SELECT value FROM Инвентаризация.dbo.fn_SplitInts('{0}')))", textParts["ID"].Replace(" ", ","));
                sql = sql.Length == 0 ? wId : string.Format("(({0}) OR {1})", sql, wId);
            }

            return sql;
        }

        /// <summary>
        ///     Проверка: является ли слово ID документа
        /// </summary>
        /// <param name="word">Введенное слово</param>
        /// <returns>Истина - если является, ложь - иначе</returns>
        private static bool IsID(string word)
        {
            if (!Regex.IsMatch(word, "^[1-9]\\d+$")) return false;

            //Entities.Documents.Document doc = new Entities.Documents.Document(word);
            //return !doc.Unavailable;

            return true;
        }

        /// <summary>
        ///     Проверка: является ли слово датой
        /// </summary>
        /// <param name="word">Введенное слово</param>
        /// <param name="date">Возвращаемая дата в формате yyyyMMdd</param>
        /// <returns>Истина - если является, ложь - иначе</returns>
        private static bool IsDate(string word, ref string date)
        {
            if (!Regex.IsMatch(word, "^[0-9]{1,4}[.,/-][0-9]{1,4}[.,/-][0-9]{1,4}$")) return false;
            try
            {
                date = DateParser.Parse(word).ToString("yyyyMMdd");
                return true;
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        ///     Проверка: является ли слово типом документа
        /// </summary>
        /// <param name="str">Входящая строка</param>
        /// <param name="typeIDs">Строка IDs</param>
        /// <returns>Истина - если является, ложь - иначе</returns>
        private static bool IsType(string str, ref string typeIDs)
        {
            var docTypes =
                DocType.GetDocTypesByNameAndTypes(
                    GetWords(str, new Regex("[0-9A-ZА-ЯŠŽÕÄÖÜÉÀÈÙÂÊÎÔÛÇËÏŸÆæŒœßŇñ_-]+", RegexOptions.IgnoreCase)),
                    typeIDs);

            if (docTypes.Count == 0) return false;
            typeIDs = "";
            for (var i = 0; i < docTypes.Count; i++)
                typeIDs += (typeIDs.Length == 0 ? "" : ",") + docTypes[i].Id;

            return true;
        }

        /// <summary>
        ///     Проверка: является ли слово номером документа
        /// </summary>
        /// <param name="str">Введенное слово</param>
        /// <returns>Истина - если является, ложь - иначе</returns>
        private static bool IsNumber(string str)
        {
            return str.Length > 0;
        }

        /// <summary>
        ///     Извлечение слов из строки согласно паттерну
        /// </summary>
        /// <param name="s"></param>
        /// <param name="wordPattern"></param>
        /// <returns></returns>
        public static string GetWords(string s, Regex wordPattern)
        {
            var k = "";
            var m = wordPattern.Matches(s);
            for (var i = 0; i < m.Count; i++) k += (i > 0 ? " " : "") + m[i].Value;
            return k;
        }
    }
}