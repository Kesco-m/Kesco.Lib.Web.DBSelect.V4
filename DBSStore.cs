using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Stores;
using Kesco.Lib.Entities.Transport;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.Controls.V4.Common;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;
using Item = Kesco.Lib.Entities.Item;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления "Склад"
    /// </summary>
    public class DBSStore : DBSelect
    {
        //Список элементов
        private List<Store> _listOfItems;

        /// <summary>
        ///     Конструктор контрола
        /// </summary>
        public DBSStore()
        {
            base.Filter = new DSOStore();
            KeyField = "Id";
            ValueField = "Name";
            DisplayFields = "Name,StoreTypeName,ResourceName,KeeperName,ManagerName";
            AnvancedHeaderPopupResult =
                string.Format("<tr class='gridHeaderSelect v4s_noselect'><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                    Resx.GetString("STORE_Name"), Resx.GetString("TTN_lblType"), Resx.GetString("STORE_Resource"), Resx.GetString("STORE_Keeper"), Resx.GetString("TTN_lblSteward"));
            IsNotUseSelectTop = false;

            URLAdvancedSearch = Config.store_search;
            URLShowEntity = Config.store_form;
            URIsCreateEntity = new List<URICreateEntity>();
            URIsCreateEntity.Add(new URICreateEntity("/styles/Store.gif", Config.store_form,
                Resx.GetString("STORE_CreateStore")));
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOStore Filter
        {
            get { return (DSOStore) base.Filter; }
        }

        /// <summary>
        ///     Фильтр Подзапрос
        /// </summary>
        public DSOStore GetFilter()
        {
            return Filter;
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            var displayFields = "Name";
            var anvancedHeaderPopupResult = string.Format("<tr class='gridHeaderSelect v4s_noselect'><td>{0}</td>", Resx.GetString("STORE_Name"));
            if (Filter.StoreTypeId.Value == "")
            {
                displayFields += ",StoreTypeName";
                anvancedHeaderPopupResult += string.Format("<td>{0}</td>",Resx.GetString("TTN_lblType"));
            }
            if (Filter.StoreResourceId.Value == "")
            {
                displayFields += ",ResourceName";
                anvancedHeaderPopupResult += string.Format("<td>{0}</td>",Resx.GetString("STORE_Resource"));
            }

            displayFields += ",KeeperName";
            anvancedHeaderPopupResult += string.Format("<td>{0}</td>",Resx.GetString("STORE_Keeper"));

            if (Filter.ManagerId.Value == "")
            {
                displayFields += ",ManagerName";
                anvancedHeaderPopupResult += string.Format("<td>{0}</td>",Resx.GetString("TTN_lblSteward"));
            }

            DisplayFields = displayFields;
            AnvancedHeaderPopupResult = anvancedHeaderPopupResult;

            return GetStores(search, MaxItemsInQuery);
        }

        /// <summary>
        /// Метод заполоняет список элементов из БД
        /// </summary>
        /// <param name="search"></param>
        /// <param name="maxItemsInQuery"></param>
        /// <returns></returns>
        public List<Store> GetStores(string search, int maxItemsInQuery)
        {
            _listOfItems = new List<Store>();

            Filter.Exceptions.Set(SelectedItemsString);

            var dtItems = DBManager.GetData(SQLGetText(false), Config.DS_person, CommandType.Text, SQLGetInnerParams());

            foreach (DataRow row in dtItems.Rows)
            {
                _listOfItems.Add(new Store { Id = row[Filter.KeyField].ToString()
                    ,Name = row["СкладFull"].ToString()
                    ,StoreTypeName = row["Псевдоним"].ToString()
                    ,ResourceName = row["Ресурс"].ToString()
                    ,KeeperName = row["Хранитель"].ToString()
                    ,ManagerName = row["Распорядитель"].ToString()
                });
            }

            return _listOfItems;
        }

        /// <summary>
        ///     Получение Единица измерения дополнительная по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Единица измерения</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return new Item {Id = id, Value = name};
            
            var p = V4Page.ParentPage ?? V4Page;
            var  obj = p.GetObjectById(typeof (Store), id) as Store;
          
            return new Item {Id = obj.Id, Value = obj.FullName};
            
        }
    }
}