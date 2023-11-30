using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;

namespace CMPSC487W_Project3.Entities.Forms.Inputs
{
    public interface IRenderable
    {
        public IHtmlContent Render(IHtmlHelper Html, string outerClass, string outerStyle);
    }
    public interface IRenderable<T>
    {
        string GetValue(T model);
        public IHtmlContent Render(IHtmlHelper<T> Html, T Model, string outerClass, string outerStyle);
    }


    public abstract class Input<T> : IRenderable<T>
    {
        public Input(string label)
        {
            Label = label;
        }


        protected abstract void RenderInner(IHtmlHelper<T> Html, T Model, HtmlConcat htmlConcat,
            string outerClass, string outerStyle);
        public abstract Expression GetField();
        public string GetValue(IHtmlHelper<T> Html)
        {
            return Html.Value(GetField().ToString());
        }
        public string GetName()
        {
            dynamic b = GetField();
            string body = b.Body.ToString();
            return body.ToString()[(body.ToString().IndexOf(".") + 1)..].Replace(".", "_");
        }


        protected bool Required { get; set; }
        protected string Label { get; set; }
        protected string OuterClass { get; set; }
        protected string PlaceHolder { get; set; }
        public string OnChange { get; set; }
        public void SetOnChange(string divSelector)
        {
            OnChange = $"changeHiddenDiv({divSelector},this)";
        }
        public IHtmlContent Render(IHtmlHelper<T> Html, T Model, string outerClass, string outerStyle)
        {
            HtmlConcat htmlConcat = new();
            RenderInner(Html, Model, htmlConcat, outerClass, outerStyle);
            return htmlConcat.Concatenate();
        }


        protected string ToJSString(bool b) { return b.ToString().ToLower(); }

        public string GetValue(T model)
        {
            throw new NotImplementedException();
        }
    }
}