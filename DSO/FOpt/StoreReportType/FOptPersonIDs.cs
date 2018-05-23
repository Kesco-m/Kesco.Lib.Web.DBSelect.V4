namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StoreReportType
{
    public class FOptPersonIDs : FOptBase, IFilterOption
    {
        public string SQLGetClause()
        {
            if (string.IsNullOrEmpty(Value)) return string.Empty;
            return string.Format("EXISTS(SELECT КодРоли, КодЛица FROM Инвентаризация.dbo.fn_ТекущиеРоли() X WHERE X.КодРоли IN (ТО.КодРоли1, ТО.КодРоли2) AND X.КодЛица IN(0, {0}))", Value);
        }
    }
}
