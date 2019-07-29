using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Resources;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс сущности Единица измерения
    /// </summary>
    public class DBSUnit : DBSelect
    {
        /// <summary>
        ///     Конструктор Единица измерения
        /// </summary>
        public DBSUnit()
        {
            base.Filter = new DSOUnit();

            KeyField = "Id";
            ValueField = "Описание";
        }

        /// <summary>
        ///     Фильтр Единица измерения
        /// </summary>
        public new DSOUnit Filter => (DSOUnit) base.Filter;

        /// <summary>
        ///     Строка с перечислением через "," ID выбранных элементов
        /// </summary>
        public override string SelectedItemsString
        {
            get
            {
                var temp = SelectedItems.Aggregate("", (current, item) => current + "'" + item.Id + "',");
                if (!string.IsNullOrEmpty(temp)) temp = temp.Remove(temp.Length - 1, 1);
                return temp;
            }
        }

        /// <summary>
        ///     Заполнение списка Единица измерения
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetUnits();
        }

        /// <summary>
        ///     Получение списка Единица измерения
        /// </summary>
        /// <returns>Список</returns>
        public List<Unit> GetUnits()
        {
            var dt = DBManager.GetData(SQLGetText(true), Config.DS_resource, CommandType.Text, SQLGetInnerParams());

            var unit = dt.AsEnumerable().Select(dr => new Unit
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return unit;
        }

        /// <summary>
        ///     Получение Единица измерения по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Единица измерения</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return new Unit {Id = id, Name = name};

            var obj = V4Page.GetObjectById(typeof(Unit), id) as Unit;

            return obj;
        }
    }
}