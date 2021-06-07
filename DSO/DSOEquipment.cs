using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Equipment;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Оборудование
    /// </summary>
    public class DSOEquipment : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по названию
        /// </summary>
        [FilterOption("Name")]
        public FOptName Name;

        /// <summary>
        ///     Опция фильтра по типу
        /// </summary>
        [FilterOption("TypeIDs")]
        public FOptTypeIDs TypeIDs;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOEquipment()
        {
            KeyField = "КодОборудования";
            NameField = "Название";
            Name = new FOptName();
            TypeIDs = new FOptTypeIDs();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_Оборудование;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", NameField);
    }
}