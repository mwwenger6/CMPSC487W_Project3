using CMPSC487W_Project3.Entities.Forms;
using CMPSC487W_Project3.Entities.Forms.Elements;
using CMPSC487W_Project3.Entities.Forms.Inputs;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMPSC487W_Project3.Entities.Forms
{
    public interface IModal
    {
        public string ObjectName { get; set; }

        public const string ADD = "add";
        //public const string SUCCESS = "add";
        public const string DELETE = "delete";

        public IHtmlContent BeginHtml();
        public IHtmlContent GetPagesHtml(IHtmlHelper html, int startIndex = 0);
        public IHtmlContent GetPagesHtml(IHtmlHelper html, int startIndex, int endIndex);
        public IHtmlContent EndHtml();

        public static IHtmlContent ModalContainer(string modalPrefix, string url, bool staticBackdrop = false)
        {
            HtmlConcat htmlConcat = new();
            htmlConcat.Add($"<div class=\"modal fade\" href=\"{url}\" id=\"{modalPrefix}Modal\" ");
            if (staticBackdrop)
                htmlConcat.Add($"data-bs-backdrop=\"static\" ");
            htmlConcat.Add($"tabindex=\"-1\" aria-labelledby=\"{modalPrefix}ModalLabel\" aria-hidden=\"true\"></div>");
            return htmlConcat.Concatenate();
        }
    }

    public class Modal<T> : IModal
    {
        /// <summary>
        /// Creates a new instance of a modal with inputs and pages
        /// </summary>
        /// <param name="title">Modal title text</param>
        /// <param name="objName">object name used in methods</param>
        /*public Modal(string title, string objName, T model, string submitBtnText)
        {
            Title = title;
            ObjectName = objName;
            Model = model;
            //SubmitBtnText = submitBtnText;
            Type = IModal.ADD;
            SetDefaults(submitBtnText);
        }*/
        public Modal(string title, string objName, T model, string submitBtnText, string modalType)
        {
            Title = title;
            ObjectName = objName;
            Model = model;
            Type = modalType;
            SetDefaults(submitBtnText);
        }
        private void SetDefaults(string submitBtnText)
        {
            Pages = new();
            Footer = new(submitBtnText, ObjectName, Type);
            ActivePageIndex = 0;
            //Layout = IModalPage.PADDED;
        }
        public T Model { get; set; }
        public IHtmlHelper<T> Html { get; set; }
        //public int Layout { get; set; }
        private string Title { get; set; }
        //private string Id { get; set; }
        private string Type { get; set; }
        private string FormAction { get; set; }
        private int ActivePageIndex { get; set; }

        public ModalPage<T> ActivePage { get { return Pages[ActivePageIndex]; } private set { } }
        public string ObjectName { get; set; }
        public List<ModalPage<T>> Pages { get; set; }
        public bool VerticallyCentered { get; set; }
        public ModalFooter Footer { get; set; }

        public Modal<T> AddPages(params ModalPage<T>[] pages)
        {
            foreach (ModalPage<T> page in pages)
            {
                page.PageNumber = Pages.Count;
                Pages.Add(page);
            }
            return this;
        }
        public Modal<T> SetFooterButtons(List<Button> buttons)
        {
            return this;
        }
        public Modal<T> SetFooterNote(string noteText, string @class = null)
        {
            Footer.SetNote(noteText, @class);
            return this;
        }
        /* public Modal<T> UseLayout(int modalpageLayout)
         {
             Layout = modalpageLayout;
             return this;
         }*/
        public Modal<T> UseFormAction(string formAction)
        {
            FormAction = formAction;
            return this;
        }
        public Modal<T> SetVerticallyCentered(bool value)
        {
            VerticallyCentered = value;
            return this;
        }


        public IHtmlContent GetHtml(IHtmlHelper<T> html)
        {
            return new HtmlConcat()
                .Add(BeginHtml())
                .Add(GetPagesHtml(html))
                .Add(EndHtml())
                .Concatenate();
        }
        public IHtmlContent BeginHtml()
        {
            string verticallyCentered = VerticallyCentered ? "modal-dialog-centered" : null;
            return new HtmlString($"<div class=\"modal-dialog modal-lg {verticallyCentered}\">" +
                "<div class=\"modal-content\">" +
                    GetHeaderHtml() +
                    "<div class=\"modal-body\">" +
                        GetTabsHtml() +
                        $"<form id=\"{Type}{ObjectName}Form\" method=\"post\" " +
                            $"action=\"{FormAction}\" enctype=\"multipart/form-data\">");// enctype to support upload files via a form
        }

        private IHtmlContent GetHeaderHtml()
        {
            HtmlConcat htmlConcat = new();
            htmlConcat.Add("<div class=\"modal-header\">")
                .Add($"<h5 class=\"modal-title\" id=\"{Type}{ObjectName}ModalLabel\">{Title}</h5>")
                .Add(new Button(string.Empty, null, "close", isDismiss: true).Render())
            .Add($"</div>");
            return htmlConcat.Concatenate();
        }

        public IHtmlContent GetTabsHtml()
        {
            if (Pages.Count <= 1)
                return new HtmlString(string.Empty);

            string content = "<ul id=\"tab-strip\" class=\"nav nav-tabs mb-3\" role=\"tablist\">";
            foreach (ModalPage<T> page in Pages)
            {
                string active = page == ActivePage ? "active" : string.Empty;
                string disabled = page.IsDisabled ? "disabled" : string.Empty;
                content += $"<li class=\"modal-page-tab\" tabindex=\"{page.PageNumber}\">" +
                    $"<a class=\"{active} {disabled} nav-link\" {page.GetTriggerAttrs()}>{page.Title}</a></li>";
            }
            content += "</ul>";
            return new HtmlString(content);
        }
        public IHtmlContent GetPagesHtml(IHtmlHelper html, int startIndex = 0)
        { return GetPagesHtml(html, startIndex, Pages.Count); }
        public IHtmlContent GetPagesHtml(IHtmlHelper html, int startIndex, int endIndex)
        {
            HtmlConcat htmlConcat = new();
            for (int i = startIndex; i < endIndex; i++)
                htmlConcat.Add(Pages[i].GetHtml(html, Model, Pages[i] == ActivePage, i));
            //htmlConcat.Add(Pages[i].GetHtml(html, Model, Layout, Pages[i] == ActivePage, i));

            return htmlConcat.Concatenate();
        }

        public IHtmlContent GetJavaScript()
        {
            HtmlConcat htmlConcat = new();
            /* foreach (ModalPage<T> page in Pages.Where(p => p.TriggeredBy != null))
             {
                 string functionName = $"shouldDisable{ObjectName}{page.Title}".Replace(" ", null),
                     length = page.LengthOf ? ".length" : null;
                 htmlConcat.Add($"function {functionName}(tab){{" +
                     $"if ($('#{page.TriggeredBy}').val(){length}{page.TriggerOperator}'{page.TriggerValue}') tab.removeClass(\"disabled\");" +
                     //$"if ($('#{page.TriggeredBy}').val(){length}{page.TriggerOperator}tab.attr(\"triggerval\")) tab.removeClass(\"disabled\");" +
                     "else tab.addClass(\"disabled\");" +
                 "};");
             }*/

            htmlConcat.Add("$(document).ready(function(){" +
                "$('.modal-page-tab .nav-link').on('click',function(){" +
                    "$('.modal-page,.modal-page-tab>a').removeClass('active');" +
                    "$(`.modal-page[data-title='${$(this).text()}']`).add(this).addClass('active');" +
                "});");

            // triggered by section
            foreach (ModalPage<T> page in Pages.Where(p => p.TriggeredBy != null))
            {
                string //functionName = $"shouldDisable{ObjectName}{page.Title}".Replace(" ", null),
                    length = page.LengthOf ? ".length" : null;
                htmlConcat.Add("$('.modal-page-tab .nav-link').each(function(){" +
                    $"let triggeredBy='{page.TriggeredBy}', tab=$(this);" +
                    "if (typeof triggeredBy!=='undefined' && triggeredBy !== false){" +
                        //$"$('#{page.TriggeredBy}').on('change',function(){{{functionName}(tab)}}).trigger('change');" +
                        $"$('#{page.TriggeredBy}').on('change',function(){{" +
                            $"if ($('#{page.TriggeredBy}').val(){length}{page.TriggerOperator}'{page.TriggerValue}') tab.removeClass('disabled');" +
                            "else tab.addClass('disabled');" +
                            $"console.log($('#{page.TriggeredBy}').val(){length}{page.TriggerOperator}'{page.TriggerValue}')" +
                        $"}}).trigger('change');" +
                    "}" +
                "});");
            }
            htmlConcat.Add("});");

            return htmlConcat.Concatenate();
        }
        public IHtmlContent EndHtml()
        {
            HtmlConcat htmlConcat = new();
            htmlConcat.Add(
                        "</form>" +
                    "</div>" +
                    Footer.GetHtml() +
                "</div>" +
            "</div>");
            if (Pages.Count > 1)// will need changed if modals ever have non page related JS
                htmlConcat.Add($"<script>{GetJavaScript()}</script>");
            return htmlConcat.Concatenate();
        }
    }
}