using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Entities.Persons;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс select сущности Организационно правовая форма
    /// </summary>
    public class DBSPersonIncorporationForm : DBSelect
    {
        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DBSPersonIncorporationForm()
        {
            base.Filter = new DSOPersonIncorporationForm();

            KeyField = "Id";
            ValueField = "Name";
            //URLAdvancedSearch = Config.bproject_search;
            //URLShowEntity = Config.territory_form;
        }

        /// <summary>
        ///     Тип лица
        /// </summary>
        public byte PersonType { get; set; }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOPersonIncorporationForm Filter => (DSOPersonIncorporationForm) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetBusinessProject(search);
        }

        /// <summary>
        ///     Получение списка форм
        /// </summary>
        /// <returns>Список</returns>
        public List<PersonIncorporationForm> GetBusinessProject(string search)
        {
            var sqlParams = new Dictionary<string, object>
            {
                {"@search", new object[] {search, DBManager.ParameterTypes.String}},
                {"@personKind", new object[] {PersonType, DBManager.ParameterTypes.String}}
            };
            var dt = DBManager.GetData(SQLQueries.SELECT_ОргПравФормуПоИмени, Config.DS_person, CommandType.Text,
                sqlParams);
            var businessProject = dt.AsEnumerable().Select(dr => new PersonIncorporationForm
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField).ToString(CultureInfo.InvariantCulture),
                ShortName = dr.Field<string>("КраткоеНазвание").ToString(CultureInfo.InvariantCulture),
                PersonType = Convert.ToByte(dr.Field<byte>("ТипЛица"))
            }).ToList();

            return businessProject;
        }

        /// <summary>
        ///     Получение типа формата атрибута по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            return new PersonIncorporationForm(id);
        }
    }
}