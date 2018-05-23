namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.PersonCatalog
{
    public class FOptPersonCatalog : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Построение условия WHERE для ограничения по каталогу
        /// </summary>
        /// <returns>Строка с условием WHERE для ограничения по каталогу</returns>
        public string SQLGetClause()
        {
            string[] fields = {"Каталог"};
            return !string.IsNullOrEmpty(Value) ? GetWhereStrBySearchWords(fields, WordsGroup) : "";
        }
    }
}