using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.BaseExtention.Enums.Controls;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Corporate;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс сущности Сотрудник
    /// </summary>
    public class DBSEmployee : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSEmployee()
        {
            base.Filter = new DSOEmployee();

            KeyField = "Id";
            ValueField = "FullName";

            URLShowEntity = Config.user_form;
            URLAdvancedSearch = Config.user_search;
            CallerType = CallerTypeEnum.Employee;
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOEmployee Filter
        {
            get { return (DSOEmployee) base.Filter; }
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            Filter.SelectedId.Clear();
            Filter.SelectedId.Add(SelectedItemsString);
            base.FillSelect(search);
            return GetEmployees();
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <returns>Список</returns>
        public List<Employee> GetEmployees()
        {
            URLAdvancedSearch = IsMultiSelect
                ? Config.user_search + "?selectedid=" + string.Join(",", SelectedItems.Select(t => t.Id))
                : Config.user_search;
            if (IsMultiSelect)
            {
                CLID = 1;
                //Filter.EmployeeAvaible.ValueEmployeeAvaible = true;
                Filter.UserOur = "true";
                Filter.UserStaffMembers.ValueStatus = "true";
            }
            var dt = DBManager.GetData(SQLGetText(), Config.DS_user, CommandType.Text, SQLGetInnerParams());

            var result = dt.AsEnumerable().Select(dr => new Employee(false)
            {
                Id = dr.Field<int>("КодСотрудника").ToString(CultureInfo.InvariantCulture),
                FullName = dr.Field<string>("Сотрудник"),
                FullNameEn = dr.Field<string>("Employee")
            }).ToList();

            return result;
        }

        /// <summary>
        ///     Получение сотрудника по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Сотрудник</returns>
        public override object GetObjectById(string id, string name = "")
        {
            return new Employee(id);
        }
    }
}