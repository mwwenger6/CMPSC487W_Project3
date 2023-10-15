
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Reflection;

namespace CMPSC487W_Project2.Entities.Forms.Inputs
{
    public class TextInput<T, TResult> : Input<T>
    {
        public TextInput(Expression<Func<T, TResult>> field, string label, string placeHolder,
            bool required, bool readOnly, string outerClass, bool hideable
        ) : base(label)
        {
            Field = field;
            PlaceHolder = placeHolder;
            Required = required;
            ReadOnly = readOnly;
            OuterClass = outerClass;
            Hideable = hideable;
        }

        public virtual Expression<Func<T, TResult>> Field { get; set; }
        /*public bool IsPassword(IHtmlHelper<T> Html)
        {
            string[] names = Html.NameFor(Field)?.Split(".");
            var a = Field.Compile().GetMethodInfo().ReturnParameter;
            Html.for
            if (names == null)
                return false;
            var prop = typeof(T).GetProperty(names.First());
            foreach (string name in names[1..])
                prop = prop.PropertyType.GetProperty(name);

            return Attribute.IsDefined(prop, typeof(Password));
        }*/
        public bool ReadOnly { get; set; }
        public bool Hideable { get; set; }

        public override Expression GetField()
        {
            return Field;
        }

        /*public override string GetName()
        {
            return GetName<TResult>();
        }*/

        /*public override string GetValue(T obj)
        {
            TResult result = Field.Compile().Invoke(obj);
            return result?.ToString()??string.Empty;
        }*/
        protected virtual IHtmlContent RenderLabel(IHtmlHelper<T> Html, T Model, string @class = null)
        {
            var htmlAttr = new
            {
                @class = (Required ? "required " : string.Empty) + @class,
            };
            return Html.LabelFor(Field, Label ?? string.Empty, htmlAttr);
        }
        protected override void RenderInner(IHtmlHelper<T> Html, T Model, HtmlConcat htmlConcat,
            string outerClass, string outerStyle
        )
        {
            object htmlAttr = ReadOnly
                ? new
                {
                    placeholder = PlaceHolder,
                    @readonly = ReadOnly,
                    req = ToJSString(Required)
                } : new
                {
                    placeholder = PlaceHolder,
                    req = ToJSString(Required)
                };
            string noLbl = string.IsNullOrEmpty(Label) ? " noLbl" : null;
            htmlConcat.Add($"<div class=\"myform-control{noLbl} {outerClass ?? OuterClass}\" style=\"{outerStyle}\">")
                .Add(RenderLabel(Html, Model))
                .Add(Html.TextBoxFor(Field, htmlAttr))
                /*.Add(IsPassword(Html)
                    ? Html.PasswordFor(Field, htmlAttr)
                    : Html.TextBoxFor(Field, htmlAttr))*/
                .Add(Html.ValidationMessageFor(Field))
                .Add("</div>");
        }

        /* public class IComparableConverter : ExpressionVisitor
         {
             protected override Expression VisitMethodCall(MethodCallExpression node)
             {
                 if (node.Method.Name == "Convert" && node.Method.GetParameters().Length == 2 &&
                     node.Method.GetParameters()[1].ParameterType == typeof(IComparable))
                     return Visit(node.Arguments[0]);
                 return base.VisitMethodCall(node);
             }
         }*/
    }

    public class MultiLineInput<T, TResult> : TextInput<T, TResult>
    {
        public MultiLineInput(Expression<Func<T, TResult>> field, string label, int rows, string placeHolder,
            bool required, bool readOnly, string outerClass, bool hideable)
            : base(field, label, placeHolder, required, readOnly, outerClass, hideable)
        {
            Rows = rows;
        }

        private int Rows { get; set; }
        public override Expression GetField()
        {
            return Field;
        }

        protected override void RenderInner(IHtmlHelper<T> Html, T Model, HtmlConcat htmlConcat,
            string outerClass, string outerStyle
        )
        {
            object htmlAttr = ReadOnly
                ? new
                {
                    placeholder = PlaceHolder,
                    @readonly = ReadOnly,
                    req = ToJSString(Required),
                    //rows = Rows,
                } : new
                {
                    placeholder = PlaceHolder,
                    req = ToJSString(Required),
                    //rows = Rows,
                };
            string noLbl = string.IsNullOrEmpty(Label) ? " noLbl" : null;
            htmlConcat.Add($"<div class=\"myform-control{noLbl} {outerClass ?? OuterClass}\" " +
                $"style=\"height:{Rows * 22}px;{outerStyle}\">")
                .Add(RenderLabel(Html, Model))
                //.Add(RenderLabel(Html, Model, "bg-white"))
                .Add(Html.TextAreaFor(Field, htmlAttr))
                .Add(Html.ValidationMessageFor(Field))
                .Add("</div>");
        }
    }
    public class PasswordInput<T, TResult> : TextInput<T, TResult>
    {
        public PasswordInput(Expression<Func<T, TResult>> field, string label, string placeHolder,
            bool required, bool readOnly, string outerClass)
            : base(field, label, placeHolder, required, readOnly, outerClass, false)
        { }

        public override Expression GetField()
        {
            return Field;
        }

        protected override void RenderInner(IHtmlHelper<T> Html, T Model, HtmlConcat htmlConcat,
            string outerClass, string outerStyle
        )
        {
            object htmlAttr = ReadOnly
                ? new
                {
                    placeholder = PlaceHolder,
                    @readonly = ReadOnly,
                    req = ToJSString(Required)
                } : new
                {
                    placeholder = PlaceHolder,
                    req = ToJSString(Required)
                };
            string noLbl = string.IsNullOrEmpty(Label) ? " noLbl" : null;
            htmlConcat.Add($"<div class=\"myform-control{noLbl} {outerClass ?? OuterClass}\" style=\"{outerStyle}\">")
                .Add(RenderLabel(Html, Model))
                .Add(Html.PasswordFor(Field, htmlAttr))
                .Add(Html.ValidationMessageFor(Field))
                .Add("</div>");
        }
    }
}