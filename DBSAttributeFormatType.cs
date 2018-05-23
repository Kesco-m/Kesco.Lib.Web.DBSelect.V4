using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Attributes;
using Kesco.Lib.Web.DBSelect.V4.DSO;
using Kesco.Lib.Web.Settings;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс select сущности Тип формата атрибута
    /// </summary>
    public class DBSAttributeFormatType : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSAttributeFormatType()
        {
            base.Filter = new DSOAttributeFormatType();
            KeyField = "Id";
            ValueField = "Name";
            IsNotUseSelectTop = true;
        }

        /// <summary>
        ///     Тип лица: 0-все типы, 1-юридическое, 2-физическое
        /// </summary>
        public int PersonType { get; set; }

        /// <summary>
        ///     Текущий язык интерфейса
        /// </summary>
        private string LangName { get; set; }

        /// <summary>
        ///     Фильтр
        /// </summary>
        public new DSOAttributeFormatType Filter
        {
            get { return (DSOAttributeFormatType) base.Filter; }
        }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            base.FillSelect(search);
            return GetAttributeFormatTypes();
        }

        /// <summary>
        ///     Получение списка типов форматов атрибутов
        /// </summary>
        /// <returns>Список</returns>
        public List<AttributeFormatType> GetAttributeFormatTypes()
        {
            LangName = V4Page.CurrentUser.Language;
            var dt = DBManager.GetData(SQLGetText(false), Config.DS_person, CommandType.Text, SQLGetInnerParams());

            List<AttributeFormatType> attributeFormatType;
            if (PersonType != 0)
                attributeFormatType = dt.AsEnumerable().Select(dr => new AttributeFormatType
                {
                    Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                    Name =
                        LangName == "ru"
                            ? dr.Field<string>(Filter.NameField)
                            : dr.Field<string>(Filter.AttributeFormatTypeNameLat),
                    PersonTypeAvailability = dr.Field<byte>(Filter.PersonTypeAvailabilityField),
                    Order = dr.Field<int>(Filter.OrderField),
                    AttributeFormatTypeNameLat = dr.Field<string>(Filter.AttributeFormatTypeNameLat)
                }).Where(t => t.PersonTypeAvailability == PersonType || t.PersonTypeAvailability == 0).ToList();
            else
            {
                attributeFormatType = dt.AsEnumerable().Select(dr => new AttributeFormatType
                {
                    Id = dr.Field<int>(Filter.KeyField).ToString(CultureInfo.InvariantCulture),
                    Name =
                        LangName == "ru"
                            ? dr.Field<string>(Filter.NameField)
                            : dr.Field<string>(Filter.AttributeFormatTypeNameLat),
                    PersonTypeAvailability = dr.Field<byte>(Filter.PersonTypeAvailabilityField),
                    Order = dr.Field<int>(Filter.OrderField),
                    AttributeFormatTypeNameLat = dr.Field<string>(Filter.AttributeFormatTypeNameLat)
                }).ToList();
            }
            return attributeFormatType;
        }

        /// <summary>
        ///     Получение типа формата атрибута по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            LangName = V4Page.CurrentUser.Language;
            return new AttributeFormatType(id, LangName);
        }
    }
}