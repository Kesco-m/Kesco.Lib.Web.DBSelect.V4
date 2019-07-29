using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.AttributeFormatType;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Тип формата атрибута
    /// </summary>
    public class DSOAttributeFormatType : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени типа формата атрибута
        /// </summary>
        [FilterOption("ТипАтрибута", false, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOAttributeFormatType()
        {
            KeyField = "КодТипаАтрибута";
            NameField = "ТипАтрибута";
            OrderField = "ПорядокВыводаАтрибута";
            PersonTypeAvailabilityField = "ДоступностьДляТипаЛица";
            AttributeFormatTypeNameLat = "ТипАтрибутаЛат";
            Name = new FOptName();
        }

        /// <summary>
        ///     Параметр фильтрации: условие поиска по типу формата атрибута
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        ///     2 - Любое значение (не фильтруем по полю),
        ///     3 - Значение не указано (значение поля NULL)
        /// </summary>
        [FilterOption("AttributeFormatTypeHowSearch", false)]
        public int? AttributeFormatTypeHowSearch { get; set; }

        /// <summary>
        ///     Доступность типа формата атрибута
        /// </summary>
        public string PersonTypeAvailabilityField { get; set; }

        /// <summary>
        ///     Порядковый номер вывода эллемента
        /// </summary>
        public string OrderField { get; set; }

        /// <summary>
        ///     Имя типа атрибута на латинице
        /// </summary>
        public string AttributeFormatTypeNameLat { get; set; }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ТипыАтрибутов;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", NameField);
    }
}