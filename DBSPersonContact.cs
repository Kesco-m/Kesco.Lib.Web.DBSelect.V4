using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Entities.Corporate;
using Kesco.Lib.Entities.Persons.Contacts;
using Kesco.Lib.Entities.Transport;
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
        private List<Contact> _listOfItems;

        /// <summary>
        ///     Конструктор контрола
        /// </summary>
        public DBSPersonContact()
        {
            Filter = new DSOPersonContact();
            KeyField = "Id";
            ValueField = "Name";
            DisplayFields = "Name, ContactTypeName";
            IsNotUseSelectTop = false;
            AnvancedHeaderPopupResult =
                string.Format(
                    "<tr class='gridHeaderSelect v4s_noselect'><td><b>{0}</b></td><td><b>{1}</b></td></tr>",
                    Resx.GetString("lblContact"), Resx.GetString("TTN_lblType"));

            //URLAdvancedSearch = Config.person_search;
            //URLShowEntity = Config.person_contact_form_v4;
            //URLShowEntity = string.Concat(Config.person_contact_form_v4, "?id=", 0);
            //id=0&idclient={3}&personcontacttext={2}&personcontactcategor=1&personcontacttype={4}&docview=yes');\">",ID,Env.PersonsRoot,txt,dso.Person.GetItemValue(0),dso.Type.GetItemValue(0));

            var employee = new Employee(true);

            // добавление разрешено для роли "Администратор лиц"
            if (employee.HasRole(11))
            {
                URIsCreateEntity = new List<URICreateEntity>
                {
                    new URICreateEntity("/styles/User.gif", Config.person_contact_form_v4,
                        Resx.GetString("TTN_lblCreateContact"))
                };
            }
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
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetContacts(search, MaxItemsInQuery);
        }

        /// <summary>
        ///     Список для выбора транспортных узлов
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <param name="maxItemsInQuery">Количество возвращаемых записей в запросе (top n)</param>
        /// <returns>Список транспортных узлов</returns>
        public List<Contact> GetContacts(string search, int maxItemsInQuery)
        {
            _listOfItems = new List<Contact>();
            var dtItems = DBManager.GetData(SQLGetText(true), Config.DS_person, CommandType.Text, SQLGetInnerParams());

            foreach (DataRow row in dtItems.Rows)
            {
                _listOfItems.Add(new Contact
                {
                    Id = row["КодКонтакта"].ToString(),
                    Name = Regex.Replace(row["Контакт"].ToString(), @"[\n]", ""),
                    ContactName = row["Контакт"].ToString(),
                    ContactTypeName = row["ТипКонтакта"].ToString(),
                });
            }
            

            return _listOfItems;
        }

        /// <summary>
        ///     Поиск объекта по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="name">название объекта</param>
        /// <returns>Результат поиска, null - не найден</returns>
        public override object GetObjectById(string id, string name = "")
        {
            //if (!string.IsNullOrEmpty(name))
            //    return new Item { Id = id, Value = name };

            //var p = V4Page.ParentPage ?? V4Page;
            //var obj = p.GetObjectById(typeof(Contact), id) as Contact;

            //return new Item { Id = obj.Id, Value = obj.Name };

            //if (null == _listOfItems)
            //{
                FillSelect("");
            //}

            Contact store_item;
            if (!string.IsNullOrWhiteSpace(name) && name!="undefined")
                store_item = _listOfItems.Find(x =>
                    0 == string.Compare(x.Id, id, true) && 0 == string.Compare(x.Name.ToString(), name, true));
            else
                store_item = _listOfItems.Find(x => 0 == string.Compare(x.Id, id, true));

            if (Equals(store_item, default(Item))) //значение Item по умолчанию все поля null
            {
                var sqlParams = new Dictionary<string, object> { { "@КодКонтакта", id } };
                var dtItems = DBManager.GetData(SQLQueries.SELECT_ID_КонтактыЛица, Config.DS_person, CommandType.Text,
                    sqlParams);
                if (null != dtItems && dtItems.Rows.Count > 0)
                {
                    var row = dtItems.Rows[0];
                    return new Contact{ Id = row[Filter.KeyField].ToString(), Name = row[Filter.NameField].ToString() };
                }

                return null;
            }

            return store_item;
        }
    }
}