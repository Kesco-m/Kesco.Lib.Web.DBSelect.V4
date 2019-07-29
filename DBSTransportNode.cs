using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities;
using Kesco.Lib.Entities.Transport;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс сущности транспортный узел
    /// </summary>
    public class DBSTransportNode : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSTransportNode()
        {
            base.Filter = new DSOTransportNode();
            KeyField = "Id";
            ValueField = "Id,Name,TypeTransport";
            DisplayFields = "Id,Name,TypeTransport";
            AnvancedHeaderPopupResult =
                string.Format(
                    "<tr class='gridHeaderSelect v4s_noselect'><td><b>{0}</b></td><td><b>{1}</b></td><td><b>{2}</b></td></tr>",
                    Resx.GetString("sCode"), Resx.GetString("STORE_Name"), Resx.GetString("sTransport"));

            IndexField = 1;
            OnRenderNtf += TransportNodeNtf;
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOTransportNode Filter => (DSOTransportNode) base.Filter;

        /// <summary>
        ///     Коллекция нотификаций контрола
        /// </summary>
        /// <param name="sender">Контрол</param>
        /// <param name="ntf">Нотификация</param>
        protected void TransportNodeNtf(object sender, Ntf ntf)
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
            return GetTransportNodes(search, Filter.TransportType, MaxItemsInQuery);
        }

        /// <summary>
        ///     Список для выбора транспортных узлов
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <param name="type">Код Вида Транспорта</param>
        /// <param name="maxItemsInQuery">Количество возвращаемых записей в запросе (top n)</param>
        /// <returns>Список транспортных узлов</returns>
        public List<TransportNode> GetTransportNodes(string search, string type, int maxItemsInQuery)
        {
            var dt = DBManager.GetData(SQLQueries.SELECT_ТранспортныеУзлыВыбор(search, type, maxItemsInQuery),
                Config.DS_document);
            var result = dt.AsEnumerable().Select(dr => new TransportNode
            {
                Id = dr.Field<int>("КодТранспортногоУзла").ToString(),
                Name = dr.Field<string>("Название"),
                TypeTransport = dr.Field<string>("Транспорт"),
                CodeRailway = dr.Field<int?>("КодЖелезнойДороги"),
                TransportTypeCode = dr.Field<int>("КодВидаТранспорта")
            }).ToList();

            return result;
        }

        /// <summary>
        ///     Получение транспортного узла по ID
        /// </summary>
        /// <param name="id">ID транспортного узла</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>транспортный узел</returns>
        public override object GetObjectById(string id, string name = "")
        {
            var sql = string.Format(SQLQueries.SELECT_ID_ТранспортныйУзел, id);
            var dt = DBManager.GetData(sql, Config.DS_document);
            var result = dt.AsEnumerable().Select(dr => new TransportNode
            {
                Unavailable = false,
                Id = dr.Field<int>("КодТранспортногоУзла").ToString(),
                Name = dr.Field<int>("КодВидаТранспорта") == 1
                    ? "ст. " + dr.Field<string>("Название") + " (" + dr.Field<int>("КодТранспортногоУзла").ToString() +
                      ")"
                    : dr.Field<string>("Название"),
                TypeTransport = dr.Field<string>("Транспорт"),
                CodeRailway = dr.Field<int?>("КодЖелезнойДороги"),
                TransportTypeCode = dr.Field<int>("КодВидаТранспорта")
            }).FirstOrDefault();

            return result;
        }
    }
}