using Microsoft.AspNetCore.Html;

namespace CMPSC487W_Project3.Entities.Forms.Elements
{
    public class HtmlElement
    {
        public static readonly IHtmlContent LOADING_LIGHT = new HtmlString("<div class=\"spinner-border text-light\" role=\"status\" style=\"width:20px;height:20px;\"></div>");
        public static readonly IHtmlContent LOADING_DARK = new HtmlString("<div class=\"spinner-border text-dark\" role=\"status\" style=\"width:20px;height:20px;\"></div>");

        public HtmlElement(string tagName, IHtmlContent innerHtml = null, string id = null, string @class = null)
        {
            TagName = tagName;
            InnerHtml = innerHtml;
            Id = id;
            Class = @class;
        }
        public HtmlElement(string tagName, string innerHtml, string id = null, string @class = null)
        {
            TagName = tagName;
            InnerHtml = new HtmlString(innerHtml);
            Id = id;
            Class = @class;
        }

        public string Id { get; set; }
        protected string Class { get; set; }
        public string OnClick { get; set; }
        protected IHtmlContent InnerHtml { get; set; }
        protected string TagName { get; set; }
        public virtual string GetUniqueAttributes()
        {
            return string.Empty;
        }
        public HtmlElement AddClass(string newClass)
        {
            Class += $" {newClass}";
            return this;
        }

        public void SetText(string text)
        {
            InnerHtml = new HtmlString(text);
        }
        public virtual IHtmlContent Render()
        {
            HtmlConcat htmlConcat = new();
            htmlConcat
                .Add($"<{TagName} ");
            if (OnClick != null)
                htmlConcat.Add($"onclick=\"{OnClick}\" ");
            if (Id != null)
                htmlConcat.Add($"id=\"{Id}\" ");
            if (Class != null)
                htmlConcat.Add($"class=\"{Class}\" ");
            htmlConcat.Add(GetUniqueAttributes());
            htmlConcat.Add($">")
                    .Add(InnerHtml)
                .Add($"</{TagName}>");
            return htmlConcat.Concatenate();
        }
    }
    public class SelfClosingElement : HtmlElement
    {
        public SelfClosingElement(string tagName, IHtmlContent innerHtml = null, string id = null)
            : base(tagName, innerHtml, id) { }

        public override IHtmlContent Render()
        {
            HtmlConcat htmlConcat = new();
            htmlConcat.Add($"<{TagName} ");
            if (OnClick != null)
                htmlConcat.Add($"onclick=\"{OnClick}\" ");
            if (Id != null)
                htmlConcat.Add($"id=\"{Id}\"");
            if (Class != null)
                htmlConcat.Add($"class=\"{Class}\" ");
            htmlConcat.Add(GetUniqueAttributes());
            htmlConcat.Add($"/>");
            return htmlConcat.Concatenate();
        }
    }
}