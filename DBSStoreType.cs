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
    ///     Класс Select для элемента управления Тип склада
    /// </summary>
    public class DBSStoreType : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSStoreType()
        {
            base.Filter = new DSOStoreType();
            KeyField = "Id";
            ValueField = "Name";
        }

        /// <summary>
        ///     Дополнительный фильтр по условиям поиска
        /// </summary>
        public new DSOStoreType Filter => (DSOStoreType) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetStoreTypes();
        }

        /// <summary>
        ///     Получение списка типов складов
        /// </summary>
        /// <returns>Список</returns>
        public List<StoreType> GetStoreTypes()
        {
            var dt = DBManager.GetData(SQLGetText(), Config.DS_person);

            var storeTypes = dt.AsEnumerable().Select(dr => new StoreType
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return storeTypes;
        }

        /// <summary>
        ///     Получение типа склада по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Сотрудник</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return new StoreType {Id = id, Name = name};
            return new StoreType(id);
        }
    }
}