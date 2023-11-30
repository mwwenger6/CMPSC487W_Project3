
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace CMPSC487W_Project3.Entities.Forms.Inputs
{
    public class HiddenInput<T, TResult> : TextInput<T, TResult>
    {
        public HiddenInput(Expression<Func<T, TResult>> field, bool required)
            : base(field, string.Empty, string.Empty, required, false, string.Empty, false)
        { }

        protected override void RenderInner(IHtmlHelper<T> Html, T Model, HtmlConcat htmlConcat,
            string outerClass, string outerStyle
        )
        {
            string name = Html.NameFor(Field);
            htmlConcat.Add($"<input type=\"hidden\" id=\"{name.Replace(".", "_")}\" name=\"{name}\" value=\"{Html.ValueFor(Field)}\">");
        }
    }
}
