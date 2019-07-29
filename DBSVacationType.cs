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
    ///     Класс select сущности Видов отпуска
    /// </summary>
    public class DBSVacationType : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSVacationType()
        {
            base.Filter = new DSOVacationType();
            KeyField = "Id";
            ValueField = "Name";
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOVacationType Filter => (DSOVacationType) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetVacationTypes();
        }

        /// <summary>
        ///     Получение списка видов отпуска
        /// </summary>
        /// <returns>Список</returns>
        public List<VacationType> GetVacationTypes()
        {
            var dt = DBManager.GetData(SQLGetText(false), Config.DS_resource);

            var types = dt.AsEnumerable().Select(dr => new VacationType
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return types;
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
                return new VacationType(id);

            return new VacationType();
        }
    }
}