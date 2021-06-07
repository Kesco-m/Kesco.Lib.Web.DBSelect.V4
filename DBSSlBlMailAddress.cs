using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Corporate.Equipments;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{

    /// <summary>
    ///     Класс select сущности SlBlMailAddress
    /// </summary>
    public class DBSSlBlMailAddress : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSSlBlMailAddress()
        {
            base.Filter = new DSOSlBlMailAddress();
            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = "";
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOSlBlMailAddress Filter => (DSOSlBlMailAddress)base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetAddresses();
        }

        /// <summary>
        ///     Получение списка адресов
        /// </summary>
        /// <returns>Список</returns>
        public List<SlBlAddress> GetAddresses()
        {
            var dt = DBManager.GetData(SQLGetText(false), Config.DS_user);

            var addrs = dt.AsEnumerable().Select(dr => new SlBlAddress
            {
                Id = dr.Field<string>(Filter.KeyField),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return addrs;
        }

        /// <summary>
        ///     Получение типа формата атрибута по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>SlBl</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(id))
                return new SlBlAddress(id);

            return new SlBlAddress();
        }
    }
}