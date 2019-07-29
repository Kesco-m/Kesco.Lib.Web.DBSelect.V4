using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Contacts;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс select сущности Тип контакта
    /// </summary>
    public class DBSContactType : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSContactType()
        {
            base.Filter = new DSOContactType();

            KeyField = "Id";
            ValueField = "Name";
            IsNotUseSelectTop = true;
        }

        /// <summary>
        ///     Текущий язык интерфейса
        /// </summary>
        private string LangName { get; set; }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOContactType Filter => (DSOContactType) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetContactTypes();
        }

        /// <summary>
        ///     Получение списка складов
        /// </summary>
        /// <returns>Список</returns>
        public List<ContactType> GetContactTypes()
        {
            LangName = V4Page.CurrentUser.Language;
            var dt = DBManager.GetData(SQLGetText(false), Config.DS_person);

            var contactTypes = dt.AsEnumerable().Select(dr => new ContactType
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name =
                    LangName == "ru" ? dr.Field<string>(Filter.NameField) : dr.Field<string>(Filter.ContactTyleNameLat)
            }).ToList();

            return contactTypes;
        }

        /// <summary>
        ///     Получение типа формата атрибута по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            LangName = V4Page.CurrentUser.Language;
            if (!string.IsNullOrEmpty(id))
                return new ContactType(id, LangName);

            return new ContactType();
        }
    }
}