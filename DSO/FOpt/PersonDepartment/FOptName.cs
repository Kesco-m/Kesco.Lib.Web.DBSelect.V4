
namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonDepartment
{
    /// <summary>
    /// Класс опции поиска
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords("T0.Подразделение", WordsGroup) : "";
        }
    }
}
