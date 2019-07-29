using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Entities.Persons;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.Controls.V4.Common;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления Типы лиц
    /// </summary>
    public class DBSPersonDossierThemes : DBSelect
    {
        /// <summary>
        ///     Конструктор контрола
        /// </summary>
        public DBSPersonDossierThemes()
        {
            base.Filter = new DSOPersonTheme();
            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = Config.person_types_search;
            IsMultiReturn = true;
            IsMultiSelect = false;
            PersonTypesList = new List<PersonType>();
            RenderFieldsContainer = "";
        }

        /// <summary>
        ///     Список типов лица
        /// </summary>
        public List<PersonType> PersonTypesList { get; set; }

        /// <summary>
        ///     ID дива, в котором будут отрисованы записи
        /// </summary>
        public string RenderFieldsContainer { get; set; }

        /// <summary>
        ///     Дополнительный фильтр по условиям поиска
        /// </summary>
        public new DSOPersonTheme Filter => (DSOPersonTheme) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetPersonsTypes();
        }

        /// <summary>
        ///     Получение списка тем лица
        /// </summary>
        /// <returns>Список</returns>
        public List<PersonTheme> GetPersonsTypes()
        {
            var personThemesIDs = string.Join(",", PersonTypesList.Select(t => t.ThemeID.Id.ToString()).Distinct());
            URLAdvancedSearch =
                string.Format(@"{0}{1}{2}", Config.person_types_search, "?selectedid=", personThemesIDs);

            var dt = DBManager.GetData(SQLGetText(true), Config.DS_person);

            var persons = dt.AsEnumerable().Select(dr => new PersonTheme
            {
                Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                Name = dr.Field<string>(Filter.NameField)
            }).ToList();

            return persons;
        }

        private void RenderFields()
        {
            if (string.IsNullOrEmpty(RenderFieldsContainer))
                return;


            JS.Write(@"$('#{0}').empty();", RenderFieldsContainer);
            var innerHtml = "";

            if (PersonTypesList.Any(t => t.TypeID != null))
                innerHtml =
                    string.Format(
                        @"<div class=contentBlock><div class=rusSpace><div class=themeLeftLineBlock>{0}</div><div class=lineBlock><div class=buttonBlock></div></div></div><div class=themeRightLineBlock><div class=typesList>{1}</div></div></div>",
                        "Типы:", "Каталог(и):");

            foreach (var type in PersonTypesList.Where(t => t.TypeID != null).Select(t => t.ThemeID.Id).Distinct())
                innerHtml +=
                    string.Format(
                        @"<div class=contentBlock><div class=rusSpace><div class=themeLeftLineBlock><img src=/Styles/delete.gif id=themeDeleteBtn{2} onclick=deletePersonTheme({2});></img><a onclick=editPersonTheme({2});>{0}</a></div><div class=lineBlock><div class=buttonBlock></div></div></div><div class=themeRightLineBlock><div class=typesList>{1}</div></div></div>",
                        PersonTypesList.Where(t => t.ThemeID.Id == type)
                            .Select(t => t.ThemeID.NameTheme)
                            .FirstOrDefault(),
                        string.Join(",", PersonTypesList.Where(t => t.ThemeID.Id == type).Select(t => t.Catalog)),
                        PersonTypesList.Where(t => t.ThemeID.Id == type).Select(t => t.ThemeID.Id).FirstOrDefault());
            JS.Write(@"$('#{1}').html('{0}');", innerHtml, RenderFieldsContainer);
            JS.Write(
                "$('.typesList').width(300);  $('#personThemeContaincerDiv > .contentBlock').css({'padding-bottom':'5px'});");
        }

        /// <summary>
        ///     Получение темы лица по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Тема лица</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(id))
            {
                var url = "";
                if (id.IndexOf('[') != -1 || id.IndexOf('[') != -1)
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    var items = jsonSerializer.Deserialize<List<JSONModel>>(id);

                    id = string.Join(",", items.Select(t => t.value));
                    url = string.Format(@"{0}?themeId={1}&typesids={2}&type=multiply", Config.v4person_themes, id,
                        string.Join(",", PersonTypesList.Select(t => t.TypeID).Distinct()));
                }
                else
                {
                    url = string.Format(@"{0}?themeId={1}&typesids={2}&type=single", Config.v4person_themes, id,
                        string.Join(",", PersonTypesList.Select(t => t.TypeID).Distinct()));
                }

                var personTypesDataTable = DBManager.GetData(SQLQueries.SELECT_ТипыЛиц_Темы, Config.DS_person,
                    CommandType.Text, new Dictionary<string, object> {{"@id", id}});
                if (personTypesDataTable != null && personTypesDataTable.Rows != null &&
                    personTypesDataTable.Rows.Count > 1)
                {
                    var tempListPersonTypes =
                        (from DataRow type in personTypesDataTable.Rows select new PersonType(type)).ToList();
                    if (tempListPersonTypes.Select(t => t.ThemeID.Id).Distinct().Count() != tempListPersonTypes.Count())
                    {
                        OpenPopup(url);
                    }
                    else
                    {
                        PersonTypesList = tempListPersonTypes;
                        RenderFields();
                    }

                    return null;
                }

                if (personTypesDataTable != null && personTypesDataTable.Rows != null &&
                    !PersonTypesList.Any(t => t.ThemeID.Id == personTypesDataTable.Rows[0]["КодТемыЛица"].ToString()))
                    PersonTypesList.Add(new PersonType(personTypesDataTable.Rows[0]));
            }

            RenderFields();


            return null;
            //if (!string.IsNullOrEmpty(name))
            //    return new PersonTheme() { Id = id, Name = name };
            //return new PersonTheme(id);
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        protected void OpenPopup(string url)
        {
            var callbackUrl = HttpContext.Current.Request.Url.Scheme + "://" +
                              HttpContext.Current.Request.Url.Authority +
                              HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/dialogResult.ashx";

            if (url.IndexOf('?') == -1) url += "?";
            else url += "&";

            url += string.Format("return={0}&mvc=4&clid={1}&control={2}&callbackKey={3}&callbackUrl={4}",
                IsMultiReturn ? 2 : 1,
                CLID,
                HttpUtility.UrlEncode(HtmlID),
                HttpUtility.UrlEncode(((Page) Page).IDPage),
                HttpUtility.UrlEncode(callbackUrl)
            );

            JS.Write("v4_isStopBlur = false;");
            JS.Write("$.v4_windowManager.selectEntity('{0}', '{1}', '{2}', {3}, {4}, v4s_setSelectedValue, '{5}');",
                HttpUtility.JavaScriptStringEncode(url),
                HttpUtility.JavaScriptStringEncode(HtmlID),
                HttpUtility.JavaScriptStringEncode(((Page) Page).IDPage),
                AdvSearchWindowWidth,
                AdvSearchWindowHeight,
                IsMultiReturn);
        }

        private void SetPersonTypes(string personTypes, string themeID, string types)
        {
            var newList = new List<PersonType>();

            newList = PersonTypesList.Where(t => t.ThemeID.Id != themeID).ToList();

            foreach (var type in personTypes.Split(',')) newList.Add(new PersonType(type));
            PersonTypesList = newList;
            RenderFields();
        }

        /// <summary>
        /// </summary>
        /// <param name="collection"></param>
        public override void ProcessCommand(NameValueCollection collection)
        {
            switch (collection["cmd"])
            {
                case "search":
                    OpenPopup(URLAdvancedSearch);
                    return;
                case "clearSelectedItems":
                    //PersonTypesList = new List<PersonType>();
                    return;
                case "setPersonTypes":
                    SetPersonTypes(collection["ptypes"], collection["ptheme"], collection["type"]);
                    break;
                case "deletePersonTheme":
                    if (!string.IsNullOrEmpty(collection["themeid"]))
                    {
                        PersonTypesList.RemoveAll(t => t.ThemeID.Id == collection["themeid"]);
                        RenderFields();
                    }

                    break;
                case "editPersonTheme":
                    if (!string.IsNullOrEmpty(collection["themeid"])) GetObjectById(collection["themeid"]);
                    break;
            }

            base.ProcessCommand(collection);

            if (IsRequired && PersonTypesList.Count == 0)
            {
                JS.Write("gi('{0}_0').setAttribute('isRequired','{1}');", HtmlID, 1);
                JS.Write("v4_replaceStyleRequired(gi('{0}_0'));", HtmlID);
            }
            else if (IsRequired && PersonTypesList.Count > 0)
            {
                JS.Write("gi('{0}_0').setAttribute('isRequired','{1}');", HtmlID, 0);
                JS.Write("v4_replaceStyleRequired(gi('{0}_0'));", HtmlID);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="w"></param>
        protected override void RenderControlBody(TextWriter w)
        {
            w.Write(
                @"<script> function deletePersonTheme(themeid){{ cmd('ctrl', '{0}', 'cmd', 'deletePersonTheme', 'themeid', themeid); }};
                                             function editPersonTheme(themeid){{ cmd('ctrl', '{0}', 'cmd', 'editPersonTheme', 'themeid', themeid); }}; </script>",
                HtmlID);
            base.RenderControlBody(w);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        public override void OnChanged(ProperyChangedEventArgs e)
        {
            IsMultiSelect = false;
            base.OnChanged(e);
            Value = "";
            //if (!string.IsNullOrEmpty(e.NewValue) && !SelectedItems.Exists(x => x.Id == Value))
            //{
            //    object item = ValueObject;
            //    Type itemType = item.GetType();
            //    string id = itemType.GetProperty(KeyField).GetValue(item, null).ToString();
            //    SelectedItems.Add(new Entities.Item { Id = id, Value = item });
            //    V4Page.RefreshHtmlBlock(string.Concat(HtmlID, "Data"), RenderSelectedItems);
            //    base.OnChanged(e);
            //}
            //Focus();
        }

        private class JSONModel
        {
            public string value { get; set; }
            public string label { get; set; }
        }
    }
}