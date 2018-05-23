using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Territory;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Организационно правовая форма
    /// </summary>
    public class DSOPersonIncorporationForm : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени бизнес проекта
        /// </summary>
        [FilterOption("ОргПравФорма", false, "Search")] public FOptName Name;

        /// <summary>
        ///     Опция фильтра по имени ТипЛица
        /// </summary>
        public byte PersonType;

        /// <summary>
        ///     Опция фильтра по имени КраткоеНазвание
        /// </summary>
        public string ShortName;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPersonIncorporationForm()
        {
            KeyField = "КодОргПравФормы";
            NameField = "ОргПравФорма";
            Name = new FOptName();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_ID_ОрганизационноПравоваяФорма; }
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy
        {
            get { return string.Format("{0}", NameField); }
        }
    }
}