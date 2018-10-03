using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.UnitAdv;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Единиц измерения
    /// </summary>
    public class DSOUnitAdv : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по ресурсу
        /// </summary>
        [FilterOption("КодРесурса", true)] 
        public int Resource { get; set; } 

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOUnitAdv()
        {
            KeyField = "КодЕдиницыИзмеренияДополнительной";
            NameField = "ЕдиницаРус";
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_ЕдиницыИзмеренияДополнительные; }
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