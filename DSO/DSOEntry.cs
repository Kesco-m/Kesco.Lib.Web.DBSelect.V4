using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Записи
    /// </summary>
    public class DSOEntry : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по названию
        /// </summary>
        [FilterOption("Search", true, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOEntry()
        {
            KeyField = "КодЗаписи";
            NameField = "Название";

            Name = new FOptName();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => "SELECT КодЗаписи, Название FROM Записи";
    }
}