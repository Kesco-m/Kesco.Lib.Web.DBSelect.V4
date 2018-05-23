using System.Collections;
using System.Collections.Generic;
using System.Data;
using Kesco.Lib.DALC;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.Controls.V4;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления "Склад"
    /// </summary>
    public class DBSStore : DBSelect
    {
        //Список элементов
        List<Kesco.Lib.Entities.Item> _listOfItems = null;

        /// <summary>
        ///     Конструктор контрола
        /// </summary>
        public DBSStore()
        {
            base.Filter = new DSOStore();
            KeyField = "Id";
            ValueField = "Name";

            IsNotUseSelectTop = false;

            URLAdvancedSearch = Config.store_search;
            URLShowEntity = Config.store_form;
            URIsCreateEntity = new List<URICreateEntity>();
            URIsCreateEntity.Add(new URICreateEntity("/styles/Store.gif", Config.store_form,
                Resx.GetString("STORE_CreateStore")));
        }

        /// <summary>
        /// Фильтр Подзапрос
        /// </summary>
        public DSOStore GetFilter()
        {
            return Filter as DSOStore;
        }

        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);

            FillItemsList();

            return _listOfItems;
        }

        /// <summary>
        /// Медот заполоняет список элементов из БД
        /// </summary>
        private void FillItemsList()
        {
            _listOfItems = new List<Kesco.Lib.Entities.Item>();

            (Filter as DSOStore).Exceptions.Set(SelectedItemsString);

            DataTable dtItems = DBManager.GetData(SQLGetText(false), Config.DS_person, CommandType.Text, SQLGetInnerParams());

            foreach (DataRow row in dtItems.Rows)
            {
                _listOfItems.Add(new Kesco.Lib.Entities.Item { Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField] });
            }
        }

        public override object GetObjectById(string id, string name = "")
        {
            if (null == _listOfItems) FillItemsList();

            Kesco.Lib.Entities.Item store_item;
            if (!string.IsNullOrWhiteSpace(name))
            {
                store_item = _listOfItems.Find(x => 0 == string.Compare(x.Id, id, true) && 0 == string.Compare(x.Value.ToString(), name, true));
            }
            else
            {
                store_item = _listOfItems.Find(x => 0 == string.Compare(x.Id, id, true));
            }

            if (object.Equals(store_item, default(Kesco.Lib.Entities.Item)))//значение Item по умолчанию все поля null
            {
                var sqlParams = new Dictionary<string, object> { { "@КодСклада", id } };
                DataTable dtItems = DBManager.GetData(SQLQueries.SELECT_ID_Склад, Config.DS_person, CommandType.Text, sqlParams);
                if (null != dtItems && dtItems.Rows.Count > 0)
                {
                    DataRow row = dtItems.Rows[0];
                    return new Kesco.Lib.Entities.Item { Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField] };
                }

                return null;
            }

            return store_item;
        }
    }
}