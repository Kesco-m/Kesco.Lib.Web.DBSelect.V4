using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    /// </summary>
    public class DBSTMDogovor : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSTMDogovor()
        {
            base.Filter = new DSOTMDogovor();

            KeyField = "Id";
            ValueField = "Name";
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOTMDogovor Filter
        {
            get { return (DSOTMDogovor) base.Filter; }
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetDogovors();
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <returns>Список</returns>
        public List<Ghost> GetDogovors()
        {
            var dt = DBManager.GetData(SQLGetText(true), Config.DS_user, CommandType.Text, SQLGetInnerParams());

            var result = dt.AsEnumerable().Select(dr => new Ghost
            {
                Id = dr.Field<int>("КодДоговора").ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>("Договор")
            }).ToList();

            return result;
        }

        /// <summary>
        ///     Получение сотрудника по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Сотрудник</returns>
        public override object GetObjectById(string id, string name = "")
        {
            var dt = DBManager.GetData(SQLQueries.SELECT_ID_ДоговорСотовойСвязи, Config.DS_user, CommandType.Text,
                new Dictionary<string, object> {{"@id", id}});

            return dt.AsEnumerable().Select(dr => new Ghost
            {
                Id = dr.Field<int>("КодДоговора").ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>("Договор")
            }).FirstOrDefault();
        }
    }
}