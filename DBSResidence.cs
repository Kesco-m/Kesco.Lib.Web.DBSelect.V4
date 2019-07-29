using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Stores;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления Место хранения
    /// </summary>
    public class DBSResidence : DBSelect
    {
        /// <summary>
        ///     Конструктор контрола
        /// </summary>
        public DBSResidence()
        {
            base.Filter = new DSOResidence();
            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = Config.residences_search;
        }

        /// <summary>
        ///     Дополнительный фильтр по условиям поиска
        /// </summary>
        public new DSOResidence Filter => (DSOResidence) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetResidences();
        }

        /// <summary>
        ///     Получение списка мест хранения
        /// </summary>
        /// <returns>Список</returns>
        public List<Residence> GetResidences()
        {
            var dt = DBManager.GetData(SQLGetText(), Config.DS_person);

            var residences = dt.AsEnumerable().Select(dr => new Residence
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return residences;
        }

        /// <summary>
        ///     Получение места хранения по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Сотрудник</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return new Residence {Id = id, Name = name};
            return new Residence(id);
        }
    }
}