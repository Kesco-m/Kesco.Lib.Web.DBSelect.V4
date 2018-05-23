using System;
using System.Text;
using System.Text.RegularExpressions;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Фильтр класса сотрудники
    /// </summary>
    public class DSOEmployee : DSOCommon
    {
        /// <summary>
        ///     Параметр фильтрации: Отображать сотрудников, имеющих проходы за указанный период
        /// </summary>
        [FilterOption("EmployeeAvaible", optionNameURL: "EmployeeAvaible")] public FOptEmployeeAvaible EmployeeAvaible;

        /// <summary>
        ///     Опция поиска по сотрудникам, имеющим логин
        /// </summary>
        [FilterOption("HasLogin", optionNameURL: "HasLogin")]
        public FOptHasLogin HasLogin;

        /// <summary>
        ///     Опция поиска по сотрудникам, имеющим email
        /// </summary>
        [FilterOption("HasEmail", optionNameURL: "HasEmail")]
        public FOptHasEmail HasEmail;

        /// <summary>
        ///     Опция поиска по сотрудникам, включая виртуальных
        /// </summary>
        [FilterOption("HasVirtual", optionNameURL: "HasVirtual")]
        public FOptHasVirtual HasVirtual;

        /// <summary>
        ///     Опция поиска по кодам сотрудника
        /// </summary>
        [FilterOption("ids", optionNameURL: "ids")] public FOptIDs Ids;

        /// <summary>
        ///     Опция фильтра по ID компании заказчика в таблице сотрудники
        /// </summary>
        [FilterOption("КодЛица", optionNameURL: "Company")] public FOptIDsCompany IdsCompany;

        /// <summary>
        ///     Опция поиска по тексту
        /// </summary>
        [FilterOption("Name", optionNameURL: "Search")] public FOptName Name;

        /// <summary>
        ///     Опция фильтра по Должности
        /// </summary>
        [FilterOption("Должности", optionNameURL: "Position")] public FOptPositionIDs PositionIDs;

        /// <summary>
        ///     Опция расширенного поиска по кодам сотрудника
        /// </summary>
        [FilterOption("selectedid", optionNameURL: "selectedid")] public FOpt.Common.FOptName SelectedId;

        /// <summary>
        ///     Опция поиска по состоянию сотрудника
        /// </summary>
        [FilterOption("status", optionNameURL: "UserState")] public FOptStatus Status;

        /// <summary>
        ///     Опция фильтра по Подразделение
        /// </summary>
        [FilterOption("Подразделение", optionNameURL: "Subdivision")] public FOptSubdivisionIDs SubdivisionIDs;

        /// <summary>
        ///     Опция поиска
        /// </summary>
        [FilterOption("UserOur", optionNameURL: "UserOur")] public string UserOur;

        /// <summary>
        ///     Опция поиска
        /// </summary>
        [FilterOption("UserStaffMembers", optionNameURL: "UserStaffMembers")] public FOptUserStaffMembers
            UserStaffMembers;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOEmployee()
        {
            KeyField = "T0.КодСотрудника";
            NameField = "T0.Сотрудник";
            Name = new FOptName();
            Ids = new FOptIDs();
            SelectedId = new FOpt.Common.FOptName();
            Status = new FOptStatus();
            HasLogin = new FOptHasLogin();
            HasEmail = new FOptHasEmail();
            HasVirtual = new FOptHasVirtual();
            IdsCompany = new FOptIDsCompany();
            SubdivisionIDs = new FOptSubdivisionIDs();
            PositionIDs = new FOptPositionIDs();
            EmployeeAvaible = new FOptEmployeeAvaible();
            UserStaffMembers = new FOptUserStaffMembers();
        }

        /// <summary>
        ///     Параметр фильтрации: условие поиска по сотрудникам
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        ///     2 - Любое значение (не фильтруем по полю),
        ///     3 - Значение не указано (значение поля NULL)
        /// </summary>
        [FilterOption("EmployeeHowSearch", optionNameURL: "employeehowuserlist")]
        public string EmployeeHowSearch
        {
            get { return Ids.EmployeeHowSearch; }
        }

        /// <summary>
        ///     Параметр фильтрации: условие поиска по компаниям
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        ///     2 - Любое значение (не фильтруем по полю),
        ///     3 - Значение не указано (значение поля NULL)
        /// </summary>
        [FilterOption("CompanyHowSearch", optionNameURL: "companyhowuserlist")]
        public string CompanyHowSearch
        {
            get { return IdsCompany.CompanyHowSearch; }
        }

        /// <summary>
        ///     Параметр фильтрации: условие поиска по подразделениям
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        ///     2 - Любое значение (не фильтруем по полю),
        ///     3 - Значение не указано (значение поля NULL)
        /// </summary>
        [FilterOption("SubdivisionHowSearch", optionNameURL: "subdivisionhowuserlist")]
        public string SubdivisionHowSearch
        {
            get { return SubdivisionIDs.SubdivisionHowSearch; }
        }

        /// <summary>
        ///     Параметр фильтрации: условие поиска по должностям
        ///     0 - Элементы из списка,
        ///     1 - Элементы за исключением,
        ///     2 - Любое значение (не фильтруем по полю),
        ///     3 - Значение не указано (значение поля NULL)
        /// </summary>
        [FilterOption("PositionHowSearch", optionNameURL: "positionhowuserlist")]
        public string PositionHowSearch
        {
            get { return PositionIDs.PositionHowSearch; }
        }

        /// <summary>
        ///     Кусок запроса выполняется непосредственно перед выборкой
        /// </summary>
        public override string SQLBatchPrepare
        {
            get
            {
                var m = Name.WordsGroup;
                var wordsCount = m.Count;

                var sb = new StringBuilder();
                const string sql = @"DECLARE @S1 varchar(50), @S2 varchar(50), @S3 varchar(50)";

                for (var i = 1; i <= wordsCount; i++)
                {
                    sb.Append("\n");
                    sb.AppendFormat(@"SET @S{0} = {1}", i, ReplaceRusLat(m[i - 1].Value + "%"));
                }

                if (wordsCount == 1 && Regex.IsMatch(m[0].Value, "^\\d{1,6}$"))
                {
                    Name.CheckId = Int32.Parse(m[0].Value);
                    sb.Append("\n");
                    sb.Append(
                        @" DECLARE @TblTel TABLE(КодТелефонногоНомера int, КодТипаТелефонныхНомеров int, Абонент nvarchar(100), ПолныйНомер nvarchar(50), КодСотрудника int, Login varchar(50), Email varchar(50))");
                    sb.Append("\n");
                    sb.Append(@" INSERT @TblTel EXEC sp_ПоискТелефонногоНомера @НомерВнутренний = " + Name.CheckId);
                }
                else
                {
                    Name.CheckId = 0;
                }
                return sb.Length > 0 ? sql + sb : "";
            }
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get
            {
                var result = "SELECT {0} T0.КодСотрудника, T0.Сотрудник, T0.Employee FROM Сотрудники T0";
                if (!string.IsNullOrEmpty(SubdivisionIDs.Value) || SubdivisionIDs.SubdivisionHowSearch == "3" ||
                    !string.IsNullOrEmpty(PositionIDs.Value) || PositionIDs.PositionHowSearch == "3")
                    result +=
                        " LEFT JOIN [Инвентаризация].[dbo].[vwДолжности] T1 ON T0.КодСотрудника = T1.КодСотрудника ";
                return result;
            }
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy
        {
            get { return "T0.Сотрудник"; }
        }
    }
}