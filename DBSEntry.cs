using System.Collections;
using System.Collections.Generic;
using System.Data;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс select сущности Автоподстановка.Записи
    /// </summary>
    public class DBSEntry : DBSelect
    {
        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DBSEntry()
        {
            base.Filter = new DSOEntry();
            KeyField = "Id";
            ValueField = "Name";
            URLAdvancedSearch = Config.person_search;
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOEntry Filter => (DSOEntry) base.Filter;

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetBusinessProject(search);
        }

        /// <summary>
        ///     Получение списка записей
        /// </summary>
        /// <returns>Список</returns>
        public List<Item> GetBusinessProject(string search)
        {
            var listOfItems = new List<Item>();

            var sqlQuery = @"
DECLARE @pattern varchar(100)
SET @pattern = Инвентаризация.dbo.fn_ReplaceRusLat(Инвентаризация.dbo.fn_ReplaceKeySymbols(@search)) + '%'
SET @pattern = COALESCE(@pattern, '')
SELECT КодЗаписи, Название FROM Записи
WHERE @pattern = '' OR Инвентаризация.dbo.fn_ReplaceRusLat(Инвентаризация.dbo.fn_ReplaceKeySymbols(Название)) LIKE @pattern
ORDER BY Название
";
            var sqlParams = new Dictionary<string, object>
            {
                {"@search", new object[] {search, DBManager.ParameterTypes.String}}
            };

            var dtItems = DBManager.GetData(sqlQuery, Config.DS_person, CommandType.Text, sqlParams);

            foreach (DataRow row in dtItems.Rows)
                listOfItems.Add(new Item {Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField]});

            return listOfItems;
        }

        /// <summary>
        ///     Получение записи по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Название</param>
        /// <returns>Запись</returns>
        public override object GetObjectById(string id, string name)
        {
            return new {Id = id, Name = name};
        }
    }
}