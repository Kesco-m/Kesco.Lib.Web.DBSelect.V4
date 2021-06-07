using System.Globalization;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска по состоянию сотрудника, отбираются сотрудники с состоянием меньше указанного
    /// </summary>
    public class FOptEmployeeAvaible : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Значение
        /// </summary>
        public override string Value => ValueEmployeeAvaible.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        ///     Значение
        /// </summary>
        public bool ValueEmployeeAvaible { get; set; }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            return ValueEmployeeAvaible
                ? @" T0.КодСотрудника IN (SELECT КодСотрудника FROM(SELECT КодСотрудника FROM vwДолжности WHERE КодСотрудника IS NOT NULL 
		UNION SELECT КодСотрудника FROM КарточкиСотрудников) X
WHERE КодСотрудника IN (SELECT КодСотрудника FROM dbo.fn_Подчинённые()) 
	OR EXISTS (SELECT * FROM dbo.fn_ТекущиеРоли() Y 
			WHERE КодРоли IN(31,32,33) AND (КодЛица = 0 OR КодЛица IN(SELECT КодЛица FROM vwДолжности UNION SELECT КодЛицаЗаказчика FROM Сотрудники))))"
                : "";
        }
    }
}