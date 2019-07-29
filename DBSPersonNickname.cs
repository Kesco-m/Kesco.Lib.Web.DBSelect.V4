using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kesco.Lib.Entities.Persons;

namespace Kesco.Lib.Web.DBSelect.V4
{
    /// <summary>
    ///     Класс Select для элемента управления связи лиц
    /// </summary>
    public class DBSPersonNickname : DBSelect
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public DBSPersonNickname()
        {
            KeyField = "Name";
            ValueField = "Name";
            IsNotUseSelectTop = true;
            NamesReg = new List<PersonNameReg>();
            NamesLat = new List<PersonNameLat>();
            NickNames = new List<string>();
        }

        /// <summary>
        ///     Имена на языке страны регистрации
        /// </summary>
        public List<PersonNameReg> NamesReg { get; set; }

        /// <summary>
        ///     Имена на латинице
        /// </summary>
        public List<PersonNameLat> NamesLat { get; set; }

        /// <summary>
        ///     СПисок кличек
        /// </summary>
        public List<string> NickNames { get; set; }

        /// <summary>
        ///     Заполнение списка
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns>Список</returns>
        public override IEnumerable FillSelect(string search)
        {
            //base.FillSelect(search);

            if (!string.IsNullOrEmpty(search))
            {
                var listResult = new List<PersonNickName>();
                var fullListResult = GetPersonNickNames();
                foreach (var searchWord in search.Split(' '))
                    listResult.AddRange(fullListResult.Where(o => o.Name.Contains(searchWord)));
                return listResult.Distinct();
            }

            return GetPersonNickNames();
        }

        /// <summary>
        ///     Получение списка псевдонимов лица
        /// </summary>
        /// <returns>Список</returns>
        public List<PersonNickName> GetPersonNickNames()
        {
            var personNickNames = GetRegNickNames().Select(nickName => new PersonNickName {Name = nickName}).ToList();
            personNickNames.Add(new PersonNickName {Name = "------------------------------------"});
            personNickNames.AddRange(GetLatNickNames().Select(nickName => new PersonNickName {Name = nickName}));

            NickNames = personNickNames.Select(t => t.Name.ToString()).ToList();

            return personNickNames;
        }

        /// <summary>
        ///     Получение списка псевдонимов лица на латинице
        /// </summary>
        /// <returns>Список</returns>
        private List<string> GetRegNickNames()
        {
            var onlyFirstName =
                NamesReg.Select(t => t)
                    .Where(k => !string.IsNullOrEmpty(k.FirstName) && string.IsNullOrEmpty(k.SecondName));
            var onlySecondName =
                NamesReg.Select(t => t)
                    .Where(k => !string.IsNullOrEmpty(k.SecondName) && string.IsNullOrEmpty(k.FirstName));
            var fistAndSecondName =
                NamesReg.Select(t => t)
                    .Where(k => !string.IsNullOrEmpty(k.FirstName) && !string.IsNullOrEmpty(k.SecondName));
            var personNickNames = new List<string>();

            //Фамилия Имя Отчество
            personNickNames.AddRange(
                onlyFirstName.Select(
                    name => name.FirstName + (!string.IsNullOrEmpty(name.MiddleName) ? " " + name.MiddleName : "")));
            personNickNames.AddRange(onlySecondName.Select(name => name.SecondName));
            personNickNames.AddRange(
                fistAndSecondName.Select(
                    name =>
                        name.SecondName + " " + name.FirstName +
                        (!string.IsNullOrEmpty(name.MiddleName) ? " " + name.MiddleName : "")));

            //Имя Отчество Фамилия
            //personNickNames.AddRange(onlyFirstName.Select(name => name.FirstName + (!String.IsNullOrEmpty(name.MiddleName) ? " " + name.MiddleName : "")));
            personNickNames.AddRange(
                fistAndSecondName.Select(
                    name =>
                        name.FirstName + (!string.IsNullOrEmpty(name.MiddleName) ? " " + name.MiddleName + " " : "") +
                        name.SecondName));

            //Фамилия И.О.
            personNickNames.AddRange(
                onlyFirstName.Where(k => !string.IsNullOrEmpty(k.MiddleName))
                    .Select(name => name.FirstName[0] + ". " + name.MiddleName[0] + "."));
            personNickNames.AddRange(
                fistAndSecondName.Select(
                    name =>
                        name.SecondName + " " + name.FirstName[0] + ". " +
                        (!string.IsNullOrEmpty(name.MiddleName) ? " " + name.MiddleName[0] + "." : "")));

            //И.О. Фамилия
            personNickNames.AddRange(
                fistAndSecondName.Select(
                    name =>
                        name.FirstName[0] + ". " +
                        (!string.IsNullOrEmpty(name.MiddleName) ? " " + name.MiddleName[0] + ". " : "") +
                        name.SecondName));

            return personNickNames;
        }

        /// <summary>
        ///     Получение списка псевдонимов лица на латинице
        /// </summary>
        /// <returns>Список</returns>
        private List<string> GetLatNickNames()
        {
            var onlyFirstName =
                NamesLat.Select(t => t)
                    .Where(k => !string.IsNullOrEmpty(k.FirstName) && string.IsNullOrEmpty(k.SecondName));
            var onlySecondName =
                NamesLat.Select(t => t)
                    .Where(k => !string.IsNullOrEmpty(k.SecondName) && string.IsNullOrEmpty(k.FirstName));
            var fistAndSecondName =
                NamesLat.Select(t => t)
                    .Where(k => !string.IsNullOrEmpty(k.FirstName) && !string.IsNullOrEmpty(k.SecondName));

            var personNickNames = new List<string>();

            //Фамилия Имя Отчетво
            personNickNames.AddRange(
                onlyFirstName.Select(
                    name => name.FirstName + (!string.IsNullOrEmpty(name.MiddleName) ? " " + name.MiddleName : "")));
            personNickNames.AddRange(onlySecondName.Select(name => name.SecondName));
            personNickNames.AddRange(
                fistAndSecondName.Select(
                    name =>
                        name.SecondName + " " + name.FirstName +
                        (!string.IsNullOrEmpty(name.MiddleName) ? " " + name.MiddleName : "")));

            //Имя Отчество Фамилия
            personNickNames.AddRange(
                fistAndSecondName.Where(o => !string.IsNullOrEmpty(o.MiddleName))
                    .Select(name => name.FirstName + " " + name.MiddleName + " " + name.SecondName));

            //Фамилия И.О.
            personNickNames.AddRange(
                onlyFirstName.Select(
                    name =>
                        name.FirstName[0] + "." +
                        (!string.IsNullOrEmpty(name.MiddleName) ? " " + name.MiddleName[0] + "." : "")));
            personNickNames.AddRange(
                fistAndSecondName.Select(
                    name =>
                        name.SecondName + " " + name.FirstName[0] + "." +
                        (!string.IsNullOrEmpty(name.MiddleName) ? " " + name.MiddleName[0] + "." : "")));

            //И.О. Фамилия
            //personNickNames.AddRange(fistAndSecondName.Select(name => name.FirstName[0] + ". " + (!String.IsNullOrEmpty(name.MiddleName) ? " " + name.MiddleName[0] + ". " : "") + name.SecondName));

            //Фаимилия Имя О.
            personNickNames.AddRange(
                onlyFirstName.Where(k => !string.IsNullOrEmpty(k.MiddleName))
                    .Select(name => name.FirstName + " " + name.MiddleName[0] + "."));
            //personNickNames.AddRange(fistAndSecondName.Where(k => !String.IsNullOrEmpty(k.MiddleName)).Select(name => name.SecondName + " " + name.FirstName + " " + name.MiddleName[0] + "."));

            //Имя О. Фамилия
            //personNickNames.AddRange(fistAndSecondName.Where(k => !String.IsNullOrEmpty(k.MiddleName)).Select(name => name.FirstName + " " + name.MiddleName[0] + "." + " " + name.SecondName));


            return personNickNames;
        }

        /// <summary>
        ///     Получение связи лиц по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name </param>
        /// <returns>Компания</returns>
        public override object GetObjectById(string id, string name = "")
        {
            return new PersonNickName {Name = name};
        }
    }
}