using System;
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
    ///     Класс select сущности Тема лица
    /// </summary>
    public class DBSPersonTheme : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSPersonTheme()
        {
            base.Filter = new DSOPersonTheme();
            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = Config.person_types_search;
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOPersonTheme Filter => (DSOPersonTheme) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetPersonThemes();
        }

        /// <summary>
        ///     Получение списка тем
        /// </summary>
        /// <returns>Список</returns>
        public List<PersonTheme> GetPersonThemes()
        {
            URLAdvancedSearch = IsMultiSelect
                ? Config.person_types_search +
                  (Config.person_types_search.IndexOf("?", StringComparison.CurrentCulture) > 0 ? "&" : "?") +
                  "selectedid=" + string.Join(",", SelectedItems.Select(t => t.Id))
                : Config.person_types_search;


            var dt = DBManager.GetData(SQLGetText(false), Config.DS_person);

            var personThemes = dt.AsEnumerable().Select(dr => new PersonTheme
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return personThemes;
        }

        /// <summary>
        ///     Получение темы по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(id))
                return new PersonTheme(id);

            return new PersonTheme();
        }
    }
}