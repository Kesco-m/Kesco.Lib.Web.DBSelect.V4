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
    ///     Класс select сущности Модель оборудования
    /// </summary>
    public class DBSModelEquipment : DBSelect
    {
        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DBSModelEquipment()
        {
            base.Filter = new DSOModelEquipment();

            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = Config.model_search;
            URLShowEntity = Config.model_form;
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOModelEquipment Filter => (DSOModelEquipment) base.Filter;

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
        public List<ModelEquipment> GetObjectsList()
        {
            var dt = DBManager.GetData(SQLGetText(true), Config.DS_user, CommandType.Text, SQLGetInnerParams());

            var list = dt.AsEnumerable().Select(dr => new ModelEquipment()
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
            return new ModelEquipment(id);
        }
    }
}