namespace Kesco.Lib.Web.DBSelect.V4.DSO.FOpt.Socket
{
    /// <summary>
    ///     Класс опции поиска розетки свободна / занята
    /// </summary>
    public class FOptFree : FOptBase, IFilterOption
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public FOptFree()
        {
            Free = true;
            SocketInclude = -1;
        }

        /// <summary>
        ///     Использовать опцию при поиске
        /// </summary>
        public bool Free { get; set; }

        /// <summary>
        /// Включить код розетки в условие
        /// </summary>
        public int SocketInclude { get; set; }

        /// <summary>
        ///     Построение блока WHERE для опции
        /// </summary>
        /// <returns>Блок WHERE опции</returns>
        public string SQLGetClause()
        {
            if (!Free) return "";

            if (SocketInclude != -1)
                return "(T0.КодРозетки=" + SocketInclude + @" OR  NOT (EXISTS (SELECT * FROM vwТелефонныеНомераИзменение WHERE КодРозетки = T0.КодРозетки) 
						 OR EXISTS (SELECT * FROM vwЛокальнаяСеть WHERE КодРозетки = T0.КодРозетки)))";
            else
                return @" NOT (EXISTS (SELECT * FROM vwТелефонныеНомераИзменение WHERE КодРозетки = T0.КодРозетки) 
						 OR EXISTS (SELECT * FROM vwЛокальнаяСеть    WHERE КодРозетки = T0.КодРозетки))";

        }
    }
}