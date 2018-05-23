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
    ///     Класс select сущности Каталоги
    /// </summary>
    public class DBSPersonCatalog : DBSelect
    {
        
        //public bool IsUseEmptyValue { get; set; }

        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSPersonCatalog()
        {
            base.Filter = new DSOPersonCatalog();
            KeyField = "Id";
            ValueField = "Name";
            IsNotUseSelectTop = true;
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOPersonCatalog Filter
        {
            get { return (DSOPersonCatalog) base.Filter; }
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetCatalogs();
        }

        /// <summary>
        ///     Получение списка каталогов
        /// </summary>
        /// <returns>Список</returns>
        public List<PersonCatalog> GetCatalogs()
        {
            var dt = DBManager.GetData(SQLGetText(false), Config.DS_person);
            
            var personCatalogs = dt.AsEnumerable().Select(dr => new PersonCatalog
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();


            return personCatalogs;
        }

        /// <summary>
        ///     Получение типа формата атрибута по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!String.IsNullOrEmpty(id))
                return new PersonCatalog(id);

            return new PersonCatalog();
        }
    }
}