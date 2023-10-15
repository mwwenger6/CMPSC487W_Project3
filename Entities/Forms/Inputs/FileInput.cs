using CMPSC487W_Project2.Entities.Forms.Elements;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace CMPSC487W_Project2.Entities.Forms.Inputs
{
    public class FileInput<T> : Input<T>
    {
        public FileInput(Expression<Func<T, IFormFile>> field, string label, bool required, string outerClass)
         : base(label)
        {
            Field = field;
            Required = required;
            OuterClass = outerClass;
        }

        public Expression<Func<T, IFormFile>> Field { get; set; }
        public override Expression GetField()
        {
            return Field;
        }

        /*public override string GetName()
        {
            return GetName<IFormFile>();
        }*/
        private string GetValue(T obj)
        {
            IFormFile result = Field.Compile().Invoke(obj);
            return result.FileName;
        }

        protected virtual IHtmlContent RenderLabel(IHtmlHelper<T> Html, T Model)
        {
            string fileName = GetValue(Model)?.Split('/').Last() ?? string.Empty;
            string span = $"<span class=\"p-2\">{fileName}</span>";
            HtmlConcat htmlConcat = new();
            htmlConcat.Add($"<label for=\"{Html.NameFor(Field)}\" title=\"{fileName}\" class=\"custom-file-upload ");
            if (Required)
                htmlConcat.Add("required");
            htmlConcat.Add($"\">")
                .Add(Label).Add(span)
            .Add("</label>");
            return htmlConcat.Concatenate();
        }

        protected override void RenderInner(IHtmlHelper<T> Html, T Model, HtmlConcat htmlConcat,
            string outerClass, string outerStyle
        )
        {
            string fileName = Html.ValueFor(Field)?.Split('/').Last() ?? string.Empty;

            string name = Html.NameFor(Field);
            htmlConcat.Add($"<div class=\"myform-control {outerClass ?? OuterClass}\" style=\"{outerStyle}\">")
                .Add(RenderLabel(Html, Model))
                .Add($"<input class=\"form-control\" type=\"file\" id=\"{name.Replace(".", "_")}\" " +
                    $"name=\"{name}\" req=\"{ToJSString(Required)}\" " +
                    $"accept=\".jpeg,.jpg,.gif,.tiff,.png,.pdf\" noupload=\"{ToJSString(fileName.Length > 0)}\"" +
                    "onchange=\"updateFileInput(this)\")" +
                    $">")
            .Add("</div>");
        }
    }
}