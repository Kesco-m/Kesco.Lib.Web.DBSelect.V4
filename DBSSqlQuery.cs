using System.Collections;
using System.Collections.Generic;
using System.Data;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс сущности
    /// </summary>
    public class DBSSqlQuery : DBSelect
    {
        //Список элементов
        private List<Item> _listOfItems;

        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSSqlQuery()
        {
            var FilterSqlQuery = new DSOSqlQuery();

            Filter = FilterSqlQuery;
            KeyField = "Id"; //Filter.KeyField;
            ValueField = "Value"; //Filter.NameField;

            IsNotUseSelectTop = true;
        }

        /// <summary>
        ///     ТипУсловия
        /// </summary>
        public string QueryType { get; set; }

        /// <summary>
        ///     Фильтр Подзапрос
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
        ///     Метод заполоняет список элементов из БД
        /// </summary>
        private void FillItemsList()
        {
            _listOfItems = new List<Item>();

            var dsoSqlQuery = Filter as DSOSqlQuery;
            if (dsoSqlQuery != null) dsoSqlQuery.QueryType = QueryType;

            var dtItems = DBManager.GetData(SQLGetText(true), Config.DS_user, CommandType.Text, SQLGetInnerParams());
            //return dtItems.AsEnumerable();

            foreach (DataRow row in dtItems.Rows)
                _listOfItems.Add(new Item {Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField]});
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
                var i_name = _listOfItems.Find(x =>
                    0 == string.Compare(x.Id, id, true) && 0 == string.Compare(x.Value.ToString(), name, true));
                if (Equals(i_name, default(Item))) return null; //значение Item по умолчанию все поля null
                return i_name;
            }

            var i = _listOfItems.Find(x => 0 == string.Compare(x.Id, id, true));
            if (Equals(i, default(Item))) return null; //значение Item по умолчанию все поля null
            return i;
        }
    }
}