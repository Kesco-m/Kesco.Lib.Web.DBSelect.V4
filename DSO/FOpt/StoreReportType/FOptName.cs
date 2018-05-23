
namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StoreReportType
{
    /// <summary>
    /// Класс опции поиска
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords("ТипОтчётаПоСкладам", WordsGroup) : "";
        }
    }
}
