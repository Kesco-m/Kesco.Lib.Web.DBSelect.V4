using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления Выбор расположений
    /// </summary>
    public class DBSLocation : DBSelect
    {
        /// <summary>
        ///     Конструктор контрола
        /// </summary>
        public DBSLocation()
        {
            base.Filter = new DSOLocation();
            KeyField = "Id";
            ValueField = "Name";
            URLShowEntity = Config.location_search;
            URLAdvancedSearch = Config.location_search;
        }

        /// <summary>
        ///     Дополнительный фильтр по условиям поиска
        /// </summary>
        public new DSOLocation Filter => (DSOLocation) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetLocations();
        }

        /// <summary>
        ///     Получение списка расположений
        /// </summary>
        /// <returns>Список</returns>
        public List<Location> GetLocations()
        {
            var dt = DBManager.GetData(SQLGetText(), Config.DS_user);

            var locations = dt.AsEnumerable().Select(dr => new Location
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return locations;
        }

        /// <summary>
        ///     Получение расположения по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Сотрудник</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return new Location {Id = id, Name = name};
            return new Location(id);
        }
    }
}