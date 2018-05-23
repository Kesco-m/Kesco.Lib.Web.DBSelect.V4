using System.Text;
using Kesco.Lib.BaseExtention.Enums.Controls;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Resource;

namespace Kesco.Lib.Web.DBSelect.V4.DSO
{
    /// <summary>
    ///     Класс объекта базы данных "Ресурс"
    /// </summary>
    public class DSOResource : DSOCommon
    {
        /// <summary>
        ///     Опция фильтра: все подчиненные ресурсы
        /// </summary>
        [FilterOption("AllChildrenWithoutParentIDs", false)] public FOptParentsIDs AllChildrenWithoutParentIDs;

        /// <summary>
        ///     Опция фильтра: все подчиненные ресурсы включая родителя
        /// </summary>
        [FilterOption("AllChildrenWithParentIDs", false)] public FOptParentsIDs AllChildrenWithParentIDs;

        /// <summary>
        ///     Опция фильтра: все непосредственные подчиненные ресурсы
        /// </summary>
        [FilterOption("ChildrenWithoutParentIDs", false)] public FOptParentsIDs ChildrenWithoutParentIDs;

        /// <summary>
        ///     Опция фильтра: все непосредственные подчиненные ресурсы включая родителя
        /// </summary>
        [FilterOption("ChildrenWithParentIDs", false)] public FOptParentsIDs ChildrenWithParentIDs;

        /// <summary>
        ///     Опция фильтра по указанным кодам ресурсов, которые являются валютой
        /// </summary>
        [FilterOption("CurrencyIDs", false, "CurrencyIDs")] public FOptCurrencyIDs CurrencyIDs;

        /// <summary>
        ///     Опция фильтра по ID ресурсов
        /// </summary>
        [FilterOption("IDs", false, "IDs")] public FOptIDs IDs;

        /// <summary>
        ///     Опция фильтра по имени ресурса
        /// </summary>
        [FilterOption("Name", false, "Search")] public FOptName Name;

        /// <summary>
        ///     Опция фильтра: id ресурсов имеющие привязку к лицам
        /// </summary>
        [FilterOption("PersonIDs", false)] public FOptPersonIDs PersonIDs;

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        public DSOResource()
        {
            var tableName = "Ресурсы";

            KeyField = "КодРесурса";
            NameField = "РесурсРус";

            IDs = new FOptIDs();
            Name = new FOptName();
            CurrencyIDs = new FOptCurrencyIDs();

            AllChildrenWithParentIDs = new FOptParentsIDs(TreeQueryType.AllChildrenWithParent, tableName, KeyField, NameField);
            AllChildrenWithoutParentIDs = new FOptParentsIDs(TreeQueryType.AllChildrenWithoutParent, tableName, KeyField, NameField);
            ChildrenWithParentIDs = new FOptParentsIDs(TreeQueryType.ChildrenWithParent, tableName, KeyField);
            ChildrenWithoutParentIDs = new FOptParentsIDs(TreeQueryType.ChildrenWithoutParent, tableName, KeyField);

            PersonIDs = new FOptPersonIDs();
        }

        /// <summary>
        ///     Кусок запроса выполняется непосредственно перед выборкой
        /// </summary>
        public override string SQLBatchPrepare
        {
            get
            {
                var sb = new StringBuilder();

                if (!string.IsNullOrEmpty(Name.Value))
                {
                    const string varDeclPattern =
                        @"DECLARE @s{1} nvarchar(50); SET @s{1} = Инвентаризация.dbo.fn_SplitWords(Инвентаризация.dbo.fn_ReplaceRusLat(Инвентаризация.dbo.fn_ReplaceKeySymbols(N'{0}')));";

                    for (var i = 0; i < Name.WordsGroup.Count; i++)
                    {
                        sb.Append(string.Format(varDeclPattern, Name.WordsGroup[i], i));
                        sb.Append("\n");
                    }
                }

                if (PersonIDs.Count > 0 && PersonIDs.SearchStrategy == SearchResources.SearchResOnlyForSpecifiedPersons)
                {
                    const string sqlVarDeclaration =
                        @"DECLARE @Persons TABLE(PersonID int); CREATE TABLE #Resources (ResourceID int, PersonID int) ";

                    sb.Append(sqlVarDeclaration);
                    sb.Append("\n");
                    sb.Append(
                        string.Format(@"INSERT @Persons SELECT КодЛица FROM vwЛица (nolock) WHERE КодЛица IN ({0}); ",
                            PersonIDs.Value));
                    sb.Append("\n");
                    sb.Append(
                        string.Format(
                            @"INSERT #Resources SELECT T1.КодРесурса, Лица.PersonID FROM @Persons Лица INNER JOIN РесурсыЛица T1 (nolock) ON Лица.PersonID = T1.КодЛица; "));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        ///     Запрос выборки данных
        /// </summary>
        public override string SQLBatch
        {
            get { return SQLQueries.SELECT_Ресурсы; }
        }

        /// <summary>
        ///     Запрос получения ресурса по ID
        /// </summary>
        public override string SQLEntityById
        {
            get { return SQLQueries.SELECT_ID_Ресурс; }
        }

        /// <summary>
        ///     Задание сортировки выборки
        /// </summary>
        public override string SQLOrderBy
        {
            get { return string.Format("T0.{0}", NameField); }
        }
    }
}