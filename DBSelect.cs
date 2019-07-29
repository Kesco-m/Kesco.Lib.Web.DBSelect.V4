using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.Controls.V4.Common;
using Kesco.Lib.Web.DBSelect.V4.DSO;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Базовый класс для источника данных контрола Select
    /// </summary>
    public abstract class DBSelect : Select
    {
        /// <summary>
        ///     Высота открываемого окна расширенного поиска
        /// </summary>
        public int AdvSearchWindowHeight = 600;

        /// <summary>
        ///     Ширина открываемого окна расширенного поиска
        /// </summary>
        public int AdvSearchWindowWidth = 800;

        /// <summary>
        ///     Объект данных сущности
        /// </summary>
        protected DSOCommon Filter;

        /// <summary>
        ///     Конструктор
        /// </summary>
        protected DBSelect()
        {
            Filter = new DSOCommon();
            FillPopupWindow = FillSelect;
            SelectedItemById = GetObjectById;
        }

        /// <summary>
        ///     Переопреденная функция обработки событий расширенного поиска и открытия формы сущности, если значение в контроле
        ///     заполнено
        /// </summary>
        /// <param name="collection">Коллекция параметров, переданных с клиента</param>
        public override void ProcessCommand(NameValueCollection collection)
        {
            base.ProcessCommand(collection);
            if (collection == null) return;
            switch (collection["cmd"])
            {
                case "search":
                    EvalURLClick(URLAdvancedSearch);
                    break;
                case "create":
                    var idUrl = collection["idUrl"];
                    GetURLCreateEntity(idUrl);
                    break;
                case "btn":
                    if (Value.Length > 0 && SearchText.Length == 0 && URLShowEntity.Length > 0)
                        OpenEntityForm();
                    break;
            }
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public virtual IEnumerable FillSelect(string search)
        {
            OnBeforeSearch();
            var type = Filter.GetType();
            var field = type.GetField("Name");
            if (field == null) return null;

            var typeF = field.FieldType;
            var valF = GetFieldValue(Filter, field);

            var prop = typeF.GetProperty("Value");
            if (prop == null) return null;
            prop.SetValue(valF, search, null);

            return null;
        }

        /// <summary>
        ///     Получение сущности по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Сущность</returns>
        public virtual object GetObjectById(string id, string name = "")
        {
            return null;
        }

        /// <summary>
        ///     Получение сгенеренного текста запроса, будет добавлено WHERE
        /// </summary>
        /// <returns>Текст запроса</returns>
        public virtual string SQLGetText()
        {
            return SQLGetText(false);
        }

        /// <summary>
        ///     Получение сгенеренного текста запроса
        /// </summary>
        /// <param name="existWhere">Существует ли в шаблоне запроса выражение WHERE</param>
        /// <returns></returns>
        public virtual string SQLGetText(bool existWhere)
        {
            var sb = new StringBuilder();
            var colWhere = new StringCollection();
            string value;
            var type = Filter.GetType();

            #region SQLBatchPrepare

            var prop = type.GetProperty("SQLBatchPrepare");
            if (prop != null)
            {
                value = GetPropertyValue(Filter, prop).ToString();
                if (value.Length > 0) sb.Append(value);
            }

            #endregion

            #region SQL_Batch

            prop = type.GetProperty("SQLBatch");
            if (prop != null)
            {
                value = GetPropertyValue(Filter, prop).ToString();
                value = string.Format(value, IsNotUseSelectTop ? "" : " TOP " + MaxItemsInQuery);
                if (value.Length > 0)
                {
                    if (sb.Length > 0) sb.Append("\n\n");
                    sb.Append(value);
                }
            }

            #endregion

            #region SQLClause

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var attrs = field.GetCustomAttributes(true);
                if (attrs.Length == 0) continue;
                var fOpt = attrs.OfType<FilterOption>().Any();

                if (!fOpt) continue;

                var typeF = field.FieldType;
                var methodF = typeF.GetMethod("SQLGetClause");
                if (methodF == null) continue;

                var valF = GetFieldValue(Filter, field);
                if (valF == null) continue;

                value = methodF.Invoke(valF, null).ToString();
                if (value.Length > 0)
                    colWhere.Add(value);
            }

            var keyProp = type.GetProperty("KeyField");
            var isAddExcludedProp = type.GetProperty("IsAddExcludeCondition");

            var isAddExcludedToQuery = Convert.ToBoolean(GetPropertyValue(Filter, isAddExcludedProp));
            if (keyProp != null)
            {
                var keyPropValue = GetPropertyValue(Filter, keyProp);

                var keyFieldName = keyPropValue == null ? string.Empty : keyPropValue.ToString();

                isAddExcludedToQuery = isAddExcludedToQuery && !string.IsNullOrEmpty(SelectedItemsString) &&
                                       !string.IsNullOrEmpty(keyFieldName);

                if (isAddExcludedToQuery)
                {
                    sb.Append(existWhere ? "\n AND " : "\n WHERE ");
                    sb.Append(string.Format("{0} NOT IN ({1})", keyFieldName, SelectedItemsString));
                }
            }

            for (var i = 0; i < colWhere.Count; i++)
            {
                if (i == 0 && !existWhere && !isAddExcludedToQuery) sb.Append("\n WHERE ");
                if (i > 0 || existWhere || isAddExcludedToQuery) sb.Append("\n AND ");
                sb.Append("\n");
                sb.Append(colWhere[i]);
            }

            #endregion

            #region SQLOrderBy

            prop = type.GetProperty("SQLOrderBy");
            if (prop != null)
            {
                value = GetPropertyValue(Filter, prop).ToString();
                if (value.Length > 0)
                {
                    sb.Append("\n");
                    sb.Append("ORDER BY " + value);
                }
            }

            #endregion

            return sb.ToString();
        }

        /// <summary>
        ///     Получение всех объектов, помеченных как FilterInnerParamsOption для формирования словаря с параметрами запроса
        /// </summary>
        /// <returns>Словарь с параметрами запроса</returns>
        public virtual Dictionary<string, object> SQLGetInnerParams()
        {
            var sqlParams = new Dictionary<string, object>();
            var type = Filter.GetType();
            object value;
            var key = "";

            #region Опрос свойств объекта

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                key = "";
                var attrs = property.GetCustomAttributes(true);
                if (attrs.Length == 0) continue;
                var fOpt = attrs.OfType<FilterOption>().Any();
                if (!fOpt) continue;

                foreach (var attr in attrs)
                    if (attr.GetType() == typeof(FilterOption))
                    {
                        var innerParams = attr as FilterOption;
                        if (innerParams == null) continue;
                        if (!innerParams.IsInnerParam) continue;
                        key = innerParams.OptionName;
                        break;
                    }

                if (key.Length > 0)
                {
                    value = GetPropertyValue(Filter, property);
                    sqlParams.Add(key, value);
                }
            }

            #endregion

            #region Опрос полей объекта

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                key = "";
                var attrs = field.GetCustomAttributes(true);
                if (attrs.Length == 0) continue;
                var fOpt = attrs.OfType<FilterOption>().Any();

                if (!fOpt) continue;

                foreach (var attr in attrs)
                    if (attr.GetType() == typeof(FilterOption))
                    {
                        var innerParams = attr as FilterOption;
                        if (innerParams == null) continue;
                        if (!innerParams.IsInnerParam) continue;
                        key = innerParams.OptionNameURL;
                        break;
                    }

                if (key.Length > 0)
                {
                    var typeF = field.FieldType;
                    var valF = GetFieldValue(Filter, field);

                    var prop = typeF.GetProperty("Value");
                    if (prop == null) continue;
                    value = GetPropertyValue(valF, prop);
                    sqlParams.Add(key, value);
                }
            }

            #endregion

            return sqlParams;
        }

        /// <summary>
        ///     Получение сущности по ID
        /// </summary>
        /// <returns>Сущность</returns>
        public virtual string SQLGetEntityById()
        {
            var sb = new StringBuilder();
            var type = Filter.GetType();
            var prop = type.GetProperty("SQLEntityById");
            if (prop != null)
            {
                var value = GetPropertyValue(Filter, prop).ToString();
                if (value.Length > 0) sb.Append(value);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Открытие нового окна с выбранной сущностью
        /// </summary>
        public void OpenEntityForm()
        {
            if (URLShowEntity.Length == 0) return;
            JS.Write("v4_windowOpen('{0}?id={1}');", URLShowEntity, HttpUtility.JavaScriptStringEncode(Value));
        }

        /// <summary>
        ///     Открывает форму поиска сущности и передает параметры фильтрации
        /// </summary>
        public void EvalURLClick(string url)
        {
            var isKescoRun = url.Contains("kescorun");
            var parameters = GetURLParams(isKescoRun);

            ReturnDialogResult.ShowAdvancedDialogSearch((Page) Page, "v4s_setSelectedValue", HtmlID, url, parameters,
                IsMultiReturn, CLID, AdvSearchWindowWidth, AdvSearchWindowHeight);
        }

        /// <summary>
        ///     Получить значения и названия параметров для формирования URL из свойств фильтра
        /// </summary>
        /// <param name="urlParams">Словарь с параметрами</param>
        /// <param name="type">Системный тип фильтра</param>
        private void GetURLParamsFromProperties(Dictionary<string, string> urlParams, Type type)
        {
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var svalue = "";
                var key = "";
                var alwaysEnable = false;
                var attrs = property.GetCustomAttributes(true);
                if (attrs.Length == 0) continue;
                var fOpt = attrs.OfType<FilterOption>().Any();

                if (!fOpt) continue;
                foreach (var attr in attrs)
                    if (attr.GetType() == typeof(FilterOption))
                    {
                        var fopt = attr as FilterOption;
                        if (fopt == null) continue;
                        alwaysEnable = fopt.AlwaysEnable;
                        key = fopt.OptionNameURL;
                        break;
                    }

                if (key.Length > 0)
                {
                    var value = GetPropertyValue(Filter, property);

                    if (value != null) svalue = value.ToString();
                    if (svalue.Length == 0 && !alwaysEnable) continue;
                    // svalue = HttpUtility.UrlEncode(svalue);
                    if (!urlParams.Keys.Contains(key)) urlParams.Add(key, svalue);
                }
            }
        }

        /// <summary>
        ///     Получить значения и названия параметров для формирования URL из полей фильтра
        /// </summary>
        /// <param name="urlParams">Словарь с параметрами</param>
        /// <param name="type">Системный тип фильтра</param>
        private void GetURLParamsFromFields(Dictionary<string, string> urlParams, Type type)
        {
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var key = "";
                var svalue = "";
                var alwaysEnable = false;
                var attrs = field.GetCustomAttributes(true);
                if (attrs.Length == 0) continue;
                var fOpt = attrs.OfType<FilterOption>().Any();

                if (!fOpt) continue;

                foreach (var attr in attrs)
                    if (attr.GetType() == typeof(FilterOption))
                    {
                        var fopt = attr as FilterOption;
                        if (fopt == null) continue;
                        alwaysEnable = fopt.AlwaysEnable;
                        key = fopt.OptionNameURL;
                        break;
                    }

                if (key.Length > 0)
                {
                    var typeF = field.FieldType;
                    var valF = GetFieldValue(Filter, field);

                    var prop = typeF.GetProperty("Value");
                    if (prop == null) continue;

                    var value = GetPropertyValue(valF, prop);
                    if (value != null) svalue = value.ToString();
                    if (svalue.Length == 0 && !alwaysEnable) continue;

                    //svalue = HttpUtility.UrlEncode(svalue).Replace("+", "%20");
                    if (!urlParams.Keys.Contains(key)) urlParams.Add(key, svalue);
                }
            }
        }

        /// <summary>
        ///     Формирует строку из параметров фильтрации
        /// </summary>
        /// <returns>Возвращает строку с параметрами и их значенями</returns>
        private string GetURLParams(bool isKescoRun)
        {
            var sb = new StringBuilder();
            var urlParams = new Dictionary<string, string>();
            var type = Filter.GetType();

            GetURLParamsFromProperties(urlParams, type);
            GetURLParamsFromFields(urlParams, type);

            foreach (var key in urlParams.Keys)
            {
                if (sb.Length > 0) sb.Append("&");
                sb.AppendFormat("{0}={1}", key,
                    isKescoRun
                        ? HttpUtility.UrlEncodeUnicode(urlParams[key])
                        : HttpUtility.UrlEncode(urlParams[key]).Replace("+", "%20"));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Получает ссылку на форму создания сущности
        /// </summary>
        /// <param name="idUrl">Идентификатор ссылки на создание сущности</param>
        private void GetURLCreateEntity(string idUrl)
        {
            var gIdUrl = new Guid(idUrl);
            var uce = URIsCreateEntity.FirstOrDefault(x => x.Id == gIdUrl);
            if (uce == null) return;
            EvalURLClick(uce.URL);
        }

        private object GetPropertyValue(object targetClass, PropertyInfo property)
        {
            object value;
            var propertyType = property.PropertyType;
            var isNullable = property.PropertyType.IsGenericType &&
                             property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (!isNullable)
            {
                value = property.GetValue(targetClass, null);
            }
            else
            {
                value = property.GetValue(targetClass, BindingFlags.GetProperty, null, null,
                    CultureInfo.CurrentUICulture);
                propertyType = property.PropertyType.GetGenericArguments()[0];
            }

            var typeCode = Type.GetTypeCode(propertyType);
            switch (typeCode)
            {
                case TypeCode.DateTime:
                    if (value != null) value = Convert.ToDateTime(value).ToString("yyyyMMdd");
                    break;
                case TypeCode.Boolean:
                    if (value != null) value = Convert.ToInt32(value);
                    break;
            }

            return value;
        }

        private object GetFieldValue(object targetClass, FieldInfo field)
        {
            var fieldType = field.FieldType;
            var value = field.GetValue(targetClass);
            var typeCode = Type.GetTypeCode(fieldType);
            switch (typeCode)
            {
                case TypeCode.DateTime:
                    if (value != null) value = Convert.ToDateTime(value).ToString("yyyyMMdd");
                    break;
                case TypeCode.Boolean:
                    if (value != null) value = Convert.ToInt32(value);
                    break;
            }

            return value;
        }

        /// <summary>
        ///     Получение списка иконок
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> GetAdvIcons()
        {
            return null;
        }

        /// <summary>
        ///     Отрисовка контрола
        /// </summary>
        /// <param name="w"></param>
        protected override void RenderControlBody(TextWriter w)
        {
            base.RenderControlBody(w);
            if (!(this is DBSDocument)) return;
            SetAdvIcons();
        }

        /// <summary>
        ///     Отправка клиенту скрипта с изменениями контрола
        /// </summary>
        public override void Flush()
        {
            base.Flush();

            if (!(this is DBSDocument)) return;

            if (PropertyChanged.Contains("ValueText") || PropertyChanged.Contains("Value") || RefreshRequired)
                SetAdvIcons();
        }

        /// <summary>
        ///     Отрисовка иконок
        /// </summary>
        private void SetAdvIcons()
        {
            var advIcons = GetAdvIcons();
            if (advIcons != null && advIcons.Count > 0)
                V4Page.JS.Write(
                    "setTimeout(function(){{$('#v3il_{0}').html('{1}'); var wthI = $('#v3il_{0}').width(); var wthT = $('#{0}_0').width(); $('#{0}_0').width(wthT-wthI);}},10);",
                    HtmlID, HttpUtility.JavaScriptStringEncode(advIcons[0]));
            else
                V4Page.JS.Write("$('#v3il_{0}').html('{1}'); $('#{0}_0').width({2});", HtmlID, "", Width.Value);
        }
    }
}