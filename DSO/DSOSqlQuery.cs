using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для контрола выбора подзапроса SQL
    /// </summary>
    public class DSOSqlQuery : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени
        /// </summary>
        [FilterOption("Условие")] public FOptName Name;

        /// <summary>
        ///     Опция фильтра по ID
        /// </summary>
        [FilterOption("КодУсловия")] public FOptIDs QueryId;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOSqlQuery()
        {
            KeyField = "КодУсловия";
            NameField = "Условие";
            QueryId = new FOptIDs("Условие");
            Name = new FOptName("КодУсловия");
        }

        /// <summary>
        ///     Опция фильтра по КодПриложения
        /// </summary>
        [FilterOption("ТипУсловия", true)]
        public string QueryType { get; set; }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ДополнительныеФильтрыПриложений;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => NameField;
    }
}