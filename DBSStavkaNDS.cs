using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Corporate;
using Kesco.Lib.Entities.Resources;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс select сущности Ставка НДС
    /// </summary>
    public class DBSStavkaNDS : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSStavkaNDS()
        {
            base.Filter = new DSOStavkaNDS();
            KeyField = "Id";
            ValueField = "Name";
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOStavkaNDS Filter
        {
            get { return (DSOStavkaNDS)base.Filter; }
        }

        /// <summary>
        ///     Заполнение списка Ставка НДС
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetStavkaNDS();
        }

        /// <summary>
        ///     Получение списка ставок 
        /// </summary>
        /// <returns>Список</returns>
        public List<StavkaNDS> GetStavkaNDS()
        {
            var dt = DBManager.GetData(SQLGetText(true), Config.DS_resource, CommandType.Text, SQLGetInnerParams());

            var stavkaList = dt.AsEnumerable().Select(dr => new StavkaNDS()
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return stavkaList;
        }

        /// <summary>
        ///     Получение ставки НДС по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        public override object GetObjectById(string id, string name = "")
        {
            if (!String.IsNullOrEmpty(id))
                return new StavkaNDS(id);

            return new StavkaNDS();
        }
    }
}