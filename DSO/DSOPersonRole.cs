﻿using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonRole;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса Компания
    /// </summary>
    public class DSOPersonRole : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра по имени компании
        /// </summary>
        [FilterOption("Кличка", false, "Search")]
        public FOptName Name;

        /// <summary>
        ///     Опция фильтра по роли
        /// </summary>
        [FilterOption("Role")] public FOptPersonRole PersonRole;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOPersonRole()
        {
            KeyField = "КодЛица";
            NameField = "Кличка";
            Name = new FOptName();
            PersonRole = new FOptPersonRole();
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch => SQLQueries.SELECT_ЛицаЗаказчикиDBS;

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy => string.Format("T0.{0}", NameField);
    }
}