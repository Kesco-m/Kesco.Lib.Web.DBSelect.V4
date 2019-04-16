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
    ///     Класс select сущности Вид перемещений денежных средств
    /// </summary>
    public class DBSCashFlowType : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSCashFlowType()
        {
            base.Filter = new DSOCashFlowType();

            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = Config.cash_flow_type_search;
            URLShowEntity = Config.cash_flow_type_form;
        }

        /// <summary>
        ///     Текущий язык интерфейса
        /// </summary>
        private string LangName { get; set; }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOCashFlowType Filter
        {
            get { return (DSOCashFlowType)base.Filter; }
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetCashFlowType();
        }

        /// <summary>
        ///     Получение списка
        /// </summary>
        /// <returns>Список</returns>
        public List<CashFlowType> GetCashFlowType()
        {
            var dt = DBManager.GetData(SQLGetText(false), Config.DS_resource, CommandType.Text, SQLGetInnerParams());

            var unit = dt.AsEnumerable().Select(dr => new CashFlowType
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField),
                Name1С = dr.Field<string>(Filter.Name1CField)
            }).ToList();

            return unit;
        }

        /// <summary>
        ///     Получение по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>сущность</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return new CashFlowType { Id = id, Name = name };

            var obj = V4Page.GetObjectById(typeof(CashFlowType), id) as CashFlowType;

            return obj;
        }
    }
}