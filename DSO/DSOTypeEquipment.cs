using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.TypeEquipment;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Тип оборудования
    /// </summary>
    public class DSOTypeEquipment : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по названию
        /// </summary>
        [FilterOption("ТипыОборудования")]
        public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOTypeEquipment()
        {
            KeyField = "КодТипаОборудования";
            NameField = "ТипОборудования";
            Name = new FOptName();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ТипыОборудования;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => "ЕстьХарактеристикиSoft, ТипОборудования";
    }
}