using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Kesco.Lib.ConvertExtention;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt
{
    /// <summary>
    ///     Базовый абстрактный класс, реализующий работу со значением опции фильтрации
    /// </summary>
    public abstract class FOptBase
    {
        /// <summary>
        ///     Список значений опций фильтрации
        /// </summary>
        private readonly StringCollection _value = new StringCollection();

        /// <summary>
        ///     Возвращает количество значений опции фильтрации
        /// </summary>
        public int Count => _value.Count;

        /// <summary>
        ///     Свойство, возвращающее значения опции через ',' и устанавливающее единиственное значение опции(аналогично методу
        ///     Set)
        /// </summary>
        public virtual string Value
        {
            get { return Convert.Collection2Str(_value); }
            set
            {
                Clear();
                var arrV = value.Split(',');
                for (var i = 0; i < arrV.Length; i++)
                    Add(arrV[i]);
            }
        }

        /// <summary>
        ///     Группа слов поиска
        /// </summary>
        public MatchCollection WordsGroup
        {
            get
            {
                var namePattern = new Regex("((?<=\")[^\"]+(?=\"))|[-\\*'0-9A-ZА-ЯŠŽÕÄÖÜÉÀÈÙÂÊÎÔÛÇËÏŸÆæŒœßŇñ_]+",
                    RegexOptions.IgnoreCase);
                var m = namePattern.Matches(Value);
                if (m.Count != 0) return m;

                namePattern = new Regex("(.+)", RegexOptions.IgnoreCase);
                m = namePattern.Matches(Value);
                return m;
            }
        }

        /// <summary>
        ///     Использовать задания значения, когда необходимо задать единственное значение опции фильтрации
        /// </summary>
        /// <param name="v">Значение опции фильтрации</param>
        public void Set(string v)
        {
            _value.Clear();
            v = v.Trim();
            if (!_value.Contains(v)) _value.Add(v);
        }

        /// <summary>
        ///     Использовать для добавления значения, когда опция фильтрации может имеет несколько(перечисление) значений
        /// </summary>
        /// <param name="v"></param>
        public void Add(string v)
        {
            v = v.Trim();
            if (!_value.Contains(v)) _value.Add(v);
        }

        /// <summary>
        ///     Использовать для удаления значения из списка выбранных
        /// </summary>
        /// <param name="v"></param>
        public void Remove(string v)
        {
            if (_value.Contains(v)) _value.Remove(v);
        }

        /// <summary>
        ///     Очищает значения опции фильтрации
        /// </summary>
        public void Clear()
        {
            _value.Clear();
        }

        /// <summary>
        ///     Получить перечисления
        /// </summary>
        protected StringCollection GetValuesCollection()
        {
            return _value;
        }

        /// <summary>
        ///     Формирование и возврат строки запроса по введенным словам по нескольким полям
        /// </summary>
        /// <param name="nameFields">Имена полей, по которым идет поиск</param>
        /// <param name="wordsGroup">Группа введенных слов</param>
        /// <param name="searchByRL">Искать по полю с постфиксом RL</param>
        /// <returns>Строка запроса по введенным словам</returns>
        public static string GetWhereStrBySearchWords(string[] nameFields, MatchCollection wordsGroup,
            bool searchByRL = false)
        {
            var result = "";
            foreach (var nameField in nameFields)
                if (!string.IsNullOrEmpty(nameField))
                    result = string.Concat(result, GetWhereStrBySearchWords(nameField, wordsGroup, searchByRL), " OR ");
            result = result.Substring(0, result.Length - 4);
            return !string.IsNullOrEmpty(result) ? $"({result})" : "";
        }

        //TODO: Если понадобиться без разбития по-словам
        //private static string GetWhereStrBySearchWords(string[] nameFields, string searchtext, bool searchByRL = false)
        //{
        //    var result = "";
        //    foreach (var nameField in nameFields)
        //    {
        //        if (!string.IsNullOrEmpty(nameField))
        //        {
        //            var sqlClause = GetWhereStrBySearchWords(nameField, searchtext);
        //            if (!string.IsNullOrEmpty(sqlClause))
        //                result = string.Concat(result, GetWhereStrBySearchWords(nameField, searchtext), " OR ");
        //        }
        //    }
        //    return !string.IsNullOrEmpty(result) ? $"({result})" : "";
        //}


        /// <summary>
        ///     Формирование и возврат строки запроса по введенным словам
        /// </summary>
        /// <param name="nameField">Имя поля, по которому идет поиск</param>
        /// <param name="wordsGroup">Группа введенных слов</param>
        /// <param name="searchByRL">Искать по полю с постфиксом RL</param>
        /// <returns>Строка запроса по введенным словам</returns>
        /// Необходимо добавить в DSO в SQLBatchPrepare Пример:(DECLARE @s{1} nvarchar(50); SET @s{1} = Инвентаризация.dbo.fn_SplitWords(Инвентаризация.dbo.fn_ReplaceRusLat(Инвентаризация.dbo.fn_ReplaceKeySymbols(N'{0}')));)
        public static string GetWhereStrBySearchWords(string nameField, MatchCollection wordsGroup,
            bool searchByRL = false)
        {
            const string patternDefault = "'%{0}%'";
            const string patternRL = "'% ' + @s{0} + '%'";
            var patternStr = string.Concat(string.Format(" {0} ", nameField), "LIKE ",
                searchByRL ? patternRL : patternDefault);
            var sb = new StringBuilder();

            for (var i = 0; i < wordsGroup.Count; i++)
            {
                sb.Append(string.Format(patternStr,
                    searchByRL
                        ? i.ToString(CultureInfo.InvariantCulture).Replace("'", "''")
                        : wordsGroup[i].ToString().Replace("'", "''")));

                if (i < wordsGroup.Count - 1) sb.Append("\n AND");
            }

            if (sb.Length == 0) return "";
            return string.Concat("(", sb.ToString(), ")");
        }

        //TODO: Если понадобиться без разбития по-словам
        //private static string GetWhereStrBySearchWords(string nameField, string searchText)
        //{
        //    const string patternDefault = "'{0}%'";
        //    var patternStr = string.Concat(string.Format(" {0} ", nameField), "LIKE ", patternDefault);
        //    var sb = new StringBuilder();
        //    sb.Append(string.Format(patternStr, searchText.Replace("'", "''")));

        //    return sb.Length == 0 ? "" : string.Concat("(", sb.ToString(), ")");
        //}

        /// <summary>
        ///     Экранирование спец. символов при sql-поиске текста
        /// </summary>
        /// <param name="s">Строка, которую надо экранировать</param>
        /// <returns>Экранированная строка</returns>
        public static string SqlEscape(string s)
        {
            var chr9 = new string((char) 9, 1);
            var chr10 = new string((char) 10, 1);
            var chr13 = new string((char) 13, 1);
            var chr160 = new string((char) 160, 1);

            return s.Replace(chr9, " ").Replace(chr10, " ").Replace(chr13, " ").Replace(chr10, " ").Replace(chr160, " ")
                .Replace("[", "[[]").Replace("_", "[_]").Replace("%", "[%]").Replace("'", "''");
        }

        /// <summary>
        ///     Вызов SQL-функции fn_ReplaceRusLat
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceRusLat(string str)
        {
            var s = "";
            var inputParameters = new Dictionary<string, object>();
            var outpuParameters = new Dictionary<string, object>();

            inputParameters.Add("@str", str);
            outpuParameters.Add("@s", s);

            DBManager.ExecuteNonQuery(SQLQueries.SELECT_FN_ReplaceRusLat, CommandType.Text, Config.DS_user,
                inputParameters, outpuParameters);

            s = outpuParameters["@s"].ToString();

            return s;
        }
    }
}