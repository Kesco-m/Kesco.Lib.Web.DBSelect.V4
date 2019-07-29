using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Entities.Persons.BusinessProject;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс select сущности Бизнес проект
    /// </summary>
    public class DBSBusinessProject : DBSelect
    {
        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DBSBusinessProject()
        {
            base.Filter = new DSOBusinessProject();

            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = Config.bproject_search;
            //URLShowEntity = Config.territory_form;
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOBusinessProject Filter => (DSOBusinessProject) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetBusinessProject(search);
        }

        /// <summary>
        ///     Получение списка бизнес проектов
        /// </summary>
        /// <returns>Список</returns>
        public List<BusinessProject> GetBusinessProject(string search)
        {
            var sqlParams = new Dictionary<string, object>
            {
                {"@search", new object[] {search, DBManager.ParameterTypes.String}}
            };
            var dt = DBManager.GetData(SQLQueries.SELECT_БизнесПроектыПоИмени, Config.DS_person, CommandType.Text,
                sqlParams);
            var businessProject = dt.AsEnumerable().Select(dr => new BusinessProject
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField).ToString(CultureInfo.InvariantCulture),
                BusinessProjectName = dr.Field<string>(Filter.NameField).ToString(CultureInfo.InvariantCulture)
            }).ToList();

            return businessProject;
        }

        /// <summary>
        ///     Получение типа формата атрибута по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            return new BusinessProject(id);
        }
    }
}