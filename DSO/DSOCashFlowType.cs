using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.CashFlowType;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Территория
    /// </summary>
    public class DSOCashFlowType : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени
        /// </summary>
        [FilterOption("ВидДвиженияДенежныхСредств", false, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOCashFlowType()
        {
            KeyField = "КодВидаДвиженияДенежныхСредств";
            NameField = "ВидДвиженияДенежныхСредств";
            Name1CField = "Название1С";
            Name = new FOptName();
        }

        /// <summary>
        ///     Название 1С
        /// </summary>
        public string Name1CField { get; set; }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ВидыДвиженийДенежныхСредств;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", NameField);
    }
}