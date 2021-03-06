﻿using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.TMDogovor;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    /// </summary>
    public class DSOTMDogovor : DSOCommon
    {
        /// <summary>
        ///     Опция поиска по тексту
        /// </summary>
        [FilterOption("Name", false, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOTMDogovor()
        {
            Name = new FOptName();
        }

        /// <summary>
        ///     Параметр фильтарации: Незакрытый месяц
        /// </summary>
        [FilterOption("OpenMonth", true, "PersonType")]
        public int OpenMonth { get; set; }

        /// <summary>
        ///     Параметр фильтарации: Год тарификации
        /// </summary>
        [FilterOption("Год", true)]
        public int? Year { get; set; }

        /// <summary>
        ///     Месяц
        /// </summary>
        [FilterOption("Месяц", true)]
        public int? Month { get; set; }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ДоговораПоКоторымБылаТарификация;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => "T0.Договор";
    }
}