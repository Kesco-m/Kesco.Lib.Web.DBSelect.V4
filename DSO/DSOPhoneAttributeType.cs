using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PhoneAttributeType;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Атрибут шаблона IP-Телефона
    /// </summary>
    public class DSOPhoneAttributeType : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени атрибута
        /// </summary>
        [FilterOption("ТипАтрибутаТелефона")]
        public FOptPhoneAttributeType Name;

        /// <summary>
        ///     Опция фильтра: источник значений
        /// </summary>
        [FilterOption("ИсточникЗначений")]
        public FOptValuesSource ValuesSource;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPhoneAttributeType()
        {
            KeyField = "ТипАтрибутаТелефона";
            NameField = "ТипАтрибутаТелефона";
            Name = new FOptPhoneAttributeType();
            ValuesSource = new FOptValuesSource();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ТипыАтрибутовТелефонов;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", NameField);
    }
}