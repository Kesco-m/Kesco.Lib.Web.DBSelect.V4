using System.Collections.Generic;
using System.Collections;
using System.Data;
using Kesco.Lib.Web.Settings;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    /// Класс объекта для выбора типа отчета по складам из списка всех отчетов выписок
    /// </summary>
    public class DBSStoreReportType : DBSelect
    {
        //Список элементов
        private List<Item> _listOfItems = null;

        public DBSStoreReportType()
        {
            Filter = new DSOStoreReportType();

            KeyField = "Id"; //Filter.KeyField;
            ValueField = "Value"; //Filter.NameField;

            IsNotUseSelectTop = true;
        }

        /// <summary>
        /// Фильтр Типы отчетов
        /// </summary>
        public DSOStoreReportType GetFilter()
        {
            return Filter as DSOStoreReportType;
        }

        /// <summary>
        /// Заполнение списка Типы отчетов
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Интерфейс доступа к списку</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);

            FillItemsList();

            //if (!string.IsNullOrWhiteSpace(search))
            //    return _listOfItems.FindAll(x => { return x.Value.ToString().ToLower().Contains(search.ToLower()); });

            return _listOfItems;
        }

        /// <summary>
        /// Медот заполоняет список элементов из БД
        /// </summary>
        private void FillItemsList()
        {
            _listOfItems = new List<Item>();

            DataTable dtItems = DBManager.GetData(SQLGetText(true), Config.DS_person, CommandType.Text, SQLGetInnerParams());
            foreach (DataRow row in dtItems.Rows)
            {
                _listOfItems.Add(new Item { Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField] });
            }
        }

        public override object GetObjectById(string id, string name = "")
        {
            if (null == _listOfItems) FillItemsList();

            if (!string.IsNullOrWhiteSpace(name))
            {
                Item i_name = _listOfItems.Find(x => 0 == string.Compare(x.Id, id, true) && 0 == string.Compare(x.Value.ToString(), name, true));
                if (object.Equals(i_name, default(Kesco.Lib.Entities.Item))) return null;//значение Item по умолчанию все поля null
                return i_name;
            }

            Item i = _listOfItems.Find(x => 0==string.Compare(x.Id, id, true));
            if (object.Equals(i, default(Kesco.Lib.Entities.Item))) return null;//значение Item по умолчанию все поля null
            return i;
        }
    }
}
