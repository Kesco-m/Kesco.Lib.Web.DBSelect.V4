using System.Text;
using Kesco.Lib.BaseExtention.Enums.Controls;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt
{
    /// <summary>
    ///     Класс-построитель строки запроса к таблице древовидной структуры
    /// </summary>
    public class TreeQueryBuilder
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="qType">Тип запроса</param>
        /// <param name="tableName">Имя таблицы, к которой осуществляется запрос</param>
        /// <param name="keyField">Имя ключевого поля таблицы</param>
        /// <param name="nameField">Имя поля наименования элемента таблицы</param>
        public TreeQueryBuilder(TreeQueryType qType, string tableName, string keyField, string nameField = "")
        {
            QueryType = qType;
            TableName = tableName;
            KeyField = keyField;
            NameField = nameField;
        }

        /// <summary>
        ///     Тип запроса
        /// </summary>
        private TreeQueryType QueryType { get; }

        /// <summary>
        ///     Имя таблицы, к которой осуществляется запрос
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     Имя ключевого поля таблицы
        /// </summary>
        public string KeyField { get; set; }

        /// <summary>
        ///     Имя поля наименования элемента таблицы
        /// </summary>
        public string NameField { get; set; }

        /// <summary>
        ///     Метод, возвращающий строку запроса к таблице с древовидной структурой
        /// </summary>
        /// <returns>Строка запроса</returns>
        public string GetQueryString(string idsEnumStr)
        {
            var sbQuery = new StringBuilder();
            switch (QueryType)
            {
                case TreeQueryType.AllChildrenWithParent:
                    sbQuery.AppendFormat(_allChildQueryStr, TableName, KeyField, NameField, idsEnumStr, "=");
                    break;
                case TreeQueryType.AllChildrenWithoutParent:
                    sbQuery.AppendFormat(_allChildQueryStr, TableName, KeyField, NameField, idsEnumStr, string.Empty);
                    break;
                case TreeQueryType.ChildrenWithParent:
                    sbQuery.AppendFormat(_childQueryStr, TableName, KeyField, idsEnumStr,
                        string.Format(_parentQueryStr, KeyField, idsEnumStr));
                    break;
                case TreeQueryType.ChildrenWithoutParent:
                    sbQuery.AppendFormat(_childQueryStr, TableName, KeyField, idsEnumStr, ")");
                    break;
            }

            return sbQuery.ToString();
        }

        #region Строковые константы-маски для построения запросов

        private const string _allChildQueryStr =
            @"{1} IN ( SELECT Child.{1} FROM {0} Child (nolock) INNER JOIN {0} Parent (nolock) ON Parent.L <{4} Child.L AND Parent.R >{4} Child.R WHERE Parent.{1} IN ({3})) ";

        private const string _childQueryStr =
            @"{1} IN ( SELECT Child.{1} FROM {0} Child WHERE Child.Parent IN ({2}) {3}";

        private const string _parentQueryStr = @"Child.{0} IN({1}) ";

        #endregion
    }
}