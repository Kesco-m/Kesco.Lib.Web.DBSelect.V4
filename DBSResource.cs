using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.BaseExtention.Enums.Controls;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Resources;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления Ресурс
    /// </summary>
    public class DBSResource : DBSelect
    {
        /// <summary>
        ///     Конструктор контрола
        /// </summary>
        public DBSResource()
        {
            base.Filter = new DSOResource();
            KeyField = "Id";
            ValueField = "Name";
            URLShowEntity = Config.resource_form;
            URLAdvancedSearch = Config.resource_search;
            URIsCreateEntity = new List<URICreateEntity>();
            URIsCreateEntity.Add(new URICreateEntity("/styles/Resource.gif", Config.resource_form,
                Resx.GetString("sCreateResource")));
        }

        /// <summary>
        ///     Дополнительный фильтр по условиям поиска
        /// </summary>
        public new DSOResource Filter
        {
            get { return (DSOResource) base.Filter; }
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetResources();
        }

        /// <summary>
        ///     Получение списка ресурсов
        /// </summary>
        /// <returns>Список ресурсов</returns>
        public List<Resource> GetResources()
        {
            var sqlBatchDefault = Filter.SQLBatch;
            var dt = new DataTable();

            if (Filter.PersonIDs.Count > 0)
            {
                Filter.PersonIDs.SearchStrategy = SearchResources.SearchResOnlyForSpecifiedPersons;
                dt = DBManager.GetData(SQLGetText(), Config.DS_resource);

                if (dt.Rows.Count == 0)
                {
                    Filter.PersonIDs.SearchStrategy = SearchResources.SearchAllResForPersons;
                    dt = DBManager.GetData(SQLGetText(), Config.DS_resource);
                }
            }

            if (Filter.PersonIDs.Count == 0 || dt.Rows.Count == 0)
            {
                Filter.PersonIDs.SearchStrategy = SearchResources.None;
                dt = DBManager.GetData(SQLGetText(), Config.DS_resource);
            }

            var resources = dt.AsEnumerable().Select(dr => new Resource
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return resources;
        }

        /// <summary>
        ///     Получение ресурса по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Ресурс</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return new Resource {Id = id, Name = name};
            return new Resource(id);
        }
    }
}