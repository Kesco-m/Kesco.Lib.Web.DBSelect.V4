using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Unit;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Единиц измерения
    /// </summary>
    public class DSOUnit : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени
        /// </summary>
        [FilterOption("ЕдиницаРус", false, "Search")] public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOUnit()
        {
            var tableName = "ЕдиницыИзмерения";

            KeyField = "КодЕдиницыИзмерения";
            NameField = "Описание";

            Name = new FOptName();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_ЕдиницыИзмерения; }
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy
        {
            get { return string.Format("T0.{0}", NameField); }
        }
    }
}