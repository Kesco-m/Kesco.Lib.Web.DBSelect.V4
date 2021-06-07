using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Territories;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс select сущности Территория
    /// </summary>
    public class DBSTerritory : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSTerritory()
        {
            base.Filter = new DSOTerritory();

            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = Config.territory_search;
            URLShowEntity = Config.territory_form;
        }

        /// <summary>
        ///     Текущий язык интерфейса
        /// </summary>
        private string LangName { get; set; }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOTerritory Filter => (DSOTerritory) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetTerritories();
        }

        /// <summary>
        ///     Получение списка территорий
        /// </summary>
        /// <returns>Список</returns>
        public List<Territory> GetTerritories()
        {
            LangName = V4Page.CurrentUser.Language;
            var dt = DBManager.GetData(SQLGetText(false), Config.DS_user, CommandType.Text, SQLGetInnerParams());

            var territory = dt.AsEnumerable().Select(dr => new Territory
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = LangName == "ru" ? dr.Field<string>(Filter.NameField) : dr.Field<string>(Filter.CaptionField),
                Caption = dr.Field<string>(Filter.CaptionField),
                Abbreviation = dr.Field<string>(Filter.AbbreviationField),
                TelephoneCode = dr.Field<string>(Filter.TelephoneCodeField)
            }).ToList();
            territory.OrderBy(order => order.Name);

            return territory;
        }

        /// <summary>
        ///     Получение территории по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            LangName = V4Page.CurrentUser.Language;
            return new Territory(id, LangName);
        }
    }
}