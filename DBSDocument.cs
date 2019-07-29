using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using Kesco.Lib.BaseExtention;
using Kesco.Lib.BaseExtention.Enums.Controls;
using Kesco.Lib.Entities.Corporate;
using Kesco.Lib.Entities.Documents;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.DBSelect.V4.DSO;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления "Документ"
    /// </summary>
    public class DBSDocument : DBSelect
    {
        /// <summary>
        ///     Инициализация объекта
        /// </summary>
        public DBSDocument()
        {
            base.Filter = new DSODocument();
            KeyField = "Id";
            ValueField = "FullDocumentName";
            MethodsGetEntityValue = new List<SelectMethodGetEntityValue>
            {
                new SelectMethodGetEntityValue
                {
                    ValueField = "FullDocumentName",
                    MethodName = "GetFullDocumentName",
                    MethodParams = new object[] {new Employee(false) {IsLazyLoadingByCurrentUser = true}}
                }
            };

            FuncShowEntity = "v4_docView_Show(\"{0}\",\"{1}\");";
            URLAdvancedSearch = "0";
            IsAlwaysAdvancedSearch = true;
            IsNotUseSelectTop = false;
        }

        /// <summary>
        ///     Продавец
        /// </summary>
        public int? Seller { get; set; }

        /// <summary>
        ///     Покупатель
        /// </summary>
        public int? Buyer { get; set; }

        /// <summary>
        ///     Дополнительный фильтр по условиям поиска
        /// </summary>
        public new DSODocument Filter => (DSODocument) base.Filter;

        /// <summary>
        ///     Иконка документа
        /// </summary>
        public bool IsAdvIcons { get; set; }

        /// <summary>
        /// </summary>
        public List<string> AdvIcons => GetAdvIcons();

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetDocuments();
        }

        /// <summary>
        ///     Получение списка документов
        /// </summary>
        /// <returns>Список</returns>
        public List<Document> GetDocuments()
        {
            var query = SQLGetText();
            return Document.GetDocumentsList(query);
        }

        /// <summary>
        ///     Получение документа по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>Склад</returns>
        public override object GetObjectById(string id, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return new Document {Id = id, DocumentName = name};
            return new Document(id, false);
        }

        /// <summary>
        ///     Переопреденная функция обработки событий расширенного поиска и открытия формы сущности, если значение в контроле
        ///     заполнено
        /// </summary>
        /// <param name="collection">Коллекция параметров, переданных с клиента</param>
        public override void ProcessCommand(NameValueCollection collection)
        {
            if (collection == null) return;

            if (V4Page.IsKescoRun)
            {
                switch (collection["cmd"])
                {
                    case "search":
                        JS.Write(DocViewInterop.SearchDocument(HttpContext.Current, HtmlID,
                            Filter.Type.DocTypeParamsStr,
                            Filter.PersonIDs.DocViewParamsStr, Filter.Name.Value));
                        break;

                    case "OpenDocument":
                        JS.Write(DocViewInterop.OpenDocument(collection["id"], false));
                        break;
                    default:
                        base.ProcessCommand(collection);
                        break;
                }

                return;
            }

            //TODO: УДАЛИТЬ ПОСЛЕ ВНЕДРЕНИЯ KESCORUN
            switch (collection["cmd"])
            {
                case "search":
                    SearchDoc(string.Format("types={0}&persons={1}&search={2}",
                        Filter.Type.DocTypeParamsStr,
                        Filter.PersonIDs.DocViewParamsStr,
                        Regex.Replace(Filter.Name.Value, @"[^\\?]{0,}[?]", string.Empty)));
                    break;

                case "OpenDocument":
                    OpenDoc(collection["id"], false);
                    break;
                default:
                    base.ProcessCommand(collection);
                    break;
            }
        }

        //TODO: УДАЛИТЬ ПОСЛЕ ВНЕДРЕНИЯ KESCORUN

        /// <summary>
        ///     Открытие документа в Архиве
        /// </summary>
        /// <param name="id"></param>
        /// <param name="openImage"></param>
        private void OpenDoc(string id, bool openImage = true)
        {
            JS.Write("srv4js('OPENDOC','id={0}&newwindow=1{1}',", id, openImage ? "&imageid=-2" : "&imageid=0");
            JS.Write("function(rez,obj){if(rez.error){");
            JS.Write("v4_showMessage(rez.errorMsg, '{0}',{1});", Resx.GetString("alertWarning"),
                (int) MessageStatus.Warning);
            JS.Write("}},null);");
        }

        /// <summary>
        ///     Поиск документов Архиве по строке параметров
        /// </summary>
        /// <param name="searchParams">Строка параметров</param>
        private void SearchDoc(string searchParams)
        {
            JS.Write("srv4js('SEARCH','{0}',", searchParams);
            JS.Write(@"function(rez,obj)
                        {{
	                        if(!rez.error)
                            {{
		                        switch(rez.value)
		                        {{
			                        case '-1': v4_showMessage('Ошибка взаимодействия с архивом документов!', '{1}',{2}); break;
			                        case '0': break;
			                        default:
                                        v4_setValue('{0}', rez.value, ''); 				                        
				                        break;
		                        }}
                            }}
	                        else
                            {{
		                        v4_showMessage('Ошибка взаимодействия с архивом документов:<br>'+rez.errorMsg, '{1}',{2});
                            }}
                        }}", ID, Resx.GetString("alertError"), (int) MessageStatus.Error);
            JS.Write(", null);");
        }

        /// <summary>
        ///     Получение иконок для документа
        /// </summary>
        /// <returns></returns>
        protected override List<string> GetAdvIcons()
        {
            if (IsAdvIcons && !string.IsNullOrEmpty(Value))
            {
                var advIconsCollection = new NameValueCollection();
                var listIcons = new List<string>();
                Document.FillAdvIcons(Value, advIconsCollection);
                foreach (var key in advIconsCollection.AllKeys)
                    listIcons.Add(string.Format("<img src=\"/styles/{0}\" title=\"{1}\" border=\"0\"/>", key,
                        advIconsCollection.Get(key)));

                return listIcons;
            }

            return null;
        }
    }
}