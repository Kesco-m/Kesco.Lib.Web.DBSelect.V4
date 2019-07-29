using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Link;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления связи лиц
    /// </summary>
    public class DBSPersonSigner : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSPersonSigner()
        {
            base.Filter = new DSOPersonSigner();
            KeyField = "Id";
            ValueField = "Name";
        }

        /// <summary>
        ///     Тип связи
        /// </summary>
        public string LinkTypeID { get; set; }

        /// <summary>
        ///     Ролитель
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        ///     Потомок
        /// </summary>
        public string ChildID { get; set; }

        /// <summary>
        ///     Автоматическая установка значения, если оно является единственным
        /// </summary>
        public bool AutoSetFirstValue { get; set; }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOPersonSigner Filter => (DSOPersonSigner) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetPersonLinks();
        }

        /// <summary>
        ///     Получение списка связей лиц
        /// </summary>
        /// <returns>Список</returns>
        public List<Link> GetPersonLinks()
        {
            var dt = DBManager.GetData(SQLGetText(false), Config.DS_person, CommandType.Text, SQLGetInnerParams());

            var personLinks = dt.AsEnumerable().Select(dr => new Link
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name =
                    !string.IsNullOrEmpty(dr.Field<string>(Filter.NameField))
                        ? dr.Field<string>(Filter.NameField)
                        : "<Нет описания>"
            }).ToList();

            if (personLinks.Count == 1 && AutoSetFirstValue)
            {
                var firstOrDefault = personLinks.FirstOrDefault();
                if (firstOrDefault != null) Value = firstOrDefault.Id;
                IsDisabled = true;
            }
            else if (AutoSetFirstValue)
            {
                IsDisabled = false;
            }

            return personLinks;
        }

        /// <summary>
        ///     Получение связи лиц по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            return new Link(id);
        }
    }
}