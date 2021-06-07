using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.BaseExtention.Enums.Controls;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Persons;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс сущности Лицо-заказчик
    /// </summary>
    public class DBSPersonCustomer : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSPersonCustomer()
        {
            base.Filter = new DSOPersonCustomer();

            KeyField = "Id";
            ValueField = "Name";
            URLShowEntity = Config.person_form;
            URLAdvancedSearch = Config.person_search;
            IsCaller = true;
            CallerType = CallerTypeEnum.Person;
        }


        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOPersonCustomer Filter => (DSOPersonCustomer) base.Filter;

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
        ///     Получение списка компании
        /// </summary>
        /// <returns>Список</returns>
        public List<PersonCustomer> GetCompanies()
        {
            var dt = DBManager.GetData(SQLGetText(true), Config.DS_user, CommandType.Text, SQLGetInnerParams());

            var persons = dt.AsEnumerable().Select(dr => new PersonCustomer
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField),
                NameLat = dr.Field<string>(Filter.NameLatField)
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
                return new PersonCustomer {Id = id, Name = name};
            return new PersonCustomer(id);
        }
    }
}