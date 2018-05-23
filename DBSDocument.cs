using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using Kesco.Lib.BaseExtention;
using Kesco.Lib.Entities.Documents;
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
            ValueField = "FullDocName";

            Index = 1;
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
        public new DSODocument Filter
        {
            get { return (DSODocument) base.Filter; }
        }

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
                return new Document {Id = id, Name = name};
            return new Document(id);
        }

        /// <summary>
        ///     Переопреденная функция обработки событий расширенного поиска и открытия формы сущности, если значение в контроле
        ///     заполнено
        /// </summary>
        /// <param name="collection">Коллекция параметров, переданных с клиента</param>
        public override void ProcessCommand(NameValueCollection collection)
        {
            if (collection == null) return;
            switch (collection["cmd"])
            {
                case "search":
                    JS.Write(DocViewInterop.SearchDocument(HttpContext.Current, HtmlID, Filter.Type.DocTypeParamsStr, Filter.PersonIDs.DocViewParamsStr, Filter.Name.Value));
                    break;

                case "OpenDocument":
                    JS.Write(DocViewInterop.OpenDocument(collection["id"], false));
                    break;
                default:
                    base.ProcessCommand(collection);
                    break;
            }
        }

    }
}