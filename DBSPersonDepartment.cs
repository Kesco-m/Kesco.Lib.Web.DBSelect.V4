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
    /// Класс объекта для выбора конкретного Подразделения лица из списка всех подразделений лица
    /// </summary>
    public class DBSPersonDepartment : DBSelect
    {
        //Список элементов
        List<Item> _listOfItems = null;

        public DBSPersonDepartment()
        {
            Filter = new DSOPersonDepartment();

            KeyField = "Id"; //Filter.KeyField;
            ValueField = "Value"; //Filter.NameField;

            IsNotUseSelectTop = true;
        }

        /// <summary>
        /// Фильтр Подразделение
        /// </summary>
        public DSOPersonDepartment GetFilter()
        {
            return Filter as DSOPersonDepartment;
        }

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

            DataTable dtItems = DBManager.GetData(SQLGetText(false), Config.DS_person, CommandType.Text, SQLGetInnerParams());
            //return dtItems.AsEnumerable();

            foreach (DataRow row in dtItems.Rows)
            {
                _listOfItems.Add(new Item { Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField] });
            }
        }

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
