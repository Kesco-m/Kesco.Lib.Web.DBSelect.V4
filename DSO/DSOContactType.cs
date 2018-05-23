using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.ContactType;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Тип контакта
    /// </summary>
    public class DSOContactType : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по типу контакта
        /// </summary>
        [FilterOption("ТипКонтакта", false, "Search")] public FOptContactType Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOContactType()
        {
            KeyField = "КодТипаКонтакта";
            ContactTyleNameLat = "ТипКонтактаЛат";
            NameField = "ТипКонтакта";
            Name = new FOptContactType();
        }

        /// <summary>
        ///     Наименование типа контакта на английском
        /// </summary>
        public string ContactTyleNameLat { get; set; }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_ТипыКонтактов; }
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy
        {
            get { return string.Format("{0}", KeyField); }
        }
    }
}