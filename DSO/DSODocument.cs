using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Document;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для контрола поиска документов
    /// </summary>
    public class DSODocument : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра: документы в работе
        /// </summary>
        [FilterOption("AtWork")] public FOptAtWork AtWork;

        /// <summary>
        ///     Опция фильтра: изменен
        /// </summary>
        [FilterOption("ChangeBy")] public FOptChangeBy ChangeBy;

        /// <summary>
        ///     Опция фильтра: дата документа
        /// </summary>
        [FilterOption("Date")] public FOptDate Date;

        /// <summary>
        ///     Опция фильтра: описание документа
        /// </summary>
        [FilterOption("Description")] public FOptDescription Description;

        /// <summary>
        ///     Опция фильтра: ID документа
        /// </summary>
        [FilterOption("IDs")] public FOptIDs IDs;

        /// <summary>
        ///     Опция фильтра: связанные документы
        /// </summary>
        [FilterOption("LinkedDoc")] public FOptLinkedDoc LinkedDoc;

        /// <summary>
        ///     Опция фильтра: по названию документа
        /// </summary>
        [FilterOption("Name")] public FOptName Name;

        /// <summary>
        ///     Опция фильтра: по типу вытекающего документа
        /// </summary>
        [FilterOption("NextType")] public FOptNextDocType NextType;

        /// <summary>
        ///     Опция фильтра: номер документа
        /// </summary>
        [FilterOption("Number")] public FOptDocNumber Number;

        /// <summary>
        ///     Опция фильтра: ID лиц
        /// </summary>
        [FilterOption("PersonIDs")] public FOptPersonIDs PersonIDs;

        /// <summary>
        ///     Опция фильтра: тип документа
        /// </summary>
        [FilterOption("Type")] public FOptDocType Type;

        /// <summary>
        ///     Опция фильтра: с эл.формой
        /// </summary>
        [FilterOption("HasForm")] public FOptHasForm HasForm;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSODocument()
        {
            KeyField = "КодДокумента";
            NameField = "НазваниеДокумента";

            IDs = new FOptIDs();
            Name = new FOptName();
            PersonIDs = new FOptPersonIDs();
            Type = new FOptDocType();
            Number = new FOptDocNumber();
            Date = new FOptDate();
            Description = new FOptDescription();
            LinkedDoc = new FOptLinkedDoc();
            AtWork = new FOptAtWork();
            ChangeBy = new FOptChangeBy();
            NextType = new FOptNextDocType();
            HasForm = new FOptHasForm();
        }

        /// <summary>
        ///     Контракт
        /// </summary>
        [FilterOption("Contract")]
        public int? Contract { get; set; }

        /// <summary>
        ///     Продавец
        /// </summary>
        [FilterOption("Seller")]
        public int? Seller { get; set; }

        /// <summary>
        ///     Покупатель
        /// </summary>
        [FilterOption("Buyer")]
        public int? Buyer { get; set; }

        /// <summary>
        ///     Параметры поиска
        /// </summary>
        public string ParamSearch { get; set; }

        /// <summary>
        ///     Запрос выборки документов
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_Документы;

        /// <summary>
        ///     Запрос получения документа по ID
        /// </summary>
        public override string SQLEntityById => SQLQueries.SELECT_ID_Документ;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => "T0.КодДокумента DESC";
    }
}