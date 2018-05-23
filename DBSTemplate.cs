using System;
using System.Collections;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.DBSelect.V4.DSO;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс сущности шаблон печатной формы
    /// </summary>
    public class DBSTemplate : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSTemplate()
        {
            base.Filter = new DSOTemplate();
            KeyField = "CodeTemplate";
            ValueField = "NameTemplate";
            OnRenderNtf += TemplateNtf;
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
        public new DSOTemplate Filter
        {
            get { return (DSOTemplate) base.Filter; }
        }

        /// <summary>
        ///     Коллекция нотификаций контрола
        /// </summary>
        /// <param name="sender">Контрол</param>
        /// <param name="ntf">Нотификация</param>
        protected void TemplateNtf(object sender, Ntf ntf)
        {
            ntf.List.Clear();
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            throw new NotImplementedException("Не закоммичен класс Template");
            // return GetTemplates();
        }

        ///// <summary>
        ///// Список для выбора шаблон печатной формы
        ///// </summary>
        ///// <returns>Список шаблон печатной формы</returns>
        //public List<Template> GetTemplates()
        //{
        //    DataTable dt = DBManager.GetData(SQLGetText(), Config.DS_document, CommandType.Text, SQLGetInnerParams());

        //    List<Template> result = dt.AsEnumerable().Select(dr => new Template
        //    {
        //        CodePerson = dr.Field<int?>("КодЛица"),
        //        CodeContractor = dr.Field<int?>("КодКонтрагента"),
        //        CodeTemplate = dr.Field<int>("КодШаблонаПечатнойФормы"),
        //        NameTemplate = dr.Field<string>("НазваниеШаблона"),
        //        NameTemplateLat = dr.Field<string>("НазваниеШаблонаЛат")
        //    }).ToList();

        //    return result;
        //}

        /// <summary>
        ///     Получение шаблон печатной формы по ID
        /// </summary>
        /// <param name="id">ID шаблон печатной формы</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>шаблон печатной формы</returns>
        public override object GetObjectById(string id, string name = "")
        {
            throw new NotImplementedException("Не закоммичен класс Template");
            //if (!string.IsNullOrEmpty(name))
            //    return new Template { CodeTemplate = System.Convert.ToInt32(id), NameTemplate = name };
            //return new Template(id);
        }
    }
}