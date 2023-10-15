using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMPSC487W_Project2.Entities.Forms
{
    public interface IModalPage : IContainer
    {
        //public const int PADDED = 1;
        //public const int COLUMN = 2;

        public string Title { get; set; }
    }
    public class Operator
    {
        public Operator(string value) { Value = value; }
        private string Value { get; set; }
        public override string ToString() { return Value; }
    }
    public static class OPERATORS
    {
        public static Operator EQUAL { get { return new Operator("=="); } }
        public static Operator GREATER_THAN { get { return new Operator(">"); } }
        public static Operator LESS_THAN { get { return new Operator("<"); } }
        public static Operator NOT_EQUAL { get { return new Operator("!="); } }
        public static Operator GREATER_EQUAL_TO { get { return new Operator(">="); } }
        public static Operator LESS_EQUAL_TO { get { return new Operator("<="); } }
    }

    public class ModalPage<T> : Container<T>, IModalPage
    {
        /// <summary>
        /// Creates a new instance of a modal page with inputs
        /// </summary>
        /// <param name="title">Modal title text</param>
        /// <param name="isDisabled">set wether or not the user will be able to access this page of the modal</param>
        /// <param name="triggeredBy">Html Id of the field that determines whether or not this page is disabled</param>
        /// <param name="triggerValue">value of the field that gets compared to determine if the page is disabled</param>
        /// <param name="triggerOperator">operator such that when the value of triggeredBy compared to
        /// triggerValue using this operator evaluates to True the modal page will not be disabled
        /// (ex: IModalPage.OPERATORS.LESS_THAN)</param>
        /// <param name="lengthOf">by default if this is false the value of triggeredBy is compared to 
        /// triggerValue, but when this is true, the length of the value of triggeredBy is compared to 
        /// triggerVal (so triggerVal should probably be an int)</param>
        public ModalPage(string title = null, bool isDisabled = false, string triggeredBy = null,
            string triggerValue = null, Operator triggerOperator = null, bool lengthOf = false
        )
        {

            Title = title ?? string.Empty;
            IsDisabled = isDisabled;
            TriggeredBy = triggeredBy;
            TriggerValue = triggerValue;
            Columns = new();
            TriggerOperator = triggerOperator ?? OPERATORS.EQUAL;
            LengthOf = lengthOf;
            PageForm = null;
        }
        public int PageNumber { get; set; }
        public bool IsDisabled { get; set; }
        public bool LengthOf { get; set; }
        public string TriggeredBy { get; set; }
        public string TriggerValue { get; set; }
        public Operator TriggerOperator { get; set; }
        private List<Container<T>> Columns { get; set; }
        private Elements.Form PageForm { get; set; }

        public ModalPage<T> AddColumns(params Container<T>[] columns)
        {
            foreach (Container<T> col in columns)
                Columns.Add(col);
            return this;
        }
        public string GetTriggerAttrs()
        {
            if (TriggeredBy == null)
                return string.Empty;
            return $"triggeredBy=\"{TriggeredBy}\" triggerVal=\"{TriggerValue}\"";
        }
        public void SetPageForm(string id, string action, bool isAjax = false)
        {
            PageForm = new(null, id, action, isAjax);
        }
        /// <summary>
        /// Used to get inner page
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Model"></param>
        // <param name="layout">IModalPage.PADDED</param>
        /// <returns>HtmlContent for this modal page</returns>
        /// <exception cref="InvalidParameterException"></exception>
        public IHtmlContent GetHtml(IHtmlHelper Html, T Model)
        //public IHtmlContent GetHtml(IHtmlHelper Html, T Model, int layout)
        {
            HtmlConcat htmlConcat = new();
            htmlConcat.Add("<div class=\"row\">");
            if (Columns.Count > 0)// using columns instead of page as a container
            {
                foreach (Container<T> column in Columns)
                {
                    int colWidth = 12 / Columns.Count;
                    htmlConcat.Add($"<div class=\"col-lg-{colWidth} col-12\">");
                    htmlConcat.Add(column.Render((IHtmlHelper<T>)Html, Model, outerClass: null));// set to null to use the pre-set class*/
                    htmlConcat.Add("</div>");
                }
                //htmlConcat.Add("</div>");
            }
            else //else if (layout == IModalPage.PADDED)
            {
                //htmlConcat.Add("<div class=\"row\">");
                htmlConcat.Add(Render((IHtmlHelper<T>)Html, Model, outerClass: null, outerStyle: "margin-bottom:5px;"));// set to null to use the pre-set class
            }
            htmlConcat.Add("</div>");
            /*else
                throw new InvalidParameterException(nameof(layout));*/

            return htmlConcat.Concatenate();
        }

        /// <summary>
        /// Used to get full page and container
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Model"></param>
        /// <param name="active"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public IHtmlContent GetHtml(IHtmlHelper Html, T Model, bool active, int index)
        //public IHtmlContent GetHtml(IHtmlHelper Html, T Model, int layout, bool active, int index)
        {
            HtmlConcat htmlConcat = new();
            string activeClass = active ? "active" : null;
            if (PageForm != null)
                htmlConcat.Add($"</form><form id=\"{PageForm.Id}\" {PageForm.GetUniqueAttributes()}>");
            htmlConcat.Add($"<div class=\"container modal-page {activeClass}\" data-title=\"{Title}\" " +
                $"pageindex=\"{index}\">" +
                GetHtml(Html, Model) + //GetHtml(Html, Model, layout) + 
            "</div>");
            return htmlConcat.Concatenate();
        }
    }
}