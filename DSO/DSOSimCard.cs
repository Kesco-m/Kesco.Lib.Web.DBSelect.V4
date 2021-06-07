using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.SimCard;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Sim-карты
    /// </summary>
    public class DSOSimCard : DSOCommon
    {
        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOSimCard()
        {
            KeyField = "КодОборудования";
            NameField = "НомерТелефона";

            Name = new FOptName();
            Ids = new FOptIDs();
        }

        /// <summary>
        ///     Опция поиска по кодам сотрудника
        /// </summary>
        [FilterOption("ids", optionNameURL: "ids")]
        public FOptIDs Ids;

        /// <summary>
        ///     Опция поиска по тексту
        /// </summary>
        [FilterOption("Name", optionNameURL: "NOMERTELEFONA")]
        public FOptName Name;

        /// <summary>
        ///     Параметр фильтрации: условие поиска по сотрудникам
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        ///     2 - Любое значение (не фильтруем по полю),
        ///     3 - Значение не указано (значение поля NULL)
        /// </summary>
        [FilterOption("SimCardHowSearch", optionNameURL: "KODOBORUDOVANIA")]
        public string SimCardHowSearch => Ids.SimCardHowSearch;

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_SimКарты;

        /// <summary>
        ///     Задание сортировки выборки
        ///     Доделать управляемую сортировку
        /// </summary>
        public override string SQLOrderBy => $"{NameField}";
    }
}
