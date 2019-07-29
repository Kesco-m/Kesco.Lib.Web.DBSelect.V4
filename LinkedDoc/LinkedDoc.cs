using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kesco.Lib.BaseExtention;
using Kesco.Lib.BaseExtention.Enums.Controls;
using Kesco.Lib.BaseExtention.Enums.Docs;
using Kesco.Lib.Entities;
using Kesco.Lib.Entities.Documents;
using Kesco.Lib.Web.Controls.V4;
using Kesco.Lib.Web.Controls.V4.Common;
using Convert = Kesco.Lib.ConvertExtention.Convert;
using DropDownList = Kesco.Lib.Web.Controls.V4.DropDownList;
using Item = Kesco.Lib.Web.Controls.V4.Item;
using Page = Kesco.Lib.Web.Controls.V4.Common.DocumentPage.DocPage;

namespace Kesco.Lib.Web.DBSelect.V4.LinkedDoc
{
    /// <summary>
    ///     Вытекающие документы
    /// </summary>
    public class LinkedDoc : V4Control, IClientCommandProcessor
    {
        private DBSDocument _currentDbSelectCtrl;

        private Radio _currentRadioCtrl;
        private DropDownList _currentTypeSelectCtrl;
        private Label _currentTypeSelectCtrlText;
        private List<DocumType> _documTypeList;

        private string _docValue;

        private DataTable _dtSequelTypes;
        private string _field;
        private string _linkedDocs;
        private string _linkType;
        private string _type;

        /// <summary>
        ///     Вытекающие документы
        /// </summary>
        protected int LinkedDocCmdListnerIndex;

        /// <summary>
        ///     Акцессор V4Page
        /// </summary>
        public new Page V4Page
        {
            get { return Page as Page; }
            set { Page = value; }
        }

        /// <summary>
        ///     По-умолчанию подставляемый тип вытекающего
        /// </summary>
        public int DefaultLinkedDocType { get; set; }

        /// <summary>
        ///     Текущий тип документа
        /// </summary>
        public int CurrentDocType { get; set; }

        /// <summary>
        ///     Обработка клиентских команд
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

                    _currentRadioCtrl.Value = "0";
                    _currentRadioCtrl.Flush();
                    _currentDbSelectCtrl.Value = null;
                    _currentDbSelectCtrl.IsDisabled = true;

                    JS.Write("dialogShow('{0}', '{1}');", Resx.GetString("title"), LinkedDocCmdListnerIndex);
                    break;
                case "OK":
                    if (_type == "")
                    {
                        V4Page.ShowMessage($"{V4Page.Resx.GetString("LinkedDocs_lbl_УкажитеТип")}",
                            _currentTypeSelectCtrl, V4Page.Resx.GetString("CONFIRM_StdTitle"));
                        return;
                    }

                    var url = "";
                    var bAdd = _currentRadioCtrl.Value == "0";
                    var dtp = new DocType(_type);

                    if (bAdd)
                        // создаем новый документ
                        url = dtp.URL + (dtp.URL.IndexOf("?", StringComparison.Ordinal) > 0 ? "&" : "?") + "DocId=" +
                              V4Page.Doc.Id + "&fieldId=" + _field;
                    else
                        // связываем существующий документ
                        url = dtp.URL + (dtp.URL.IndexOf("?", StringComparison.Ordinal) > 0 ? "&" : "?") +
                              "DocId=" + V4Page.Doc.Id + "&Id=" + _currentDbSelectCtrl.Value + "&fieldId=" + _field;

                    if (url != "")
                    {
                        JS.Write("dialogShow.form.dialog('close');");
                        JS.Write(
                            "win=window.open('{0}','_blank','status=no,toolbar=no,menubar=no,location=no,resizable=yes,scrollbars=yes');",
                            url);
                    }

                    _currentTypeSelectCtrl.Value = "";
                    _type = "";
                    break;
            }
        }

        /// <summary>
        ///     Инициализация
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            if (!V4Page.Listeners.Contains(this)) V4Page.Listeners.Add(this);
            LinkedDocCmdListnerIndex = V4Page.Listeners.IndexOf(this);

            base.OnInit(e);

            if (V4Page.V4IsPostBack) return;

            _currentTypeSelectCtrlText = new Label
                {Text = $"{V4Page.Resx.GetString("LinkedDocs_lbl_ТипВытекающего")}:"};

            _currentTypeSelectCtrl = new DropDownList
            {
                V4Page = V4Page,
                ID = "type_" + ID,
                Width = new Unit("350px"),
                IsReadOnly = true
            };

            _currentTypeSelectCtrl.Changed += TypeChanged;

            V4Page.V4Controls.Add(_currentTypeSelectCtrl);
            _dtSequelTypes = DocType.GetSettingsLinkedDocsInfo(CurrentDocType);
            LoadDropDownListData();
            if (DefaultLinkedDocType > 0 &&
                _currentTypeSelectCtrl.DataItems.ContainsKey(DefaultLinkedDocType.ToString()))
            {
                _currentTypeSelectCtrl.Value = _type = DefaultLinkedDocType.ToString();
                var documType = _documTypeList.Find(l => l.Type == _type);
                _field = documType.FieldId;
                _linkType = documType.LinkType;
            }

            _currentRadioCtrl = new Radio
            {
                V4Page = V4Page,
                ID = "radio_" + ID,
                IsRow = false,
                Name = "DocRadio",
                HtmlID = "radio_" + ID
            };
            _currentRadioCtrl.Changed += _currentRadioCtrl_OnChanged;
            _currentRadioCtrl.Items.Add(new Item("0", $" {V4Page.Resx.GetString("LinkedDocs_lbl_НовыйВытекающий")}"));
            _currentRadioCtrl.Items.Add(new Item("1", $" {V4Page.Resx.GetString("LinkedDocs_lbl_Существующий")}"));
            _currentRadioCtrl.Value = "0";
            V4Page.V4Controls.Add(_currentRadioCtrl);

            _currentDbSelectCtrl = new DBSDocument
            {
                V4Page = V4Page,
                ID = "dbsDocument_" + ID,
                HtmlID = "linkedDoc",
                Width = new Unit("350px")
            };

            _currentDbSelectCtrl.OnRenderNtf += LinkedDocumentOnOnRenderNtf;
            _currentDbSelectCtrl.ValueChanged += LinkedDocumentOnValueChanged;
            _currentDbSelectCtrl.BeforeSearch += DBSelect_BeforeSearch;
            _currentDbSelectCtrl.IsDisabled = true;


            V4Page.V4Controls.Add(_currentDbSelectCtrl);
        }


        internal void TypeChanged(object sender, ProperyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewValue))
            {
                var _documType = _documTypeList.Find(l => l.Type == e.NewValue);
                _type = _documType.Type;
                _field = _documType.FieldId;
                _linkType = _documType.LinkType;


                if (_dtSequelTypes.Rows.Count == 0) return;

                var dv = new DataView
                {
                    Table = _dtSequelTypes,
                    Sort = "ПорядокВыводаВОсновании",
                    RowFilter = "КодТипаДокументаОснования=" + CurrentDocType
                };

                var dtSd = Document.LoadSequelDocs(V4Page.Doc.Id);
                for (var i = 0; i < dv.Count; i++)
                {
                    var dtable = dtSd.Clone();
                    var query = dtSd.AsEnumerable().Where(dr =>
                        dr.Field<int>("КодТипаДокумента").Equals(dv[i]["КодТипаДокументаВытекающего"]));
                    query.CopyToDataTable(dtable, LoadOption.OverwriteChanges);

                    if (dtable.Rows.Count == 0) continue;

                    var dtp = V4Page.GetObjectById(typeof(DocType), _type) as DocType;
                    _linkedDocs = "";
                    if (!dtp.Unavailable)
                    {
                        _currentRadioCtrl.IsDisabled = _linkType != "12" && _linkType != "22";
                        if (_linkType == "12" || _linkType == "22")
                        {
                            var col = new StringCollection();
                            foreach (DataRow dr in dtable.Rows)
                                col.Add(dr["КодДокумента"].ToString());

                            _linkedDocs = Collection2Str(col);
                        }
                    }
                }

                _currentRadioCtrl.Value = "0";
                _currentRadioCtrl.Flush();
                _currentDbSelectCtrl.Value = null;
                _currentDbSelectCtrl.IsDisabled = true;
            }
        }

        /// <summary>
        ///     Событие, устанавливающее параметры фильтрации
        /// </summary>
        /// <param name="sender">Контрол</param>
        private void DBSelect_BeforeSearch(object sender)
        {
            _currentDbSelectCtrl.Filter.Type.Clear();
            if (!string.IsNullOrEmpty(_type)) _currentDbSelectCtrl.Filter.Type.Add(_type, DocTypeQueryType.Equals);

            _currentDbSelectCtrl.Filter.Date.Clear();
            if (!string.IsNullOrEmpty(V4Page.Doc.Date.ToString("yyyyMMdd")))
            {
                _currentDbSelectCtrl.Filter.Date.DateSearchType = DateSearchType.MoreThan;
                _currentDbSelectCtrl.Filter.Date.Add(V4Page.Doc.Date.ToString("yyyyMMdd"));
            }

            _currentDbSelectCtrl.Filter.IDs.Clear();
            if (!string.IsNullOrEmpty(_linkedDocs))
            {
                var col = Convert.Str2Collection(_linkedDocs);
                foreach (var id in col)
                    _currentDbSelectCtrl.Filter.IDs.Add(id);

                if (col.Count > 0)
                    _currentDbSelectCtrl.Filter.IDs.Inverse = true;
            }
        }

        /// <summary>
        ///     Обработка изменения позиции выбора действия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _currentRadioCtrl_OnChanged(object sender, ProperyChangedEventArgs e)
        {
            _currentDbSelectCtrl.IsDisabled = e.NewValue == "0";
            if (_currentDbSelectCtrl.IsDisabled)
                _currentDbSelectCtrl.Value = _docValue = null;
        }


        private void LoadDropDownListData()
        {
            if (_dtSequelTypes.Rows.Count != 0)
            {
                var da = new Dictionary<string, object>();
                var dv = new DataView
                {
                    Table = _dtSequelTypes,
                    Sort = "ПорядокВыводаВОсновании",
                    RowFilter = "КодТипаДокументаОснования=" + CurrentDocType
                };

                _documTypeList = new List<DocumType>();
                da.Add("", "");
                for (var i = 0; i < dv.Count; i++)
                {
                    _documTypeList.Add(new DocumType
                    {
                        Type = dv[i]["КодТипаДокументаВытекающего"].ToString(),
                        LinkType = dv[i]["ТипСвязи"].ToString(),
                        Field = dv[i]["ПолеДокумента"].ToString(),
                        FieldId = dv[i]["КодПоляДокумента"].ToString()
                    });

                    da.Add(dv[i]["КодТипаДокументаВытекающего"].ToString(),
                        dv[i]["ТипДокументаВытекающего"].ToString());
                }

                _currentTypeSelectCtrl.DataItems = da;
            }
        }

        /// <summary>
        ///     Отрисовка контрола
        /// </summary>
        /// <param name="output"></param>
        public override void RenderControl(HtmlTextWriter output)
        {
            var currentAsm = Assembly.GetExecutingAssembly();
            var linkedDocContent =
                currentAsm.GetManifestResourceStream("Kesco.Lib.Web.DBSelect.V4.LinkedDoc.LinkedDocContent.htm");
            if (linkedDocContent == null) return;
            var reader = new StreamReader(linkedDocContent);
            var sourceContent = reader.ReadToEnd();

            sourceContent = sourceContent.Replace(_constIdTag, ID);

            if (_currentTypeSelectCtrl != null)
            {
                if (V4Page.V4IsPostBack)
                    LoadDropDownListData();

                using (TextWriter currentTypeSelectTextWriter = new StringWriter())
                {
                    var currentWriter = new HtmlTextWriter(currentTypeSelectTextWriter);
                    _currentTypeSelectCtrlText.RenderControl(currentWriter);
                    _currentTypeSelectCtrl.RenderControl(currentWriter);
                    sourceContent = sourceContent.Replace(_constCtrlTypeSelect, currentTypeSelectTextWriter.ToString());
                }
            }


            using (TextWriter currentRadioTextWriter = new StringWriter())
            {
                var currentWriter = new HtmlTextWriter(currentRadioTextWriter);
                _currentRadioCtrl.RenderControl(currentWriter);
                sourceContent = sourceContent.Replace(_constCtrlRadio, currentRadioTextWriter.ToString());
            }

            if (_currentDbSelectCtrl != null)
                using (TextWriter currentDBSelectTextWriter = new StringWriter())
                {
                    var currentWriter = new HtmlTextWriter(currentDBSelectTextWriter);
                    _currentDbSelectCtrl.RenderControl(currentWriter);
                    sourceContent = sourceContent.Replace(_constCtrlDBSelect, currentDBSelectTextWriter.ToString());
                }

            sourceContent = sourceContent.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            output.Write(sourceContent);
            RefreshData();
        }

        /// <summary>
        ///     Обновление списка документов
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
        ///     Получение списка документов
        /// </summary>
        /// <param name="w"></param>
        public void RenderLinkedDocsInfo(TextWriter w)
        {
            if (V4Page.Doc.IsNew || V4Page.Doc.Unavailable || V4Page.Doc.DataUnavailable) return;


            if (_dtSequelTypes.Rows.Count == 0) return;

            var dv = new DataView();
            dv.Table = _dtSequelTypes;
            dv.Sort = "ПорядокВыводаВОсновании";
            dv.RowFilter = "КодТипаДокументаОснования=" + CurrentDocType;
            GetLinkedDocsInfo(w, dv);
        }

        /// <summary>
        ///     Обработка списка документов
        /// </summary>
        /// <param name="w"></param>
        /// <param name="dv"></param>
        private void GetLinkedDocsInfo(TextWriter w, DataView dv)
        {
            if (V4Page.Doc.IsNew || V4Page.Doc.Unavailable || V4Page.Doc.DataUnavailable) return;

            var dt = Document.LoadSequelDocs(V4Page.Doc.Id);

            if (dv.Count > 0)
            {
                w.Write(
                    "<img id=\"img_doc\" style=\"border:0;cursor:pointer;\" border=\"0\" src=\"/styles/new.gif\" title=\"{0}\" ",
                    Resx.GetString("listNewDoc"));
                w.Write("onclick=\"cmd('cmd', 'Listener', 'ctrlId', {0}, 'cmdName', 'OpenLinkedDoc')\"",
                    LinkedDocCmdListnerIndex);
                w.Write("/>&nbsp;{0}:", Resx.GetString("listFollowType"));
            }

            for (var i = 0; i < dv.Count; i++)
            {
                var dtable = dt.Clone();
                var query = dt.AsEnumerable().Where(dr =>
                    dr.Field<int>("КодТипаДокумента").Equals(dv[i]["КодТипаДокументаВытекающего"]));
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
        ///     Отрисовка списка документов
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

            if (dv.Count == 0) return;

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
                w.Write("<td {0}>", dv.Count == 0 ? "style=\"padding-bottom:15px;\"" : "");

                w.Write(" \"" + (V4Page.IsRusLocal ? dtp.Name : dtp.TypeDocEn) + "\"");
                w.Write(" (" + Resx.GetString("LBL_ПоПолю") + " \"" + (V4Page.IsRusLocal ? field : fieldEn) + "\")");


                w.Write(":</td>");
                w.Write("</tr>");
                w.Write("</table>");
            }

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
                {
                    curSml = "&nbsp;";
                }

                w.Write("<tr>");
                w.Write("<td style='PADDING-LEFT:10px;' valign='top'>");

                V4Page.RenderLinkDoc(w, d.Id);
                w.Write("<img src='/styles/DocMain.gif' border=0>");
                V4Page.RenderLinkEnd(w);
                w.Write("</td>");
                w.Write("<td noWrap valign='top'>");

                V4Page.RenderLinkDoc(w, d.Id);

                w.Write(d.Number.Length > 0 ? Resx.GetString("lN") + " " + d.Number + " " : "");
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
                    (i < dv.Count - 1 && !dv[i + 1]["КодРесурса1"].Equals(dv[i]["КодРесурса1"])
                     || i == dv.Count - 1))
                {
                    dvSys.RowFilter = "КодРесурса1=" + dv[i]["КодРесурса1"];
                    if (dvSys.Count > 1)
                    {
                        sum = System.Convert.ToDecimal(dt.Compute("SUM(Money1)",
                            "КодРесурса1=" + dv[i]["КодРесурса1"]));
                        w.Write("<tr>");
                        w.Write("<td colspan=3 align='right'>");
                        w.Write(Resx.GetString("listTotal") + ":");
                        w.Write("</td>");
                        w.Write("<td align='right' noWrap style='PADDING-LEFT:5px;'>");
                        V4Page.RenderNumber(w, Convert.Decimal2Str(sum, curScale), curScale);
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
            return Convert.Collection2Str(col);
        }

        /// <summary>
        ///     Преобразование коллекции к строке с разделителем ','
        /// </summary>
        public static string Collection2Str(IEnumerable<string> col)
        {
            return Convert.Collection2Str(col);
        }

        /// <summary>
        ///     Проверка соответствия выбранного документа ограничениям по типу связи
        /// </summary>
        private void LinkedDocumentOnOnRenderNtf(object sender, Ntf ntf)
        {
            if (!string.IsNullOrEmpty(_docValue))
            {
                ntf.Clear();
                // Проверка выбранного документа на подписанность
                var akt = new Document(_docValue);
                if (akt.Signed)
                    ntf.Add(new Notification
                    {
                        Message = Resx.GetString("signedAkt"),
                        Status = NtfStatus.Error,
                        SizeIsNtf = true,
                        DashSpace = true
                    });
                var linkType = "";
                if (_linkType != null) linkType = _linkType;

                // В данном случае требуется выполнить проверку, исключающую выбранный документ в случае,
                // если он уже является вытекающим из другого документа по этому же полю
                if (linkType == "11" || linkType == "12")
                {
                    string field;
                    if (_field != null) field = _field;
                    else return;

                    if (Document.CheckLoadSequelDoc(_docValue, field))
                        ntf.Add(new Notification
                        {
                            Message = Resx.GetString("alertAlreadyLinked"),
                            Status = NtfStatus.Error,
                            SizeIsNtf = true,
                            DashSpace = true
                        });
                }
            }
        }

        /// <summary>
        ///     Событие изменения
        /// </summary>
        private void LinkedDocumentOnValueChanged(object sender, ValueChangedEventArgs e)
        {
            _docValue = e.NewValue;
        }

        #region Constants

        private const string _constIdTag = "[CID]";
        private const string _constCtrlRadio = "[C_RADIO]";
        private const string _constCtrlDBSelect = "[C_DBSELECT]";
        private const string _constCtrlTypeSelect = "[C_TYPESELECT]";

        #endregion
    }

    internal class DocumType
    {
        internal string Type { get; set; }
        internal string Field { get; set; }
        internal string FieldId { get; set; }
        internal string LinkType { get; set; }
        internal string LinkedDocs { get; set; }
    }
}