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
    ///     Класс select сущности Оборудование
    /// </summary>
    public class DBSEquipment : DBSelect
    {
        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DBSEquipment()
        {
            base.Filter = new DSOEquipment();

            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = Config.equipment_search;
            URLShowEntity = Config.equipment_form;
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOEquipment Filter => (DSOEquipment) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetObjectsList();
        }

        /// <summary>
        ///     Получение списка экземпляров
        /// </summary>
        /// <returns>Список</returns>
        public List<Equipment> GetObjectsList()
        {
            var dt = DBManager.GetData(SQLGetText(true), Config.DS_user, CommandType.Text, SQLGetInnerParams());

            var list = dt.AsEnumerable().Select(dr => new Equipment()
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return list;
        }

        /// <summary>
        ///     Получение экземпляра объекта по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns></returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name) && id != name)
                return new Equipment { Id = id, Name = name };
            return new Equipment(id);
        }
    }
}