using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Persons;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс сущности Лицо-по ролям
    /// </summary>
    public class DBSPersonRole : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSPersonRole()
        {
            base.Filter = new DSOPersonRole();

            KeyField = "Id";
            ValueField = "Name";
            URLShowEntity = Config.person_form;
            URLAdvancedSearch = "";
        }


        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOPersonRole Filter => (DSOPersonRole) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetCompanies();
        }

        /// <summary>
        ///     Получение списка лиц
        /// </summary>
        /// <returns>Список</returns>
        public List<PersonCustomer> GetCompanies()
        {
            var dt = DBManager.GetData(SQLGetText(false), Config.DS_user, CommandType.Text, SQLGetInnerParams());

            var persons = dt.AsEnumerable().Select(dr => new PersonCustomer
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return persons;
        }

        /// <summary>
        ///     Получение компании по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return new Person {Id = id, Name = name};
            return new Person(id);
        }
    }
}