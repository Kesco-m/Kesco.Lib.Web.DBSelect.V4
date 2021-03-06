﻿using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StavkaNDS;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Источник данных для котроа выбора роли
    /// </summary>
    public class DSOStavkaNDS : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по последнему году действия Ставки НЛС
        /// </summary>
        [FilterOption("Действует")] public FOptValid ValidAt;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOStavkaNDS()
        {
            KeyField = "КодСтавкиНДС";
            NameField = "СтавкаНДС";
            ValidAt = new FOptValid();
        }

        /// <summary>
        ///     Опция фильтра по территории
        /// </summary>
        [FilterOption("КодТерритории", true)]
        public int TerritoryCode { get; set; }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_СтавкиНДС;

        /// <summary>
        ///     Задание сортировки выборки
        ///     Доделать управляемую сортировку
        /// </summary>
        public override string SQLOrderBy => string.Format("{0}", "Приоритет");
    }
}