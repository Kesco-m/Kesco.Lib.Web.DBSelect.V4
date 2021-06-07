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
    ///     Класс сущности Единица измерения дополнительная
    /// </summary>
    public class DBSUnitAdv : DBSelect
    {
        /// <summary>
        ///     Конструктор Единица измерения дополнительная
        /// </summary>
        public DBSUnitAdv()
        {
            base.Filter = new DSOUnitAdv();

            KeyField = "Id";
            ValueField = "Name";
        }

        /// <summary>
        ///     Фильтр Единица измерения дополнительная
        /// </summary>
        public new DSOUnitAdv Filter => (DSOUnitAdv) base.Filter;

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
        ///     Заполнение списка Единица измерения дополнительная
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetUnits();
        }

        /// <summary>
        ///     Получение списка Единица измерения дополнительная
        /// </summary>
        /// <returns>Список</returns>
        public List<UnitAdv> GetUnits()
        {
            var dt = DBManager.GetData(SQLGetText(), Config.DS_resource, CommandType.Text, SQLGetInnerParams());

            var unit = dt.AsEnumerable().Select(dr => new UnitAdv
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return unit;
        }

        /// <summary>
        ///     Получение Единица измерения дополнительная по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Единица измерения</returns>
        public override object GetObjectById(string id, string name = "")
        {
            return new UnitAdv(id, Filter.Resource.ToString());
        }
    }
}