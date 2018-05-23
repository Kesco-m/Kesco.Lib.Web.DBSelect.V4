namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.StoreReportType
{
    public class FOptResourceIDs : FOptBase, IFilterOption
    {
        public string SQLGetClause()
        {
            if (string.IsNullOrEmpty(Value)) return string.Empty;
            return string.Format("ТО.КодРесурса IN ( SELECT Root.КодРесурса FROM Ресурсы Root INNER JOIN Ресурсы Child ON Root.L<=Child.L AND Root.R>=Child.R WHERE Child.КодРесурса IN ({0}))", Value);
        }
    }
}
