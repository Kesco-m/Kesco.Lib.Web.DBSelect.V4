using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonCustomer
{
   
    /// <summary>
    ///     Класс опции поиска
    /// </summary>
    public class FOptPersonCustomerRole : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по типу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по типу</returns>
        public string SQLGetClause()
        {
            return !string.IsNullOrEmpty(Value) ? string.Format("(EXISTS(SELECT * FROM РолиСотрудников WHERE T0.КодЛица = РолиСотрудников.КодЛица AND КодРоли={0}))", Value) : "";
        }
    }
}
