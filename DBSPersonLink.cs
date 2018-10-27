using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Entities.Link;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления связи лиц
    /// </summary>
    public class DBSPersonLink : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSPersonLink()
        {
            base.Filter = new DSOPersonLink();
            KeyField = "Id";
            ValueField = "Name";
            IsNotUseSelectTop = true;
            AutonomySelect = false;
            PersonLinks = new List<Link>();
            InnerLinks = new List<JSONLink>();
        }

        /// <summary>
        ///     Список выбора должен работать только с переданным списком
        /// </summary>
        public bool AutonomySelect { get; set; }

        /// <summary>
        ///     Переданный список
        /// </summary>
        public List<JSONLink> InnerLinks { get; set; }

        /// <summary>
        /// Тип связи
        /// </summary>
        public string LinkTypeID { get; set; }
        
        /// <summary>
        /// Ролитель
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// Потомок
        /// </summary>
        public string ChildID { get; set; }

        /// <summary>
        /// Автоматическая установка значения, если оно является единственным
        /// </summary>
        public bool AutoSetFirstValue { get; set; }

        /// <summary>
        /// Связи лиц
        /// </summary>
        public List<Link> PersonLinks { get; set; }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOPersonLink Filter
        {
            get { return (DSOPersonLink) base.Filter; }
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            if (AutonomySelect)
            {
                if (!String.IsNullOrEmpty(search))
                    return InnerLinks.Where(t => t.LinkName.Contains(String.Format("{0}", search)));

                var tempLinkList = new List<Link>();

                if (!String.IsNullOrEmpty(search))
                {
                    tempLinkList.AddRange(
                        InnerLinks.Where(t => t.LinkName.Contains(String.Format("{0}", search)))
                            .Select(links => new Link(links.LinkID, links.LinkName)));
                    return tempLinkList;
                }

                tempLinkList.AddRange(InnerLinks.Select(links => new Link(links.LinkID, links.LinkName)));

                return tempLinkList;
            }

            base.FillSelect(search);
            return GetPersonLinks();
        }

        /// <summary>
        ///     Получение списка связей лиц
        /// </summary>
        /// <returns>Список</returns>
        public List<Link> GetPersonLinks()
        {
            var query = SQLQueries.SELECT_СвязиЛиц;
            var sqlParams = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(LinkTypeID))
            {
                sqlParams.Add("@LinkTypeID", new object[] {LinkTypeID, DBManager.ParameterTypes.Int32});
            }

            if (!String.IsNullOrEmpty(ParentID))
            {
                sqlParams.Add("@ParentID", new object[] {ParentID, DBManager.ParameterTypes.Int32});
            }

            if (!String.IsNullOrEmpty(ChildID))
            {
                sqlParams.Add("@ChildID", new object[] {ChildID, DBManager.ParameterTypes.Int32});
            }

            if (!String.IsNullOrEmpty(LinkTypeID) && !String.IsNullOrEmpty(ParentID) && !String.IsNullOrEmpty(ChildID))
                query = SQLQueries.SELECT_СвязиЛиц_ПоРодителю_ПоПотомку_ПоТипуСвязи;
            else if (!String.IsNullOrEmpty(ParentID) && !String.IsNullOrEmpty(ChildID))
                query = SQLQueries.SELECT_СвязиЛиц_ПоРодителю_ПоПотомку;
            else if (!String.IsNullOrEmpty(ParentID) && !String.IsNullOrEmpty(LinkTypeID))
                query = SQLQueries.SELECT_СвязиЛиц_ПоРодителю_ПоТипуСвязи;
            else if (!String.IsNullOrEmpty(ChildID) && !String.IsNullOrEmpty(LinkTypeID))
                query = SQLQueries.SELECT_СвязиЛиц_ПоПотомку_ПоТипуСвязи;
            else if (!String.IsNullOrEmpty(LinkTypeID))
                query = SQLQueries.SELECT_СвязиЛиц_ПоТипуСвязи;
            else if (!String.IsNullOrEmpty(ParentID))
                query = SQLQueries.SELECT_СвязиЛиц_ПоРодителю;
            else if (!String.IsNullOrEmpty(ChildID))
                query = SQLQueries.SELECT_СвязьЛиц_ПоПотомку;

            var dt = DBManager.GetData(query, Config.DS_person, CommandType.Text, sqlParams);
            PersonLinks = dt.AsEnumerable().Select(dr => new Link
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name =
                    !String.IsNullOrEmpty(dr.Field<string>(Filter.NameField))
                        ? dr.Field<string>(Filter.NameField)
                        : "<Нет описания>"
            }).ToList();

            if (PersonLinks.Count == 1 && AutoSetFirstValue)
            {
                var firstOrDefault = PersonLinks.FirstOrDefault();
                if (firstOrDefault != null) Value = firstOrDefault.Id;
                IsDisabled = true;
            }
            else if (AutoSetFirstValue)
            {
                IsDisabled = false;
            }


            return PersonLinks;
        }

        /// <summary>
        ///     Получение связи лиц по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (AutonomySelect)
                return new Link(InnerLinks.Where(t => t.LinkID == id).Select(t => t.LinkID).FirstOrDefault(),
                    InnerLinks.Where(t => t.LinkID == id).Select(t => t.LinkName).FirstOrDefault());

            return new Link(id);
        }
    }
}