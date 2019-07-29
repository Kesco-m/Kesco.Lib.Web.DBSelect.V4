using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Currency;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для контрола выбора валюты
    /// </summary>
    public class DSOCurrency : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по ID
        /// </summary>
        [FilterOption("КодВалюты")] public FOptIDs CurrencyId;

        /// <summary>
        ///     Опция фильтра по имени
        /// </summary>
        [FilterOption("РесурсРус")] public FOptName Name;

        /// <summary>
        ///     Контстуктор класса
        /// </summary>
        public DSOCurrency()
        {
            KeyField = "КодВалюты";
            NameField = "Название";
            CurrencyId = new FOptIDs("КодВалюты");
            Name = new FOptName("РесурсРус");
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_Валюты;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => "КодВалюты";
    }
}