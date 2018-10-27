using System.Collections;
using System.Collections.Generic;
using System.Data;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Stores;
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
        private List<Item> _listOfItems;

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

            FillItemsList();

            return _listOfItems;
        }

        /// <summary>
        ///     Медот заполоняет список элементов из БД
        /// </summary>
        private void FillItemsList()
        {
            _listOfItems = new List<Item>();

            Filter.Exceptions.Set(SelectedItemsString);

            var dtItems = DBManager.GetData(SQLGetText(false), Config.DS_person, CommandType.Text, SQLGetInnerParams());

            foreach (DataRow row in dtItems.Rows)
            {
                _listOfItems.Add(new Item {Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField]});
            }
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
          
            return new Item {Id = obj.Id, Value = obj.Name};
            
        }
    }
}