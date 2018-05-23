using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Kesco.Lib.DALC;
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
            KeyField = "CodeHub";
            ValueField = "CodeHub,Name,TypeTransport";
            AnvancedHeaderPopupResult =
                string.Format("<tr class='v4s_noselect'><td><b>{0}</b></td><td><b>{1}</b></td><td><b>{2}</b></td></tr>",
                    Resx.GetString("sCode"), Resx.GetString("STORE_Name"), Resx.GetString("sTransport"));

            Index = 1;
            OnRenderNtf += TransportNodeNtf;
        }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOTransportNode Filter
        {
            get { return (DSOTransportNode) base.Filter; }
        }

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
        public List<Basis> GetTransportNodes(string search, string type, int maxItemsInQuery)
        {
            var sql = String.Format(@"
SELECT TOP {0} T0.КодТранспортногоУзла, T0.Название+ISNULL('['+ЖД.ЖелезнаяДорога +']','') Название, ВТ.ВидТранспорта Транспорт, T0.КодЖелезнойДороги,T0.КодВидаТранспорта		
FROM [Справочники].[dbo].ТранспортныеУзлы T0
LEFT OUTER JOIN [Справочники].[dbo].ЖелезныеДороги ЖД ON T0.КодЖелезнойДороги=ЖД.КодЖелезнойДороги
INNER JOIN [Справочники].[dbo].ВидыТранспорта ВТ ON  T0.КодВидаТранспорта=ВТ.КодВидаТранспорта	
WHERE (1=1) {1} {2} ORDER BY T0.Название,T0.КодВидаТранспорта", maxItemsInQuery,
                !String.IsNullOrEmpty(type) ? @" and T0.КодВидаТранспорта = " + type : "",
                !String.IsNullOrEmpty(search)
                    ? @" and (' ' + (CASE WHEN T0.КодВидаТранспорта = 1 THEN RIGHT((T0.КодТранспортногоУзла+1000000),6) ELSE CONVERT(varchar,T0.КодТранспортногоУзла) END) + ' ' +
T0.НазваниеRL LIKE N'% ' + Инвентаризация.dbo.fn_ReplaceRusLat(N'" + search + "') + '%')"
                    : "");

            var dt = DBManager.GetData(sql, Config.DS_document);
            var result = dt.AsEnumerable().Select(dr => new Basis
            {
                CodeHub = dr.Field<int>("КодТранспортногоУзла"),
                Name = dr.Field<string>("Название"),
                TypeTransport = dr.Field<string>("Транспорт"),
                CodeRailway = dr.Field<int?>("КодЖелезнойДороги"),
                CodeTypeTransport = dr.Field<int>("КодВидаТранспорта")
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
            var sql = String.Format(@"
SELECT TOP 1 T0.КодТранспортногоУзла, T0.Название+ISNULL('['+ЖД.ЖелезнаяДорога +']','') Название, ВТ.ВидТранспорта Транспорт, T0.КодЖелезнойДороги,T0.КодВидаТранспорта		
FROM [Справочники].[dbo].ТранспортныеУзлы T0
LEFT OUTER JOIN [Справочники].[dbo].ЖелезныеДороги ЖД ON T0.КодЖелезнойДороги=ЖД.КодЖелезнойДороги
INNER JOIN [Справочники].[dbo].ВидыТранспорта ВТ ON  T0.КодВидаТранспорта=ВТ.КодВидаТранспорта	
WHERE T0.КодТранспортногоУзла={0}", id);

            var dt = DBManager.GetData(sql, Config.DS_document);
            var result = dt.AsEnumerable().Select(dr => new Basis
            {
                CodeHub = dr.Field<int>("КодТранспортногоУзла"),
                Name = dr.Field<string>("Название"),
                TypeTransport = dr.Field<string>("Транспорт"),
                CodeRailway = dr.Field<int?>("КодЖелезнойДороги"),
                CodeTypeTransport = dr.Field<int>("КодВидаТранспорта")
            }).FirstOrDefault();

            return result;
        }
    }
}