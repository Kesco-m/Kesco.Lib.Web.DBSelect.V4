using System;
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
    ///     Класс сущности Должность
    /// </summary>
    public class DBSPosition : DBSelect
    {
        /// <summary>
        ///     Конструктор Должность
        /// </summary>
        public DBSPosition()
        {
            base.Filter = new DSOPosition();

            KeyField = "Id";
            ValueField = "Name";
        }

        /// <summary>
        ///     Фильтр Должность
        /// </summary>
        public new DSOPosition Filter
        {
            get { return (DSOPosition) base.Filter; }
        }

        /// <summary>
        ///     Строка с перечислением через "," ID выбранных элементов
        /// </summary>
        public override string SelectedItemsString
        {
            get
            {
                var temp = SelectedItems.Aggregate("", (current, item) => current + ("'" + item.Id + "',"));
                if (!String.IsNullOrEmpty(temp)) temp = temp.Remove(temp.Length - 1, 1);
                return temp;
            }
        }

        /// <summary>
        ///     Заполнение списка Должность
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetPositions();
        }

        /// <summary>
        ///     Получение списка Должность
        /// </summary>
        /// <returns>Список</returns>
        public List<Position> GetPositions()
        {
            var dt = DBManager.GetData(SQLGetText(true), Config.DS_user, CommandType.Text, SQLGetInnerParams());

            var position = dt.AsEnumerable().Select(dr => new Position
            {
                Id = dr.Field<string>(Filter.KeyField),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return position;
        }

        /// <summary>
        ///     Получение Должность по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Должность</returns>
        public override object GetObjectById(string id, string name = "")
        {
            return new Position {Id = id, Name = id};
        }
    }
}