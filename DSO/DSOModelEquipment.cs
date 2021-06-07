using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.ModelEquipment;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Модель оборудования
    /// </summary>
    public class DSOModelEquipment : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по названию
        /// </summary>
        [FilterOption("МодельОборудования")]
        public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOModelEquipment()
        {
            KeyField = "КодМоделиОборудования";
            NameField = "МодельОборудования";
            Name = new FOptName();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_МоделиIPТелефонов;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", NameField);
    }
}