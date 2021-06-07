using Kesco.Lib.Entities;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Единиц измерения
    /// </summary>
    public class DSOUnitAdv : DSOCommon
    {
        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOUnitAdv()
        {
            KeyField = "КодЕдиницыИзмеренияДополнительной";
            NameField = "ЕдиницаРус";
        }

        /// <summary>
        ///     Опция фильтра по ресурсу
        /// </summary>
        [FilterOption("КодРесурса", true)]
        public int Resource { get; set; }

        /// <summary>
        ///     Опция фильтра по коэффициенту
        /// </summary>
        [FilterOption("Коэффициент", true)]
        public double? Coef { get; set; }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ЕдиницыИзмеренияДополнительные;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("T0.{0}", NameField);
    }
}