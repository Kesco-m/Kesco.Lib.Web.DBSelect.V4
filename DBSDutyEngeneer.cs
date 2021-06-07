using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Corporate;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;
using Item = Kesco.Lib.Entities.Item;

namespace Kesco.Lib.Web.DBSelect.V4
{  
    /// <summary>
    ///     Класс select сущности Дежурный инженер
    /// </summary>
    public class DBSDutyEngeneer : DBSelect
    {
        //Список элементов
        private List<Employee> _listOfItems;

        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSDutyEngeneer()
        {
            base.Filter = new DSODutyEngeneer();
            KeyField = "Id";
            ValueField = "Name";
            DisplayFields = "Name,DutyDays";
            URLShowEntity = Config.user_form;
            AnvancedHeaderPopupResult =
               string.Format(
                   "<tr class='gridHeaderSelect v4s_noselect'><td>{0}</td><td>{1}</td></tr>",
                   "Инженер", "Дежурств");
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSODutyEngeneer Filter => (DSODutyEngeneer)base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetDutyEngeneers();
        }

        /// <summary>
        ///     Получение списка дежурных инженеров
        /// </summary>
        /// <returns>Список</returns>
        public List<Employee> GetDutyEngeneers()
        {

            _listOfItems = new List<Employee>();
                       

            var dtItems = DBManager.GetData(SQLGetText(false), Config.DS_user);

            foreach (DataRow row in dtItems.Rows)
                _listOfItems.Add(new Employee
                {
                    Id = row[Filter.KeyField].ToString(),
                    Name = row[Filter.NameField].ToString(),
                    DutyDays = row["Дежурства"].ToString()
                });

            return _listOfItems;
           
        }

        /// <summary>
        ///     Получение типа формата атрибута по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Сотрудник</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return new Item { Id = id, Value = name };

            var p = V4Page.ParentPage ?? V4Page;
            var obj = p.GetObjectById(typeof(Employee), id) as Employee;

            return new Item { Id = obj.Id, Value = obj.FullName };
        }
    }
}
