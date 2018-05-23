﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Kesco.Lib.DALC;
using Kesco.Lib.Web.Settings;
using Kesco.Lib.Entities;
using Kesco.Lib.Web.DBSelect.V4.DSO;

namespace Kesco.Lib.Web.DBSelect.V4
{
	/// <summary>
	///     Класс Select для элемента управления "Адрес лица"
	/// </summary>
	public class DBSPersonContact : DBSelect
	{
		//Список элементов
		List<Kesco.Lib.Entities.Item> _listOfItems = null;

		/// <summary>
		///     Конструктор контрола
		/// </summary>
		public DBSPersonContact()
		{
			base.Filter = new DSOPersonContact();
			KeyField = "Id";
			ValueField = "Name";

			IsNotUseSelectTop = false;
		}


		/// <summary>
		/// Фильтр Подзапрос
		/// </summary>
		public DSOPersonContact GetFilter()
		{
			return Filter as DSOPersonContact;
		}

		public override IEnumerable FillSelect(string search)
		{
			base.FillSelect(search);

			FillItemsList();

			return _listOfItems;
		}

		/// <summary>
		/// Медот заполоняет список элементов из БД
		/// </summary>
		private void FillItemsList()
		{
			_listOfItems = new List<Kesco.Lib.Entities.Item>();

			DataTable dtItems = DBManager.GetData(SQLGetText(true), Config.DS_person, CommandType.Text, SQLGetInnerParams());

			foreach (DataRow row in dtItems.Rows)
			{
				_listOfItems.Add(new Kesco.Lib.Entities.Item { Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField] });
			}
		}

		public override object GetObjectById(string id, string name = "")
		{
			if (null == _listOfItems) FillItemsList();

			Kesco.Lib.Entities.Item store_item;
			if (!string.IsNullOrWhiteSpace(name))
			{
				store_item = _listOfItems.Find(x => 0 == string.Compare(x.Id, id, true) && 0 == string.Compare(x.Value.ToString(), name, true));
			}
			else
			{
				store_item = _listOfItems.Find(x => 0 == string.Compare(x.Id, id, true));
			}

			if (object.Equals(store_item, default(Kesco.Lib.Entities.Item)))//значение Item по умолчанию все поля null
			{
				var sqlParams = new Dictionary<string, object> { { "@КодКонтакта", id } };
				DataTable dtItems = DBManager.GetData(SQLQueries.SELECT_ID_КонтактыЛица, Config.DS_person, CommandType.Text, sqlParams);
				if (null != dtItems && dtItems.Rows.Count > 0)
				{
					DataRow row = dtItems.Rows[0];
					return new Kesco.Lib.Entities.Item { Id = row[Filter.KeyField].ToString(), Value = row[Filter.NameField] };
				}

				return null;
			}

			return store_item;
		}
	}
}