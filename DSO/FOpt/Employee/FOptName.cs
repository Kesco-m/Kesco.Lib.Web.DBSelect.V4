using System.Text;

namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Employee
{
    /// <summary>
    ///     Класс опции поиска по введенному в контрол тексту
    /// </summary>
    public class FOptName : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Проверка ID
        /// </summary>
        public int CheckId { get; set; }

        /// <summary>
        ///     Построение запроса
        /// </summary>
        /// <returns>Построенный запрос</returns>
        public string SQLGetClause()
        {
            var sb = new StringBuilder();
            /* в зависимости от количества слов в поиске определяем поля для поиска.
                * 1 слово - фамилия | имя
                * 2 слова - фамилия + имя | имя + фамилия | имя + отчество
                * 3 слова - фамилия + имя + отчество | имя + отчество + фамилия
                * 4+ слов - не накладываем ограничений
                * к указанным полям добавляются соответствующие английские поля ("Иван" + " " + "Ivan") - в полученной строке ищем */

            switch (WordsGroup.Count)
            {
                case 1:
                    sb.Append("(");
                    if (CheckId > 0)
                    {
                        sb.AppendFormat(
                            "(T0.КодСотрудника = {0} OR T0.КодСотрудника IN (SELECT КодСотрудника FROM @TblTel X)) OR ",
                            CheckId);
                    }
                    sb.Append(
                        "T0.ФамилияRL LIKE @S1 OR T0.LastName LIKE @S1 OR T0.ИмяRL LIKE @S1 OR T0.FirstName LIKE @S1");
                    sb.Append(")");
                    break;
                case 2:
                    sb.Append(@"(
                        ( (T0.ФамилияRL LIKE @S1 OR T0.LastName LIKE @S1) AND (T0.ИмяRL LIKE @S2 OR T0.FirstName LIKE @S2) ) OR
                        ( (T0.ИмяRL LIKE @S1 OR T0.FirstName LIKE @S1) AND (T0.ФамилияRL LIKE @S2 OR T0.LastName LIKE @S2) ) OR
                        ( (T0.ИмяRL LIKE @S1 OR T0.FirstName LIKE @S1) AND (T0.ОтчествоRL LIKE @S2 OR T0.MiddleName LIKE @S2) ) )");
                    break;
                case 3:
                    sb.Append(@"(
                        ( (T0.ФамилияRL LIKE @S1 OR T0.LastName LIKE @S1) AND (T0.ИмяRL LIKE @S2 OR T0.FirstName LIKE @S2) AND (T0.ОтчествоRL LIKE @S3 OR T0.MiddleName LIKE @S3) ) OR
                        ( (T0.ИмяRL LIKE @S1 OR T0.FirstName LIKE @S1) AND (T0.ОтчествоRL LIKE @S2 OR T0.MiddleName LIKE @S2) AND (T0.ФамилияRL LIKE @S3 OR T0.LastName LIKE @S3) ) )");
                    break;
            }
            return sb.ToString();
        }
    }
}