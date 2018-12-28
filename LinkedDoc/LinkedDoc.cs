using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Kesco.Lib.BaseExtention;
using Kesco.Lib.BaseExtention.Enums.Controls;
using Kesco.Lib.BaseExtention.Enums.Docs;
using Kesco.Lib.DALC;
using Kesco.Lib.Entities.Documents;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.Controls.V4.Common;
using Kesco.Lib.Web.Controls.V4.Common.DocumentPage;
using Kesco.Lib.Web.Settings;
using Page = Kesco.Lib.Web.Controls.V4.Common.DocumentPage.DocPage;

namespace Kesco.Lib.Web.DBSelect.V4.LinkedDoc
{
    /// <summary>
    /// Вытекающие документы
    /// </summary>
    public class LinkedDoc : V4Control, IClientCommandProcessor
    {
        #region Constants

        private const string _constIdTag = "[CID]";
        private const string _constCtrlRadio = "[C_RADIO]";
        private const string _constCtrlDBSelect = "[C_DBSELECT]";

        #endregion

        private Radio _currentRadioCtrl;
        private DBSDocument _currentDBSelectCtrl;

        /// <summary>
        /// Вытекающие документы
        /// </summary>
        protected int LinkedDocCmdListnerIndex;

        /// <summary>
        ///     Акцессор V4Page
        /// </summary>
        public Page V4Page
        {
            get { return Page as DocPage; }
            set { Page = value; }
        }

        private string _docValue;
        private string _type;
        private string _field;
        private string _linkType;
        private string _linkedDocs;

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            if (!V4Page.Listeners.Contains(this)) V4Page.Listeners.Add(this);
            LinkedDocCmdListnerIndex = V4Page.Listeners.IndexOf(this);

            base.OnInit(e);
            
            if (V4Page.V4IsPostBack) return;

            _currentRadioCtrl = new Radio {V4Page = V4Page, ID = "radio_" + ID, IsRow = false, Name = "DocRadio"};
            _currentRadioCtrl.HtmlID = "radio_" + ID;
            _currentRadioCtrl.Changed += _currentRadioCtrl_OnChanged;
            _currentRadioCtrl.Items.Add(new Item("0", " Создать новый документ"));
            _currentRadioCtrl.Items.Add(new Item("1", " Добавить существующий"));
            _currentRadioCtrl.Value = "0";
            V4Page.V4Controls.Add(_currentRadioCtrl);

            _currentDBSelectCtrl = new DBSDocument
            {
                V4Page = V4Page,
                ID = "dbsDocument_" + ID,
                HtmlID = "linkedDoc",
                Width = new System.Web.UI.WebControls.Unit("350px")
            };

            _currentDBSelectCtrl.OnRenderNtf += LinkedDocumentOnOnRenderNtf;
            _currentDBSelectCtrl.ValueChanged += LinkedDocumentOnValueChanged;
            _currentDBSelectCtrl.BeforeSearch += DBSelect_BeforeSearch;
            _currentDBSelectCtrl.IsDisabled = true;
            V4Page.V4Controls.Add(_currentDBSelectCtrl);
        }

        /// <summary>
        ///     Событие, устанавливающее параметры фильтрации
        /// </summary>
        /// <param name="sender">Контрол</param>
        private void DBSelect_BeforeSearch(object sender)
        {
            if (!string.IsNullOrEmpty(_type))
            {
                _currentDBSelectCtrl.Filter.Type.Add(_type, DocTypeQueryType.Equals);
            }
            if (!string.IsNullOrEmpty(V4Page.Doc.Date.ToString("yyyyMMdd")))
            {
                _currentDBSelectCtrl.Filter.Date.DateSearchType = DateSearchType.MoreThan;
                _currentDBSelectCtrl.Filter.Date.Add(V4Page.Doc.Date.ToString("yyyyMMdd"));
            }
            if (!string.IsNullOrEmpty(_linkedDocs))
            {
                var col = ConvertExtention.Convert.Str2Collection(_linkedDocs);
                foreach (string id in col)
                    _currentDBSelectCtrl.Filter.IDs.Add(id);

                if (col.Count > 0)
                    _currentDBSelectCtrl.Filter.IDs.Inverse = true;
            }

        }

        /// <summary>
        /// Обработка изменения позиции выбора действия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _currentRadioCtrl_OnChanged(object sender, ProperyChangedEventArgs e)
        {
            _currentDBSelectCtrl.IsDisabled = e.NewValue == "0";
            if (_currentDBSelectCtrl.IsDisabled)
                _currentDBSelectCtrl.Value = _docValue = null;
        }

        /// <summary>
        /// Обработка клиентских команд
        /// </summary>
        /// <param name="param"></param>
        public void ProcessClientCommand(NameValueCollection param)
        {
            switch (param["cmdName"])
            {
                case "RefreshGridData":
                    RefreshData();
                    break;
                case "OpenLinkedDoc":
                    _type = param["type"];
                    _field = param["field"];
                    _linkType = param["linkType"];
                    _linkedDocs = param["linkedDocs"];

                    _currentRadioCtrl.Value = "0";
                    _currentRadioCtrl.Flush();
                    _currentDBSelectCtrl.Value = null;
                    _currentDBSelectCtrl.IsDisabled = true;

                    JS.Write("dialogShow('{0}', '{1}');", Resx.GetString("title"), LinkedDocCmdListnerIndex);
                    break;
                case "OK":
                    string url = "";
                    bool bAdd = _currentRadioCtrl.Value == "0";
                    DocType dtp = new DocType(_type);

                    if (bAdd)
                        // создаем новый документ
                        url = dtp.URL + (dtp.URL.IndexOf("?", StringComparison.Ordinal) > 0 ? "&" : "?") + "DocId=" + V4Page.Doc.Id + "&fieldId=" + _field;
                    else
                        // связываем существующий документ
                        url = dtp.URL + (dtp.URL.IndexOf("?", StringComparison.Ordinal) > 0 ? "&" : "?") +
                            "DocId=" + V4Page.Doc.Id + "&Id=" + _currentDBSelectCtrl.Value + "&fieldId=" + _field;

                    if (url != "")
                    {
                        JS.Write("dialogShow.form.dialog('close');");
                        JS.Write("win=window.open('{0}','_blank','status=no,toolbar=no,menubar=no,location=no,resizable=yes,scrollbars=yes');", url);
                    }
                    break;
            }
        }

        /// <summary>
        /// Отрисовка контрола
        /// </summary>
        /// <param name="output"></param>
        public override void RenderControl(HtmlTextWriter output)
        {
            
            var currentAsm = Assembly.GetExecutingAssembly();
            var linkedDocContent = currentAsm.GetManifestResourceStream("Kesco.Lib.Web.DBSelect.V4.LinkedDoc.LinkedDocContent.htm");
            if (linkedDocContent == null) return;
            var reader = new StreamReader(linkedDocContent);
            var sourceContent = reader.ReadToEnd();
            
            sourceContent = sourceContent.Replace(_constIdTag, ID);
            
            using (TextWriter currentRadioTextWriter = new StringWriter())
            {
                var currentWriter = new HtmlTextWriter(currentRadioTextWriter);
                _currentRadioCtrl.RenderControl(currentWriter);
                sourceContent = sourceContent.Replace(_constCtrlRadio, currentRadioTextWriter.ToString());
            }

            if (_currentDBSelectCtrl != null)
            {
                using (TextWriter currentDBSelectTextWriter = new StringWriter())
                {
                    var currentWriter = new HtmlTextWriter(currentDBSelectTextWriter);
                    _currentDBSelectCtrl.RenderControl(currentWriter);
                    sourceContent = sourceContent.Replace(_constCtrlDBSelect, currentDBSelectTextWriter.ToString());
                }
            }
            
            sourceContent = sourceContent.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            output.Write(sourceContent);
            RefreshData();
        }

        /// <summary>
        /// Обновление списка документов
        /// </summary>
        public void RefreshData()
        {
            var w = new StringWriter();
            RenderLinkedDocsInfo(w);
            V4Page.JS.Write("v4_fixedHeaderDestroy();");
            V4Page.JS.Write("$('#{0}').html('{1}');", ID, HttpUtility.JavaScriptStringEncode(w.ToString()));
            V4Page.JS.Write("setTimeout(v4_fixedHeader,50);");
            
            V4Page.JS.Write(@"lb_clientLocalization = {{
                ok_button:""{0}"",
                cancel_button:""{1}"" 
            }};",
                Resx.GetString("cmdApply"),
                Resx.GetString("cmdCancel")
            );
            
            V4Page.RestoreCursor();
        }

        /// <summary>
        /// Получение списка документов
        /// </summary>
        /// <param name="w"></param>
        public void RenderLinkedDocsInfo(TextWriter w)
        {
            if (V4Page.Doc.IsNew) return;

            var dt = V4Page.GetSettingsLinkedDocsInfo();
            if (dt.Rows.Count == 0) return;

            var dv = new DataView();
            dv.Table = dt;
            dv.Sort = "ПорядокВыводаВОсновании";
            dv.RowFilter = "КодТипаДокументаОснования=" + V4Page.Doc.TypeID;
            GetLinkedDocsInfo(w, dv);
        }

        /// <summary>
        /// Обработка списка документов
        /// </summary>
        /// <param name="w"></param>
        /// <param name="dv"></param>
        private void GetLinkedDocsInfo(TextWriter w, DataView dv)
        {
            var dt = Document.LoadSequelDocs(V4Page.Doc.Id);
            for (var i = 0; i < dv.Count; i++)
            {
                var dtable =  dt.Clone();
                var query = dt.AsEnumerable().Where(dr => dr.Field<Int32>("КодТипаДокумента").Equals(dv[i]["КодТипаДокументаВытекающего"]));
                query.CopyToDataTable(dtable, LoadOption.OverwriteChanges);

                RenderLinkedDocsInfoDetails(w, dtable,
                    Resx.GetString("listFollowType"),
                    Resx.GetString("oneFollowType"),
                    dv[i]["КодТипаДокументаВытекающего"].ToString(), dv[i]["ТипСвязи"].ToString(),
                    dv[i]["КодПоляДокумента"].ToString(),
                    dv[i]["ПолеДокумента"].ToString(), dv[i]["ПолеДокументаEn"].ToString());
                
            }
        }

        /// <summary>
        /// Отрисовка списка документов
        /// </summary>
        /// <param name="w"></param>
        /// <param name="dt"></param>
        /// <param name="_title"></param>
        /// <param name="_oneTitle"></param>
        /// <param name="_type"></param>
        /// <param name="_linkType"></param>
        /// <param name="fieldId"></param>
        /// <param name="field"></param>
        /// <param name="fieldEn"></param>
        private void RenderLinkedDocsInfoDetails(TextWriter w, DataTable dt, string _title, string _oneTitle,
            string _type, string _linkType, string fieldId, string field, string fieldEn)
        {
            var dvSys = new DataView();
            dvSys.Table = dt;

            var dv = new DataView();
            dv.Table = dt;
            dv.Sort = "КодРесурса1, ДатаДокумента";

            Document d = null;
            //Currency cur = null;

            var curSml = "&nbsp;";
            var curScale = 2;
            decimal sum = 0;

            var dtp = V4Page.GetObjectById(typeof(DocType), _type) as DocType; 

            if (!dtp.Unavailable)
            {
                w.Write("<table border=0 style='BORDER-COLLAPSE:collapse;'>");
                w.Write("<tr>");
                w.Write("<td {0}>", (dv.Count == 0 ? "style=\"padding-bottom:15px;\"" : ""));
                if (_linkType == "21" || _linkType == "11") w.Write(_oneTitle);
                else w.Write(_title);
                w.Write(" \"" + (V4Page.IsRusLocal ? dtp.Name : dtp.TypeDocEn) + "\"");
                w.Write(" (" + Resx.GetString("LBL_ПоПолю") + " \"" + (V4Page.IsRusLocal ? field : fieldEn) + "\")");

                // ссылку на создание (связывание с документом) выводим в случае, если тип связи
                //    (типы связи 12, 22 - означают, что может быть несколько вытекающих; 11, 21 - только 1 вытекающий)
                //    и количество уже связанных документов это допускают 
                if (_linkType == "12" || _linkType == "22" || dv.Count == 0)
                {
                    // для допускающих несколько вытекающих документов - выводим диалог (выбор или привязка к существующему)
                    if (_linkType == "12" || _linkType == "22")
                    {
                        var col = new StringCollection();
                        foreach (DataRow dr in dt.Rows)
                            col.Add(dr["КодДокумента"].ToString());

                        w.Write(
                            " <a href=\"#\" onclick=\"return false;\"><img style=\"border:0;cursor:pointer;\" border=\"0\" src=\"/styles/new.gif\" title=\"{0}\"",
                            Resx.GetString("listNewDoc"));

                        w.Write(
                            " onclick=\"cmd('cmd', 'Listener', 'ctrlId', {0}, 'cmdName', 'OpenLinkedDoc', 'type', '{1}', 'linkType', {2}, 'field', '{3}', 'linkedDocs', '{4}')\"/></a>",
                            LinkedDocCmdListnerIndex, _type, _linkType, fieldId, Collection2Str(col));
                    }
                    // для допускающих 1 вытекающий - сразу открываем создание нового документа на основе текущего
                    else
                    {
                        var url = dtp.URL + (dtp.URL.IndexOf("?", StringComparison.Ordinal) > 0 ? "&" : "?") + "DocId=" +
                                  V4Page.Doc.Id + "&fieldId=" + fieldId;

                        w.Write(
                            " <a href=\"#\" onclick=\"return false;\"><img style=\"border:0;cursor:pointer;\" border=\"0\" src=\"/styles/new.gif\" title=\"{0}\"",
                            Resx.GetString("listNewDoc"));
                        w.Write(" onclick=\"v4_windowOpen('{0}');\"/></a>",url);
                    }
                }
                w.Write(":</td>");
                w.Write("</tr>");
                w.Write("</table>");
            }

            if (dv.Count == 0) return;
            w.Write("<table border=0 style='BORDER-COLLAPSE:collapse;padding-bottom:15px;'>");
            for (var i = 0; i < dv.Count; i++)
            {
                d = V4Page.GetObjectById(typeof(Document), dv[i]["КодДокумента"].ToString()) as Document; 

                if (d.Unavailable) continue;
                var res = d.DocumentData.ResourceId1;
                if (res.HasValue && res > 0)
                {
                    //todo
                    //cur = Currency.GetCurrency(res.Value);
                    //cur._PersonID = doc._Person1;
                    //if (cur.Unavailable || cur.ParentOf(V2.Resources.Resource._Money)) curSml = "&nbsp;";
                    //else
                    //{
                    //    curSml = cur.CurrencySymbol;
                    //    curScale = int.Parse(cur._UnitScale);
                    //}
                }
                else
                    curSml = "&nbsp;";

                w.Write("<tr>");
                w.Write("<td style='PADDING-LEFT:10px;' valign='top'>");
                
                V4Page.RenderLinkDoc(w, d.Id);

                w.Write("<img src='/styles/DocMain.gif' border=0>");
                V4Page.RenderLinkEnd(w);
                w.Write("</td>");
                w.Write("<td noWrap valign='top'>");

                V4Page.RenderLinkDoc(w, d.Id);

                w.Write((d.Number.Length > 0) ? Resx.GetString("lN") + " " + d.Number + " " : "");
                if (d.Date != DateTime.MinValue) w.Write(Resx.GetString("lD") + " " + d.Date.ToString("dd.MM.yyyy"));
                V4Page.RenderLinkEnd(w);
                w.Write("</td>");
                w.Write("<td noWrap valign='top'style='PADDING-LEFT:5px;'>");
                if (d.Finished) w.Write(Resx.GetString("listComplete"));
                else if (d.Signed) w.Write(Resx.GetString("listSigned"));
                else w.Write(Resx.GetString("listNotSigned"));
                w.Write("</td>");
                w.Write("<td align='right' noWrap valign='top' style='PADDING-LEFT:5px;'>");
                if (d.DocumentData.Money1.HasValue && d.DocumentData.Money1 > 0)
                    V4Page.RenderNumber(w, d.DocumentData.Money1.Value.ToString(), curScale);
                else
                    w.Write("&nbsp;");
                w.Write("</td>");
                w.Write("<td valign='top'>");
                w.Write(curSml);
                w.Write("</td>");
                w.Write("<td valign='top' style='PADDING-LEFT:5px;'>");
                w.Write(d.Description.Length == 0 ? "&nbsp;" : d.Description);
                w.Write("</td>");
                w.Write("</tr>");

                if (!dv[i]["КодРесурса1"].Equals(DBNull.Value) &&
                    ((i < dv.Count - 1 && !dv[i + 1]["КодРесурса1"].Equals(dv[i]["КодРесурса1"]))
                     || i == dv.Count - 1))
                {
                    dvSys.RowFilter = "КодРесурса1=" + dv[i]["КодРесурса1"];
                    if (dvSys.Count > 1)
                    {
                        sum = Convert.ToDecimal(dt.Compute("SUM(Money1)", "КодРесурса1=" + dv[i]["КодРесурса1"]));
                        w.Write("<tr>");
                        w.Write("<td colspan=3 align='right'>");
                        w.Write(Resx.GetString("listTotal") + ":");
                        w.Write("</td>");
                        w.Write("<td align='right' noWrap style='PADDING-LEFT:5px;'>");
                        V4Page.RenderNumber(w, ConvertExtention.Convert.Decimal2Str(sum, curScale), curScale);
                        w.Write("</td>");
                        w.Write("<td>");
                        w.Write(curSml);
                        w.Write("</td>");
                        w.Write("<td>");
                        w.Write("&nbsp;");
                        w.Write("</td>");
                        w.Write("</tr>");
                    }
                }
                curScale = 2;
                curSml = "&nbsp;";
            }

            w.Write("</table>");
        }

        /// <summary>
        ///     Преобразование коллекции к строке с разделителем ','
        /// </summary>
        public static string Collection2Str(StringCollection col)
        {
            return ConvertExtention.Convert.Collection2Str(col);
        }

        /// <summary>
        ///     Преобразование коллекции к строке с разделителем ','
        /// </summary>
        public static string Collection2Str(IEnumerable<string> col)
        {
            return ConvertExtention.Convert.Collection2Str(col);
        }

        /// <summary>
        /// Проверка соответствия выбранного документа ограничениям по типу связи
        /// </summary>
        private void LinkedDocumentOnOnRenderNtf(object sender, Ntf ntf)
        {
            if (!string.IsNullOrEmpty(_docValue))
            {
                ntf.Clear();
                // Проверка выбранного документа на подписанность
                Document akt = new Document(_docValue);
                if (akt.Signed)
                    ntf.Add(Resx.GetString("signedAkt"));
                string linkType = "";
                if (_linkType != null) linkType = _linkType;

                // В данном случае требуется выполнить проверку, исключающую выбранный документ в случае,
                // если он уже является вытекающим из другого документа по этому же полю
                if (linkType == "11" || linkType == "12")
                {
                    string field;
                    if (_field != null) field = _field;
                    else return;

                    if (Document.CheckLoadSequelDoc(_docValue, field))
                        ntf.Add(Resx.GetString("alertAlreadyLinked"));
                }
            }
        }

        /// <summary>
        ///  Событие изменения 
        /// </summary>
        private void LinkedDocumentOnValueChanged(object sender, ValueChangedEventArgs e)
        {
            _docValue = e.NewValue;
        }

    }
}
