using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Corporate;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс select сущности Роль
    /// </summary>
    public class DBSRole : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSRole()
        {
            base.Filter = new DSORole();
            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = Config.roles_search;
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSORole Filter => (DSORole) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetRoles();
        }

        /// <summary>
        ///     Получение списка ролей
        /// </summary>
        /// <returns>Список</returns>
        public List<Role> GetRoles()
        {
            var dt = DBManager.GetData(SQLGetText(false), Config.DS_user);

            var roles = dt.AsEnumerable().Select(dr => new Role
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = "[" + dr.Field<int>(Filter.KeyField) + "] " + dr.Field<string>(Filter.NameField)
            }).ToList();

            return roles;
        }

        /// <summary>
        ///     Получение типа формата атрибута по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(id))
                return new Role(id);

            return new Role();
        }
    }
}