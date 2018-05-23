using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Transport;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс сущности базис поставки
    /// </summary>
    public class DBSBasis : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSBasis()
        {
            KeyField = "CodeBasis";
            ValueField = "TypeTransport,Incoterms,Name";
            AnvancedHeaderPopupResult =
                string.Format("<tr class='v4s_noselect'><td><b>{0}</b></td><td><b>{2}</b></td><td><b>{1}</b></td></tr>",
                    Resx.GetString("sTypeOfTransport"), Resx.GetString("sName"), Resx.GetString("sCode"));

            Index = 2;
            OnRenderNtf += BasisNtf;
        }

        /// <summary>
        ///     Коллекция нотификаций контрола
        /// </summary>
        /// <param name="sender">Контрол</param>
        /// <param name="ntf">Нотификация</param>
        protected void BasisNtf(object sender, Ntf ntf)
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
            return GetBasises(search, MaxItemsInQuery);
        }

        /// <summary>
        ///     Список для выбора базиса
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <param name="maxItemsInQuery">Количество возвращаемых записей в запросе (top n)</param>
        /// <returns>Список базисов</returns>
        public List<Basis> GetBasises(string search, int maxItemsInQuery)
        {
            var sql =
                String.Format(@"SELECT TOP {0} T0.*, T1.ВидТранспорта FROM [Справочники].[dbo].БазисыПоставок T0 (nolock) 
            INNER JOIN [Справочники].[dbo].ВидыТранспорта T1 ON T0.КодВидаТранспорта=T1.КодВидаТранспорта
            {1}
            ORDER BY T1.ВидТранспорта, T0.Инкотермс, T0.Название", maxItemsInQuery,
                    !String.IsNullOrEmpty(search)
                        ? @"WHERE (((T0.Инкотермс + ' ' + T0.Инкотермс2010 + ' ' + T0.Название + ' ' + T0.НазваниеЛат) LIKE +'%" +
                          search + "%'))"
                        : "");

            var dt = DBManager.GetData(sql, Config.DS_document);
            var result = dt.AsEnumerable().Select(dr => new Basis
            {
                CodeBasis = dr.Field<int>("КодБазисаПоставки"),
                CodeTypeTransport = dr.Field<int>("КодВидаТранспорта"),
                Incoterms = dr.Field<string>("Инкотермс"),
                Incoterms2010 = dr.Field<string>("Инкотермс2010"),
                Name = dr.Field<string>("Название"),
                NameEn = dr.Field<string>("НазваниеЛат"),
                Description = dr.Field<string>("Описание"),
                InPrice = dr.Field<byte>("ТранспортВЦене"),
                TypeTransport = dr.Field<string>("ВидТранспорта")
            }).ToList();

            return result;
        }

        /// <summary>
        ///     Получение базиса поставки по ID
        /// </summary>
        /// <param name="id">ID базиса поставки</param>
        /// <param name="name">Наименование сущности</param>
        /// <returns>базис поставки</returns>
        public override object GetObjectById(string id, string name = "")
        {
            return new Basis(id);
        }
    }
}