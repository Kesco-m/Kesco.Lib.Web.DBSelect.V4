using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Socket;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Розетка
    /// </summary>
    public class DSOSocket : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по названию
        /// </summary>
        [FilterOption("Name")]
        public FOptName Name;

        /// <summary>
        ///     Опция фильтра по расположению
        /// </summary>
        [FilterOption("LocationIDs")]
        public FOptLocationIDs LocationIDs;

        /// <summary>
        ///     Опция фильтра по расположению
        /// </summary>
        [FilterOption("Location")]
        public FOptLocation Location;

        /// <summary>
        ///     Опция фильтра по состоянию (работает/не работает)
        /// </summary>
        [FilterOption("Connected")]
        public FOptAlive Alive;

        /// <summary>
        ///     Опция фильтра по состоянию (работает/не работает)
        /// </summary>
        [FilterOption("Free")]
        public FOptFree Free;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOSocket()
        {
            KeyField = "КодРозетки";
            NameField = "Название";
            Name = new FOptName();
            LocationIDs = new FOptLocationIDs();
            Location = new FOptLocation();
            Free = new FOptFree();
            Alive = new FOptAlive();

        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_Розетки;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", NameField);
    }
}