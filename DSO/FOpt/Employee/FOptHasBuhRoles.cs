using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска по сотрудникам, имеющим доступ к указанной бухгалтерии
    /// </summary>
    public class FOptHasBuhRoles : FOptBase, IFilterOption
    {

        /// <summary>
        ///     Значение
        /// </summary>
        public bool ValueBuhRoles { get; set; } = false;

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            if (!ValueBuhRoles) return "";
            var sql = @"
EXISTS(
        SELECT * FROM РолиСотрудников 
		WHERE T0.КодСотрудника = РолиСотрудников.КодСотрудника 
		AND КодРоли BETWEEN 58 AND 70
		)
";
            return sql;
        }
    }
}
