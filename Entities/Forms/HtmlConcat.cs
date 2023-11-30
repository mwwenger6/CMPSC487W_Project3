using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;

namespace CMPSC487W_Project3.Entities.Forms
{
    public class HtmlConcat
    {
        public HtmlConcat()
        {
            Content = new();
        }
        private List<IHtmlContent> Content { get; set; }

        public HtmlConcat Add(params IHtmlContent[] content)
        {
            Content = Content.Concat(content).ToList();
            return this;
        }
#nullable enable
        public HtmlConcat Add(params string?[] content)
        {
            Content = Content.Concat(content.Select(i => new HtmlString(i))).ToList();
            return this;
        }
#nullable disable

        public IHtmlContent Concatenate()
        {
            StringWriter writer = new();
            HtmlContentBuilder builder = new();

            foreach (IHtmlContent piece in Content)
                builder.AppendHtml(piece);

            builder.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }
        public static IHtmlContent Concatenate(IEnumerable<IHtmlContent> content)
        {
            return new HtmlConcat() { Content = content.ToList() }.Concatenate();
        }
    }
}