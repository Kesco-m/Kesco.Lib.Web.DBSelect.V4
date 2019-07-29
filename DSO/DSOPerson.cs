using System;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Common;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для контрола выбора лица
    /// </summary>
    public class DSOPerson : DSOCommon
    {
        /// <summary>
        ///     Параметр фильтрации: поиск по. По-умолчанию по названию
        /// </summary>
        private int _personWhereSearch = 1;

        /// <summary>
        ///     Опция фильтра по наименованию лица
        /// </summary>
        [FilterOption("Search", true, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPerson()
        {
            KeyField = "КодЛица";
            NameField = "Кличка";

            Name = new FOptName();

            IsAddExcludeCondition = false;
        }

        /// <summary>
        ///     Параметр фильтрации: поиск по
        ///     1 - названию;
        ///     2 - реквизитам;
        ///     4 - контактам
        /// </summary>
        [FilterOption("PersonWhereSearch", true)]
        public int PersonWhereSearch
        {
            get { return _personWhereSearch; }
            set { _personWhereSearch = value; }
        }

        /// <summary>
        ///     Параметр фильтрации:
        ///     0 - для каждого слова в строке поиска существует начинающееся с него слово в реквизите;
        ///     1 - для каждого слова в строке поиска существует содержащее его слово в реквизите;
        /// </summary>
        [FilterOption("PersonHowSearch", true)]
        public int PersonHowSearch { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по
        ///     1	- Юридическим лицам
        ///     2	- Физическим лицм
        ///     4	- Банкам(БИК или SWIFT)
        ///     0;7	- всем
        /// </summary>
        [FilterOption("PersonType", true, "PersonType")]
        public int? PersonType { get; set; }

        /// <summary>
        ///     Параметр фильтрации: ID бизнес-проекта
        /// </summary>
        [FilterOption("PersonBProject", true, "PersonBProject")]
        public int? PersonBProject { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по родительскому и подчиненным бизнес-проектам
        /// </summary>
        [FilterOption("PersonSubBProject", true, "PersonSubBProject")]
        public bool PersonSubBProject { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по всем бизнес-проектам
        /// </summary>
        [FilterOption("PersonAllBProject", true, "PersonAllBProject")]
        public bool? PersonAllBProject { get; set; }

        /// <summary>
        ///     Параметр фильтрации:
        ///     1 - Лицо проверено
        ///     2 - Лицо не проверено
        /// </summary>
        [FilterOption("PersonCheck", true, "PersonCheck")]
        public int? PersonCheck { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по ID организационно-правовой формы
        /// </summary>
        [FilterOption("PersonOPForma", true, "PersonOPForma")]
        public int? PersonOPForma { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по типам лиц
        /// </summary>
        [FilterOption("PersonThemes", true, "PersonThemes")]
        public string PersonThemes { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по ответственным сотрудникам
        /// </summary>
        [FilterOption("PersonUsers", true, "PersonUsers")]
        public string PersonUsers { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по родительскому и подчиненным типам лиц
        /// </summary>
        [FilterOption("PersonSubThemes", true, "PersonSubThemes")]
        public bool? PersonSubThemes { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по коду территории (страна регистрации)
        /// </summary>
        [FilterOption("PersonArea", true, "PersonArea")]
        public int? PersonArea { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по принадлежности страны таможенному союзу
        /// </summary>
        [FilterOption("PersonTUnion", true, "PersonTUnion")]
        public bool? PersonTUnion { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по связям лиц
        /// </summary>
        [FilterOption("PersonLink", true, "PersonLink")]
        public int? PersonLink { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по типам связи лиц
        ///     1 --Работники				1
        ///     2 --Места работы			1
        ///     21 --Подчиненные организации		2
        ///     22 --Головные организаци		2
        ///     31 --Представители			3
        ///     32 --Руководители			3
        ///     41 --Собственность			4
        ///     42 --Владельцы				4
        /// </summary>
        [FilterOption("PersonLinkType", true, "PersonLinkType")]
        public int? PersonLinkType { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по дате действия связи
        /// </summary>
        [FilterOption("PersonLinkValidAt", true, "PersonLinkValidAt")]
        public DateTime? PersonLinkValidAt { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по типу подписи
        /// </summary>
        [FilterOption("PersonSignType", true, "PersonSignType")]
        public int? PersonSignType { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по действующим на дату лицам
        /// </summary>
        [FilterOption("PersonValidAt", true, "PersonValidAt")]
        public DateTime? PersonValidAt { get; set; }

        /// <summary>
        ///     Параметр фильтрации: поиск по контакту лица
        /// </summary>
        [FilterOption("PersonForSend", true, "PersonForSend")]
        public int? PersonForSend { get; set; }

        /// <summary>
        ///     Параметр фильтрации: параметр расширенного поиска
        ///     1|2|3|4|5|6 -> ИНН|БИК|КS|SWIFT|КПП|ГосОрганизации
        /// </summary>
        [FilterOption("PersonAdvSearch", true, "PersonAdvSearch")]
        public int? PersonAdvSearch { get; set; }

        /// <summary>
        ///     Число строк вывода в запросе
        /// </summary>
        [FilterOption("PersonSelectTop", true)]
        public int? PersonSelectTop { get; set; }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SP_Лица_Поиск;

        /// <summary>
        ///     Запрос получения лица по ID
        /// </summary>
        public override string SQLEntityById => SQLQueries.SELECT_ID_Лицо;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Empty;
    }
}