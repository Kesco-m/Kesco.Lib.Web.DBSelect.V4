using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.SlBl;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
    { /// <summary>
      ///     Источник данных для котроа выбора SlBl адреса
      /// </summary>
    public class DSOSlBlMailAddress : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по адресу
        /// </summary>
        [FilterOption("Address", false, "Search")]
        public FOptSlBlMailAddress Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOSlBlMailAddress()
        {
            KeyField = "Address";
            NameField = "Address";
            Name = new FOptSlBlMailAddress();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_SlBlAddresses;

        /// <summary>
        ///     Задание сортировки выборки
        ///     Доделать управляемую сортировку
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", KeyField);
    }
}
