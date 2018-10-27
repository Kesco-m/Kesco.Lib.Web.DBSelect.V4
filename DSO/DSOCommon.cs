using System;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Аттрибут опций фильтрации, указывающий построителю какие свойства использовать для фильтрации
    /// </summary>
    public class FilterOption : Attribute
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="optionName">Название параметра фильтрации, так же как назвается в запросе</param>
        /// <param name="isInnerParam">
        ///     Использовать значение данного параметра фильтрации: true- в качестве параметра в запросе;
        ///     false- добавить строку условия, которая описана в классе опции
        /// </param>
        /// <param name="optionNameURL">Название опции фильтрации в строке запроса</param>
        /// <param name="alwaysEnable">Ипользовать, даже если не присвоено значение</param>
        public FilterOption(String optionName, bool isInnerParam = false, String optionNameURL = "", bool alwaysEnable = false)
        {
            OptionName = optionName;
            IsInnerParam = isInnerParam;
            OptionNameURL = optionNameURL;
            AlwaysEnable = alwaysEnable;
        }

        /// <summary>
        ///     Наименование опции
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        ///     Наименование опции в URL
        /// </summary>
        public string OptionNameURL { get; set; }

        /// <summary>
        ///     Наименование опции в URL
        /// </summary>
        public bool IsInnerParam { get; set; }

        /// <summary>
        ///     Ипользовать, даже если не присвоено значение
        /// </summary>
        public bool AlwaysEnable { get; set; }
    }


    /// <summary>
    ///     Базовый класс источника данных для Select
    /// </summary>
    public class DSOCommon
    {
        /// <summary>
        ///     Условие добавления в запрос исключений по ранее выбранным элементам сущности
        /// </summary>
        private bool _isAddExcludeCondition = true;

        /// <summary>
        ///     Имя ключевого поля таблицы
        /// </summary>
        public string KeyField { get; set; }

        /// <summary>
        ///     Имя поля таблицы с наименованием сущности
        /// </summary>
        public string NameField { get; set; }

        /// <summary>
        ///     Аксессор к полю _isAddExcludeCondition
        /// </summary>
        public bool IsAddExcludeCondition
        {
            get { return _isAddExcludeCondition; }
            set { _isAddExcludeCondition = value; }
        }

        /// <summary>
        ///     Предварительный SQL Пакет
        /// </summary>
        public virtual string SQLBatchPrepare
        {
            get { return ""; }
        }

        /// <summary>
        ///     SQL Пакет
        /// </summary>
        public virtual string SQLBatch
        {
            get { return ""; }
        }

        /// <summary>
        ///     Сортировка
        /// </summary>
        public virtual string SQLOrderBy
        {
            get { return ""; }
        }

        /// <summary>
        ///     Сущность по ID
        /// </summary>
        public virtual string SQLEntityById
        {
            get { return ""; }
        }

        /// <summary>
        ///     Функция, возвращающая латинскую транскрипцию
        /// </summary>
        /// <param name="s">строка на русском</param>
        /// <returns>латинская транскрипция</returns>
        public static string ReplaceRusLat(string s)
        {
            return String.Format("Инвентаризация.dbo.fn_ReplaceRusLat(N'{0}')", s);
        }
    }
}