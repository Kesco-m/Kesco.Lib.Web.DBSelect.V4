using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Corporate.Voip;
using Kesco.Lib.Web.Settings;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Kesco.Lib.Web.DBSelect.V4.DSO;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс select сущности Тип атрибута телефона
    /// </summary>
    public class DBSPhoneAttributeType : DBSelect
    {
        private List<PhoneAttributeType> _objectsList;

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOPhoneAttributeType Filter => (DSOPhoneAttributeType)base.Filter;

        /// <summary>
        ///      Источник значений
        /// </summary>
        public byte ValuesSource
        {
            get
            {
                return byte.Parse(Filter.ValuesSource.Value);
            }
            set
            {
                Filter.ValuesSource.Value = value.ToString();
            }
        }

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DBSPhoneAttributeType()
        {
            base.Filter = new DSOPhoneAttributeType();

            KeyField = "Id";
            ValueField = "Id";
            URLAdvancedSearch = string.Empty;
            URLShowEntity = string.Empty;
            MaxItemsInPopup = 1000;
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            //base.FillSelect(search);
            //return GetObjectsList();

            if (!string.IsNullOrEmpty(search))
                return GetObjectsList(search);

            return _objectsList ?? (_objectsList = GetObjectsList(search));
        }

        /// <summary>
        ///     Получение списка экземпляров
        /// </summary>
        /// <returns>Список</returns>
        public List<PhoneAttributeType> GetObjectsList(string search)
        {
            var dt = DBManager.GetData(SQLGetText(false), Config.DS_user, CommandType.Text, SQLGetInnerParams());

            var rows = dt.AsEnumerable().Select(dr => new PhoneAttributeType()
            {
                Id = dr.Field<string>("ТипАтрибутаТелефона"),
                ValuesSource = dr.Field<byte>("ИсточникЗначений"),
                Description = dr.Field<string>("Описание")
            });

            return string.IsNullOrEmpty(search) ? rows.ToList() : rows.Where(x => x.Id.Contains(search)).ToList();
        }

        /// <summary>
        ///     Получение экземпляра по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns></returns>
        public override object GetObjectById(string id, string name = "")
        {
            //var obj = new VoipAttribute(id);
            //obj.Load();
            //return obj;
            return new PhoneAttributeType(id);
        }
    }
}