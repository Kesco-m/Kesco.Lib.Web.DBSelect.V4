using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Corporate;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс сущности Подразделение
    /// </summary>
    public class DBSSubdivision : DBSelect
    {
        /// <summary>
        ///     Конструктор Подразделение
        /// </summary>
        public DBSSubdivision()
        {
            base.Filter = new DSOSubdivision();

            KeyField = "Id";
            ValueField = "Name";
        }

        /// <summary>
        ///     Фильтр Подразделение
        /// </summary>
        public new DSOSubdivision Filter => (DSOSubdivision) base.Filter;

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
        ///     Заполнение списка Подразделение
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetSubdivisions();
        }

        /// <summary>
        ///     Получение списка Подразделение
        /// </summary>
        /// <returns>Список</returns>
        public List<Subdivision> GetSubdivisions()
        {
            var dt = DBManager.GetData(SQLGetText(true), Config.DS_user, CommandType.Text, SQLGetInnerParams());

            var subdivision = dt.AsEnumerable().Select(dr => new Subdivision
            {
                Id = dr.Field<string>(Filter.KeyField),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return subdivision;
        }

        /// <summary>
        ///     Получение Подразделение по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Подразделение</returns>
        public override object GetObjectById(string id, string name = "")
        {
            return new Subdivision {Id = id, Name = id};
        }
    }
}