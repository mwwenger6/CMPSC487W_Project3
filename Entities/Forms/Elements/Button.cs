using Microsoft.AspNetCore.Html;

namespace CMPSC487W_Project3.Entities.Forms.Elements
{
    public class Button : HtmlElement
    {
        public static readonly Button BACK_BTN = new("Back", null, "blue") { OnClick = "backButton()" };

        public Button(IHtmlContent content, string id, string color, bool isDismiss = false)
            : base("button", content, id, $"btn btn-{color} fw-thick")
        {
            IsDismiss = isDismiss;
        }
        public Button(string text, string id, string color, bool isDismiss = false)
            : base("button", new HtmlString(text), id, $"btn btn-{color} fw-thick")
        {
            IsDismiss = isDismiss;
        }

        private bool IsDismiss { get; set; }
        public override string GetUniqueAttributes()
        {
            return IsDismiss ? $"data-bs-dismiss=\"modal\" " : string.Empty;
        }
    }

    public class AddButton : Button
    {
        public AddButton(string objName, string color = "blue") : base(string.Empty, null, color)
        {
            HtmlConcat htmlConcat = new();
            htmlConcat
                .Add(" Add " + objName);
            InnerHtml = htmlConcat.Concatenate();
        }
    }
}