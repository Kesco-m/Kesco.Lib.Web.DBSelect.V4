using System.Collections;
using System.Collections.Generic;
using System.Data;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;
using Item = Kesco.Lib.Entities.Item;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления "Адрес лица"
    /// </summary>
    public class DBSPersonContact : DBSelect
    {
        //Список элементов
        private List<Item> _listOfItems;

        /// <summary>
        ///     Конструктор контрола
        /// </summary>
        public DBSPersonContact()
        {
            Filter = new DSOPersonContact();
            KeyField = "Id";
            ValueField = "Name";

            IsNotUseSelectTop = false;

            //URLAdvancedSearch = Config.person_search;
            //URLShowEntity = Config.person_contact_form_v4;
            //URLShowEntity = string.Concat(Config.person_contact_form_v4, "?id=", 0);

            //id=0&idclient={3}&personcontacttext={2}&personcontactcategor=1&personcontacttype={4}&docview=yes');\">",ID,Env.PersonsRoot,txt,dso.Person.GetItemValue(0),dso.Type.GetItemValue(0));
            URIsCreateEntity = new List<URICreateEntity>();
            URIsCreateEntity.Add(new URICreateEntity("/styles/User.gif", Config.person_contact_form_v4,
                Resx.GetString("TTN_lblCreateContact")));
        }


        /// <summary>
        ///     Фильтр Подзапрос
        /// </summary>
        public DSOPersonContact GetFilter()
        {
            return Filter as DSOPersonContact;
        }

        /// <summary>
        ///     Метод заполоняет список элементов из БД
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);

            FillItemsList();

            return _listOfItems;
        }

        /// <summary>
        ///     Возвращает список элементов из БД
        /// </summary>
        private void FillItemsList()
        {
            _listOfItems = new List<Item>();

            var dtItems = DBManager.GetData(SQLGetText(true), Config.DS_person, CommandType.Text, SQLGetInnerParams());

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

            Item store_item;
            if (!string.IsNullOrWhiteSpace(name))
                store_item = _listOfItems.Find(x =>
                    0 == string.Compare(x.Id, id, true) && 0 == string.Compare(x.Value.ToString(), name, true));
            else
                store_item = _listOfItems.Find(x => 0 == string.Compare(x.Id, id, true));

            if (Equals(store_item, default(Item))) //значение Item по умолчанию все поля null
            {
                var sqlParams = new Dictionary<string, object> {{"@КодКонтакта", id}};
                var dtItems = DBManager.GetData(SQLQueries.SELECT_ID_КонтактыЛица, Config.DS_person, CommandType.Text,
                    sqlParams);
                if (null != dtItems && dtItems.Rows.Count > 0)
                {
                    var row = dtItems.Rows[0];
                    return new Item {Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField]};
                }

                return null;
            }

            return store_item;
        }
    }
}