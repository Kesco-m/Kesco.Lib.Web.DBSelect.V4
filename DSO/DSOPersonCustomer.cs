using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonCustomer;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Компания
    /// </summary>
    public class DSOPersonCustomer : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени компании
        /// </summary>
        [FilterOption("Кличка", false, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Опция фильтра по роли
        /// </summary>
        [FilterOption("CustomerRole")] public FOptPersonCustomerRole PersonCustomerRole;


        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPersonCustomer()
        {
            KeyField = "КодЛица";
            NameField = "Кличка";
            NameLatField = "КраткоеНазваниеЛат";
            Name = new FOptName();
            PersonCustomerRole = new FOptPersonCustomerRole();
        }

        /// <summary>
        ///     Параметр фильтрации: условие поиска по компаниям
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        ///     2 - Любое значение (не фильтруем по полю),
        ///     3 - Значение не указано (значение поля NULL)
        /// </summary>
        [FilterOption("CompanyHowSearch")]
        public int? CompanyHowSearch { get; set; }

        /// <summary>
        ///     Наименование Лат.
        /// </summary>
        public string NameLatField { get; set; }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ЛицаЗаказчики;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("T0.{0}", NameField);
    }
}