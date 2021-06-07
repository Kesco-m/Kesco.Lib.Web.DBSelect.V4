using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonCabinetType;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для котрола выбора вида личного кабинета
    /// </summary>
    public class DSOPersonCabinetType : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по виду личного кабинета
        /// </summary>
        [FilterOption("ТипЛичногоКабинета", false, "Search")]
        public FOptPersonCabinetType Name;

        /// <summary>
        ///     Опция фильтра по виду личного кабинета
        /// </summary>
        [FilterOption("КодТипаЛичногоКабинета")] public FOptIDs PersonCabinetTypeId;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPersonCabinetType()
        {
            KeyField = "КодТипаЛичногоКабинета";
            NameField = "ТипЛичногоКабинета";
            Name = new FOptPersonCabinetType();
            PersonCabinetTypeId = new FOptIDs();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ТипыЛичногоКабинета;

        /// <summary>
        ///     Задание сортировки выборки
        ///     Доделать управляемую сортировку
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", KeyField);
    }
}