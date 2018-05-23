using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Template;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для контрола поиска шаблон печатной формы
    /// </summary>
    public class DSOTemplate : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра: Код контрагента
        /// </summary>
        [FilterOption("CodeKontragent")] public FOptKontragent CodeKontragent;

        /// <summary>
        ///     Опция фильтра: Код лица
        /// </summary>
        [FilterOption("CodePerson")] public FOptCodePerson CodePerson;

        /// <summary>
        ///     Опция фильтра: Код типа документа
        /// </summary>
        [FilterOption("CodeTypeDoc")] public FOptCodeTypeDoc CodeTypeDoc;

        /// <summary>
        ///     Опция фильтра: Дата документа
        /// </summary>
        [FilterOption("DocDate")] public FOptDate DocDate;

        /// <summary>
        ///     Опция фильтра: по названию шаблон печатной формы
        /// </summary>
        [FilterOption("Name", false, "Search")] public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOTemplate()
        {
            KeyField = "CodeTemplate";
            NameField = "NameTemplate";

            Name = new FOptName();
            CodeTypeDoc = new FOptCodeTypeDoc();
            CodePerson = new FOptCodePerson();
            DocDate = new FOptDate();
            CodeKontragent = new FOptKontragent();
        }

        /// <summary>
        ///     Запрос выборки документов
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_ШаблоныПечатныхФорм; }
        }

        /// <summary>
        ///     Запрос получения документа по ID
        /// </summary>
        public override string SQLEntityById
        {
            get { return SQLQueries.SELECT_ID_ШаблонПечатнойФормы; }
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy
        {
            get { return "IsNull(КодЛица, 0) DESC, IsNull(КодКонтрагента, 0) DESC, КодШаблонаПечатнойФормы"; }
        }
    }
}