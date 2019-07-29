using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Location;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Класс объекта базы данных "Рабочие места"
    /// </summary>
    public class DSOLocation : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра: все расположения, для которых существует рабочие места указанного сотрудника
        /// </summary>
        [FilterOption("Employee", false)] public FOptEmployee Employee;

        /// <summary>
        ///     Опция фильтра по ID расположения
        /// </summary>
        [FilterOption("IDs", false, "IDs")] public FOptIDs IDs;

        /// <summary>
        ///     Опция фильтра по наименованию расположения
        /// </summary>
        [FilterOption("Name", false, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Опция фильтра: все подчиненные места хранения включая родителя
        /// </summary>
        [FilterOption("WorkPlace", false, "WorkPlace")]
        public FOptWorkPlace WorkPlace;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOLocation()
        {
            KeyField = "КодРасположения";
            NameField = "РасположениеPath1";

            IDs = new FOptIDs();
            Name = new FOptName();
            WorkPlace = new FOptWorkPlace();
            Employee = new FOptEmployee();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_Расположения;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => "T0.L";
    }
}