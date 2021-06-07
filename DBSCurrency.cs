using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс select сущности Валюта отпуска
    /// </summary>
    public class DBSCurrency : DBSelect
    {
        //Список элементов
        private List<Item> _listOfItems;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DBSCurrency()
        {
            base.Filter = new DSOCurrency();
            KeyField = "Id";
            ValueField = "Name";

            IsNotUseSelectTop = true;
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOCurrency Filter => (DSOCurrency) base.Filter;

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
        ///     Заполнение списка валют
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Интерфейс доступа к списку</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);

            FillItemsList();

            return _listOfItems;
        }

        /// <summary>
        ///     Метод заполоняет список элементов из БД
        /// </summary>
        private void FillItemsList()
        {
            _listOfItems = new List<Item>();

            var dtItems = DBManager.GetData(SQLGetText(false), Config.DS_person, CommandType.Text, SQLGetInnerParams());

            foreach (DataRow row in dtItems.Rows)
                _listOfItems.Add(new Item {Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField]});
        }

        /// <summary>
        ///     Поиск объекта по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="name">название объекта</param>
        /// <returns>Результат поиска, null - не найден</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (null == _listOfItems) FillItemsList();

            Item currency_item;
            if (!string.IsNullOrWhiteSpace(name))
                currency_item = _listOfItems.Find(x =>
                    0 == string.Compare(x.Id, id, true) && 0 == string.Compare(x.Value.ToString(), name, true));
            else
                currency_item = _listOfItems.Find(x => 0 == string.Compare(x.Id, id, true));

            if (Equals(currency_item, default(Item))) //значение Item по умолчанию все поля null
            {
                var sqlParams = new Dictionary<string, object> {{"@КодВалюты", id}};
                var dtItems = DBManager.GetData(SQLQueries.SELECT_ID_Валюты, Config.DS_person, CommandType.Text,
                    sqlParams);
                if (null != dtItems && dtItems.Rows.Count > 0)
                {
                    var row = dtItems.Rows[0];
                    return new Item {Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField]};
                }

                return null;
            }

            return currency_item;
        }
    }
}