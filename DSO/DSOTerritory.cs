using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Territory;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Территория
    /// </summary>
    public class DSOTerritory : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени территории
        /// </summary>
        [FilterOption("ТипАтрибута", false, "Search")]
        public FOptName Name;


        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOTerritory()
        {
            KeyField = "КодТерритории";
            NameField = "Территория";
            CaptionField = "Caption";
            AbbreviationField = "Аббревиатура";
            TelephoneCodeField = "ТелКодСтраны";
            Name = new FOptName();
            CodTTerritory = new FOptTerritoryCode {Value = "2"};
        }

        /// <summary>
        ///     Параметр фильтрации: условие поиска по типу формата атрибута
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        ///     2 - Любое значение (не фильтруем по полю),
        ///     3 - Значение не указано (значение поля NULL)
        /// </summary>
        [FilterOption("TerritoryHowSearch", false)]
        public int? TerritoryHowSearch { get; set; }

        /// <summary>
        ///     Опция поиска по состоянию сотрудника
        /// </summary>
        [FilterOption("codTTerritory")]
        public FOptTerritoryCode CodTTerritory;

        /// <summary>
        ///     Наименование территории на английском
        /// </summary>
        public string CaptionField { get; set; }

        /// <summary>
        ///     Аббревиатура территории
        /// </summary>
        public string AbbreviationField { get; set; }

        /// <summary>
        ///     Телефонный код территории
        /// </summary>
        public string TelephoneCodeField { get; set; }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_Территории_Страны;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", NameField);
    }
}