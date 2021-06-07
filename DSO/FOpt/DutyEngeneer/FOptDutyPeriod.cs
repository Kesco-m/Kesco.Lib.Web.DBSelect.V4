using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.DutyEngeneer
{
    /// <summary>
    /// Опция поиска: За какую дату смотрим дежурства
    /// </summary>
    public class FOptDutyPeriod : FOptBase, IFilterOption
    {
        /// <summary>
        /// Формирование строки запроса
        /// </summary>
        /// <returns></returns>
        public string SQLGetClause()
        {
            return "";
        }
    }
}
