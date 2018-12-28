using System.Collections;
using System.Data;
using System.Collections.Generic;
using Kesco.Lib.Web.Settings;
using Kesco.Lib.DALC;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Entities;


namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс сущности 
    /// </summary>
    public class DBSSqlQuery : DBSelect
    {
        //Список элементов
        List<Item> _listOfItems = null;

        /// <summary>
        /// ТипУсловия 
        /// </summary>
        public string QueryType { get; set; }

        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSSqlQuery()
        {
            DSOSqlQuery FilterSqlQuery = new DSOSqlQuery();

            base.Filter = FilterSqlQuery;
            KeyField = "Id"; //Filter.KeyField;
            ValueField = "Value"; //Filter.NameField;

            IsNotUseSelectTop = true;
        }

        /// <summary>
        /// Фильтр Подзапрос
        /// </summary>
        public DSOSqlQuery GetFilter()
        {
            return Filter as DSOSqlQuery;
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

            //if (!string.IsNullOrWhiteSpace(search))
            //    return _listOfItems.FindAll(x => { return x.Value.ToString().ToLower().Contains(search.ToLower()); });

            return _listOfItems;
        }

        /// <summary>
        /// Метод заполоняет список элементов из БД
        /// </summary>
        private void FillItemsList()
        {
            _listOfItems = new List<Item>();

            var dsoSqlQuery = Filter as DSOSqlQuery;
            if (dsoSqlQuery != null) dsoSqlQuery.QueryType = QueryType;

            DataTable dtItems = DBManager.GetData(SQLGetText(true), Config.DS_user, CommandType.Text, SQLGetInnerParams());
            //return dtItems.AsEnumerable();

            foreach (DataRow row in dtItems.Rows)
            {
                _listOfItems.Add(new Item { Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField] });
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
            /*
            if (null == _dtItems) return null;

            string filter = KeyField + "=" + id;
            if (!string.IsNullOrWhiteSpace(name))
               filter += " AND " + ValueField + "='" + name + "'";
            DataRow[] rows = _dtItems.Select(filter);
            return rows[0];
            */

            if (null == _listOfItems) FillItemsList();

            if (!string.IsNullOrWhiteSpace(name))
            {
                Item i_name = _listOfItems.Find(x => 0 == string.Compare(x.Id, id, true) && 0 == string.Compare(x.Value.ToString(), name, true));
                if (object.Equals(i_name, default(Kesco.Lib.Entities.Item))) return null;//значение Item по умолчанию все поля null
                return i_name;
            }

            Item i = _listOfItems.Find(x => 0 == string.Compare(x.Id, id, true));
            if (object.Equals(i, default(Kesco.Lib.Entities.Item))) return null;//значение Item по умолчанию все поля null
            return i;
        }
    }
}
