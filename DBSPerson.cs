using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.BaseExtention.Enums.Controls;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Persons;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления Лицо
    /// </summary>
    public class DBSPerson : DBSelect
    {
        /// <summary>
        ///     Конструктор контрола
        /// </summary>
        public DBSPerson()
        {
            base.Filter = new DSOPerson();
            AutonomySelect = false;
            InnerPersonsIDs = new List<string>();
            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = Config.person_search;
            URLShowEntity = Config.person_form;
            URIsCreateEntity = new List<URICreateEntity>
            {
                new URICreateEntity("/styles/DataNP.gif", Config.person_np_add,
                    Resx.GetString("sCreateIndividual")),
                new URICreateEntity("/styles/DataJP.gif", Config.person_jp_add,
                    Resx.GetString("sCreateLegal"))
            };
            CallerType = CallerTypeEnum.Person;
        }

        /// <summary>
        ///     Список выбора должен работать только с переданным списком лиц
        /// </summary>
        public bool AutonomySelect { get; set; }

        /// <summary>
        ///     Переданный список ID лиц(для строгого поиска)
        /// </summary>
        public List<string> InnerPersonsIDs { get; set; }

        /// <summary>
        ///     Список рекомендуемых ID лиц
        /// </summary>
        public List<string> WeakList { get; set; }

        /// <summary>
        ///     Дополнительный фильтр по условиям поиска
        /// </summary>
        public new DSOPerson Filter
        {
            get { return (DSOPerson) base.Filter; }
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetPersons();
        }

        /// <summary>
        ///     Получение списка лиц
        /// </summary>
        /// <returns>Список</returns>
        public List<Person> GetPersons()
        {
            var dt = DBManager.GetData(SQLGetText(), Config.DS_person, CommandType.StoredProcedure, SQLGetInnerParams());

            var persons = dt.AsEnumerable().Select(dr => new Person
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            if (null!=WeakList && WeakList.Count > 0)
            {
                var toInsert = new List<Person>(WeakList.Count);
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var w in WeakList)
                {
                    var wPerson = persons.FirstOrDefault(p => p.Id == w);
                    toInsert.Add(wPerson ?? new Person(w));
                }
                MaxItemsInPopup = toInsert.Count;
                persons.InsertRange(0, toInsert);
            }

            if (AutonomySelect)
            {
                return persons.Where(t => InnerPersonsIDs.Contains(t.Id)).ToList();
            }
            return persons;
        }

        /// <summary>
        ///     Получение лица по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Сотрудник</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return new Person {Id = id, Name = name};
            return new Person(id);
        }
    }
}